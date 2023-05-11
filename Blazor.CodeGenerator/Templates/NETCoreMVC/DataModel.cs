using CodeGenerator.Data;
using CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGenerator.Templates.NETCoreMVC
{
    public class DataModel
    {

        public string Name { get; set; } = "APP-02 - Model";
        public string Description { get; set; } = "Generates a Base Class Model for .NET";

        #region private property

        private string OutputFolder { get; set; }
        private string FullNameFile { get; set; }
        private string Project { get; set; }
        private TableModel Table { get; set; }
        private TemplateModel Template { get; set; }
        public DataModel() { } // Para usarse con reflection

        #endregion

        public DataModel(CodeGeneratorModel CodeGeneratorModel)
        {
            OutputFolder = CodeGeneratorModel.PathGenerate + "/Models/";
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
                if (Table.Columns.Exists(x=>x.IsPrimaryKey))
                {
                    sw.WriteLine(@"using {0}.Infrastructure.Entities;", Project);
                    sw.WriteLine();
                    sw.WriteLine(@"namespace {0}.WebApp.Models", Project);
                    sw.WriteLine(@"{");
                    sw.WriteLine();

                    sw.WriteLine(@"   public partial class {0}Model", Table.Code);
                    sw.WriteLine(@"   {");
                    sw.WriteLine(@"      public {0} Entity {1} get; set; {2}", Table.Code, "{", "}");
                    sw.WriteLine();
                    sw.WriteLine(@"      public {0}Model()", Table.Code);
                    sw.WriteLine(@"      {");
                    sw.WriteLine(@"         Entity = new {0}();", Table.Code);
                    sw.WriteLine(@"      }");
                    sw.WriteLine();
                    sw.WriteLine(@"   }");
                    sw.WriteLine();
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
