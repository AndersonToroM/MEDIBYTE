using Blazor.BusinessLogic;
using Blazor.Infrastructure.Entities;
using Blazor.WebApp.Models;
using DevExpress.ClipboardSource.SpreadsheetML;
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Blazor.WebApp.Controllers
{

    [Authorize]
    public partial class PacientesEntidadesController : BaseAppController
    {

        //private const string Prefix = "PacientesEntidades"; 

        public PacientesEntidadesController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
        }

        #region Functions Master

        [HttpPost]
        public LoadResult Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<PacientesEntidades>().Tabla(true), loadOptions);
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

        private PacientesEntidadesModel NewModel()
        {
            PacientesEntidadesModel model = new PacientesEntidadesModel();
            model.Entity.IsNew = true;
            return model;
        }

        [HttpGet]
        public IActionResult Edit(long Id)
        {
            return PartialView("Edit", EditModel(Id));
        }

        private PacientesEntidadesModel EditModel(long Id)
        {
            PacientesEntidadesModel model = new PacientesEntidadesModel();
            model.Entity = Manager().GetBusinessLogic<PacientesEntidades>().FindById(x => x.Id == Id, false);
            model.Entity.IsNew = false;
            return model;
        }

        [HttpPost]
        public IActionResult Edit(PacientesEntidadesModel model)
        {
            return PartialView("Edit", EditModel(model));
        }

        private PacientesEntidadesModel EditModel(PacientesEntidadesModel model)
        {
            ViewBag.Accion = "Save";
            var OnState = model.Entity.IsNew;

            var entidadesPaciente = Manager().GetBusinessLogic<PacientesEntidades>().Tabla(true)
                .Include(x => x.Entidades.TipoEntidades)
                .Where(x => x.PacientesId == model.Entity.PacientesId).ToList();

            var entidadExiste = entidadesPaciente.FirstOrDefault(x => x.EntidadesId == model.Entity.EntidadesId && x.Id != model.Entity.Id);
            if (entidadExiste is not null)
            {
                ModelState.AddModelError("Error: ", DApp.DefaultLanguage.GetResource("Pacientes.YAEXISTEENTIDAD") + " " + entidadExiste.Entidades.Alias + (entidadExiste.Activo ? " - Activo" : " - Inactivo"));
            }

            var entidadAgregar = Manager().GetBusinessLogic<Entidades>().FindById(x => x.Id == model.Entity.EntidadesId, true);
            var entidadEPS = entidadesPaciente.FirstOrDefault(x => x.Entidades.TipoEntidades.Codigo == "EPS" && x.Activo && x.Id != model.Entity.Id);
            if (entidadAgregar.TipoEntidades.Codigo == "EPS" && entidadEPS is not null)
            {
                ModelState.AddModelError("Error: ", DApp.DefaultLanguage.GetResource("Pacientes.YAEXISTEENTIDADEPS") + " " + entidadEPS.Entidades.Alias);
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
                        model.Entity = Manager().GetBusinessLogic<PacientesEntidades>().Add(model.Entity);
                        model.Entity.IsNew = false;
                    }
                    else
                    {
                        model.Entity = Manager().GetBusinessLogic<PacientesEntidades>().Modify(model.Entity);
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Entity.Id", e.GetFrontFullErrorMessage());
                }
            }
            return model;
        }

        [HttpPost]
        public IActionResult Delete(PacientesEntidadesModel model)
        {
            return PartialView("Edit", DeleteModel(model));
        }

        private PacientesEntidadesModel DeleteModel(PacientesEntidadesModel model)
        {
            ViewBag.Accion = "Delete";
            PacientesEntidadesModel newModel = NewModel();
            if (ModelState.IsValid)
            {
                try
                {
                    model.Entity = Manager().GetBusinessLogic<PacientesEntidades>().FindById(x => x.Id == model.Entity.Id, false);
                    Manager().GetBusinessLogic<PacientesEntidades>().Remove(model.Entity);
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


        [HttpGet]
        public IActionResult NewDetail(long IdFather)
        {
            return PartialView("EditDetail", NewModelDetail(IdFather));
        }

        private PacientesEntidadesModel NewModelDetail(long IdFather)
        {
            PacientesEntidadesModel model = new PacientesEntidadesModel();
            model.Entity.PacientesId = IdFather;
            model.Entity.IsNew = true;
            return model;
        }

        [HttpGet]
        public IActionResult EditDetail(long Id)
        {
            return PartialView("EditDetail", EditModel(Id));
        }

        [HttpPost]
        public IActionResult EditDetail(PacientesEntidadesModel model)
        {
            return PartialView("EditDetail", EditModel(model));
        }

        [HttpPost]
        public IActionResult DeleteDetail(PacientesEntidadesModel model)
        {
            return PartialView("EditDetail", DeleteModelDetail(model));
        }

        private PacientesEntidadesModel DeleteModelDetail(PacientesEntidadesModel model)
        {
            ViewBag.Accion = "Delete";
            PacientesEntidadesModel newModel = NewModelDetail(model.Entity.PacientesId);
            if (ModelState.IsValid)
            {
                try
                {
                    model.Entity = Manager().GetBusinessLogic<PacientesEntidades>().FindById(x => x.Id == model.Entity.Id, false);
                    Manager().GetBusinessLogic<PacientesEntidades>().Remove(model.Entity);
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

        #region Funcions Detail Edit in Grid 

        [HttpPost]
        public IActionResult AddInGrid(string values)
        {
            PacientesEntidades entity = new PacientesEntidades();
            JsonConvert.PopulateObject(values, entity);
            PacientesEntidadesModel model = new PacientesEntidadesModel();
            model.Entity = entity;
            model.Entity.IsNew = true;
            this.EditModel(model);
            if (ModelState.IsValid)
                return Ok(ModelState);
            else
                return BadRequest(ModelState.GetModelFullErrorMessage());
        }

        [HttpPost]
        public IActionResult ModifyInGrid(int key, string values)
        {
            PacientesEntidades entity = Manager().GetBusinessLogic<PacientesEntidades>().FindById(x => x.Id == key, false);
            JsonConvert.PopulateObject(values, entity);
            PacientesEntidadesModel model = new PacientesEntidadesModel();
            model.Entity = entity;
            model.Entity.IsNew = false;
            this.EditModel(model);
            if (ModelState.IsValid)
                return Ok(ModelState);
            else
                return BadRequest(ModelState.GetModelFullErrorMessage());
        }

        [HttpPost]
        public void DeleteInGrid(int key)
        {
            PacientesEntidades entity = Manager().GetBusinessLogic<PacientesEntidades>().FindById(x => x.Id == key, false);
            Manager().GetBusinessLogic<PacientesEntidades>().Remove(entity);
        }


        #endregion

        #region Datasource Combobox Foraneos 

        [HttpPost]
        public LoadResult GetEntidadesId(DataSourceLoadOptions loadOptions)
        {
            if (this.ActualEntidadId() != 0)
                return DataSourceLoader.Load(Manager().GetBusinessLogic<Entidades>().Tabla(true).Where(x => x.Id == this.ActualEntidadId()), loadOptions);
            else
                return DataSourceLoader.Load(Manager().GetBusinessLogic<Entidades>().Tabla(true), loadOptions);
        }
        [HttpPost]
        public LoadResult GetPacientesId(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Pacientes>().Tabla(true), loadOptions);
        }
        [HttpPost]
        public LoadResult GetTipoEntidadesId(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<TipoEntidades>().Tabla(true), loadOptions);
        }
        #endregion

    }
}
