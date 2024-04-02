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

namespace Blazor.BusinessLogic.ServiciosExternos;

public class IntegracionFE
{

    private const string _urlGetToken = "/v2/auth/gettoken";

    private readonly ParametrosGenerales _parametrosGenerales;

    public IntegracionFE(ParametrosGenerales parametrosGenerales)
    {
        _parametrosGenerales = parametrosGenerales;
    }

    private async Task<RespuestaFeGetToken> GetToken(string host)
    {
        var services = DApp.GetTenant(host).Services;
        var urlRips = services[DApp.Util.ServiceFE];
        HttpClient http = new HttpClient();
        http.BaseAddress = new Uri(urlRips);
        http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


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

        FeGetToken feGetToken = new FeGetToken
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
            var resultado = JsonConvert.DeserializeObject<FEResult<RespuestaFeGetToken>>(jsonResult);
            return resultado.ResultData;
        }
        else
        {
            throw new Exception($"Error en LoginSISPRO. Estado: {httpResult.StatusCode.ToString()}. Error: {jsonResult}");
        }

    }

}

