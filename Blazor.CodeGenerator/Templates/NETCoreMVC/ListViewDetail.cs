using CodeGenerator.Data;
using CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGenerator.Templates.NETCoreMVC
{
    public class ListViewDetail
    {

        public string Name { get; set; } = "APP-05 - List View Detail";
        public string Description { get; set; } = "Generates a Razor page for .NET";

        #region private property

        private string OutputFolder { get; set; }
        private string FullNameFile { get; set; }
        private string Project { get; set; }
        private TableModel Table { get; set; }
        private TemplateModel Template { get; set; }
        public ListViewDetail() { } // Para usarse con reflection

        #endregion

        public ListViewDetail(CodeGeneratorModel CodeGeneratorModel)
        {
            OutputFolder = CodeGeneratorModel.PathGenerate + "/Views/" + CodeGeneratorModel.TableGenerated.Code + "/";
            FullNameFile = "ListDetail.cshtml";

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
                    sw.WriteLine(@"@*  Remplazar FATHERModel por el modelo del padre al que pertenece   *@ ");
                    sw.WriteLine(@"@* ");
                    sw.WriteLine(@"@model FATHERModel ");
                    sw.WriteLine(@"@{ ");
                    sw.WriteLine(@"    string Prefix = ""{0}""; ", Table.Code);
                    sw.WriteLine(@"    string UrlOnClick = Url.Action(""EditDetail"", ""{0}""); ", Table.Code);
                    sw.WriteLine(@"    string UrlNew = Url.Action(""NewDetail"", ""{2}"", new {0} IdFather = Model.Entity.Id {1}); ", "{", "}", Table.Code);
                    sw.WriteLine();
                    sw.WriteLine(@"    var DataGridDetailConfig = new DataGridConfiguration<{0}>(Prefix) ", Table.Code);
                    sw.WriteLine(@"        .OnClick(UrlOnClick, new {0} Id = ""Id"" {1}, Prefix + ""MainPanelForm"") ", "{", "}");
                    sw.WriteLine(@"        .New(UrlNew,Model.Entity.IsNew,Prefix + ""MainPanelForm"") ");
                    sw.WriteLine(@"        .Exports(true,true); ");
                    sw.WriteLine(@"} ");
                    sw.WriteLine(@"*@ ");
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine(@"@* ------------ Descomentar el siguiente codigo si es una edicion en maestro --------------- *@");
                    sw.WriteLine(@"@* ");
                    sw.WriteLine();
                    sw.WriteLine(@"<div class=""card cardDeatil""> ");
                    sw.WriteLine(@"    <div class=""card-body"">");
                    sw.WriteLine(@"        <div class=""box-body table-responsive no-padding""> ");
                    sw.WriteLine(@"        @(Html.DControls().DataGridSimple<{0}>(DataGridDetailConfig) ", Table.Code);
                    sw.WriteLine(@"            .ID(""{0}DetalleDataGrid"") ", Table.Code);
                    sw.WriteLine(@"            .DataSource(d => d.Mvc().LoadMethod(""POST"").Controller(""{0}"").LoadAction(""Get"").Key(""Id"")) ", Table.Code);
                    sw.WriteLine(@"            .DataSourceOptions(x=>x.Filter(""['IdFather','=','""+@Model.Entity.Id+""']"")) // Cambiar IdFather por el campo foraneo hacia el padre");
                    sw.WriteLine(@"            .Columns(columns => ");
                    sw.WriteLine(@"            { ");
                    foreach (ColumnModel column in Table.Columns)
                    {
                        if (!(column.Code.Contains("LastUpdate") || column.Code.Contains("UpdatedBy") || column.Code.Contains("CreationDate") || column.Code.Contains("CreatedBy") || column.IsPrimaryKey))
                        {
                            if (column.IsFKIn)
                            {
                                InReferencesModel inReferences = Table.InReferences.Find(x => x.ColumnCode == column.Code);
                                sw.WriteLine(@"             columns.AddFor(m => m.{0}.{1}); ", inReferences.ParentTableCode, inReferences.ParentColumnCode);
                            }
                            else
                                sw.WriteLine(@"             columns.AddFor(m => m.{0}); ", column.Code);
                        }
                    }
                    sw.WriteLine(@"            }) ");
                    sw.WriteLine(@"        ) ");
                    sw.WriteLine(@"        </div> ");
                    sw.WriteLine(@"        <div id=""{0}MainPanelForm""></div> ", Table.Code);
                    sw.WriteLine(@"    </div> ");
                    sw.WriteLine(@"</div> ");
                    sw.WriteLine(@"*@ ");
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine(@"@* ------------ Descomentar el siguiente codigo si es una edicion en grilla y COMENTAR LOS METODOS OnClick y New del DataGridDetailConfig --------------- *@");
                    sw.WriteLine(@"@* ");
                    sw.WriteLine(@"<script> ");
                    sw.WriteLine(@"     function @(Prefix)OnInitNewRowDetalleDataGrid(model) { ");
                    sw.WriteLine(@"         model = GetAuditoryData(model); ");
                    sw.WriteLine(@"         model.data.IdFather = ""@Model.Entity.Id""; // Cambiar IdFather por el campo foraneo hacia el padre ");
                    sw.WriteLine(@"     } ");
                    sw.WriteLine(@"</script> ");
                    sw.WriteLine(@"<div class=""card cardDeatil""> ");
                    sw.WriteLine(@"    <div class=""card-body"">");
                    sw.WriteLine(@"        <div class=""box-body table-responsive no-padding""> ");
                    sw.WriteLine(@"             @(Html.DControls().DataGridSimple<{0}>(DataGridDetailConfig) ", Table.Code);
                    sw.WriteLine(@"                 .ID(""{0}DetalleDataGrid"") ", Table.Code);
                    sw.WriteLine(@"                 .DataSource(d => d.Mvc().LoadMethod(""POST"").Controller(""{0}"").LoadAction(""Get"").Key(""Id"") ", Table.Code);
                    sw.WriteLine(@"                     .InsertAction(""AddInGrid"").InsertMethod(""POST"").UpdateAction(""ModifyInGrid"").UpdateMethod(""POST"").DeleteAction(""DeleteInGrid"").DeleteMethod(""POST"") ");
                    sw.WriteLine(@"                 ) ");
                    sw.WriteLine(@"                 .DataSourceOptions(x=>x.Filter(""['IdFather','=','""+@Model.Entity.Id+""']"")) // Cambiar IdFather por el campo foraneo hacia el padre");
                    sw.WriteLine(@"                 .Editing(editing => editing.Mode(GridEditMode.Batch).AllowAdding(!Model.Entity.IsNew).AllowUpdating(true).AllowDeleting(true)) ");
                    sw.WriteLine(@"                 .OnInitNewRow(Prefix + ""OnInitNewRowDetalleDataGrid"") ", Table.Code);
                    sw.WriteLine(@"                 .OnRowInserted(""OnRowInsertedDetalleDataGrid"").OnRowUpdated(""OnRowUpdatedDetalleDataGrid"").OnRowRemoved(""OnRowRemovedDetalleDataGrid"").OnDataErrorOccurred(""OnDataErrorOccurredDetalleDataGrid"") ", Table.Code);
                    sw.WriteLine(@"                 .Columns(columns => ");
                    sw.WriteLine(@"                 { ");
                    foreach (ColumnModel column in Table.Columns)
                    {
                        if (!(column.Code.Contains("LastUpdate") || column.Code.Contains("UpdatedBy") || column.Code.Contains("CreationDate") || column.Code.Contains("CreatedBy") || column.IsPrimaryKey))
                        {
                            string NetDataType = GCUtil.GetNetDataTypeSimply(column);
                            if (column.IsFKIn)
                            {
                                InReferencesModel inReferences = Table.InReferences.Find(x => x.ColumnCode == column.Code);
                                sw.WriteLine(@"                     columns.AddFor(m => m.{0}) ", column.Code);
                                if (column.IsRequired)
                                {
                                    sw.WriteLine(@"                         .ValidationRules(rules => rules.AddRequired().Message(DApp.GetRequiredResource(""{0}""))) ", Table.Name + "." + column.Name);
                                    sw.WriteLine(@"                         .ValidationRules(rules => rules.AddRange().Min(1).Message(DApp.GetRequiredResource(""{0}""))) ", Table.Name + "." + column.Name);
                                }
                                sw.WriteLine(@"                             .Lookup(lookup => lookup ", column.Code);
                                sw.WriteLine(@"                             .DataSource(d => d.Mvc().LoadMethod(""POST"").Controller(""{0}"").LoadAction(""Get{1}"").Key(""Id"")) ", Table.Code, column.Code);
                                sw.WriteLine(@"                             .DataSourceOptions(o => o.Paginate(true).PageSize(10)) ");
                                sw.WriteLine(@"                             .ValueExpr(""Id"").DisplayExpr(""Id"")); ");
                            }
                            else
                            {
                                if (NetDataType == "string")
                                {
                                    sw.WriteLine(@"                     columns.AddFor(m => m.{0}) ", column.Code);
                                    if (column.IsRequired)
                                        sw.WriteLine(@"                         .ValidationRules(rules => rules.AddRequired().Message(DApp.GetRequiredResource(""{0}""))) ", Table.Name + "." + column.Name);
                                    if (column.Length > 0)
                                        sw.WriteLine(@"                         .ValidationRules(rules => rules.AddStringLength().Min(0).Max({1}).Message(DApp.GetStringLengthResource(""{0}"", {1}))) ", Table.Name + "." + column.Name, column.Length);
                                    sw.WriteLine(@"                     ; ");
                                }
                                else if (NetDataType == "decimal" || NetDataType == "int")
                                {
                                    sw.WriteLine(@"                     columns.AddFor(m => m.{0}).Format(""#,##0.##"") ", column.Code);
                                    if (column.IsRequired)
                                        sw.WriteLine(@"                         .ValidationRules(rules => rules.AddRequired().Message(DApp.GetRequiredResource(""{0}""))); ", Table.Name + "." + column.Name);
                                }
                                else if (NetDataType == "DateTime")
                                {
                                    sw.WriteLine(@"                     columns.AddFor(m => m.{0}).Format(DApp.DefaultLanguage.DateFormat) ", column.Code);
                                    if (column.IsRequired)
                                        sw.WriteLine(@"                         .ValidationRules(rules => rules.AddRequired().Message(DApp.GetRequiredResource(""{0}""))); ", Table.Name + "." + column.Name);
                                }
                                else
                                {
                                    sw.WriteLine(@"                     columns.AddFor(m => m.{0}) ", column.Code);
                                    if (column.IsRequired)
                                        sw.WriteLine(@"                         .ValidationRules(rules => rules.AddRequired().Message(DApp.GetRequiredResource(""{0}""))); ", Table.Name + "." + column.Name);
                                }
                            }
                        }
                    }
                    sw.WriteLine(@"             }) ");
                    sw.WriteLine(@"         ) ");
                    sw.WriteLine(@"        </div> ");
                    sw.WriteLine(@"    </div> ");
                    sw.WriteLine(@"</div> ");
                    sw.WriteLine(@"*@ ");

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
