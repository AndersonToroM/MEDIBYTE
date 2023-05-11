using CodeGenerator.Data;
using CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGenerator.Templates.NETCoreMVC
{
    public class ViewJS
    {

        public string Name { get; set; } = "APP-08 - View JS";
        public string Description { get; set; } = "Generates a Razor page for inject JavaScript .NET";

        #region private property

        private string OutputFolder { get; set; }
        private string FullNameFile { get; set; }
        private string Project { get; set; }
        private TableModel Table { get; set; }
        private TemplateModel Template { get; set; }
        public ViewJS() { } // Para usarse con reflection

        #endregion

        public ViewJS(CodeGeneratorModel CodeGeneratorModel)
        {
            OutputFolder = CodeGeneratorModel.PathGenerate + "/Views/" + CodeGeneratorModel.TableGenerated.Code + "/";
            FullNameFile = "ViewJS.cshtml";

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
                    sw.WriteLine(@"@model {0}Model ", Table.Code);
                    sw.WriteLine(@"@{");
                    sw.WriteLine(@"    string Prefix = ""{0}""; ", Table.Code);
                    sw.WriteLine(@"}");
                    sw.WriteLine();
                    sw.WriteLine(@"<script>");
                    sw.WriteLine();
                    sw.WriteLine(@"    function @(Prefix)FormSuccess(data) ");
                    sw.WriteLine(@"    { ");
                    sw.WriteLine(@"        hideLoadIndicator(); ");
                    sw.WriteLine(@"        if ('@ViewData.ModelState.IsValid' == 'True') ");
                    sw.WriteLine(@"        { ");
                    sw.WriteLine(@"            if (""@ViewBag.Accion"" == ""Delete"") {");
                    sw.WriteLine(@"                DevExpress.ui.notify(""Registro eliminado correctamente."",""info""); ");
                    sw.WriteLine(@"            } else if (""@ViewBag.Accion"" == ""Save"") {");
                    sw.WriteLine(@"                DevExpress.ui.notify(""Registro guardado correctamente."",""info""); ");
                    sw.WriteLine(@"            } else {");
                    sw.WriteLine(@"                DevExpress.ui.notify(""Error no accion en submit."",""info""); ");
                    sw.WriteLine(@"            }");
                    sw.WriteLine(@"        } ");
                    sw.WriteLine(@"    } ");
                    sw.WriteLine();
                    sw.WriteLine(@"    function @(Prefix)FormFailure(data) ");
                    sw.WriteLine(@"    { ");
                    sw.WriteLine(@"        hideLoadIndicator(); ");
                    sw.WriteLine(@"        DevExpress.ui.notify(""Error en el formulario"",""error""); ");
                    sw.WriteLine(@"    } ");
                    sw.WriteLine();
                    sw.WriteLine(@"    function @(Prefix)FormSuccessDetail(data) ");
                    sw.WriteLine(@"    { ");
                    sw.WriteLine(@"        hideLoadIndicator(); ");
                    sw.WriteLine(@"        if ('@ViewData.ModelState.IsValid' == 'True') ");
                    sw.WriteLine(@"        { ");
                    sw.WriteLine(@"            $(""#{0}DetalleDataGrid"").dxDataGrid(""instance"").refresh();", Table.Code);
                    sw.WriteLine(@"            if (""@ViewBag.Accion"" == ""Delete"") {");
                    sw.WriteLine(@"                DevExpress.ui.notify(""Registro eliminado correctamente."",""info""); ");
                    sw.WriteLine(@"            } else if (""@ViewBag.Accion"" == ""Save"") {");
                    sw.WriteLine(@"                DevExpress.ui.notify(""Registro guardado correctamente."",""info""); ");
                    sw.WriteLine(@"            } else {");
                    sw.WriteLine(@"                DevExpress.ui.notify(""Error no accion en submit."",""info""); ");
                    sw.WriteLine(@"            }");
                    sw.WriteLine(@"        } ");
                    sw.WriteLine(@"    } ");
                    sw.WriteLine();
                    sw.WriteLine(@"    function @(Prefix)FormFailureDetail(data) ");
                    sw.WriteLine(@"    { ");
                    sw.WriteLine(@"        hideLoadIndicator(); ");
                    sw.WriteLine(@"        DevExpress.ui.notify(""Error en el formulario"",""error""); ");
                    sw.WriteLine(@"    } ");
                    sw.WriteLine();
                    sw.WriteLine(@"</script>");
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
