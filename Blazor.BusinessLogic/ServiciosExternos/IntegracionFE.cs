using Blazor.BusinessLogic.Models;
using Blazor.Infrastructure.Entities;
using Blazor.Infrastructure.Entities.Models;
using Dominus.Backend.Application;
using Dominus.Backend.Security;
using Dominus.Frontend.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.BusinessLogic.ServiciosExternos;

public class IntegracionFE
{

    private const string _urlGetToken = "v2/auth/gettoken";
    private readonly string _urlEnviarFactura = "v2/{0}/outbounddocuments/salesInvoiceAsync";
    private readonly string _urlGetSeries = "v2/{0}/companies/{1}/series/getall";
    private string _urlGetEstadoDocumento = "v2/{0}/outbounddocuments/{1}/status";
    private string _urlGetDatosDocumento = "v2/{0}/outbounddocuments/{1}";
    private string _urlGetXmlDocumento = "v2/{0}/outbounddocuments/{1}/ubl";

    private readonly ParametrosGenerales _parametrosGenerales;

    private readonly string _host;

    public IntegracionFE(ParametrosGenerales parametrosGenerales, string host)
    {
        _parametrosGenerales = parametrosGenerales;
        _host = host;

        ValidarDatos();

        _urlEnviarFactura = string.Format(_urlEnviarFactura, _parametrosGenerales.OperadorFE);
        _urlGetSeries = string.Format(_urlGetSeries, _parametrosGenerales.OperadorFE, _parametrosGenerales.CompanyIdFE);
    }

    private void ValidarDatos()
    {
        if (_parametrosGenerales == null)
        {
            throw new Exception($"Parametros generales no configurado.");
        }

        if (string.IsNullOrWhiteSpace(_parametrosGenerales.OperadorFE))
        {
            throw new Exception($"El el operador para facturacion electronica no se encuentra parametrizado correctamente.");
        }

        if (string.IsNullOrWhiteSpace(_parametrosGenerales.UsuarioIntegracionFE) || string.IsNullOrWhiteSpace(_parametrosGenerales.PasswordIntegracionFE))
        {
            throw new Exception($"El usuario y/o contraseña no se encuentran configurados correctamente en los parametros generales.");
        }

        if (_parametrosGenerales.CompanyIdFE == null || _parametrosGenerales.CompanyIdFE == Guid.Empty)
        {
            throw new Exception($"El el Id de la compañía para la facturacion electronica no se encuentra parametrizado correctamente.");
        }
    }

    private HttpClient BuildHttpClient()
    {
        var services = DApp.GetTenant(_host).Services;
        var urlRips = services[DApp.Util.ServiceFE];
        HttpClient http = new HttpClient();
        http.BaseAddress = new Uri(urlRips);
        http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return http;
    }

    private async Task<RespuestaFeTokenJson> GetToken()
    {
        var http = BuildHttpClient();

        FeTokenJson feGetToken = new FeTokenJson
        {
            Username = _parametrosGenerales.UsuarioIntegracionFE,
            Password = Cryptography.Decrypt(_parametrosGenerales.PasswordIntegracionFE),
            VirtualOperator = DApp.Util.ServiceFE.ToLower()
        };

        var jsonGetToken = JsonConvert.SerializeObject(feGetToken, Newtonsoft.Json.Formatting.Indented);
        var content = new StringContent(jsonGetToken, Encoding.UTF8, "application/json");
        var httpResult = await http.PostAsync(_urlGetToken, content);
        var jsonResult = await httpResult.Content.ReadAsStringAsync();
        if (httpResult.StatusCode == HttpStatusCode.OK)
        {
            var resultado = JsonConvert.DeserializeObject<FEResultJson<RespuestaFeTokenJson>>(jsonResult);
            return resultado.ResultData;
        }
        else
        {
            throw new Exception($"Error en Get Token. Estado: {httpResult.StatusCode.ToString()}. Error: {jsonResult}");
        }
    }

    public async Task<IntegracionEnviarFEModel> EnviarFacturaDian(string feJson)
    {
        IntegracionEnviarFEModel integracionFEModel = new IntegracionEnviarFEModel();
        try
        {
            var token = GetToken();
            var http = BuildHttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result.AccessToken);

            var content = new StringContent(feJson, Encoding.UTF8, "application/json");
            var httpResult = await http.PostAsync(_urlEnviarFactura, content);
            var jsonResult = await httpResult.Content.ReadAsStringAsync();
            integracionFEModel.HttpStatus = (int)httpResult.StatusCode;
            integracionFEModel.JsonResult = jsonResult;

            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<Guid>>(jsonResult);
                integracionFEModel.IdDocumentFE = feResult.ResultData;
                integracionFEModel.HuboErrorFE = false;
            }
            else if (httpResult.StatusCode == HttpStatusCode.BadRequest)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<string>>(jsonResult);
                integracionFEModel.HuboErrorFE = true;

                if (feResult.ListaErrores.Any())
                {
                    integracionFEModel.Errores = feResult.ListaErrores;
                }
                integracionFEModel.Status = "BadRequest";
            }
        }
        catch (Exception ex)
        {
            integracionFEModel.HuboErrorIntegracion = true;
            integracionFEModel.Errores.Add(ex.GetFullErrorMessage());
        }

        return integracionFEModel;
    }

    public async Task<IntegracionEnviarFEModel> ConsultarEstadoDocumento(Guid idDocumento)
    {
        IntegracionEnviarFEModel integracionConsultarEstadoFEModel = new IntegracionEnviarFEModel();
        try
        {
            var token = GetToken();
            var http = BuildHttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result.AccessToken);

            _urlGetEstadoDocumento = string.Format(_urlGetEstadoDocumento, _parametrosGenerales.OperadorFE, idDocumento.ToString("D"));

            var httpResult = await http.GetAsync(_urlGetEstadoDocumento);
            var jsonResult = await httpResult.Content.ReadAsStringAsync();
            integracionConsultarEstadoFEModel.HttpStatus = (int)httpResult.StatusCode;
            integracionConsultarEstadoFEModel.JsonResult = jsonResult;

            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<RespuestaStatus>>(jsonResult);
                if (feResult.ResultData != null)
                {
                    integracionConsultarEstadoFEModel.Status = feResult.ResultData.Status;
                    integracionConsultarEstadoFEModel.DocumentStatus = feResult.ResultData.Status;

                    if (feResult.ResultData.Status.Equals("Certified", StringComparison.OrdinalIgnoreCase) && (feResult.ResultData.ValidationErrors == null || !feResult.ResultData.ValidationErrors.Any()))
                    {
                        integracionConsultarEstadoFEModel.HuboErrorFE = false;
                        integracionConsultarEstadoFEModel = await ConsultarDatosDocumento(idDocumento);
                    }
                    else
                    {
                        if (!feResult.ResultData.Status.Equals("Staged", StringComparison.OrdinalIgnoreCase))
                        {
                            if (feResult.ResultData.ValidationErrors != null && feResult.ResultData.ValidationErrors.Any())
                            {
                                var erroresCode = feResult.ResultData.ValidationErrors.Select(x => x.Code).ToList();
                                var erroresDesc = feResult.ResultData.ValidationErrors.Select(x => x.Description).ToList();
                                var errores = feResult.ResultData.ValidationErrors.SelectMany(x => x.ExplanationValues).ToList();
                                integracionConsultarEstadoFEModel.Errores.AddRange(erroresCode);
                                integracionConsultarEstadoFEModel.Errores.AddRange(erroresDesc);
                                integracionConsultarEstadoFEModel.Errores.AddRange(errores);
                            }
                        }
                        integracionConsultarEstadoFEModel.HuboErrorFE = true;
                    }
                }
                else
                {
                    integracionConsultarEstadoFEModel.Errores.Add("Verificar integracion.");
                    integracionConsultarEstadoFEModel.HuboErrorFE = true;
                }
            }
            else if (httpResult.StatusCode == HttpStatusCode.BadRequest)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<object>>(jsonResult);
                integracionConsultarEstadoFEModel.Errores = feResult.ListaErrores;
                integracionConsultarEstadoFEModel.HuboErrorFE = true;
                integracionConsultarEstadoFEModel.Status = "BadRequest";
            }
        }
        catch (Exception ex)
        {
            integracionConsultarEstadoFEModel.HuboErrorIntegracion = true;
            integracionConsultarEstadoFEModel.Errores.Add(ex.GetFullErrorMessage());
        }

        return integracionConsultarEstadoFEModel;
    }

    private async Task<IntegracionEnviarFEModel> ConsultarDatosDocumento(Guid idDocumento)
    {
        IntegracionEnviarFEModel integracionConsultarEstadoFEModel = new IntegracionEnviarFEModel();
        try
        {
            var token = GetToken();
            var http = BuildHttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result.AccessToken);

            _urlGetDatosDocumento = string.Format(_urlGetDatosDocumento, _parametrosGenerales.OperadorFE, idDocumento.ToString("D"));

            var httpResult = await http.GetAsync(_urlGetDatosDocumento);
            var jsonResult = await httpResult.Content.ReadAsStringAsync();
            integracionConsultarEstadoFEModel.HttpStatus = (int)httpResult.StatusCode;
            integracionConsultarEstadoFEModel.JsonResult = jsonResult;

            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<FeRespuestaConsultaDocumento>>(jsonResult);
                if (feResult.ResultData != null)
                {
                    integracionConsultarEstadoFEModel.HuboErrorFE = false;
                    integracionConsultarEstadoFEModel.Cufe = feResult.ResultData.Cufe;
                    integracionConsultarEstadoFEModel.IssueDate = feResult.ResultData.CreationDate.ToLocalTime();
                    integracionConsultarEstadoFEModel.Status = feResult.ResultData.DocumentStatus;
                    integracionConsultarEstadoFEModel.DocumentStatus = feResult.ResultData.DocumentStatus;
                }
                else
                {
                    integracionConsultarEstadoFEModel.Errores.Add("Verificar integracion.");
                    integracionConsultarEstadoFEModel.HuboErrorFE = true;
                }
            }
            else if (httpResult.StatusCode == HttpStatusCode.BadRequest)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<object>>(jsonResult);
                integracionConsultarEstadoFEModel.Errores = feResult.ListaErrores;
                integracionConsultarEstadoFEModel.HuboErrorFE = true;
                integracionConsultarEstadoFEModel.Status = "BadRequest";
            }
        }
        catch (Exception ex)
        {
            integracionConsultarEstadoFEModel.HuboErrorIntegracion = true;
            integracionConsultarEstadoFEModel.Errores.Add(ex.GetFullErrorMessage());
        }

        return integracionConsultarEstadoFEModel;
    }

    public async Task<IntegracionXmlFEModel> GetXmlFile(Guid idDocumento)
    {
        IntegracionXmlFEModel integracionXmlFEModel = new IntegracionXmlFEModel();
        try
        {
            var token = GetToken();
            var http = BuildHttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result.AccessToken);

            _urlGetXmlDocumento = string.Format(_urlGetXmlDocumento, _parametrosGenerales.OperadorFE, idDocumento.ToString("D"));
            
            var httpResult = await http.GetAsync(_urlGetXmlDocumento);
            var jsonResult = await httpResult.Content.ReadAsStringAsync();
            integracionXmlFEModel.HttpStatus = (int)httpResult.StatusCode;
            integracionXmlFEModel.JsonResult = jsonResult;

            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<FeResutoGetXml>>(jsonResult);
                integracionXmlFEModel.FileName = feResult.ResultData.FileName;
                integracionXmlFEModel.ContentBase64 = feResult.ResultData.Content;
                integracionXmlFEModel.HuboErrorFE = false;
            }
            else if (httpResult.StatusCode == HttpStatusCode.BadRequest)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<FeResutoGetXml>>(jsonResult);
                integracionXmlFEModel.Errores = feResult.ListaErrores;
                integracionXmlFEModel.HuboErrorFE = true;
            }
        }
        catch (Exception ex)
        {
            integracionXmlFEModel.HuboErrorIntegracion = true;
            integracionXmlFEModel.Errores.Add(ex.GetFullErrorMessage());
        }

        return integracionXmlFEModel;
    }

    public async Task<IntegracionSeriesFEModel> GetResultadoSeries()
    {
        IntegracionSeriesFEModel integracionSeriesFEModel = new IntegracionSeriesFEModel();
        try
        {
            var token = GetToken();
            var http = BuildHttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result.AccessToken);

            var httpResult = await http.GetAsync(_urlGetSeries);
            var jsonResult = await httpResult.Content.ReadAsStringAsync();
            integracionSeriesFEModel.HttpStatus = (int)httpResult.StatusCode;
            integracionSeriesFEModel.JsonResult = jsonResult;

            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<List<FEResultadoSeries>>>(jsonResult);
                integracionSeriesFEModel.ResultadoSeries = feResult.ResultData;
                integracionSeriesFEModel.HuboErrorFE = false;
            }
            else if (httpResult.StatusCode == HttpStatusCode.BadRequest)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<List<FEResultadoSeries>>>(jsonResult);
                integracionSeriesFEModel.Errores = feResult.ListaErrores;
                integracionSeriesFEModel.HuboErrorFE = true;
            }
        }
        catch (Exception ex)
        {
            integracionSeriesFEModel.HuboErrorIntegracion = true;
            integracionSeriesFEModel.Errores.Add(ex.GetFullErrorMessage());
        }

        return integracionSeriesFEModel;
    }

}

