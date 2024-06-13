using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Dominus.Backend.Application;
using Dominus.Frontend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Blazor.WebApp.Controllers
{

    [Authorize]
    public partial class LanguageResourceController : BaseAppController
    {

        //private const string Prefix = "LanguageResource"; 

        public LanguageResourceController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
        }

        #region Functions Master

        [HttpPost]
        public LoadResult Get(DataSourceLoadOptions loadOptions)
        {
            List<Language.LanguageResource> recursos = DApp.DefaultLanguage.GetAllResources();
            return DataSourceLoader.Load(recursos, loadOptions);
        }

        [HttpPost]
        public IActionResult Edit(Language.LanguageResource model)
        {
            return PartialView("Edit", EditModel(model));
        }

        private Language.LanguageResource EditModel(Language.LanguageResource model)
        {

            try
            {
                DApp.DefaultLanguage.AddResource(model.Id, model.Description);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Entity.Id", e.GetFullErrorMessage());
            }

            return model;
        }

        #endregion

        #region Funcions Detail Edit in Grid 

        [HttpPost]
        public IActionResult AddInGrid(string values)
        {
            Language.LanguageResource entity = new Language.LanguageResource();
            JsonConvert.PopulateObject(values, entity);
            this.EditModel(entity);
            if (ModelState.IsValid)
                return Ok(ModelState);
            else
                return BadRequest(ModelState.GetModelFullError());
        }

        [HttpPost]
        public IActionResult ModifyInGrid(string key, string values)
        {
            Language.LanguageResource entity = new Language.LanguageResource();
            JsonConvert.PopulateObject(values, entity);
            entity.Id = key;
            this.EditModel(entity);
            if (ModelState.IsValid)
                return Ok(ModelState);
            else
                return BadRequest(ModelState.GetModelFullError());
        }

        [HttpPost]
        public void DeleteInGrid(string key)
        {
            DApp.DefaultLanguage.DeleteResource(key);
        }

        #endregion

    }
}
