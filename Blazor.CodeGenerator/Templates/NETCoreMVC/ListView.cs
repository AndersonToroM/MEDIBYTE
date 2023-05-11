using CodeGenerator.Data;
using CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGenerator.Templates.NETCoreMVC
{
    public class ListView
    {

        public string Name { get; set; } = "APP-04 - List View";
        public string Description { get; set; } = "Generates a Razor page for .NET";

        #region private property

        private string OutputFolder { get; set; }
        private string FullNameFile { get; set; }
        private string Project { get; set; }
        private TableModel Table { get; set; }
        private TemplateModel Template { get; set; }
        public ListView() { } // Para usarse con reflection

        #endregion

        public ListView(CodeGeneratorModel CodeGeneratorModel)
        {
            OutputFolder = CodeGeneratorModel.PathGenerate + "/Views/" + CodeGeneratorModel.TableGenerated.Code + "/";
            FullNameFile = "List.cshtml";

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
                    sw.WriteLine(@"@{ ");
                    sw.WriteLine(@"    string Prefix = ""{0}""; ", Table.Code);
                    sw.WriteLine(@"    string UrlClick = Url.Action(""Edit"", ""{0}""); ", Table.Code);
                    sw.WriteLine(@"    string UrlNew = Url.Action(""New"", ""{0}""); ", Table.Code);
                    sw.WriteLine();
                    sw.WriteLine(@"    var DataGridConfig = new DataGridConfiguration<{0}>(Prefix) ", Table.Code);
                    sw.WriteLine(@"        .OnClick(UrlClick, new {0} Id = ""Id"" {1}) ", "{", "}");
                    sw.WriteLine(@"        .New(UrlNew,DApp.ActionViewSecurity(Context,UrlNew)) ");
                    sw.WriteLine(@"        .Exports(true,true) ");
                    sw.WriteLine(@"        .ToolbarTop(DApp.DefaultLanguage.GetResource(""{0}""), ""mainPanel""); ", Table.Name);
                    sw.WriteLine(@"} ");
                    sw.WriteLine();
                    sw.WriteLine(@"<div class=""box-body table-responsive no-padding""> ");
                    sw.WriteLine(@"@(Html.DControls().DataGridSimple<{0}>(DataGridConfig) ", Table.Code);
                    sw.WriteLine(@"    .ID(""{0}DataGrid"") ", Table.Code);
                    sw.WriteLine(@"    .DataSource(d => d.Mvc().LoadMethod(""POST"").Controller(""{0}"").LoadAction(""Get"").Key(""Id"")) ", Table.Code);
                    sw.WriteLine(@"    .Columns(columns => ");
                    sw.WriteLine(@"    { ");
                    foreach (ColumnModel column in Table.Columns)
                    {
                        if (!(column.Code.Contains("LastUpdate") || column.Code.Contains("UpdatedBy") || column.Code.Contains("CreationDate") || column.Code.Contains("CreatedBy") || column.IsPrimaryKey))
                        {
                            if (column.IsFKIn)
                            {
                                InReferencesModel inReference = Table.InReferences.Find(x => x.ColumnCode == column.Code);
                                string columnReference = inReference.ColumnCode.Substring(0, inReference.ColumnCode.Length - 2);
                                sw.WriteLine(@"        columns.AddFor(m => m.{0}.{1}); ", columnReference, inReference.ParentColumnCode);
                            }else
                                sw.WriteLine(@"        columns.AddFor(m => m.{0}); ", column.Code);
                        }
                    }
                    sw.WriteLine(@"    }) ");
                    sw.WriteLine(@") ");
                    sw.WriteLine(@"</div> ");
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

    }
}
