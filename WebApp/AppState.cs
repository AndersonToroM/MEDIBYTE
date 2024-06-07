using Blazor.Infrastructure.Entities;
using Dominus.Backend.Application;
using WidgetGallery;
using Dominus.Frontend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Blazor.BusinessLogic;
using System.Linq;

namespace Blazor.WebApp
{

    [Authorize]
    public class AppState : BaseAppController
    {
        public AppState(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
            SetUser(this.ActualUsuarioId());
            SetEmpresa(this.ActualEmpresaId());
        }

        #region Empresa

        public Empresas Empresa { get; set; }

        private void SetEmpresa(long id)
        {
            if (id > 0)
            {
                Empresa = Manager().GetBusinessLogic<Empresas>().FindById(x => x.Id == id, true);
            }
            if (Empresa == null)
            {
                Empresa = new Empresas();
                Empresa.RazonSocial = (Empresa.RazonSocial ?? "Empresa sin Razon Social");
            }

        }
        #endregion

        #region Usuario

        public User Usuario { get; set; }

        private void SetUser(long id)
        {
            if (id > 0)
                Usuario = Manager().GetBusinessLogic<User>().FindById(x => x.Id == id, true);

            if (Usuario == null)
            {
                Usuario = new User();
                Usuario.Names = (Usuario.Names ?? "Usuario sin nombre");
                Usuario.LastNames = (Usuario.LastNames ?? "Usuario sin apellidos");
                Usuario.Email = (Usuario.Email ?? "Usuario sin email");
            }
        }

        #endregion
    }


}
