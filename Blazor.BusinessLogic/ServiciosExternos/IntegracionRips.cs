using Blazor.BusinessLogic.Models;
using Blazor.Infrastructure.Entities;
using Blazor.Infrastructure.Entities.Models;
using DevExpress.Pdf.Native.BouncyCastle.Ocsp;
using Dominus.Backend.Application;
using Dominus.Backend.Security;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.BusinessLogic.ServiciosExternos;

public class IntegracionRips
{
    private const string _urlLoginSISPRO = "/api/Auth/LoginSISPRO";
    private const string _urlCargarFevRips = "/api/PaquetesFevRips/CargarFevRips";

    private readonly Empresas _empresa;
    private readonly User _user;

    private readonly string _host;

    public IntegracionRips(Empresas empresa, User user, string host)
    {
        _empresa = empresa;
        _user = user;
        _host = host;
    }

    private HttpClient BuildHttpClient()
    {
        var services = DApp.GetTenant(_host).Services;
        var urlRips = services[DApp.Util.ServiceRips];

        //var handler = new HttpClientHandler();
        //handler.ClientCertificates.Add(new X509Certificate2("D:\\SIISO\\FEV_Rips\\build_docker\\fevripsapilocal.pfx", "2024"));
        HttpClient http = new HttpClient();
        http.BaseAddress = new Uri(urlRips);
        http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return http;
    }

    private async Task<RespuestaLoginRips> GetTokenRips()
    {
        var http = BuildHttpClient();

        if (_empresa == null)
        {
            throw new Exception($"Empresa no encontrada.");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(_empresa.NumeroIdentificacion))
            {
                throw new Exception($"El numero de identificación de la empresa no esta diligenciado en su maestra.");
            }
        }


        if (_user == null)
        {
            throw new Exception($"Usuario no encontrado.");
        }
        else
        {
            if (_user.IdentificationType == null || string.IsNullOrWhiteSpace(_user.IdentificationType.Codigo) ||
                string.IsNullOrWhiteSpace(_user.IdentificationNumber) || _user.IdentificationNumber.Length < 5)
            {
                throw new Exception($"El usuario {_user.UserName} no tiene el numero de identificación o el tipo de identificación correctamente diligenciados en su maestro.");
            }
        }

        LoginIntegracionRips loginIntegracionRips = new LoginIntegracionRips();
        loginIntegracionRips.Persona.Identificacion.Tipo = _user.IdentificationType.Codigo;
        loginIntegracionRips.Persona.Identificacion.Numero = _user.IdentificationNumber;
        loginIntegracionRips.Nit = _empresa.NumeroIdentificacion;
        loginIntegracionRips.Clave = Cryptography.Decrypt(_user.PasswordRips);
        loginIntegracionRips.Reps = true;

        var jsonLogin = JsonConvert.SerializeObject(loginIntegracionRips, Formatting.Indented);
        var content = new StringContent(jsonLogin, Encoding.UTF8, "application/json");
        var httpResult = await http.PostAsync(_urlLoginSISPRO, content);
        var jsonResult = await httpResult.Content.ReadAsStringAsync();
        if (httpResult.StatusCode == HttpStatusCode.OK)
        {
            return JsonConvert.DeserializeObject<RespuestaLoginRips>(jsonResult);
        }
        else
        {
            throw new Exception($"Error en LoginSISPRO. Estado: {httpResult.StatusCode.ToString()}. Error: {jsonResult}");
        }
    }

    public async Task<IntegracionRipsModel> EnviarRipsFactura(string ripsJson)
    {
        IntegracionRipsModel integracionRipsModel = new IntegracionRipsModel();

        try
        {
            var token = await GetTokenRips();
            var http = BuildHttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            var content = new StringContent(ripsJson, Encoding.UTF8, "application/json");
            var httpResult = await http.PostAsync(_urlCargarFevRips, content);
            var jsonResult = await httpResult.Content.ReadAsStringAsync();
            integracionRipsModel.HttpStatus = (int)httpResult.StatusCode;
            integracionRipsModel.JsonResult = jsonResult;


            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                var ripsResult = JsonConvert.DeserializeObject<RespuestaIntegracionRips>(jsonResult);
                integracionRipsModel.Respuesta = ripsResult;

                if (string.IsNullOrWhiteSpace(ripsResult.CodigoUnicoValidacion))
                {
                    integracionRipsModel.HuboErrorRips = true;
                }
                else
                {
                    integracionRipsModel.HuboErrorRips = false;
                }
            }
            else if (httpResult.StatusCode == HttpStatusCode.BadRequest)
            {
                var ripsResult = JsonConvert.DeserializeObject<RespuestaIntegracionRips>(jsonResult);
                integracionRipsModel.Respuesta = ripsResult;
                integracionRipsModel.HuboErrorRips = true;
            }
            else
            {
                integracionRipsModel.JsonResult = jsonResult;
            }
        }
        catch (Exception ex)
        {
            integracionRipsModel.HuboErrorIntegracion = true;
            integracionRipsModel.Error = ex.GetBackFullErrorMessage();
        }

        return integracionRipsModel;
    }
}

