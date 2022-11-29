using Dominus.Backend.Application;
using Dominus.Backend.HttpClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Dominus.Frontend.Controllers
{
    public class BaseController : Controller
    {
        protected IHttpContextAccessor httpContextAccessor;

        protected IConfiguration config;

        public BaseController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this.httpContextAccessor = httpContextAccessor;
            string path = httpContextAccessor.HttpContext.Request.Path.ToString().Trim();
            //bool security = DApp.ActionViewSecurity(httpContextAccessor.HttpContext, path);
            //if (security)
            //{
            //    Console.WriteLine("No tiene acceso a la accion: " + path);
            //    throw new System.Exception(DApp.DefaultLanguage.GetResource("SECURITY.NOACCESS.ACCION"));
            //}
        }

        protected ProxyBusinessLogic Manager()
        {
            return new ProxyBusinessLogic(httpContextAccessor.GetToken(), config["Services:ServiceAddress"]);
        }

    }

    public static class IHttpContextAccessorExtension
    {
        public static string GetToken(this IHttpContextAccessor httpContextAccessor)
        {

            return httpContextAccessor.HttpContext.User.FindFirst("Token") == null ? "" : httpContextAccessor.HttpContext.User.FindFirst("Token").Value;
        }

    }
}
