using CodeGenerator.Data;
using CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGenerator.Templates.NETCoreMVC
{
    public class EditView
    {

        public string Name { get; set; } = "APP-06 - Edit View";
        public string Description { get; set; } = "Generates a Razor page Edit View for .NET";

        #region private property

        private string OutputFolder { get; set; }
        private string FullNameFile { get; set; }
        private string Project { get; set; }
        private TableModel Table { get; set; }
        private TemplateModel Template { get; set; }
        public EditView() { } // Para usarse con reflection

        #endregion

        public EditView(CodeGeneratorModel CodeGeneratorModel)
        {
            OutputFolder = CodeGeneratorModel.PathGenerate + "/Views/" + CodeGeneratorModel.TableGenerated.Code + "/";
            FullNameFile = "Edit.cshtml";

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
                    sw.WriteLine();
                    sw.WriteLine(@"    string UrlSave = Url.Action(""Edit"", ""{0}""); ", Table.Code);
                    sw.WriteLine(@"    string UrlNew = Url.Action(""New"", ""{0}""); ", Table.Code);
                    sw.WriteLine(@"    string UrlDelete = Url.Action(""Delete"", ""{2}"", new {0} Id = Model.Entity.Id {1}); ", "{", "}", Table.Code);
                    sw.WriteLine(@"    string UrlBack = Url.Action(""ListPartial"", ""{0}""); ", Table.Code);
                    sw.WriteLine();
                    sw.WriteLine(@"    var ToolbarConfig = new ToolbarConfiguration(Prefix) ");
                    sw.WriteLine(@"        .Title(DApp.DefaultLanguage.GetResource(""{0}"")) ", Table.Name);
                    sw.WriteLine(@"        .Save(DApp.ActionViewSecurity(Context, UrlSave),Model.Entity.IsNew) ");
                    sw.WriteLine(@"        .New(UrlNew, DApp.ActionViewSecurity(Context, UrlNew)) ");
                    sw.WriteLine(@"        .Delete(UrlDelete,DApp.ActionViewSecurity(Context,UrlDelete),UrlBack,Model.Entity.IsNew) ");
                    sw.WriteLine(@"        .Back(UrlBack); ");
                    sw.WriteLine(@"}");
                    sw.WriteLine();
                    sw.WriteLine(@"<partial name=""ViewJS.cshtml"" model=""Model"" view-data=""ViewData"" /> ");
                    sw.WriteLine();
                    sw.WriteLine(@"@(Html.DControls().Toolbar(ToolbarConfig)) ");
                    sw.WriteLine();
                    sw.WriteLine(@"<div> ");
                    sw.WriteLine(@"    <div id = ""{0}MainPanelForm""> ", Table.Code);
                    sw.WriteLine(@"        <form id = ""@(Prefix)Form"" asp-controller=""{0}"" asp-action=""Edit"" class=""form-horizontal"" data-ajax-mode=""replace"" data-ajax-update=""#mainPanel"" data-ajax-success=""@(Prefix)FormSuccess"" data-ajax-failure=""@(Prefix)FormFailure"" data-ajax=""true""> ", Table.Code);
                    sw.WriteLine(@"            @(Html.DControls().ValidationSummary().ValidationGroup(Prefix + ""ValidationGroup"").ElementAttr(""class"",""ErrorValidationSummary"")) ");
                    sw.WriteLine(@"            @using (Html.DevExtreme().ValidationGroup(Prefix + ""ValidationGroup"")) ");
                    sw.WriteLine(@"            { ");
                    sw.WriteLine(@"                @(Html.DControls().CheckBoxFor(m => m.Entity.IsNew).ID(Prefix + ""IsNew"").Visible(false)) ");
                    if (Table.Columns != null && Table.Columns.Count > 0)
                    {
                        foreach (var column in Table.Columns)
                        {
                            string NetDataType = GCUtil.GetNetDataType(column);
                            if (column.IsPrimaryKey || column.Code.Contains("LastUpdate") || column.Code.Contains("UpdatedBy") || column.Code.Contains("CreationDate") || column.Code.Contains("CreatedBy") || column.IsPrimaryKey)
                            {
                                if ((NetDataType == "Int" || NetDataType == "Int16" || NetDataType == "Int32" || NetDataType == "Int64" || NetDataType == "Short"))
                                    sw.WriteLine(@"                @(Html.DControls().NumberBoxFor(m => m.Entity.{0}).ID(Prefix + ""{0}"").Visible(false)) ", column.Code);
                                else if ((NetDataType == "Double" || NetDataType == "Float" || NetDataType == "Decimal"))
                                    sw.WriteLine(@"                @(Html.DControls().NumberBoxFor(m => m.Entity.{0}).ID(Prefix + ""{0}"").Visible(false)) ", column.Code);
                                else if (NetDataType == "Boolean")
                                    sw.WriteLine(@"                @(Html.DControls().CheckBoxFor(m => m.Entity.{0}).ID(Prefix + ""{0}"").Visible(false)) ", column.Code);
                                else if (NetDataType == "DateTime")
                                    sw.WriteLine(@"                @(Html.DControls().DateBoxFor(m => m.Entity.{0}).ID(Prefix + ""{0}"").Type(DateBoxType.DateTime).Visible(false)) ", column.Code);
                                else
                                    sw.WriteLine(@"                @(Html.DControls().TextBoxFor(m => m.Entity.{0}).ID(Prefix + ""{0}"").Visible(false)) ", column.Code);

                            }
                        }
                        sw.WriteLine();
                        sw.WriteLine(@"                @(Html.DControls().Form<{0}Model>() ", Table.Code);
                        sw.WriteLine(@"                    .ID(""{0}ModelForm"").LabelLocation(FormLabelLocation.Top).ShowValidationSummary(false).FormData(Model) ", Table.Code);
                        sw.WriteLine(@"                    .Items(items => ");
                        sw.WriteLine(@"                    { ");
                        sw.WriteLine(@"                         items.AddGroup().ColCount(12).Caption(DApp.DefaultLanguage.GetResource(""DEFAULT.INFORMATIONGENERAL""))");
                        sw.WriteLine(@"                             .Items(groupItems =>{");

                        foreach (var column in Table.Columns)
                        {
                            string NetDataType = GCUtil.GetNetDataTypeSimply(column);
                            if (!(column.IsPrimaryKey || column.Code.Contains("LastUpdate") || column.Code.Contains("UpdatedBy") || column.Code.Contains("CreationDate") || column.Code.Contains("CreatedBy") || column.IsPrimaryKey))
                            {
                                if (column.IsFKIn)
                                {
                                    InReferencesModel inReferences = Table.InReferences.Find(x => x.ColumnCode == column.Code);
                                    sw.WriteLine(@"                                groupItems.AddSimpleFor(m => m.Entity.{0}).ColSpan(1) ", column.Code);
                                    if (column.IsRequired)
                                    {
                                        sw.WriteLine(@"                                .ValidationRules(rules => rules.AddRequired().Message(DApp.GetRequiredResource(""{0}""))) ", Table.Name + "." + column.Name);
                                        sw.WriteLine(@"                                .ValidationRules(rules => rules.AddRange().Min(1).Message(DApp.GetRequiredResource(""{0}""))) ", Table.Name + "." + column.Name);
                                    }
                                    sw.WriteLine(@"                                     .Editor(e => e.SelectBox().ID(Prefix + ""{0}"") ", column.Code);
                                    sw.WriteLine(@"                                     .DataSource(d => d.Mvc().LoadMethod(""POST"").Controller(""{0}"").LoadAction(""Get{1}"").Key(""Id"")) ", Table.Code, column.Code);
                                    sw.WriteLine(@"                                     .DataSourceOptions(o => o.Paginate(true).PageSize(10)) ");
                                    sw.WriteLine(@"                                     .ValueExpr(""Id"").DisplayExpr(""{0}"").SearchExpr(""{0}"") ", inReferences.ParentColumnCode);
                                    sw.WriteLine(@"                                     .SearchEnabled(true).ShowClearButton(true) ");
                                    sw.WriteLine(@"                                ); ");
                                }
                                else if (NetDataType == "string")
                                {
                                    sw.WriteLine(@"                                groupItems.AddSimpleFor(m => m.Entity.{0}).ColSpan(1) ", column.Code);
                                    if (column.IsRequired)
                                        sw.WriteLine(@"                                .ValidationRules(rules => rules.AddRequired().Message(DApp.GetRequiredResource(""{0}""))) ", Table.Name + "." + column.Name);
                                    if(column.Length > 0)
                                        sw.WriteLine(@"                                .ValidationRules(rules => rules.AddStringLength().Min(0).Max({1}).Message(DApp.GetStringLengthResource(""{0}"", {1}))) ", Table.Name + "." + column.Name, column.Length);
                                    if (column.Type.Equals("text") || column.Length > 500)
                                        sw.WriteLine(@"                                     .Editor(e => e.TextArea().ID(Prefix + ""{0}"")); ", column.Code);
                                    else
                                        sw.WriteLine(@"                                     .Editor(e => e.TextBox().ID(Prefix + ""{0}"").ShowClearButton(true)); ", column.Code);
                                }
                                else if (NetDataType == "decimal" || NetDataType == "int")
                                {
                                    sw.WriteLine(@"                                groupItems.AddSimpleFor(m => m.Entity.{0}).ColSpan(1) ", column.Code);
                                    if (column.IsRequired)
                                        sw.WriteLine(@"                                 .ValidationRules(rules => rules.AddRequired().Message(DApp.GetRequiredResource(""{0}""))) ", Table.Name + "." + column.Name);
                                    sw.WriteLine(@"                                     .Editor(e=>e.NumberBox().ID(Prefix + ""{0}"").Format(""#,##0.##"").ShowClearButton(true).ShowSpinButtons(true)); ", column.Code);
                                }
                                else if (NetDataType == "DateTime")
                                {
                                    sw.WriteLine(@"                                groupItems.AddSimpleFor(m => m.Entity.{0}).ColSpan(1) ", column.Code);
                                    if (column.IsRequired)
                                        sw.WriteLine(@"                                 .ValidationRules(rules => rules.AddRequired().Message(DApp.GetRequiredResource(""{0}""))) ", Table.Name + "." + column.Name);
                                    sw.WriteLine(@"                                     .Editor(e=>e.DateBox().ID(Prefix + ""{0}"").Type(DateBoxType.Time) ", column.Code);
                                    sw.WriteLine(@"                                     .DisplayFormat(DApp.DefaultLanguage.DateFormat).Placeholder(DateTime.Now.ToString(DApp.DefaultLanguage.DateFormat)).ShowClearButton(true).UseMaskBehavior(true)); ", column.Code);
                                }
                                else if (NetDataType == "bool")
                                {
                                    sw.WriteLine(@"                                groupItems.AddSimpleFor(m => m.Entity.{0}).Label(x=>x.Visible(false)).ColSpan(1) ", column.Code);
                                    sw.WriteLine(@"                                     .Editor(e=>e.CheckBox().ID(Prefix + ""{0}"").Text(DApp.DefaultLanguage.GetResource(""{1}""))); ", column.Code, Table.Name + "." + column.Name);
                                }
                                else
                                {
                                    sw.Write(@"                                groupItems.AddSimpleFor(m => m.Entity.{0}).ColSpan(1) ", column.Code);
                                    if (column.IsRequired)
                                        sw.Write(@"                                .ValidationRules(rules => rules.AddRequired().Message(DApp.GetRequiredResource(""{0}""))); ", Table.Name + "." + column.Name);
                                    else
                                        sw.Write(";");
                                }
                            }
                        }
                    }
                    sw.WriteLine(@"                         }); ");
                    sw.WriteLine(@"                    }) ");
                    sw.WriteLine(@"                ) ");
                    sw.WriteLine(@"            } ");
                    sw.WriteLine(@"        </form> ");
                    sw.WriteLine();
                    sw.WriteLine(@"        @* Si el maestro maneja detalle descomentar el siguiente codigo y remplazar la palabra ""MAESTRODETALLE"" *@");
                    sw.WriteLine(@"        @* ");
                    sw.WriteLine(@"        <div> ");
                    sw.WriteLine(@"           <ul class=""nav nav-tabs"" role=""tablist""> ");
                    sw.WriteLine(@"               <li role=""presentation"" class=""active""> ");
                    sw.WriteLine(@"                   <a href=""#MAESTRODETALLE"" aria-controls=""MAESTRODETALLE"" role=""tab"" data-toggle=""tab""> ");
                    sw.WriteLine(@"                       @DApp.DefaultLanguage.GetResource(""MAESTRODETALLE"") ");
                    sw.WriteLine(@"                   </a> ");
                    sw.WriteLine(@"               </li> ");
                    sw.WriteLine(@"           </ul> ");
                    sw.WriteLine(@"           <div class=""tab-content""> ");
                    sw.WriteLine(@"               <div role=""tabpanel"" class=""tab-pane active"" id=""MAESTRODETALLE"">");
                    sw.WriteLine(@"                   <partial name=""../MAESTRODETALLE/ListDetail.cshtml"" model=Model view-data=""ViewData"" />");
                    sw.WriteLine(@"               </div>");
                    sw.WriteLine(@"           </div>");
                    sw.WriteLine(@"        </div>");
                    sw.WriteLine(@"        *@");
                    sw.WriteLine();
                    sw.WriteLine(@"    </div> ");
                    sw.WriteLine(@"</div> ");
                    sw.WriteLine();
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
