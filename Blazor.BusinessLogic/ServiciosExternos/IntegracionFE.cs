using Blazor.BusinessLogic.Models;
using Blazor.BusinessLogic.Models.Enums;
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
    private readonly string _urlEnviarNotaDebito = "v2/{0}/outbounddocuments/debitNoteAsync";
    private readonly string _urlEnviarNotaCredito = "v2/{0}/outbounddocuments/creditNoteAsync";
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
        _urlEnviarNotaDebito = string.Format(_urlEnviarNotaDebito, _parametrosGenerales.OperadorFE);
        _urlEnviarNotaCredito = string.Format(_urlEnviarNotaCredito, _parametrosGenerales.OperadorFE);
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
            throw new Exception($"El operador para facturacion electronica no se encuentra parametrizado correctamente.");
        }

        if (string.IsNullOrWhiteSpace(_parametrosGenerales.UsuarioIntegracionFE) || string.IsNullOrWhiteSpace(_parametrosGenerales.PasswordIntegracionFE))
        {
            throw new Exception($"El usuario y/o contraseña no se encuentran configurados correctamente en los parametros generales.");
        }

        if (_parametrosGenerales.CompanyIdFE == null || _parametrosGenerales.CompanyIdFE == Guid.Empty)
        {
            throw new Exception($"El el Id de la compañía para la facturacion electronica no se encuentra parametrizado correctamente.");
        }

        if (string.IsNullOrWhiteSpace(_parametrosGenerales.LinkVerificacionDIAN))
        {
            throw new Exception($"El link de validacion DIAN no se encuentran configurado correctamente en los parametros generales.");
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

    public async Task<IntegracionEnviarFEModel> EnviarDocumento(string feJson, TipoEnvioDocumentoDian tipoEnvioDocumentoDian)
    {
        IntegracionEnviarFEModel resultadoEnviarDoc = new IntegracionEnviarFEModel();
        try
        {
            var token = GetToken();
            var http = BuildHttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result.AccessToken);

            switch (tipoEnvioDocumentoDian)
            {
                case TipoEnvioDocumentoDian.Factura:
                    resultadoEnviarDoc.Api = _urlEnviarFactura;
                    break;
                case TipoEnvioDocumentoDian.NotaCredito:
                    resultadoEnviarDoc.Api = _urlEnviarNotaCredito;
                    break;
                case TipoEnvioDocumentoDian.NotaDebito:
                    resultadoEnviarDoc.Api = _urlEnviarNotaDebito;
                    break;
                default:
                    throw new Exception($"Error en enviar documento. El tipo de envio no se encuentra establecido.");
            }

            var content = new StringContent(feJson, Encoding.UTF8, "application/json");
            var httpResult = await http.PostAsync(resultadoEnviarDoc.Api, content);
            var jsonResult = await httpResult.Content.ReadAsStringAsync();
            resultadoEnviarDoc.HttpStatus = (int)httpResult.StatusCode;
            resultadoEnviarDoc.JsonResult = jsonResult;

            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<Guid>>(jsonResult);
                resultadoEnviarDoc.IdDocumentFE = feResult.ResultData;
                resultadoEnviarDoc.HuboErrorFE = false;
                resultadoEnviarDoc.Status = "Received";
            }
            else if (httpResult.StatusCode == HttpStatusCode.BadRequest)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<string>>(jsonResult);
                resultadoEnviarDoc.HuboErrorFE = true;

                if (feResult.ListaErrores.Any())
                {
                    resultadoEnviarDoc.Errores = feResult.ListaErrores;
                }
                resultadoEnviarDoc.Status = "BadRequest";
            }
        }
        catch (Exception ex)
        {
            resultadoEnviarDoc.HuboErrorIntegracion = true;
            resultadoEnviarDoc.Errores.Add(ex.GetFullErrorMessage());
        }

        return resultadoEnviarDoc;
    }

    public async Task<IntegracionEnviarFEModel> ConsultarEstadoDocumento(Guid idDocumento)
    {
        IntegracionEnviarFEModel resultadoConsultarDoc = new IntegracionEnviarFEModel();
        try
        {
            var token = GetToken();
            var http = BuildHttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result.AccessToken);

            _urlGetEstadoDocumento = string.Format(_urlGetEstadoDocumento, _parametrosGenerales.OperadorFE, idDocumento.ToString("D"));
            resultadoConsultarDoc.Api = _urlGetEstadoDocumento;

            var httpResult = await http.GetAsync(_urlGetEstadoDocumento);
            var jsonResult = await httpResult.Content.ReadAsStringAsync();
            resultadoConsultarDoc.HttpStatus = (int)httpResult.StatusCode;
            resultadoConsultarDoc.JsonResult = jsonResult;

            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<RespuestaStatus>>(jsonResult);
                if (feResult.ResultData != null)
                {
                    resultadoConsultarDoc.Status = feResult.ResultData.Status;
                    if (feResult.ResultData.Status.Equals("Certified", StringComparison.OrdinalIgnoreCase) && (feResult.ResultData.ValidationErrors == null || !feResult.ResultData.ValidationErrors.Any()))
                    {
                        resultadoConsultarDoc.HuboErrorFE = false;
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
                                resultadoConsultarDoc.Errores.AddRange(erroresCode);
                                resultadoConsultarDoc.Errores.AddRange(erroresDesc);
                                resultadoConsultarDoc.Errores.AddRange(errores);
                            }
                        }
                        resultadoConsultarDoc.HuboErrorFE = true;
                    }
                }
                else
                {
                    resultadoConsultarDoc.Errores.Add("Verificar integracion.");
                    resultadoConsultarDoc.HuboErrorFE = true;
                }
            }
            else if (httpResult.StatusCode == HttpStatusCode.BadRequest)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<object>>(jsonResult);
                resultadoConsultarDoc.Errores = feResult.ListaErrores;
                resultadoConsultarDoc.HuboErrorFE = true;
                resultadoConsultarDoc.Status = "BadRequest";
            }
        }
        catch (Exception ex)
        {
            resultadoConsultarDoc.HuboErrorIntegracion = true;
            resultadoConsultarDoc.Errores.Add(ex.GetFullErrorMessage());
        }

        if (string.IsNullOrWhiteSpace(resultadoConsultarDoc.Status))
        {
            resultadoConsultarDoc.Status = "Unmarked";
        }

        return resultadoConsultarDoc;
    }

    public async Task<IntegracionEnviarFEModel> ConsultarDatosDocumento(Guid idDocumento)
    {
        IntegracionEnviarFEModel resultadoConsultarDoc = new IntegracionEnviarFEModel();
        try
        {
            var token = GetToken();
            var http = BuildHttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result.AccessToken);

            _urlGetDatosDocumento = string.Format(_urlGetDatosDocumento, _parametrosGenerales.OperadorFE, idDocumento.ToString("D"));
            resultadoConsultarDoc.Api = _urlGetDatosDocumento;

            var httpResult = await http.GetAsync(_urlGetDatosDocumento);
            var jsonResult = await httpResult.Content.ReadAsStringAsync();
            resultadoConsultarDoc.HttpStatus = (int)httpResult.StatusCode;
            resultadoConsultarDoc.JsonResult = jsonResult;

            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<FeRespuestaConsultaDocumento>>(jsonResult);
                if (feResult.ResultData != null)
                {
                    resultadoConsultarDoc.HuboErrorFE = false;
                    resultadoConsultarDoc.Cufe = feResult.ResultData.Cufe;
                    resultadoConsultarDoc.IssueDate = feResult.ResultData.CreationDate.ToLocalTime();
                    resultadoConsultarDoc.Status = feResult.ResultData.DocumentStatus;
                }
                else
                {
                    resultadoConsultarDoc.Errores.Add("Verificar integracion.");
                    resultadoConsultarDoc.HuboErrorFE = true;
                }
            }
            else if (httpResult.StatusCode == HttpStatusCode.BadRequest)
            {
                var feResult = JsonConvert.DeserializeObject<FEResultJson<object>>(jsonResult);
                resultadoConsultarDoc.Errores = feResult.ListaErrores;
                resultadoConsultarDoc.HuboErrorFE = true;
                resultadoConsultarDoc.Status = "BadRequest";
            }
        }
        catch (Exception ex)
        {
            resultadoConsultarDoc.HuboErrorIntegracion = true;
            resultadoConsultarDoc.Errores.Add(ex.GetFullErrorMessage());
        }

        if (string.IsNullOrWhiteSpace(resultadoConsultarDoc.Status))
        {
            resultadoConsultarDoc.Status = "Unmarked";
        }

        return resultadoConsultarDoc;
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
            integracionXmlFEModel.Api = _urlGetXmlDocumento;

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
            integracionSeriesFEModel.Api = _urlGetSeries;

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

