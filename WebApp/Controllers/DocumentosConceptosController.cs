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
    public partial class DocumentosConceptosController : BaseAppController
    {

        //private const string Prefix = "DocumentosConceptos"; 

        public DocumentosConceptosController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
        }

        #region Functions Master

        [HttpPost]
        public LoadResult Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<DocumentosConceptos>().Tabla(true), loadOptions);
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

        private DocumentosConceptosModel NewModel() 
        { 
            DocumentosConceptosModel model = new DocumentosConceptosModel();
            model.Entity.IsNew = true;
            return model; 
        } 

        [HttpGet]
        public IActionResult Edit(long Id)
        {
            return PartialView("Edit", EditModel(Id));
        }

        private DocumentosConceptosModel EditModel(long Id) 
        { 
            DocumentosConceptosModel model = new DocumentosConceptosModel();
            model.Entity = Manager().GetBusinessLogic<DocumentosConceptos>().FindById(x => x.Id == Id, false);
            model.Entity.IsNew = false;
            return model; 
        } 

        [HttpPost]
        public IActionResult Edit(DocumentosConceptosModel model)
        {
            return PartialView("Edit",EditModel(model));
        }

        private DocumentosConceptosModel EditModel(DocumentosConceptosModel model) 
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
                        model.Entity = Manager().GetBusinessLogic<DocumentosConceptos>().Add(model.Entity); 
                        model.Entity.IsNew = false;
                    } 
                    else 
                    { 
                        model.Entity = Manager().GetBusinessLogic<DocumentosConceptos>().Modify(model.Entity); 
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
        public IActionResult Delete(DocumentosConceptosModel model)
        {
            return PartialView("Edit", DeleteModel(model));
        }

        private DocumentosConceptosModel DeleteModel(DocumentosConceptosModel model)
        { 
            ViewBag.Accion = "Delete"; 
            DocumentosConceptosModel newModel = NewModel(); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<DocumentosConceptos>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<DocumentosConceptos>().Remove(model.Entity); 
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

        private DocumentosConceptosModel NewModelDetail(long IdFather) 
        { 
            DocumentosConceptosModel model = new DocumentosConceptosModel(); 
            model.Entity.DocumentosId = IdFather; 
            model.Entity.IsNew = true; 
            return model; 
        } 

        [HttpGet]
        public IActionResult EditDetail(long Id)
        {
            return PartialView("EditDetail", EditModel(Id));
        }

        [HttpPost]
        public IActionResult EditDetail(DocumentosConceptosModel model)
        {
            return PartialView("EditDetail",EditModel(model));
        }

        [HttpPost]
        public IActionResult DeleteDetail(DocumentosConceptosModel model)
        {
            return PartialView("EditDetail", DeleteModelDetail(model));
        }

        private DocumentosConceptosModel DeleteModelDetail(DocumentosConceptosModel model)
        { 
            ViewBag.Accion = "Delete"; 
            DocumentosConceptosModel newModel = NewModelDetail(model.Entity.DocumentosId); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<DocumentosConceptos>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<DocumentosConceptos>().Remove(model.Entity); 
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
             DocumentosConceptos entity = new DocumentosConceptos(); 
             JsonConvert.PopulateObject(values, entity); 
             DocumentosConceptosModel model = new DocumentosConceptosModel(); 
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
             DocumentosConceptos entity = Manager().GetBusinessLogic<DocumentosConceptos>().FindById(x => x.Id == key, false); 
             JsonConvert.PopulateObject(values, entity); 
             DocumentosConceptosModel model = new DocumentosConceptosModel(); 
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
             DocumentosConceptos entity = Manager().GetBusinessLogic<DocumentosConceptos>().FindById(x => x.Id == key, false); 
             Manager().GetBusinessLogic<DocumentosConceptos>().Remove(entity); 
        } 

        #endregion 

        #region Datasource Combobox Foraneos 

        [HttpPost]
        public LoadResult GetDocumentosId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Documentos>().Tabla(true), loadOptions);
        } 
        [HttpPost]
        public LoadResult GetEstadosConceptoId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Estados>().Tabla(true).Where(x=>x.Tipo.Equals("ADMISIONPAGO") && x.Id != 63), loadOptions);
        } 
       #endregion

    }
}
