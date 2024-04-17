using Blazor.BusinessLogic.Models;
using Blazor.Infrastructure.Entities;
using Blazor.Infrastructure.Entities.Models;
using DevExpress.XtraRichEdit.Import.Html;
using Dominus.Backend.Application;
using Dominus.Backend.Security;
using Dominus.Frontend.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            throw new Exception($"Error en LoginSISPRO. Estado: {httpResult.StatusCode.ToString()}. Error: {jsonResult}");
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
                var feResult = JsonConvert.DeserializeObject<FEResultJson<Guid>>(jsonResult);
                integracionFEModel.IdDocumentFE = feResult.ResultData;
                integracionFEModel.HuboErrorFE = true;
            }
        }
        catch (Exception ex)
        {
            integracionFEModel.HuboErrorIntegracion = true;
            integracionFEModel.ErrorIntegration = ex.GetFullErrorMessage();
        }

        return integracionFEModel;
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
                integracionSeriesFEModel.ErroresRespuesta = feResult.Errors;
                integracionSeriesFEModel.HuboErrorFE = true;
            }
        }
        catch (Exception ex)
        {
            integracionSeriesFEModel.HuboErrorIntegracion = true;
            integracionSeriesFEModel.ErrorIntegracion = ex.GetFullErrorMessage();
        }

        return integracionSeriesFEModel;
    }

}

