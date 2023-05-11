using CodeGenerator.Data;
using CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGenerator.Templates.NETCoreMVCRoe
{
    public class ProcessView
    {

        public string Name { get; set; } = "APP - Process View";
        public string Description { get; set; } = "Generates a Razor page for .NET";

        #region private property

        private string OutputFolder { get; set; }
        private string FullNameFile { get; set; }
        private string Project { get; set; }
        private TableModel Table { get; set; }
        private TemplateModel Template { get; set; }
        public ProcessView() { } // Para usarse con reflection

        #endregion

        public ProcessView(CodeGeneratorModel CodeGeneratorModel)
        {
            OutputFolder = CodeGeneratorModel.PathGenerate + "/APP/" + CodeGeneratorModel.TableGenerated.Comment + "/Views/Process/";
            FullNameFile = CodeGeneratorModel.TableGenerated.Code + ".cshtml";

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
                sw.WriteLine(@"@using Siesa.Enterprise.UI.Modules.{0}.Models ", Table.Comment);
                sw.WriteLine();
                sw.WriteLine(@"@model {0}Model ", Table.Code);
                sw.WriteLine(@"@{");
                sw.WriteLine(@"   var viewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(ViewData);");
                sw.WriteLine(@"   viewData.TemplateInfo.HtmlFieldPrefix = ViewBag.Prefix;");
                sw.WriteLine(@"   ViewBag.AjaxUpdatePanel = ""#mainPanel""; ");
                sw.WriteLine(@"   ViewBag.AspAction = ""Edit""; ");
                sw.WriteLine(@"   string Prefix = ViewBag.Prefix; ");
                sw.WriteLine(@"}");
                sw.WriteLine();

                sw.WriteLine(@"@((Html.Siesa().Toolbar() ");
                sw.WriteLine(@"    .ID(""{0}Toolbar"") ", Table.Name);
                sw.WriteLine(@"    .Items(items => ");
                sw.WriteLine(@"    { ");
                sw.WriteLine(@"        items.Add().Text(HCore.Recursos.GetRecurso(""{0}"")).Location(ToolbarItemLocation.Before); ", Table.Name);
                sw.WriteLine(@"        items.Add().Widget(w => w.Button().Icon(""preferences"").Text(""Procesar"").OnClick(""{0}_btnProcesar_click"").ID(""{0}_btnProcesar"")).Location(ToolbarItemLocation.After); ", Table.Name);
                sw.WriteLine(@"    }).ElementAttr(""class"", ""toolbarSection sticky-top"")) ");
                sw.WriteLine(@") ");

                sw.WriteLine();
                sw.WriteLine(@"    <ol class=""breadcrumbs"" id=""@(ViewBag.Prefix + ""DetailBread"")""></ol>");
                sw.WriteLine();
                sw.WriteLine(@" <div id = ""{0}MainPanelForm""> ", Table.Name);
                sw.WriteLine(@"   <div class=""box box-info""> ");

                sw.WriteLine(@"      <form id = ""@(ViewBag.Prefix)Form"" asp-area=""{0}"" asp-controller=""{1}"" asp-action=""Process"" class=""form-horizontal"" data-ajax-mode=""replace"" data-ajax-update=@ViewBag.AjaxUpdatePanel data-ajax-success=""@(ViewBag.Prefix)FormSuccess"" data-ajax-failure=""@(ViewBag.Prefix)FormFailure"" data-ajax=""true""> ", Table.Comment, Table.Name);
                sw.WriteLine(@"         <div asp-validation-summary=""All"" class=""text-danger"" id=""@(ViewBag.Prefix)ListError""></div> ");
                sw.WriteLine();
                if (Table.Columns != null && Table.Columns.Count > 0)
                {
                    sw.WriteLine(@"    <div class=""card""> ");
                    sw.WriteLine(@"        <div class=""card-body""> ");
                    for (int i = 0; i < Table.Columns.Count; i++)
                    {
                        var column = Table.Columns[i];

                        sw.WriteLine(@"    <div class=""row""> ");
                        sw.WriteLine(@"        <div class=""col-sm-12""> ");
                        sw.WriteLine(@"            <div id=""@(ViewBag.Prefix){0}"" class=""form-group""> ", "DivField" + column.Code);
                        sw.WriteLine(@"                <label for=""@(ViewBag.Prefix + ""_{0}"")"">@HCore.Recursos.GetRecurso(""{1}"")</label> ", column.Code, Table.Name + "." + column.Name);

                        if ((GCUtil.GetNetDataType(column) == "Int" || GCUtil.GetNetDataType(column) == "Int16" || GCUtil.GetNetDataType(column) == "Int32" || GCUtil.GetNetDataType(column) == "Int64" || GCUtil.GetNetDataType(column) == "Short"))
                            sw.WriteLine(@"                @(Html.Siesa().NumberBoxFor(m => m.{0}).ID(ViewBag.Prefix + ""{0}"").InputAttr(""class"", ""form-control"")) ", column.Code);
                        else if ((GCUtil.GetNetDataType(column) == "Double" || GCUtil.GetNetDataType(column) == "Float" || GCUtil.GetNetDataType(column) == "Decimal"))
                            sw.WriteLine(@"                @(Html.Siesa().NumberBoxFor(m => m.{0}).ID(ViewBag.Prefix + ""{0}"").InputAttr(""class"", ""form-control"")) ", column.Code);
                        else if (GCUtil.GetNetDataType(column) == "DateTime")
                            sw.WriteLine(@"                @(Html.Siesa().DateBoxFor(m => m.{0}).ID(ViewBag.Prefix + ""{0}"").DisplayFormat(""dd/MM/yyyy"").InputAttr(""class"", ""form-control"")) ", column.Code);
                        else if (GCUtil.GetNetDataType(column) == "Boolean")
                            sw.WriteLine(@"                @(Html.Siesa().CheckBoxFor(m => m.{0}).ID(ViewBag.Prefix + ""{0}"")) ", column.Code);
                        else
                            sw.WriteLine(@"                @(Html.Siesa().TextBoxFor(m => m.{0}).ID(ViewBag.Prefix + ""{0}"").InputAttr(""class"", ""form-control"")) ", column.Code);

                        sw.WriteLine(@"            </div> ");
                        sw.WriteLine(@"        </div> ");
                        sw.WriteLine(@"    </div> ");
                        sw.WriteLine();

                    }
                    sw.WriteLine(@"        </div>");
                    sw.WriteLine(@"    </div> ");
                }
                sw.WriteLine();
                sw.WriteLine(@"      </form> ");
                sw.WriteLine(@"	@* ");
                sw.WriteLine(@"	<div class=""card card-nav-tabs card-persistent""> ");
                sw.WriteLine(@"		<div class=""scrollmenu card-header card-header-primary"">  ");
                sw.WriteLine(@"			<div class=""nav-tabs-navigation"">  ");
                sw.WriteLine(@"				<div class=""nav-tabs-wrapper"">  ");
                sw.WriteLine(@"					<ul class=""nav nav-tabs"" data-tabs=""tabs"">  ");
                sw.WriteLine(@"						<li class=""nav-item"">  ");
                sw.WriteLine(@"							<a class=""nav-link active"" href=""#@(ViewBag.Prefix)TabPaneGenerales"" data-toggle=""tab""> @HCore.Recursos.GetRecurso(""{0}.c0000_generales"")</a>  ", Table.Name);
                sw.WriteLine(@"						</li>  ");
                sw.WriteLine(@"					</ul>  ");
                sw.WriteLine(@"				</div>  ");
                sw.WriteLine(@"			</div>  ");
                sw.WriteLine(@"		</div>  ");
                sw.WriteLine(@"		<div class=""card-body"">  ");
                sw.WriteLine(@"			<div class=""tab-content"">  ");
                sw.WriteLine(@"			<div class=""tab-pane active"" id=""@(ViewBag.Prefix)TabPaneGenerales"">  ");
                sw.WriteLine(@"				CONTENIDO  ");
                sw.WriteLine(@"			</div>  ");
                sw.WriteLine(@"			</div>  ");
                sw.WriteLine(@"		</div>  ");
                sw.WriteLine(@"	</div> ");
                sw.WriteLine(@"	*@ ");

                sw.WriteLine(@"      </div> ");


                sw.WriteLine(@"   </div> ");
                sw.WriteLine(@" </div> ");
                sw.WriteLine();
                sw.WriteLine(@" <script lang = ""javascript""> ");
                sw.WriteLine(@"    function {0}_btnProcesar_click(data) ", Table.Name);
                sw.WriteLine(@"    { ");
                sw.WriteLine(@"       $(""#@(ViewBag.Prefix)Form"").submit(); ");
                sw.WriteLine(@"    } ");
                sw.WriteLine();
                sw.WriteLine(@"    function @(ViewBag.Prefix)FormSuccess(data) ");
                sw.WriteLine(@"    { ");
                sw.WriteLine(@"       var isValidModel = '@ViewData.ModelState.IsValid' == 'True'; ");
                sw.WriteLine(@"       if (isValidModel) ");
                sw.WriteLine(@"       { ");
                sw.WriteLine(@"          DevExpress.ui.notify(""Proceso ejecutado correctamente"",""info""); ");
                sw.WriteLine(@"       } ");
                sw.WriteLine(@"    } ");
                sw.WriteLine();
                sw.WriteLine(@"    function @(ViewBag.Prefix)FormFailure(data) ");
                sw.WriteLine(@"    { ");
                sw.WriteLine(@"       DevExpress.ui.notify(""Error en el formulario"",""error""); ");
                sw.WriteLine(@"    } ");
                sw.WriteLine(@" </script>");

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
