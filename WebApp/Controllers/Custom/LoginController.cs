using Blazor.BusinessLogic;
using Blazor.Infrastructure.Entities;
using Blazor.Infrastructure.Models;
using Dominus.Backend.Application;

using Dominus.Backend.DataBase;
using Dominus.Frontend.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Text;
using System.IO;

namespace Blazor.WebApp.Controllers
{
    public class LoginController : BaseAppController
    {
        public IConfiguration Configuration { get; }
        public LoginController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
            Configuration = config;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            SingInModel model = new SingInModel();
            var host = httpContextAccessor.HttpContext.Request.Host;
            DataBaseSetting conexionTenant = null;
            try
            {
                conexionTenant = DApp.GetTenantConnection(host.Value);
            }
            catch (Exception)
            {
                conexionTenant = null;
            }

            if (conexionTenant == null)
            {
                model.errores.Add("Esta empresa no se encuentra registrada en nuestras conexiones. Por favor contacte con el administrador.");
                return View("InfoEmpresas", model);
            }
            else
            {
                Dominus.Backend.DataBase.BusinessLogic manager = new Dominus.Backend.DataBase.BusinessLogic(conexionTenant);
                Empresas empresa = manager.GetBusinessLogic<Empresas>().FindById(x => true, true);
                model.RazonSocialEmpresa = empresa.RazonSocial;
                model.ConnectionId = Request.Host.Value;
                model.Logo = DApp.Util.ArrayBytesToString(empresa.LogoArchivos.Archivo);

                ViewBag.VersionApp = DApp.InfoApp.VersionApp;
                ViewBag.ParcheApp = DApp.InfoApp.ParcheApp;

                return View(model);
            }
        }

        [HttpPost("[controller]/SingIn")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(SingInModel model, string returnUrl = null)
        {
            var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            DataBaseSetting currentConnection = DApp.GetTenantConnection(Request.Host.Value);
            Dominus.Backend.DataBase.BusinessLogic manager = new Dominus.Backend.DataBase.BusinessLogic(currentConnection);

            //Valido que todos los datos son obligatorios
            if (string.IsNullOrWhiteSpace(model.ConnectionId) || string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password) /*|| model.EmpresaId <= 0*/)
            {
                ModelState.AddModelError("UserName", "Todos los datos son obligatorios.");
                return View(model);
            }

            //Consulto la empresa con relacion a su conexion de base de datos
            Empresas empresa = manager.GetBusinessLogic<Empresas>().FindById(x => true, false);
            //Empresas empresa = manager.GetBusinessLogic<Empresas>().FindById(x => x.Alias == model.ConnectionId, false);
            //if (empresa == null) {
            //    ModelState.AddModelError("ConnectionId", $"La empresa no se encuentra registrada con este alias: {model.ConnectionId}.");
            //    return View(model);
            //}

            //Valido que el usuario exista
            User loggedUser = manager.UserBusinessLogic().SingIn(model.UserName, model.Password, Request.Host.Value);
            if (loggedUser == null)
            {
                ModelState.AddModelError("UserName", "El usuario y contraseña no son correctos.");
                return View(model);
            }

            if (!loggedUser.IsActive)
            {
                ModelState.AddModelError("UserName", "El usuario no esta activo.");
                return View(model);
            }

            // Valido que tenga perfiles asociados el usuario
            List<ProfileUser> perfilesUsuarios = manager.GetBusinessLogic<ProfileUser>().FindAll(x => x.UserId == loggedUser.Id, true).ToList();
            if (perfilesUsuarios == null || perfilesUsuarios.Count <= 0)
            {
                ModelState.AddModelError("UserName", "El usuario no tiene un perfil asociado. Conmunicarse con el administrador.");
                return View(model);
            }

            List<long> perfilesId = perfilesUsuarios.Select(x => x.ProfileId).ToList();
            long entidadId = 0;
            if (perfilesUsuarios.Count == 1 && perfilesUsuarios.Exists(x => x.Profile.EntidadesId != null))
                entidadId = perfilesUsuarios.First().Profile.EntidadesId.GetValueOrDefault(0);

            ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(model), CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaims(GetTokenClaims(loggedUser, perfilesId, empresa.Id, entidadId));
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            //int sessionTime = int.Parse(config["SessionTime"]);
            var authProperties = new AuthenticationProperties();
            authProperties.RedirectUri = httpContextAccessor.HttpContext.Request.Host.Value;
            authProperties.IsPersistent = true;
            var fechaExpirar = DateTime.Now.AddDays(2);
            authProperties.ExpiresUtc = new DateTimeOffset(fechaExpirar.Year, fechaExpirar.Month, fechaExpirar.Day, 3, 0, 0, TimeSpan.Zero);

            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal), authProperties);

            //Cuando se loguea se actualiza la seguridad segun la conexion selecionada.
            //DApp.DataBaseSettings[DApp.DataBaseSettings.FindIndex(x => x.Name == model.ConnectionId)] = currentConnection;
            DApp.Tenants[DApp.Tenants.FindIndex(x => x.Name.Contains(Request.Host.Value))].DataBaseSetting = currentConnection;

            DApp.LogToFile(TypeLog.Informativo, $"El usuario {loggedUser.NombreCompleto} inicio sesion.");
            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<Claim> GetTokenClaims(User user, List<long> profilesId, long EmpresaId, long entidadId)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim("UserId", user.Id.ToString()));
            claims.Add(new Claim("Email", string.IsNullOrWhiteSpace(user.Email) ? "" : user.Email));
            claims.Add(new Claim("ProfilesId", JsonConvert.SerializeObject(profilesId)));
            claims.Add(new Claim("EmpresaId", EmpresaId.ToString()));
            claims.Add(new Claim("EntidadId", entidadId.ToString()));
            return claims;
        }

        private IEnumerable<Claim> GetUserClaims(SingInModel user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));
            claims.Add(new Claim("ConnectionName", user.ConnectionId));
            return claims;
        }

        [HttpGet("[controller]/LogOff")]
        public async Task<IActionResult> LogOff()
        {
            await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("~/");
        }

        [HttpGet("/GetResponseFromServer")]
        [AllowAnonymous]
        public IActionResult GetResponseFromServer()
        {
            return Ok();
        }

        [HttpPost("/SaveLogFromClient")]
        [AllowAnonymous]
        public IActionResult SaveLogFromClient(List<string> logs, int type)
        {
            return Ok();

            ////Habilitar cuando queramos ver como se comporta el internet del cliente
            //try
            //{
            //    string title = string.Empty;
            //    string nameFile = string.Empty;
            //    if (type == 1) // ping log
            //    {
            //        title = "Net;Server;Tiempo de espera;Codigo;Estado;Fecha;Hora;Plataforma;Ubicacion" + Environment.NewLine;
            //        nameFile = $"SIISO_LogPing_{DateTime.Now:yyyyMMdd}";
            //    }
            //    else if (type == 2) // speed test
            //    {
            //        title = "Fecha;Hora;Plataforma;Tamaño KB;Min KB; Velocidad KB" + Environment.NewLine;
            //        nameFile = $"SIISO_LogSpeedTest_{DateTime.Now:yyyyMMdd}";
            //    }

            //    if (!Directory.Exists(DApp.PathDirectoryLogs))
            //        Directory.CreateDirectory(DApp.PathDirectoryLogs);
            //    string pathFile = Path.Combine(DApp.PathDirectoryLogs, $"{nameFile}.csv");
            //    System.IO.File.AppendAllText(pathFile, title);
            //    return Ok();
            //}
            //catch (Exception)
            //{
            //    return BadRequest();
            //}
        }

    }
}