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
using System.Linq;
using Newtonsoft.Json;
using Blazor.BusinessLogic;
using Microsoft.EntityFrameworkCore;
using Dominus.Backend.Application;

namespace Blazor.WebApp.Controllers
{

    [Authorize] 
    public partial class EntregaResultadosNoLecturaDetallesController : BaseAppController
    {

        //private const string Prefix = "EntregaResultadosNoLecturaDetalles"; 

        public EntregaResultadosNoLecturaDetallesController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
        }

        #region Functions Master

        [HttpPost]
        public LoadResult Get(DataSourceLoadOptions loadOptions)
        {
            var result = Manager().GetBusinessLogic<EntregaResultadosNoLecturaDetalles>().Tabla(true)
                .Include(x => x.AdmisionesServiciosPrestados.Servicios)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones);
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
        public IActionResult New()
        {
            return PartialView("Edit", NewModel());
        }

        private EntregaResultadosNoLecturaDetallesModel NewModel() 
        { 
            EntregaResultadosNoLecturaDetallesModel model = new EntregaResultadosNoLecturaDetallesModel();
            model.Entity.IsNew = true;
            return model; 
        } 

        [HttpGet]
        public IActionResult Edit(long Id)
        {
            return PartialView("Edit", EditModel(Id));
        }

        private EntregaResultadosNoLecturaDetallesModel EditModel(long Id) 
        { 
            EntregaResultadosNoLecturaDetallesModel model = new EntregaResultadosNoLecturaDetallesModel();
            model.Entity = Manager().GetBusinessLogic<EntregaResultadosNoLecturaDetalles>().FindById(x => x.Id == Id, false);
            model.Entity.IsNew = false;
            return model; 
        } 

        [HttpPost]
        public IActionResult Edit(EntregaResultadosNoLecturaDetallesModel model)
        {
            return PartialView("Edit",EditModel(model));
        }

        private EntregaResultadosNoLecturaDetallesModel EditModel(EntregaResultadosNoLecturaDetallesModel model) 
        { 
            ViewBag.Accion = "Save"; 
            var OnState = model.Entity.IsNew; 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity.LastUpdate = DateTime.Now; 
                    model.Entity.UpdatedBy = User.Identity.Name; 
                    if (model.Entity.IsNew) 
                    { 
                        model.Entity.CreationDate = DateTime.Now; 
                        model.Entity.CreatedBy = User.Identity.Name; 
                        model.Entity = Manager().GetBusinessLogic<EntregaResultadosNoLecturaDetalles>().Add(model.Entity); 
                        model.Entity.IsNew = false;
                    } 
                    else 
                    { 
                        model.Entity = Manager().GetBusinessLogic<EntregaResultadosNoLecturaDetalles>().Modify(model.Entity); 
                    } 
                } 
                catch (Exception e) 
                { 
                    ModelState.AddModelError("Entity.Id", e.GetFullErrorMessage()); 
                } 
            } 
            else 
            { 
                 ModelState.AddModelError("Entity.Id", "Error de codigo, el objeto a guardar tiene campos diferentes a los de la entidad."); 
            } 
            return model; 
        } 

        [HttpPost]
        public IActionResult Delete(EntregaResultadosNoLecturaDetallesModel model)
        {
            return PartialView("Edit", DeleteModel(model));
        }

        private EntregaResultadosNoLecturaDetallesModel DeleteModel(EntregaResultadosNoLecturaDetallesModel model)
        { 
            ViewBag.Accion = "Delete"; 
            EntregaResultadosNoLecturaDetallesModel newModel = NewModel(); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<EntregaResultadosNoLecturaDetalles>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<EntregaResultadosNoLecturaDetalles>().Remove(model.Entity); 
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

        [HttpGet]
        public IActionResult NewDetail(long IdFather)
        {
            return PartialView("EditDetail", NewModelDetail(IdFather));
        }

        private EntregaResultadosNoLecturaDetallesModel NewModelDetail(long IdFather) 
        { 
            EntregaResultadosNoLecturaDetallesModel model = new EntregaResultadosNoLecturaDetallesModel(); 
            model.Entity.EntregaResultadosNoLecturaId = IdFather; 
            model.Entity.IsNew = true; 
            return model; 
        } 

        [HttpGet]
        public IActionResult EditDetail(long Id)
        {
            return PartialView("EditDetail", EditModel(Id));
        }

        [HttpPost]
        public IActionResult EditDetail(EntregaResultadosNoLecturaDetallesModel model)
        {
            return PartialView("EditDetail",EditModel(model));
        }

        [HttpPost]
        public IActionResult DeleteDetail(EntregaResultadosNoLecturaDetallesModel model)
        {
            return PartialView("EditDetail", DeleteModelDetail(model));
        }

        private EntregaResultadosNoLecturaDetallesModel DeleteModelDetail(EntregaResultadosNoLecturaDetallesModel model)
        { 
            ViewBag.Accion = "Delete"; 
            EntregaResultadosNoLecturaDetallesModel newModel = NewModelDetail(model.Entity.EntregaResultadosNoLecturaId); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<EntregaResultadosNoLecturaDetalles>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<EntregaResultadosNoLecturaDetalles>().Remove(model.Entity); 
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

        #region Funcions Detail Edit in Grid 

        [HttpPost] 
        public IActionResult AddInGrid(string values) 
        { 
             EntregaResultadosNoLecturaDetalles entity = new EntregaResultadosNoLecturaDetalles(); 
             JsonConvert.PopulateObject(values, entity); 
             EntregaResultadosNoLecturaDetallesModel model = new EntregaResultadosNoLecturaDetallesModel(); 
             model.Entity = entity; 
             model.Entity.IsNew = true; 
             this.EditModel(model); 
             if(ModelState.IsValid) 
                 return Ok(ModelState); 
             else 
                 return BadRequest(ModelState); 
        } 

        [HttpPost] 
        public IActionResult ModifyInGrid(int key, string values) 
        { 
             EntregaResultadosNoLecturaDetalles entity = Manager().GetBusinessLogic<EntregaResultadosNoLecturaDetalles>().FindById(x => x.Id == key, false); 
             JsonConvert.PopulateObject(values, entity); 
             EntregaResultadosNoLecturaDetallesModel model = new EntregaResultadosNoLecturaDetallesModel(); 
             model.Entity = entity; 
             model.Entity.IsNew = false; 
             this.EditModel(model); 
             if(ModelState.IsValid) 
                 return Ok(ModelState); 
             else 
                 return BadRequest(ModelState); 
        } 

        [HttpPost]
        public void DeleteInGrid(int key)
        { 
             EntregaResultadosNoLecturaDetalles entity = Manager().GetBusinessLogic<EntregaResultadosNoLecturaDetalles>().FindById(x => x.Id == key, false); 
             Manager().GetBusinessLogic<EntregaResultadosNoLecturaDetalles>().Remove(entity); 
        } 

        #endregion 

        #region Datasource Combobox Foraneos 

        [HttpPost]
        public LoadResult GetAdmisionesServiciosPrestadosId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<AdmisionesServiciosPrestados>().Tabla(true), loadOptions);
        } 
        [HttpPost]
        public LoadResult GetEntregaResultadosNoLecturaId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<EntregaResultadosNoLectura>().Tabla(true), loadOptions);
        } 
       #endregion

    }
}
