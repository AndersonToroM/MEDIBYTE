using CodeGenerator.Data;
using CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGenerator.Templates.NETCoreMVCRoe
{
    public class ProcessUIController
    {

        public string Name { get; set; } = "APP - Process Controller";
        public string Description { get; set; } = "Generates a Base Class Controller for .NET";

        #region private property

        private string OutputFolder { get; set; }
        private string FullNameFile { get; set; }
        private string Project { get; set; }
        private TableModel Table { get; set; }
        private TemplateModel Template { get; set; }
        public ProcessUIController() { } // Para usarse con reflection

        #endregion

        public ProcessUIController(CodeGeneratorModel CodeGeneratorModel)
        {
            OutputFolder = CodeGeneratorModel.PathGenerate + "/APP/" + CodeGeneratorModel.TableGenerated.Comment + "/Controllers/Process/";
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
                sw.WriteLine(@"using DevExtreme.AspNet.Data.ResponseModel;");
                sw.WriteLine(@"using DevExtreme.AspNet.Mvc;");
                sw.WriteLine(@"using Hefestos.Frontend.Core.Controllers;");
                sw.WriteLine(@"using Microsoft.AspNetCore.Authorization;");
                sw.WriteLine(@"using Microsoft.AspNetCore.Http;");
                sw.WriteLine(@"using Microsoft.AspNetCore.Mvc;");
                sw.WriteLine(@"using Microsoft.Extensions.Configuration;");
                sw.WriteLine(@"using Siesa.Enterprise.Infraestructura.Entidades;");
                sw.WriteLine(@"using Siesa.Enterprise.UI.Modules.{0}.Models;", Table.Comment);
                sw.WriteLine(@"using System;");
                sw.WriteLine(@"using System.Collections.Generic;");
                sw.WriteLine();
                sw.WriteLine(@"namespace Siesa.Enterprise.UI.Modules.{0}.Controllers", Table.Comment);
                sw.WriteLine(@"{");
                sw.WriteLine();
                sw.WriteLine(@"   [Area(""{0}"")]", Table.Comment);
                sw.WriteLine(@"   [Authorize] ");
                sw.WriteLine(@"   public partial class {0}Controller : BaseController",  Table.Code);
                sw.WriteLine(@"   {");
                sw.WriteLine();
                sw.WriteLine(@"      private const string Prefix = ""{0}""; ", Table.Code);
                sw.WriteLine();
                sw.WriteLine(@"      public {0}Controller(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)", Table.Code);
                sw.WriteLine(@"      {");
                sw.WriteLine(@"      }");
                sw.WriteLine();
                sw.WriteLine(@"      [HttpGet]");
                sw.WriteLine(@"      public IActionResult List()");
                sw.WriteLine(@"      {");
                sw.WriteLine(@"         {0}Model model = new {0}Model(); ", Table.Code);
                sw.WriteLine(@"         return View(""../Process/{0}"", model); ", Table.Code);
                sw.WriteLine(@"      }");
                sw.WriteLine();
                sw.WriteLine(@"      [HttpGet]");
                sw.WriteLine(@"      public IActionResult ListPartial()");
                sw.WriteLine(@"      {");
                sw.WriteLine(@"         {0}Model model = new {0}Model(); ", Table.Code);
                sw.WriteLine(@"         return PartialView(""../Process/{0}"", model); ", Table.Code);
                sw.WriteLine(@"      }");
                sw.WriteLine();
                sw.WriteLine(@"      [HttpPost] ");
                sw.WriteLine(@"      public IActionResult Process({0}Model model) ", Table.Code);
                sw.WriteLine(@"      { ");
                sw.WriteLine(@"         ViewBag.Prefix = Prefix; ");
                sw.WriteLine(@"         if (ModelState.IsValid) ");
                sw.WriteLine(@"         { ");
                sw.WriteLine(@"            try ");
                sw.WriteLine(@"            { ");
                sw.WriteLine(@"                //Logica del proceso ");
                sw.WriteLine(@"                //model = Manager().MaestroLogicaNegocio().Procesar(Entidad); ");
                sw.WriteLine(@"            } ");
                sw.WriteLine(@"            catch (Exception e) ");
                sw.WriteLine(@"            { ");
                sw.WriteLine(@"               while (e!=null) ");
                sw.WriteLine(@"               { ");
                sw.WriteLine(@"                  ModelState.AddModelError(""Error: "", e.Message); ");
                sw.WriteLine(@"                  e = e.InnerException; ");
                sw.WriteLine(@"               } ");
                sw.WriteLine(@"            } ");
                sw.WriteLine(@"         } ");
                sw.WriteLine(@"         return PartialView(""../Process/{0}"", model); ", Table.Code);
                sw.WriteLine(@"      } ");
                sw.WriteLine();
                sw.WriteLine(@"   }");
                sw.WriteLine(@"}");

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
