﻿using Blazor.BusinessLogic;
using Blazor.Infrastructure;
using Blazor.Infrastructure.Entities;
using Blazor.Infrastructure.Models;
using Blazor.WebApp.Models;
using DevExpress.XtraReports.UI;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Dominus.Backend.Application;
using Dominus.Frontend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Reportes.AtencionNotaProcedimientos;
using WidgetGallery;
namespace WebApp.Controllers
{
    [Authorize]
    public class NotasProcedimientosController : BaseAppController
    {
        public NotasProcedimientosController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
        }

        [HttpPost]
        public LoadResult Get(DataSourceLoadOptions loadOptions)
        {
            IQueryable<Atenciones> result = Manager().GetBusinessLogic<Atenciones>().Tabla(true)
                .Include(x => x.Admisiones.Pacientes)
                .Include(x => x.Admisiones.Estados)
                .Include(x => x.Admisiones.Convenios)
                .Include(x => x.Admisiones.Diagnosticos)
                .Include(x => x.Admisiones.Filiales)
                .Include(x => x.Admisiones.ProgramacionCitas.Consultorios)
                .Include(x => x.Admisiones.ProgramacionCitas.Sedes)
                .Include(x => x.Admisiones.ProgramacionCitas.Entidades)
                .Include(x => x.Admisiones.ProgramacionCitas.TiposCitas)
                .Include(x => x.Admisiones.ProgramacionCitas.Servicios)
                .Include(x => x.Admisiones.ProgramacionCitas.Servicios.TiposServicios)
                .Where(x => x.Admisiones.ProgramacionCitas.Servicios.TiposServicios.Nombre.Equals("Procedimiento"))
                ;

            return DataSourceLoader.Load(result, loadOptions);
        }

        public IActionResult List()
        {
            return View("List");
        }

        public IActionResult ListPartial()
        {
            return PartialView("List");
        }

        [HttpGet]
        public IActionResult Edit(long Id)
        {
            return PartialView("Edit", EditModel(Id));
        }

        private AtencionesModel EditModel(long Id)
        {
            AtencionesModel model = new AtencionesModel();
            model.Entity = Manager().GetBusinessLogic<Atenciones>().Tabla()
                .Include(x => x.Admisiones)
                .Include(x => x.Admisiones.Pacientes)
                .Include(x => x.Admisiones.Pacientes.Generos)
                .Include(x => x.Admisiones.Pacientes.TiposIdentificacion)
                .Include(x => x.Admisiones.ProgramacionCitas.Entidades)
                .FirstOrDefault(x => x.Id == Id);
            model.NombreEntidad = model.Entity.Admisiones.ProgramacionCitas?.Entidades?.Nombre;
            model.Entity.IsNew = false;
            return model;
        }


        [HttpPost]
        public IActionResult ImprimirListNotaProcedimiento(List<long> atencionesId)
        {
            try
            {
                InformacionReporte informacionReporte = new InformacionReporte();
                informacionReporte.Empresa = Manager().GetBusinessLogic<Empresas>().FindById(x => x.Id == this.ActualEmpresaId(), true);
                informacionReporte.BD = DApp.GetTenantConnection(Request.Host.Value);
                informacionReporte.Ids = atencionesId.ToArray();

                AtencionNotaProcedimientosReporte report = new AtencionNotaProcedimientosReporte();
                report.SetInformacionReporte(informacionReporte);
                XtraReport xtraReport = report;
                return PartialView("_ViewerReport", report);

            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

    }
}