using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Dominus.Frontend.Controllers
{
    public class BaseReportController : Controller
    {
        protected IHttpContextAccessor httpContextAccessor;

        protected IConfiguration config;

        public BaseReportController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this.httpContextAccessor = httpContextAccessor;

        }

        //protected DataBaseSetting CurrentConnection()
        //{
        //    return httpContextAccessor.HttpContext.User.FindFirst("ConnectionName")!=null ? (DApp.DataBaseSettings.FirstOrDefault(x => x.Name.ToString() == httpContextAccessor.HttpContext.User.FindFirst("ConnectionName").Value)) : (DApp.DataBaseSettings[0]);
        //}


        protected BusinessLogic Manager()
        {
            var conexion = DApp.GetTenantConnection(Request.Host.Value);
            return new BusinessLogic(conexion);
        }

        protected string GetCurrentUser()
        {
            return httpContextAccessor.HttpContext.User.FindFirst("Name") == null ? "" : httpContextAccessor.HttpContext.User.FindFirst("Name").Value;
        }

        protected string GetCurrentUserId()
        {
            return httpContextAccessor.HttpContext.User.FindFirst("UserId") == null ? "" : httpContextAccessor.HttpContext.User.FindFirst("UserId").Value;
        }

        protected string GetCurrentToken()
        {
            return httpContextAccessor.HttpContext.User.FindFirst("Token") == null ? "" : httpContextAccessor.HttpContext.User.FindFirst("Token").Value;
        }

        protected string GetCurrentOfficeId()
        {
            return httpContextAccessor.HttpContext.User.FindFirst("OfficeId") == null ? "" : httpContextAccessor.HttpContext.User.FindFirst("OfficeId").Value;
        }
    }

}
