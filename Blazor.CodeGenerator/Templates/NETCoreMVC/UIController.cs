using CodeGenerator.Data;
using CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGenerator.Templates.NETCoreMVC
{
    public class UIController
    {

        public string Name { get; set; } = "APP-03 - Controller";
        public string Description { get; set; } = "Generates a Base Class Controller for .NET";

        #region private property

        private string OutputFolder { get; set; }
        private string FullNameFile { get; set; }
        private string Project { get; set; }
        private TableModel Table { get; set; }
        private TemplateModel Template { get; set; }
        public UIController() { } // Para usarse con reflection

        #endregion

        public UIController(CodeGeneratorModel CodeGeneratorModel)
        {
            OutputFolder = CodeGeneratorModel.PathGenerate + "/Controllers/";
            FullNameFile = CodeGeneratorModel.TableGenerated.Code + "Controller.cs";

            #region asignacion
            Table = CodeGeneratorModel.TableGenerated;
            Template = CodeGeneratorModel.TemplateGenerated;
            Project = CodeGeneratorModel.Domain;
            GenerateCodeTemplate();
            #endregion
        }

        public void GenerateCodeTemplate() 
        {
            #region Creation directory
            string fileName = this.OutputFolder;
            if (!Directory.Exists(fileName))
                Directory.CreateDirectory(this.OutputFolder);
            fileName = this.OutputFolder + this.FullNameFile;
            if (File.Exists(fileName))
                File.Delete(fileName);
            #endregion

            StreamWriter sw = File.CreateText(fileName);
            try
            {
                if (Table.Columns.Exists(x=>x.IsPrimaryKey))
                {
                    sw.WriteLine(@"using DevExtreme.AspNet.Data;");
                    sw.WriteLine(@"using DevExtreme.AspNet.Data.ResponseModel;");
                    sw.WriteLine(@"using DevExtreme.AspNet.Mvc;");
                    sw.WriteLine(@"using Dominus.Frontend.Controllers;");
                    sw.WriteLine(@"using {0}.Infrastructure.Entities;", Project);
                    sw.WriteLine(@"using {0}.WebApp.Models;", Project);
                    sw.WriteLine(@"using Microsoft.AspNetCore.Authorization;");
                    sw.WriteLine(@"using Microsoft.AspNetCore.Http;");
                    sw.WriteLine(@"using Microsoft.AspNetCore.Mvc;");
                    sw.WriteLine(@"using Microsoft.Extensions.Configuration;");
                    sw.WriteLine(@"using System;");
                    sw.WriteLine(@"using System.Linq;");
                    sw.WriteLine(@"using Newtonsoft.Json;");
                    sw.WriteLine(@"using Dominus.Backend.Application;");
                    sw.WriteLine(@"using {0}.BusinessLogic;",Project);
                    sw.WriteLine();
                    sw.WriteLine(@"namespace {0}.WebApp.Controllers", Project);
                    sw.WriteLine(@"{");
                    sw.WriteLine();
                    sw.WriteLine(@"    [Authorize] ");
                    sw.WriteLine(@"    public partial class {0}Controller : BaseAppController", Table.Code);
                    sw.WriteLine(@"    {");
                    sw.WriteLine();
                    sw.WriteLine(@"        //private const string Prefix = ""{0}""; ", Table.Code);
                    sw.WriteLine();
                    sw.WriteLine(@"        public {0}Controller(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)", Table.Code);
                    sw.WriteLine(@"        {");
                    sw.WriteLine(@"        }");
                    sw.WriteLine();
                    sw.WriteLine(@"        #region Functions Master");
                    sw.WriteLine();
                    sw.WriteLine(@"        [HttpPost]");
                    sw.WriteLine(@"        public LoadResult Get(DataSourceLoadOptions loadOptions)");
                    sw.WriteLine(@"        {");
                    sw.WriteLine(@"            return DataSourceLoader.Load(Manager().GetBusinessLogic<{0}>().Tabla(true), loadOptions);", Table.Code);
                    sw.WriteLine(@"        }");
                    sw.WriteLine();
                    sw.WriteLine(@"        public IActionResult List()");
                    sw.WriteLine(@"        {");
                    sw.WriteLine(@"            return View(""List"");");
                    sw.WriteLine(@"        }");
                    sw.WriteLine();
                    sw.WriteLine(@"        public IActionResult ListPartial()");
                    sw.WriteLine(@"        {");
                    sw.WriteLine(@"            return PartialView(""List"");");
                    sw.WriteLine(@"        }");
                    sw.WriteLine();
                    sw.WriteLine(@"        [HttpGet]");
                    sw.WriteLine(@"        public IActionResult New()");
                    sw.WriteLine(@"        {");
                    sw.WriteLine(@"            return PartialView(""Edit"", NewModel());");
                    sw.WriteLine(@"        }");
                    sw.WriteLine();
                    sw.WriteLine(@"        private {0}Model NewModel() ", Table.Code);
                    sw.WriteLine(@"        { ");
                    sw.WriteLine(@"            {0}Model model = new {0}Model();", Table.Code);
                    sw.WriteLine(@"            model.Entity.IsNew = true;");
                    sw.WriteLine(@"            return model; ");
                    sw.WriteLine(@"        } ");
                    sw.WriteLine();
                    sw.WriteLine(@"        [HttpGet]");
                    sw.WriteLine(@"        public IActionResult Edit(long Id)");
                    sw.WriteLine(@"        {");
                    sw.WriteLine(@"            return PartialView(""Edit"", EditModel(Id));");
                    sw.WriteLine(@"        }");
                    sw.WriteLine();
                    sw.WriteLine(@"        private {0}Model EditModel(long Id) ", Table.Code);
                    sw.WriteLine(@"        { ");
                    sw.WriteLine(@"            {0}Model model = new {0}Model();", Table.Code);
                    sw.WriteLine(@"            model.Entity = Manager().GetBusinessLogic<{0}>().FindById(x => x.Id == Id, false);", Table.Code);
                    sw.WriteLine(@"            model.Entity.IsNew = false;");
                    sw.WriteLine(@"            return model; ");
                    sw.WriteLine(@"        } ");
                    sw.WriteLine();
                    sw.WriteLine(@"        [HttpPost]");
                    sw.WriteLine(@"        public IActionResult Edit({0}Model model)", Table.Code);
                    sw.WriteLine(@"        {");
                    sw.WriteLine(@"            return PartialView(""Edit"",EditModel(model));");
                    sw.WriteLine(@"        }");
                    sw.WriteLine();
                    sw.WriteLine(@"        private {0}Model EditModel({0}Model model) ", Table.Code);
                    sw.WriteLine(@"        { ");
                    sw.WriteLine(@"            ViewBag.Accion = ""Save""; ");
                    sw.WriteLine(@"            var OnState = model.Entity.IsNew; ");
                    sw.WriteLine(@"            if (ModelState.IsValid) ");
                    sw.WriteLine(@"            { ");
                    sw.WriteLine(@"                try ");
                    sw.WriteLine(@"                { ");
                    sw.WriteLine(@"                    model.Entity.LastUpdate = DateTime.Now; ");
                    sw.WriteLine(@"                    model.Entity.UpdatedBy = User.Identity.Name; ");
                    sw.WriteLine(@"                    if (model.Entity.IsNew) ");
                    sw.WriteLine(@"                    { ");
                    sw.WriteLine(@"                        model.Entity.CreationDate = DateTime.Now; ");
                    sw.WriteLine(@"                        model.Entity.CreatedBy = User.Identity.Name; ");
                    sw.WriteLine(@"                        model.Entity = Manager().GetBusinessLogic<{0}>().Add(model.Entity); ", Table.Code);
                    sw.WriteLine(@"                        model.Entity.IsNew = false;");
                    sw.WriteLine(@"                    } ");
                    sw.WriteLine(@"                    else ");
                    sw.WriteLine(@"                    { ");
                    sw.WriteLine(@"                        model.Entity = Manager().GetBusinessLogic<{0}>().Modify(model.Entity); ", Table.Code);
                    sw.WriteLine(@"                    } ");
                    sw.WriteLine(@"                } ");
                    sw.WriteLine(@"                catch (Exception e) ");
                    sw.WriteLine(@"                { ");
                    sw.WriteLine(@"                    ModelState.AddModelError(""Entity.Id"", e.GetFullErrorMessage()); ");
                    sw.WriteLine(@"                } ");
                    sw.WriteLine(@"            } ");
                    sw.WriteLine(@"            else ");
                    sw.WriteLine(@"            { ");
                    sw.WriteLine(@"                 ModelState.AddModelError(""Entity.Id"", ""Error de codigo, el objeto a guardar tiene campos diferentes a los de la entidad.""); ");
                    sw.WriteLine(@"            } ");
                    sw.WriteLine(@"            return model; ");
                    sw.WriteLine(@"        } ");
                    sw.WriteLine();
                    sw.WriteLine(@"        [HttpPost]");
                    sw.WriteLine(@"        public IActionResult Delete({0}Model model)", Table.Code);
                    sw.WriteLine(@"        {");
                    sw.WriteLine(@"            return PartialView(""Edit"", DeleteModel(model));");
                    sw.WriteLine(@"        }");
                    sw.WriteLine();
                    sw.WriteLine(@"        private {0}Model DeleteModel({0}Model model)", Table.Code);
                    sw.WriteLine(@"        { ");
                    sw.WriteLine(@"            ViewBag.Accion = ""Delete""; ");
                    sw.WriteLine(@"            {0}Model newModel = NewModel(); ", Table.Code);
                    sw.WriteLine(@"            if (ModelState.IsValid) ");
                    sw.WriteLine(@"            { ");
                    sw.WriteLine(@"                try ");
                    sw.WriteLine(@"                { ");
                    sw.WriteLine(@"                    model.Entity = Manager().GetBusinessLogic<{0}>().FindById(x => x.Id == model.Entity.Id, false); ", Table.Code);
                    sw.WriteLine(@"                    Manager().GetBusinessLogic<{0}>().Remove(model.Entity); ", Table.Code);
                    sw.WriteLine(@"                    return newModel;");
                    sw.WriteLine(@"                } ");
                    sw.WriteLine(@"                catch (Exception e) ");
                    sw.WriteLine(@"                { ");
                    sw.WriteLine(@"                    ModelState.AddModelError(""Entity.Id"", e.GetFullErrorMessage()); ");
                    sw.WriteLine(@"                } ");
                    sw.WriteLine(@"            } ");
                    sw.WriteLine(@"            return model; ");
                    sw.WriteLine(@"        } ");
                    sw.WriteLine();
                    sw.WriteLine(@"        #endregion ");
                    sw.WriteLine();
                    sw.WriteLine(@"        #region Functions Detail ");
                    sw.WriteLine(@"        /*");
                    sw.WriteLine();
                    sw.WriteLine(@"        [HttpGet]");
                    sw.WriteLine(@"        public IActionResult NewDetail(long IdFather)");
                    sw.WriteLine(@"        {");
                    sw.WriteLine(@"            return PartialView(""EditDetail"", NewModelDetail(IdFather));");
                    sw.WriteLine(@"        }");
                    sw.WriteLine();
                    sw.WriteLine(@"        private {0}Model NewModelDetail(long IdFather) ", Table.Code);
                    sw.WriteLine(@"        { ");
                    sw.WriteLine(@"            {0}Model model = new {0}Model(); ", Table.Code);
                    sw.WriteLine(@"            model.Entity.IdFather = IdFather; ");
                    sw.WriteLine(@"            model.Entity.IsNew = true; ");
                    sw.WriteLine(@"            return model; ");
                    sw.WriteLine(@"        } ");
                    sw.WriteLine();
                    sw.WriteLine(@"        [HttpGet]");
                    sw.WriteLine(@"        public IActionResult EditDetail(long Id)");
                    sw.WriteLine(@"        {");
                    sw.WriteLine(@"            return PartialView(""EditDetail"", EditModel(Id));");
                    sw.WriteLine(@"        }");
                    sw.WriteLine();
                    sw.WriteLine(@"        [HttpPost]");
                    sw.WriteLine(@"        public IActionResult EditDetail({0}Model model)", Table.Code);
                    sw.WriteLine(@"        {");
                    sw.WriteLine(@"            return PartialView(""EditDetail"",EditModel(model));");
                    sw.WriteLine(@"        }");
                    sw.WriteLine();
                    sw.WriteLine(@"        [HttpPost]");
                    sw.WriteLine(@"        public IActionResult DeleteDetail({0}Model model)", Table.Code);
                    sw.WriteLine(@"        {");
                    sw.WriteLine(@"            return PartialView(""EditDetail"", DeleteModelDetail(model));");
                    sw.WriteLine(@"        }");
                    sw.WriteLine();
                    sw.WriteLine(@"        private {0}Model DeleteModelDetail({0}Model model)", Table.Code);
                    sw.WriteLine(@"        { ");
                    sw.WriteLine(@"            ViewBag.Accion = ""Delete""; ");
                    sw.WriteLine(@"            {0}Model newModel = NewModelDetail(model.Entity.IdFather); ", Table.Code);
                    sw.WriteLine(@"            if (ModelState.IsValid) ");
                    sw.WriteLine(@"            { ");
                    sw.WriteLine(@"                try ");
                    sw.WriteLine(@"                { ");
                    sw.WriteLine(@"                    model.Entity = Manager().GetBusinessLogic<{0}>().FindById(x => x.Id == model.Entity.Id, false); ", Table.Code);
                    sw.WriteLine(@"                    Manager().GetBusinessLogic<{0}>().Remove(model.Entity); ", Table.Code);
                    sw.WriteLine(@"                    return newModel;");
                    sw.WriteLine(@"                } ");
                    sw.WriteLine(@"                catch (Exception e) ");
                    sw.WriteLine(@"                { ");
                    sw.WriteLine(@"                    ModelState.AddModelError(""Entity.Id"", e.GetFullErrorMessage()); ");
                    sw.WriteLine(@"                } ");
                    sw.WriteLine(@"            } ");
                    sw.WriteLine(@"            return model; ");
                    sw.WriteLine(@"        } ");
                    sw.WriteLine();
                    sw.WriteLine(@"        #endregion ");
                    sw.WriteLine();
                    sw.WriteLine(@"        #region Funcions Detail Edit in Grid ");
                    sw.WriteLine();
                    sw.WriteLine(@"        [HttpPost] ");
                    sw.WriteLine(@"        public IActionResult AddInGrid(string values) ");
                    sw.WriteLine(@"        { ");
                    sw.WriteLine(@"             {0} entity = new {0}(); ", Table.Code);
                    sw.WriteLine(@"             JsonConvert.PopulateObject(values, entity); ");
                    sw.WriteLine(@"             {0}Model model = new {0}Model(); ", Table.Code);
                    sw.WriteLine(@"             model.Entity = entity; ");
                    sw.WriteLine(@"             model.Entity.IsNew = true; ");
                    sw.WriteLine(@"             this.EditModel(model); ");
                    sw.WriteLine(@"             if(ModelState.IsValid) ");
                    sw.WriteLine(@"                 return Ok(ModelState); ");
                    sw.WriteLine(@"             else ");
                    sw.WriteLine(@"                 return BadRequest(ModelState); ");
                    sw.WriteLine(@"        } ");
                    sw.WriteLine();
                    sw.WriteLine(@"        [HttpPost] ");
                    sw.WriteLine(@"        public IActionResult ModifyInGrid(int key, string values) ");
                    sw.WriteLine(@"        { ");
                    sw.WriteLine(@"             {0} entity = Manager().GetBusinessLogic<{0}>().FindById(x => x.Id == key, false); ", Table.Code);
                    sw.WriteLine(@"             JsonConvert.PopulateObject(values, entity); ");
                    sw.WriteLine(@"             {0}Model model = new {0}Model(); ", Table.Code);
                    sw.WriteLine(@"             model.Entity = entity; ");
                    sw.WriteLine(@"             model.Entity.IsNew = false; ");
                    sw.WriteLine(@"             this.EditModel(model); ");
                    sw.WriteLine(@"             if(ModelState.IsValid) ");
                    sw.WriteLine(@"                 return Ok(ModelState); ");
                    sw.WriteLine(@"             else ");
                    sw.WriteLine(@"                 return BadRequest(ModelState); ");
                    sw.WriteLine(@"        } ");
                    sw.WriteLine();
                    sw.WriteLine(@"        [HttpPost]");
                    sw.WriteLine(@"        public void DeleteInGrid(int key)");
                    sw.WriteLine(@"        { ");
                    sw.WriteLine(@"             {0} entity = Manager().GetBusinessLogic<{0}>().FindById(x => x.Id == key, false); ", Table.Code);
                    sw.WriteLine(@"             Manager().GetBusinessLogic<{0}>().Remove(entity); ", Table.Code);
                    sw.WriteLine(@"        } ");
                    sw.WriteLine();
                    sw.WriteLine(@"        */");
                    sw.WriteLine(@"        #endregion ");
                    sw.WriteLine();
                    if (Table.InReferences != null && Table.InReferences.Count > 0)
                    {
                        sw.WriteLine(@"        #region Datasource Combobox Foraneos ");
                        sw.WriteLine();
                        List<string> inReferenceName = new List<string>();
                        foreach (InReferencesModel inReference in Table.InReferences)
                        {
                            string columnReference = inReference.ColumnCode.Substring(0, inReference.ColumnCode.Length - 2);
                            sw.WriteLine(@"        [HttpPost]");
                            sw.WriteLine(@"        public LoadResult Get{0}(DataSourceLoadOptions loadOptions)", inReference.ColumnCode);
                            sw.WriteLine(@"        { ");
                            sw.WriteLine(@"            return DataSourceLoader.Load(Manager().GetBusinessLogic<{0}>().Tabla(true), loadOptions);", inReference.ParentTableCode);
                            sw.WriteLine(@"        } ");
                        }
                        sw.WriteLine(@"       #endregion");
                        sw.WriteLine();
                    }
                    sw.WriteLine(@"    }");
                    sw.WriteLine(@"}");
                }

                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                GCUtil.Errors.Add("La plantilla " + Template.Name + " no se genero por el siguiente error: " + e.Message);
                GCUtil.TemplateWithError.Add(Template.NameClass);
                sw.Flush();
                sw.Close();
                File.Delete(fileName);
            }
            

        }

        private string GetPkStringCols(List<ColumnModel> pks)
        {
            string cols = "";
            for (int i = 0; i < pks.Count; i++)
            {
                if (i == 0)
                    cols += "t." + pks[i].Code;
                else
                    cols += " ,t." + pks[i].Code;
            }
            return cols;
        }

    }
}
