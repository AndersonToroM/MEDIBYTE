﻿using Blazor.BusinessLogic.Models;
using Blazor.Infrastructure.Entities;
using Blazor.Infrastructure.Entities.Models;
using Dominus.Backend.Application;
using Dominus.Backend.Security;
using Dominus.Frontend.Controllers;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.BusinessLogic.ServiciosExternos
{
    public class IntegracionRips
    {
        private const string _urlLoginSISPRO = "/api/Auth/LoginSISPRO";
        private const string _urlCargarFevRips = "/api/PaquetesFevRips/CargarFevRips";

        private readonly Empresas _empresa;
        private readonly User _user;

        public IntegracionRips(Empresas empresa, User user)
        {
            _empresa = empresa;
            _user = user;
        }

        private async Task<RespuestaLoginRips> GetTokenRips(string host)
        {
            var services = DApp.GetTenant(host).Services;
            var urlRips = services[DApp.Util.ServiceRips];
            HttpClient http = new HttpClient();
            http.BaseAddress = new Uri(urlRips);
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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

            var jsonLogin = JsonConvert.SerializeObject(loginIntegracionRips, Newtonsoft.Json.Formatting.Indented);
            var content = new StringContent(jsonLogin, Encoding.UTF8, "application/json");
            var httpResult = await http.PostAsync(_urlLoginSISPRO, content);
            var jsonResult = await httpResult.Content.ReadAsStringAsync();
            RespuestaLoginRips resultadoLoginRips = new RespuestaLoginRips();
            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                resultadoLoginRips = JsonConvert.DeserializeObject<RespuestaLoginRips>(jsonResult);
            }
            else
            {
                throw new Exception($"Error en LoginSISPRO. Estado: {httpResult.StatusCode.ToString()}. Error: {jsonResult}");
            }

            return resultadoLoginRips;

        }

        public async Task<IntegracionRipsModel> EnviarRipsFactura(Facturas factura, string ripsJson, string host)
        {
            IntegracionRipsModel integracionRipsModel = new IntegracionRipsModel();

            try
            {
                var token = await GetTokenRips(host);
                var services = DApp.GetTenant(host).Services;
                var urlRips = services[DApp.Util.ServiceRips];

                HttpClient http = new HttpClient();
                http.BaseAddress = new Uri(urlRips);
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
                integracionRipsModel.Error = ex.GetFullErrorMessage();
            }

            return integracionRipsModel;
        }
    }
}