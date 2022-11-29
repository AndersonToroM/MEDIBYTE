using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Dominus.Frontend.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseAppController : Controller
    {
        protected IHttpContextAccessor httpContextAccessor;

        protected IConfiguration config;

        public BaseAppController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this.httpContextAccessor = httpContextAccessor;

        }

        //protected DataBaseSetting ActualConexion()
        //{
        //    return DApp.DataBaseSettings.FirstOrDefault(x => x.Name.ToString() == httpContextAccessor.HttpContext.User.FindFirst("ConnectionName").Value);
        //}

        protected BusinessLogic Manager()
        {
            var conexion = DApp.GetTenantConnection(httpContextAccessor.HttpContext.Request.Host.Value);
            return new BusinessLogic(conexion);
        }

        protected string ActualToken()
        {
            return httpContextAccessor.HttpContext.User.FindFirst("Token") == null ? "" : httpContextAccessor.HttpContext.User.FindFirst("Token").Value;
        }

        protected long ActualUsuarioId()
        {
            return httpContextAccessor.HttpContext.User.FindFirst("UserId") == null ? 0 : long.Parse(httpContextAccessor.HttpContext.User.FindFirst("UserId").Value);
        }

        protected long ActualEmpresaId()
        {
            return httpContextAccessor.HttpContext.User.FindFirst("EmpresaId") == null ? 0 : long.Parse(httpContextAccessor.HttpContext.User.FindFirst("EmpresaId").Value);
        }
        protected long ActualEntidadId()
        {
            return httpContextAccessor.HttpContext.User.FindFirst("EntidadId") == null ? 0 : long.Parse(httpContextAccessor.HttpContext.User.FindFirst("EntidadId").Value);
        }
    }

}
