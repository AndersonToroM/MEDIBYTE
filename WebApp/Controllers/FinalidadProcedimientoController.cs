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
using Dominus.Backend.Application;

namespace Blazor.WebApp.Controllers
{

    [Authorize] 
    public partial class FinalidadProcedimientoController : BaseAppController
    {

        //private const string Prefix = "FinalidadProcedimiento"; 

        public FinalidadProcedimientoController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
        }

        #region Functions Master

        [HttpPost]
        public LoadResult Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<FinalidadProcedimiento>().Tabla(true), loadOptions);
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

        private FinalidadProcedimientoModel NewModel() 
        { 
            FinalidadProcedimientoModel model = new FinalidadProcedimientoModel();
            model.Entity.IsNew = true;
            return model; 
        } 

        [HttpGet]
        public IActionResult Edit(long Id)
        {
            return PartialView("Edit", EditModel(Id));
        }

        private FinalidadProcedimientoModel EditModel(long Id) 
        { 
            FinalidadProcedimientoModel model = new FinalidadProcedimientoModel();
            model.Entity = Manager().GetBusinessLogic<FinalidadProcedimiento>().FindById(x => x.Id == Id, false);
            model.Entity.IsNew = false;
            return model; 
        } 

        [HttpPost]
        public IActionResult Edit(FinalidadProcedimientoModel model)
        {
            return PartialView("Edit",EditModel(model));
        }

        private FinalidadProcedimientoModel EditModel(FinalidadProcedimientoModel model) 
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
                        model.Entity = Manager().GetBusinessLogic<FinalidadProcedimiento>().Add(model.Entity); 
                        model.Entity.IsNew = false;
                    } 
                    else 
                    { 
                        model.Entity = Manager().GetBusinessLogic<FinalidadProcedimiento>().Modify(model.Entity); 
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
        public IActionResult Delete(FinalidadProcedimientoModel model)
        {
            return PartialView("Edit", DeleteModel(model));
        }

        private FinalidadProcedimientoModel DeleteModel(FinalidadProcedimientoModel model)
        { 
            ViewBag.Accion = "Delete"; 
            FinalidadProcedimientoModel newModel = NewModel(); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<FinalidadProcedimiento>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<FinalidadProcedimiento>().Remove(model.Entity); 
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

        private FinalidadProcedimientoModel NewModelDetail(long IdFather) 
        { 
            FinalidadProcedimientoModel model = new FinalidadProcedimientoModel(); 
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
        public IActionResult EditDetail(FinalidadProcedimientoModel model)
        {
            return PartialView("EditDetail",EditModel(model));
        }

        [HttpPost]
        public IActionResult DeleteDetail(FinalidadProcedimientoModel model)
        {
            return PartialView("EditDetail", DeleteModelDetail(model));
        }

        private FinalidadProcedimientoModel DeleteModelDetail(FinalidadProcedimientoModel model)
        { 
            ViewBag.Accion = "Delete"; 
            FinalidadProcedimientoModel newModel = NewModelDetail(model.Entity.IdFather); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<FinalidadProcedimiento>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<FinalidadProcedimiento>().Remove(model.Entity); 
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
             FinalidadProcedimiento entity = new FinalidadProcedimiento(); 
             JsonConvert.PopulateObject(values, entity); 
             FinalidadProcedimientoModel model = new FinalidadProcedimientoModel(); 
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
             FinalidadProcedimiento entity = Manager().GetBusinessLogic<FinalidadProcedimiento>().FindById(x => x.Id == key, false); 
             JsonConvert.PopulateObject(values, entity); 
             FinalidadProcedimientoModel model = new FinalidadProcedimientoModel(); 
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
             FinalidadProcedimiento entity = Manager().GetBusinessLogic<FinalidadProcedimiento>().FindById(x => x.Id == key, false); 
             Manager().GetBusinessLogic<FinalidadProcedimiento>().Remove(entity); 
        } 

        */
        #endregion 

    }
}
