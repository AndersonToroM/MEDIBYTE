using CodeGenerator.Data;
using CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGenerator.Templates.NETCoreMVC
{
    public class EditViewDetail
    {

        public string Name { get; set; } = "APP-07 - Edit View Detail";
        public string Description { get; set; } = "Generates a Razor page Edit View Detail for .NET";

        #region private property

        private string OutputFolder { get; set; }
        private string FullNameFile { get; set; }
        private string Project { get; set; }
        private TableModel Table { get; set; }
        private TemplateModel Template { get; set; }
        public EditViewDetail() { } // Para usarse con reflection

        #endregion

        public EditViewDetail(CodeGeneratorModel CodeGeneratorModel)
        {
            OutputFolder = CodeGeneratorModel.PathGenerate + "/Views/" + CodeGeneratorModel.TableGenerated.Code + "/";
            FullNameFile = "EditDetail.cshtml";

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
                    sw.WriteLine(@"    string UrlDelete = Url.Action(""DeleteDetail"", ""{2}"", new {0} Id = Model.Entity.Id {1}); ", "{", "}", Table.Code);
                    sw.WriteLine();
                    sw.WriteLine(@"    var toolbarDetailconfig = new ToolbarDetailConfiguration(Prefix).Delete(UrlDelete).Save().Cancel(); ");
                    sw.WriteLine(@"} ");
                    sw.WriteLine();
                    sw.WriteLine(@"<partial name=""ViewJS.cshtml"" model=""Model"" view-data=""ViewData"" /> ");
                    sw.WriteLine();
                    sw.WriteLine(@"<div>@(Html.DControls().Toolbar(toolbarDetailconfig))</div>");
                    sw.WriteLine();
                    sw.WriteLine(@"<div> ");
                    sw.WriteLine(@"     <form id=""@(Prefix)Form"" asp-controller=""{0}"" asp-action=""EditDetail"" class=""form-horizontal"" data-ajax-mode=""replace"" data-ajax-update=""#{0}MainPanelForm"" data-ajax-success=""@(Prefix)FormSuccessDetail"" data-ajax-failure=""@(Prefix)FormFailureDetail"" data-ajax=""true""> ", Table.Code);
                    sw.WriteLine(@"         @(Html.DControls().ValidationSummary().ValidationGroup(Prefix + ""ValidationGroup"").ElementAttr(""class"",""ErrorValidationSummary"")) ");
                    sw.WriteLine(@"         @using (Html.DevExtreme().ValidationGroup(Prefix + ""ValidationGroupDetail"")) ");
                    sw.WriteLine(@"         { ");
                    sw.WriteLine(@"             @(Html.DControls().CheckBoxFor(m => m.Entity.IsNew).ID(Prefix + ""IsNew"").Visible(false)) ");
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
                                    sw.WriteLine(@"                @(Html.DControls().DateBoxFor(m => m.Entity.{0}).ID(Prefix + ""{0}"").Visible(false)) ", column.Code);
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
                                        sw.WriteLine(@"                                 .ValidationRules(rules => rules.AddRequired().Message(DApp.GetRequiredResource(""{0}""))) ", Table.Name + "." + column.Name);
                                        sw.WriteLine(@"                                 .ValidationRules(rules => rules.AddRange().Min(1).Message(DApp.GetRequiredResource(""{0}""))) ", Table.Name + "." + column.Name);
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
                                        sw.WriteLine(@"                                 .ValidationRules(rules => rules.AddRequired().Message(DApp.GetRequiredResource(""{0}""))) ", Table.Name + "." + column.Name);
                                    if (column.Length > 0)
                                        sw.WriteLine(@"                                 .ValidationRules(rules => rules.AddStringLength().Min(0).Max({1}).Message(DApp.GetStringLengthResource(""{0}"", {1}))) ", Table.Name + "." + column.Name, column.Length);
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
                                    sw.WriteLine(@"                                     .Editor(e=>e.DateBox().ID(Prefix + ""{0}"") ", column.Code);
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
                                        sw.Write(@"                                 .ValidationRules(rules => rules.AddRequired().Message(DApp.GetRequiredResource(""{0}""))); ", Table.Name + "." + column.Name);
                                    else
                                        sw.Write(";");
                                }
                            }
                        }
                    }
                    sw.WriteLine(@"                     }); ");
                    sw.WriteLine(@"                 }) ");
                    sw.WriteLine(@"             ) ");
                    sw.WriteLine(@"         } ");
                    sw.WriteLine(@"         <div asp-validation-summary=""All"" class=""text-danger"" id=""@(Prefix)ListError""></div> ");
                    sw.WriteLine(@"     </form> ");
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
