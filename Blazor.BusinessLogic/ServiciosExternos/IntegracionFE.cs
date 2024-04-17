using Blazor.Infrastructure.Entities;
using Blazor.Infrastructure.Entities.Models;
using Dominus.Backend.Application;
using System.Net.Http.Headers;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Dominus.Backend.Security;
using Blazor.BusinessLogic.Models;
using Dominus.Frontend.Controllers;
using static System.Net.WebRequestMethods;

namespace Blazor.BusinessLogic.ServiciosExternos;

public class IntegracionFE
{

    private const string _urlGetToken = "/v2/auth/gettoken";
    private readonly string _urlEnviarFactura = $"/v2/{0}/outbounddocuments/salesInvoiceAsync";

    private readonly ParametrosGenerales _parametrosGenerales;

    private readonly string _host;

    public IntegracionFE(ParametrosGenerales parametrosGenerales, string host)
    {
        _parametrosGenerales = parametrosGenerales;
        _host = host;
        _urlEnviarFactura = string.Format(_urlEnviarFactura, _parametrosGenerales.OperadorFE);
    }

    private HttpClient GetHttpClient()
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
        var http = GetHttpClient();

        if (_parametrosGenerales == null)
        {
            throw new Exception($"Parametros generales no configurado.");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(_parametrosGenerales.UsuarioIntegracionFE) || string.IsNullOrWhiteSpace(_parametrosGenerales.PasswordIntegracionFE))
            {
                throw new Exception($"El usuario y/o contraseña no se encuentran configurados correctamente en los parametros generales.");
            }
        }

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

    public async Task<IntegracionFEModel> EnviarFacturaDian(string feJson)
    {
        IntegracionFEModel integracionFEModel = new IntegracionFEModel();
        try
        {
            var token = GetToken();
            var http = GetHttpClient();
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
            integracionFEModel.Error = ex.GetFullErrorMessage();
        }

        return integracionFEModel;
    }

}

