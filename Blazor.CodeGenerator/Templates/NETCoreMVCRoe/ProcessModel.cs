using CodeGenerator.Data;
using CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGenerator.Templates.NETCoreMVCRoe
{
    public class ProcessModel
    {

        public string Name { get; set; } = "APP - Process Model";
        public string Description { get; set; } = "Generates a Base Class Model for .NET";

        #region private property

        private string OutputFolder { get; set; }
        private string FullNameFile { get; set; }
        private string Project { get; set; }
        private TableModel Table { get; set; }
        private TemplateModel Template { get; set; }
        public ProcessModel() { } // Para usarse con reflection

        #endregion

        public ProcessModel(CodeGeneratorModel CodeGeneratorModel)
        {
            OutputFolder = CodeGeneratorModel.PathGenerate + "/APP/" + CodeGeneratorModel.TableGenerated.Comment + "/Models/Process/";
            FullNameFile = CodeGeneratorModel.TableGenerated.Code + "Model.cs";

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

                sw.WriteLine(@"using Hefestos.Backend.Core.Data;");
                sw.WriteLine(@"using System;");
                sw.WriteLine(@"using System.ComponentModel.DataAnnotations;");
                sw.WriteLine();
                sw.WriteLine(@"namespace {0}.UI.Modules.{1}.Models", Project, Table.Comment);
                sw.WriteLine(@"{");
                sw.WriteLine(@"     public partial class {0}Model ", Table.Code);
                sw.WriteLine(@"     {");
                foreach (ColumnModel column in Table.Columns)
                {
                    if (column.Code != "LastUpdate" && column.Code != "UpdatedBy")
                    {
                        sw.WriteLine(@"       [RecursoDisplayName(""{0}"")]", column.Name);
                        sw.WriteLine(@"       //[Required(ErrorMessage = ""El campo {0} es requerido."")]", column.Name);
                        if (GCUtil.GetNetDataType(column).Equals("Boolean") || GCUtil.GetNetDataType(column).Equals("String"))
                            sw.WriteLine(@"       public virtual " + GCUtil.GetNetDataTypeSimply(column) + " " + column.Code + " { get; set; }");
                        else {
                            sw.WriteLine(@"       //[Range(1,int.MaxValue)]");
                            sw.WriteLine(@"       public virtual " + GCUtil.GetNetDataTypeSimply(column) + "? " + GCUtil.PascalCase(column.Code) + " { get; set; }");
                        }
                        sw.WriteLine();
                    }
                }
                sw.WriteLine();
                sw.WriteLine(@"     }");
                sw.WriteLine();
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
