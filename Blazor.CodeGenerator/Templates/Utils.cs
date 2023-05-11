using CodeGenerator.Data;
using CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGenerator.Templates.Utils
{
    public class Utils
    {

        #region private property

        private string OutputFolder { get; set; }
        private string FullNameFile { get; set; }

        #endregion

        public Utils(List<TableModel> tables, string PathGenerate)
        {
            OutputFolder = PathGenerate + "/";
            FullNameFile = "Utils.txt";
            GenerateCodeTemplate(tables);
        }

        public void GenerateCodeTemplate(List<TableModel> tables) 
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
                sw.WriteLine("/****************************************  BlazorContext   *************************************************************/");
                foreach (var table in tables)
                {
                    sw.WriteLine(@"     public DbSet<{0}> {0} {1} get; set; {2}", table.Code, "{", "}");
                }
                sw.WriteLine("/***********************************************************************************************************************/");
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("/****************************************  Menu   **********************************************************************/");
                foreach (var table in tables)
                {
                    sw.WriteLine(@"         {");
                    sw.WriteLine(@"             ""Name"": ""{0}"",", table.Code);
                    sw.WriteLine(@"             ""Resource"": ""{0}"",", table.Name);
                    sw.WriteLine(@"         },");
                }
                sw.WriteLine("/***********************************************************************************************************************/");

                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                GCUtil.Errors.Add("La plantilla Utils no se genero por el siguiente error: " + e.Message);
                GCUtil.TemplateWithError.Add("Utils");
                sw.Flush();
                sw.Close();
                File.Delete(fileName);
            }
            

        }


    }
}
