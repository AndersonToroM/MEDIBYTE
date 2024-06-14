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

namespace Blazor.WebApp.Controllers
{

    [Authorize]
    public partial class AvisosInformativosController : BaseAppController
    {

        //private const string Prefix = "AvisosInformativos"; 

        public AvisosInformativosController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
        }

        #region Functions Master

        [HttpPost]
        public LoadResult Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<AvisosInformativos>().Tabla(true), loadOptions);
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

        private AvisosInformativosModel NewModel()
        {
            AvisosInformativosModel model = new AvisosInformativosModel();
            model.Entity.IsNew = true;
            model.Entity.MostrarHasta = DateTime.Now.AddDays(1);
            model.Entity.Activo = true;
            return model;
        }

        [HttpGet]
        public IActionResult Edit(long Id)
        {
            return PartialView("Edit", EditModel(Id));
        }

        private AvisosInformativosModel EditModel(long Id)
        {
            AvisosInformativosModel model = new AvisosInformativosModel();
            model.Entity = Manager().GetBusinessLogic<AvisosInformativos>().FindById(x => x.Id == Id, false);
            model.Entity.IsNew = false;
            return model;
        }

        [HttpPost]
        public IActionResult Edit(AvisosInformativosModel model)
        {
            return PartialView("Edit", EditModel(model));
        }

        private AvisosInformativosModel EditModel(AvisosInformativosModel model)
        {
            ViewBag.Accion = "Save";
            var existeAviso = Manager().GetBusinessLogic<AvisosInformativos>().Tabla().Where(x => x.Activo);
            if (!model.Entity.IsNew)
            {
                existeAviso = existeAviso.Where(x => x.Id != model.Entity.Id);
            }

            if (existeAviso.Any())
            {
                ModelState.AddModelError("Entity.Id", "Ya existe un aviso que esta activo.");
            }

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
                        model.Entity = Manager().GetBusinessLogic<AvisosInformativos>().Add(model.Entity);
                        model.Entity.IsNew = false;
                    }
                    else
                    {
                        model.Entity = Manager().GetBusinessLogic<AvisosInformativos>().Modify(model.Entity);
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
        public IActionResult Delete(AvisosInformativosModel model)
        {
            return PartialView("Edit", DeleteModel(model));
        }

        private AvisosInformativosModel DeleteModel(AvisosInformativosModel model)
        {
            ViewBag.Accion = "Delete";
            AvisosInformativosModel newModel = NewModel();
            if (ModelState.IsValid)
            {
                try
                {
                    model.Entity = Manager().GetBusinessLogic<AvisosInformativos>().FindById(x => x.Id == model.Entity.Id, false);
                    Manager().GetBusinessLogic<AvisosInformativos>().Remove(model.Entity);
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

        private AvisosInformativosModel NewModelDetail(long IdFather) 
        { 
            AvisosInformativosModel model = new AvisosInformativosModel(); 
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
        public IActionResult EditDetail(AvisosInformativosModel model)
        {
            return PartialView("EditDetail",EditModel(model));
        }

        [HttpPost]
        public IActionResult DeleteDetail(AvisosInformativosModel model)
        {
            return PartialView("EditDetail", DeleteModelDetail(model));
        }

        private AvisosInformativosModel DeleteModelDetail(AvisosInformativosModel model)
        { 
            ViewBag.Accion = "Delete"; 
            AvisosInformativosModel newModel = NewModelDetail(model.Entity.IdFather); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<AvisosInformativos>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<AvisosInformativos>().Remove(model.Entity); 
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
             AvisosInformativos entity = new AvisosInformativos(); 
             JsonConvert.PopulateObject(values, entity); 
             AvisosInformativosModel model = new AvisosInformativosModel(); 
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
             AvisosInformativos entity = Manager().GetBusinessLogic<AvisosInformativos>().FindById(x => x.Id == key, false); 
             JsonConvert.PopulateObject(values, entity); 
             AvisosInformativosModel model = new AvisosInformativosModel(); 
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
             AvisosInformativos entity = Manager().GetBusinessLogic<AvisosInformativos>().FindById(x => x.Id == key, false); 
             Manager().GetBusinessLogic<AvisosInformativos>().Remove(entity); 
        } 

        */
        #endregion

    }
}
