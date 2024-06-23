using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Dominus.Frontend.Controllers;
using Blazor.BusinessLogic;
using Blazor.Infrastructure.Entities;
using Blazor.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace Blazor.WebApp.Controllers
{

    [Authorize]
    public partial class MotivoIncapacidadRetroactivaController : BaseAppController
    {

        //private const string Prefix = "MotivoIncapacidadRetroactiva"; 

        public MotivoIncapacidadRetroactivaController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
        }

        #region Functions Master

        [HttpPost]
        public LoadResult Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<MotivoIncapacidadRetroactiva>().Tabla(true), loadOptions);
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

        private MotivoIncapacidadRetroactivaModel NewModel()
        {
            MotivoIncapacidadRetroactivaModel model = new MotivoIncapacidadRetroactivaModel();
            model.Entity.IsNew = true;
            return model;
        }

        [HttpGet]
        public IActionResult Edit(long Id)
        {
            return PartialView("Edit", EditModel(Id));
        }

        private MotivoIncapacidadRetroactivaModel EditModel(long Id)
        {
            MotivoIncapacidadRetroactivaModel model = new MotivoIncapacidadRetroactivaModel();
            model.Entity = Manager().GetBusinessLogic<MotivoIncapacidadRetroactiva>().FindById(x => x.Id == Id, false);
            model.Entity.IsNew = false;
            return model;
        }

        [HttpPost]
        public IActionResult Edit(MotivoIncapacidadRetroactivaModel model)
        {
            return PartialView("Edit", EditModel(model));
        }

        private MotivoIncapacidadRetroactivaModel EditModel(MotivoIncapacidadRetroactivaModel model)
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
                        model.Entity = Manager().GetBusinessLogic<MotivoIncapacidadRetroactiva>().Add(model.Entity);
                        model.Entity.IsNew = false;
                    }
                    else
                    {
                        model.Entity = Manager().GetBusinessLogic<MotivoIncapacidadRetroactiva>().Modify(model.Entity);
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Entity.Id", e.GetFrontFullErrorMessage());
                }
            }
            else
            {
                ModelState.AddModelError("Entity.Id", "Error de codigo, el objeto a guardar tiene campos diferentes a los de la entidad.");
            }
            return model;
        }

        [HttpPost]
        public IActionResult Delete(MotivoIncapacidadRetroactivaModel model)
        {
            return PartialView("Edit", DeleteModel(model));
        }

        private MotivoIncapacidadRetroactivaModel DeleteModel(MotivoIncapacidadRetroactivaModel model)
        {
            ViewBag.Accion = "Delete";
            MotivoIncapacidadRetroactivaModel newModel = NewModel();
            if (ModelState.IsValid)
            {
                try
                {
                    model.Entity = Manager().GetBusinessLogic<MotivoIncapacidadRetroactiva>().FindById(x => x.Id == model.Entity.Id, false);
                    Manager().GetBusinessLogic<MotivoIncapacidadRetroactiva>().Remove(model.Entity);
                    return newModel;
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Entity.Id", e.GetFrontFullErrorMessage());
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

        private MotivoIncapacidadRetroactivaModel NewModelDetail(long IdFather) 
        { 
            MotivoIncapacidadRetroactivaModel model = new MotivoIncapacidadRetroactivaModel(); 
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
        public IActionResult EditDetail(MotivoIncapacidadRetroactivaModel model)
        {
            return PartialView("EditDetail",EditModel(model));
        }

        [HttpPost]
        public IActionResult DeleteDetail(MotivoIncapacidadRetroactivaModel model)
        {
            return PartialView("EditDetail", DeleteModelDetail(model));
        }

        private MotivoIncapacidadRetroactivaModel DeleteModelDetail(MotivoIncapacidadRetroactivaModel model)
        { 
            ViewBag.Accion = "Delete"; 
            MotivoIncapacidadRetroactivaModel newModel = NewModelDetail(model.Entity.IdFather); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<MotivoIncapacidadRetroactiva>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<MotivoIncapacidadRetroactiva>().Remove(model.Entity); 
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
             MotivoIncapacidadRetroactiva entity = new MotivoIncapacidadRetroactiva(); 
             JsonConvert.PopulateObject(values, entity); 
             MotivoIncapacidadRetroactivaModel model = new MotivoIncapacidadRetroactivaModel(); 
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
             MotivoIncapacidadRetroactiva entity = Manager().GetBusinessLogic<MotivoIncapacidadRetroactiva>().FindById(x => x.Id == key, false); 
             JsonConvert.PopulateObject(values, entity); 
             MotivoIncapacidadRetroactivaModel model = new MotivoIncapacidadRetroactivaModel(); 
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
             MotivoIncapacidadRetroactiva entity = Manager().GetBusinessLogic<MotivoIncapacidadRetroactiva>().FindById(x => x.Id == key, false); 
             Manager().GetBusinessLogic<MotivoIncapacidadRetroactiva>().Remove(entity); 
        } 

        */
        #endregion

    }
}
