using DevExtreme.AspNet.Data;

using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Dominus.Frontend.Controllers;
using Blazor.Infrastructure.Entities;
using Blazor.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using Blazor.BusinessLogic;
using System.Linq;
using System.Collections.Generic;
using Dominus.Backend.Application;
using Dominus.Backend.Application;

namespace Blazor.WebApp.Controllers
{

    [Authorize] 
    public partial class ServiciosController : BaseAppController
    {

        //private const string Prefix = "Servicios"; 

        public ServiciosController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
        }

        #region Functions Master

        [HttpPost]
        public LoadResult Get(DataSourceLoadOptions loadOptions)
        {
            List<string> codes = new List<string> { "COP", "CM", "CR", "PC" };
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Servicios>().Tabla(true).Where(x=> !codes.Contains(x.Codigo)), loadOptions);
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
        public IActionResult New()
        {
            return PartialView("Edit", NewModel());
        }

        private ServiciosModel NewModel() 
        { 
            ServiciosModel model = new ServiciosModel();
            model.Entity.IsNew = true;
            model.Entity.EmpresasId = this.ActualEmpresaId();
            var impuesto = Manager().GetBusinessLogic<EsquemasImpuestos>().FindById(x => x.Id == 17, false);
            if (true)
            {
                model.Entity.TiposImpuestosId = impuesto.Id;
            }
            else
            {
                throw new Exception("El esquema de impuesto número 17 (ZZ - No Aplica) no se encuentra registrado en el sistema. Por favor, comunicarse con su administrador.");
            }
            model.Entity.EstadosIdTipoDuracion = 10081;

            return model; 
        } 

        [HttpGet]
        public IActionResult Edit(long Id)
        {
            return PartialView("Edit", EditModel(Id));
        }

        private ServiciosModel EditModel(long Id) 
        { 
            ServiciosModel model = new ServiciosModel();
            model.Entity = Manager().GetBusinessLogic<Servicios>().FindById(x => x.Id == Id, false);
            model.Entity.IsNew = false;
            return model; 
        } 

        [HttpPost]
        public IActionResult Edit(ServiciosModel model)
        {
            return PartialView("Edit",EditModel(model));
        }

        private ServiciosModel EditModel(ServiciosModel model) 
        {
            int maximoDuracionHoras = 168;
            int maximoDuracionMinutos = 10080;

            if(model.Entity.EstadosIdTipoDuracion == 10080 && model.Entity.Duracion > maximoDuracionHoras)
                ModelState.AddModelError("Entity.Id", $"El servicio no puede superar las {maximoDuracionHoras} horas.");
            if (model.Entity.EstadosIdTipoDuracion == 10081 && model.Entity.Duracion > maximoDuracionMinutos)
                ModelState.AddModelError("Entity.Id", $"El servicio no puede superar los {maximoDuracionMinutos} minutos.");

            ViewBag.Accion = "Save"; 
            var OnState = model.Entity.IsNew; 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity.LastUpdate = DateTime.Now; 
                    model.Entity.UpdatedBy = User.Identity.Name;

                    model.Entity.Nombre = DApp.Util.QuitarEspacios(model.Entity.Nombre);
                    model.Entity.CodigoInterno = DApp.Util.QuitarEspacios(model.Entity.CodigoInterno);

                    if (model.Entity.IsNew) 
                    { 
                        model.Entity.CreationDate = DateTime.Now; 
                        model.Entity.CreatedBy = User.Identity.Name; 
                        model.Entity = Manager().GetBusinessLogic<Servicios>().Add(model.Entity); 
                        model.Entity.IsNew = false;
                    } 
                    else 
                    { 
                        model.Entity = Manager().GetBusinessLogic<Servicios>().Modify(model.Entity); 
                    } 
                } 
                catch (Exception e) 
                { 
                    ModelState.AddModelError("Entity.Id", e.GetFullErrorMessage());
                } 
            }
            else
            {
                ModelState.AddModelError("Entity.Id", $"Error en vista, diferencia con base de datos. | " + ModelState.GetModelFullError());
            }
            return model; 
        } 

        [HttpPost]
        public IActionResult Delete(ServiciosModel model)
        {
            return PartialView("Edit", DeleteModel(model));
        }

        private ServiciosModel DeleteModel(ServiciosModel model)
        { 
            ViewBag.Accion = "Delete"; 
            ServiciosModel newModel = NewModel(); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<Servicios>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<Servicios>().Remove(model.Entity); 
                    return newModel;
                } 
                catch (Exception e) 
                { 
                    ModelState.AddModelError("Entity.Id", e.GetFullErrorMessage());
                } 
            } 
            return model; 
        } 

        #endregion 

        #region Functions Detail 

        /*
        [HttpGet]
        public IActionResult NewDetail(long IdFather)
        {
            return PartialView("EditDetail", NewModelDetail(IdFather));
        }

        private ServiciosModel NewModelDetail(long IdFather) 
        { 
            ServiciosModel model = new ServiciosModel(); 
            model.Entity.IdFather = IdFather; 
            model.Entity.IsNew = true; 
            return model; 
        } 

        [HttpGet]
        public IActionResult EditDetail(long Id)
        {
            return PartialView("EditDetail", EditModel(Id));
        }

        [HttpPost]
        public IActionResult EditDetail(ServiciosModel model)
        {
            return PartialView("EditDetail",EditModel(model));
        }

        [HttpPost]
        public IActionResult DeleteDetail(ServiciosModel model)
        {
            return PartialView("EditDetail", DeleteModelDetail(model));
        }

        private ServiciosModel DeleteModelDetail(ServiciosModel model)
        { 
            ViewBag.Accion = "Delete"; 
            ServiciosModel newModel = NewModelDetail(model.Entity.IdFather); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<Servicios>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<Servicios>().Remove(model.Entity); 
                    return newModel;
                } 
                catch (Exception e) 
                { 
                    ModelState.AddModelError("Entity.Id", e.GetFullErrorMessage());
                } 
            } 
            return model; 
        } 
        */

        #endregion 

        #region Datasource Combobox Foraneos 

        [HttpPost]
        public LoadResult GetCategoriasServiciosId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<CategoriasServicios>().Tabla(true), loadOptions);
        } 
        [HttpPost]
        public LoadResult GetCupsId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Cups>().Tabla(true), loadOptions);
        } 
        [HttpPost]
        public LoadResult GetEspecialidadesId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Especialidades>().Tabla(true), loadOptions);
        }
        [HttpPost]
        public LoadResult GetGrupoServciosRipsId(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<GrupoServciosRips>().Tabla(true), loadOptions);
        }
        [HttpPost]
        public LoadResult GetHabilitacionServciosRipsId(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<HabilitacionServciosRips>().Tabla(true), loadOptions);
        }
        [HttpPost]
        public LoadResult GetEstadosId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Estados>().Tabla(true).Where(x=>x.Tipo == "SERVICIOS"), loadOptions);
        } 
        [HttpPost]
        public LoadResult GetTiposServiciosId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<TiposServicios>().Tabla(true), loadOptions);
        }
        [HttpPost]
        public LoadResult GetEstadosIdTipoDuracion(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Estados>().Tabla(true).Where(x => x.Tipo == "SERVICIOSDURACION"), loadOptions);
        }
        #endregion


    }
}
