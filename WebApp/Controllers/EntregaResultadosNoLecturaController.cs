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
using System.Collections.Generic;

namespace Blazor.WebApp.Controllers
{

    [Authorize] 
    public partial class EntregaResultadosNoLecturaController : BaseAppController
    {

        //private const string Prefix = "EntregaResultadosNoLectura"; 

        public EntregaResultadosNoLecturaController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
        }

        #region Functions Master

        [HttpPost]
        public LoadResult Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<EntregaResultadosNoLectura>().Tabla(true), loadOptions);
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

        private EntregaResultadosNoLecturaModel NewModel() 
        { 
            EntregaResultadosNoLecturaModel model = new EntregaResultadosNoLecturaModel();
            model.Entity.IsNew = true;
            model.Entity.Fecha = DateTime.Now;
            model.Hora = model.Entity.Fecha;
            return model; 
        } 

        [HttpGet]
        public IActionResult Edit(long Id)
        {
            return PartialView("Edit", EditModel(Id));
        }

        private EntregaResultadosNoLecturaModel EditModel(long Id) 
        { 
            EntregaResultadosNoLecturaModel model = new EntregaResultadosNoLecturaModel();
            model.Entity = Manager().GetBusinessLogic<EntregaResultadosNoLectura>().FindById(x => x.Id == Id, true);
            model.Entity.IsNew = false;
            model.Hora = model.Entity.Fecha;

            if (model.Entity.ContanciaArchivos == null)
                model.Entity.ContanciaArchivos = new Archivos();
            else
                model.Entity.ContanciaArchivos.StringToBase64 = DApp.Util.ArrayBytesToString(model.Entity.ContanciaArchivos.Archivo);

            return model; 
        } 

        [HttpPost]
        public IActionResult Edit(EntregaResultadosNoLecturaModel model)
        {
            return PartialView("Edit",EditModel(model));
        }

        private EntregaResultadosNoLecturaModel EditModel(EntregaResultadosNoLecturaModel model) 
        { 
            ViewBag.Accion = "Save";

            if (model.Entity.IsNew)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(model.SerializedResultados))
                    {
                        model.Entity.ListAdmisionesServiciosPrestadosId = JsonConvert.DeserializeObject<List<long>>(model.SerializedResultados);
                    }

                    if ((model.Entity.ListAdmisionesServiciosPrestadosId == null || model.Entity.ListAdmisionesServiciosPrestadosId.Count <= 0))
                    {
                        ModelState.AddModelError("Entity.Id", DApp.DefaultLanguage.GetResource("EntregaResultados.SeleccionNula"));
                    }

                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Entity.Id", "Error des-serializando tablas de resultados. | " + e.Message);
                }
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
                        model.Entity = Manager().GetBusinessLogic<EntregaResultadosNoLectura>().Add(model.Entity); 
                        model.Entity.IsNew = false;
                    } 
                    else 
                    { 
                        model.Entity = Manager().GetBusinessLogic<EntregaResultadosNoLectura>().Modify(model.Entity); 
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
        public IActionResult Delete(EntregaResultadosNoLecturaModel model)
        {
            return PartialView("Edit", DeleteModel(model));
        }

        private EntregaResultadosNoLecturaModel DeleteModel(EntregaResultadosNoLecturaModel model)
        { 
            ViewBag.Accion = "Delete"; 
            EntregaResultadosNoLecturaModel newModel = NewModel(); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<EntregaResultadosNoLectura>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<EntregaResultadosNoLectura>().Remove(model.Entity); 
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

        private EntregaResultadosNoLecturaModel NewModelDetail(long IdFather) 
        { 
            EntregaResultadosNoLecturaModel model = new EntregaResultadosNoLecturaModel(); 
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
        public IActionResult EditDetail(EntregaResultadosNoLecturaModel model)
        {
            return PartialView("EditDetail",EditModel(model));
        }

        [HttpPost]
        public IActionResult DeleteDetail(EntregaResultadosNoLecturaModel model)
        {
            return PartialView("EditDetail", DeleteModelDetail(model));
        }

        private EntregaResultadosNoLecturaModel DeleteModelDetail(EntregaResultadosNoLecturaModel model)
        { 
            ViewBag.Accion = "Delete"; 
            EntregaResultadosNoLecturaModel newModel = NewModelDetail(model.Entity.IdFather); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<EntregaResultadosNoLectura>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<EntregaResultadosNoLectura>().Remove(model.Entity); 
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
             EntregaResultadosNoLectura entity = new EntregaResultadosNoLectura(); 
             JsonConvert.PopulateObject(values, entity); 
             EntregaResultadosNoLecturaModel model = new EntregaResultadosNoLecturaModel(); 
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
             EntregaResultadosNoLectura entity = Manager().GetBusinessLogic<EntregaResultadosNoLectura>().FindById(x => x.Id == key, false); 
             JsonConvert.PopulateObject(values, entity); 
             EntregaResultadosNoLecturaModel model = new EntregaResultadosNoLecturaModel(); 
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
             EntregaResultadosNoLectura entity = Manager().GetBusinessLogic<EntregaResultadosNoLectura>().FindById(x => x.Id == key, false); 
             Manager().GetBusinessLogic<EntregaResultadosNoLectura>().Remove(entity); 
        } 

        */
        #endregion 

        #region Datasource Combobox Foraneos 

        [HttpPost]
        public LoadResult GetContanciaArchivosId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Archivos>().Tabla(true), loadOptions);
        } 
        [HttpPost]
        public LoadResult GetMediosEntragasId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<MediosEntregas>().Tabla(true), loadOptions);
        } 
        [HttpPost]
        public LoadResult GetPacientesId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Pacientes>().Tabla(true), loadOptions);
        } 
        [HttpPost]
        public LoadResult GetParentescosId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Parentescos>().Tabla(true), loadOptions);
        } 
        [HttpPost]
        public LoadResult GetTiposIdentificacionid(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<TiposIdentificacion>().Tabla(true), loadOptions);
        }
        #endregion

        [HttpPost]
        public LoadResult GetResultados(DataSourceLoadOptions loadOptions)
        {
            var result = Manager().GetBusinessLogic<AdmisionesServiciosPrestados>().Tabla(true)
                .Where(x => x.Atenciones.EstadosId == 10076 && x.Servicios.RequiereLecturaResultados)
                .Include(x => x.Admisiones.Pacientes);
            return DataSourceLoader.Load(result, loadOptions);
        }

    }
}
