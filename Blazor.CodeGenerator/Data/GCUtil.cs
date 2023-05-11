using CodeGenerator.Models;
using Newtonsoft.Json;
using PluralizeService.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeGenerator.Data
{

    public static class GCUtil
    {
        public static List<DBSettings> ListDBSettings { get; set; }
        public static List<DataBaseMapModel> DataBaseInfo { get; set; } = new List<DataBaseMapModel>();
        public static List<string> Errors { get; set; } = new List<string>();
        public static List<string> TemplateWithError { get; set; } = new List<string>();
        public static void Reset()
        {
            Errors = new List<string>();
            TemplateWithError = new List<string>();
        }

        public static CodeGeneratorModel SetTableInfo(CodeGeneratorModel CodeGeneratorModel)
        {
            CodeGeneratorModel.TableGenerated.Columns = GCUtil.DataBaseInfo.FirstOrDefault(x => x.NumberConnection == CodeGeneratorModel.ConexionActual).Columns;
            CodeGeneratorModel.TableGenerated.InReferences = GCUtil.DataBaseInfo.FirstOrDefault(x => x.NumberConnection == CodeGeneratorModel.ConexionActual).InReferences;
            CodeGeneratorModel.TableGenerated.OutReferences = GCUtil.DataBaseInfo.FirstOrDefault(x => x.NumberConnection == CodeGeneratorModel.ConexionActual).OutReferences;
            CodeGeneratorModel.TableGenerated.Indexes = GCUtil.DataBaseInfo.FirstOrDefault(x => x.NumberConnection == CodeGeneratorModel.ConexionActual).Indexes;

            return CodeGeneratorModel;
        }

        //public static string Project { get; set; }
        //public static string Template { get; set; }
        public static string PathTemp { get; set; } = Path.GetTempPath();
        //public static string PathGenerate { get; set; } = PathTemp;
        public static int Framework { get; set; }

        public static string CleanName(string name, bool NoPrefix = false)
        {
            Regex cleanRegEx = new Regex(@"\s+|_|-|\.", RegexOptions.Compiled);

            if (name.Contains("_"))
            {
                String[] splitString = name.Split(Char.Parse("_"));
                name = "";

                int index = (NoPrefix ? 1 : 0);

                for (int i = index; i < splitString.Length; i++)
                {
                    name += char.ToUpper(splitString[i][0]) + splitString[i].ToLower().Substring(1);
                }
            }
            return cleanRegEx.Replace(name, "");
        }

        public static string CamelCase(string name, bool NoPrefix = false)
        {
            string output = CleanName(name, NoPrefix);
            return char.ToLower(output[0]) + output.Substring(1);
        }

        public static string PascalCase(string name, bool NoPrefix = false)
        {
            string output = CleanName(name, NoPrefix);
            int i = 0;
            bool firstIsNumber = int.TryParse(output[0].ToString(), out i);
            if (firstIsNumber)
                return "F" + output[0] + output.Substring(1);
            else
                return char.ToUpper(output[0]) + output.Substring(1);
        }


        public static string MakeSingle(string name)
        {
            if (GetFWDominusTables().Exists(x => x.Equals(name, StringComparison.OrdinalIgnoreCase)))
                name = MakeSingleING(name);
            //else
            //    name = MakeSingleESP(name);

            return name;
        }

        public static List<string> GetFWDominusTables()
        {
            List<string> FWDominusTables = new List<string>();
            if (FWDominusTables.Count == 0)
            {
                FWDominusTables.Add("Audits");
                FWDominusTables.Add("JobDetails");
                FWDominusTables.Add("JobLogs");
                FWDominusTables.Add("Jobs");
                FWDominusTables.Add("LanguageResources");
                FWDominusTables.Add("MenuActions");
                FWDominusTables.Add("Menus");
                FWDominusTables.Add("Offices");
                FWDominusTables.Add("ProfileUsers");
                FWDominusTables.Add("Profiles");
                FWDominusTables.Add("ProfileUsers");
                FWDominusTables.Add("Users");
                FWDominusTables.Add("UserOffices");
            }
            return FWDominusTables;
        }

        public static string MakeSingleESP(string name)
        {
            Regex plural1 = new Regex("(?<keep>[^aeiou])ies$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)s$");
            Regex plural3 = new Regex("(?<keep>[sxzh])es$");
            Regex plural4 = new Regex("(?<keep>[^sxzhyu])es$");
            Regex plural5 = new Regex("(?<keep>[^sxzhyu])s$");

            if (plural1.IsMatch(name))
                return plural1.Replace(name, "${keep}y");
            else if (plural3.IsMatch(name))
                return plural3.Replace(name, "${keep}");
            else if (plural2.IsMatch(name))
                return plural2.Replace(name, "${keep}");
            else if (plural4.IsMatch(name))
                return plural4.Replace(name, "${keep}");
            else if (plural5.IsMatch(name))
                return plural5.Replace(name, "${keep}");
            else if (name.ToUpper().EndsWith("S"))
                return name.Substring(0, name.Length - 1);
            return name;
        }

        public static string MakeSingleING(string name)
        {
            Regex plural1 = new Regex("(?<keep>[^aeiou])ies$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)s$");
            Regex plural3 = new Regex("(?<keep>[sxzh])es$");
            Regex plural4 = new Regex("(?<keep>[^sxzhyu])s$");
            //Regex plural5 = new Regex("(?<keep>[^n])nes$");
            //Regex plural6 = new Regex("(?<keep>[^l])les$");
            //Regex plural7 = new Regex("(?<keep>[^d])des$");
            //Regex plural8 = new Regex("(?<keep>[^r])res$");
            //Regex plural9 = new Regex("(?<keep>[^bl])bles$");
            //if (plural9.IsMatch(name))
            //    return plural9.Replace(name, "${keep}ble");
            //if (plural8.IsMatch(name))
            //    return plural8.Replace(name, "${keep}r");
            //if (plural7.IsMatch(name))
            //    return plural7.Replace(name, "${keep}d");
            //if (plural6.IsMatch(name))
            //    return plural6.Replace(name, "${keep}l");
            //else if (plural5.IsMatch(name))
            //    return plural5.Replace(name, "${keep}n");
            if (plural1.IsMatch(name))
                return plural1.Replace(name, "${keep}y");
            else if (plural3.IsMatch(name))
                return plural3.Replace(name, "${keep}");
            else if (plural2.IsMatch(name))
                return plural2.Replace(name, "${keep}");
            else if (plural4.IsMatch(name))
                return plural4.Replace(name, "${keep}");
            else if (name.ToUpper().EndsWith("S"))
                return name.Substring(0, name.Length - 1);
            return name;
        }

        public static string GetNumber(string name)
        {
            String[] splitString = name.Split(Char.Parse("_"));
            bool havePrefixNumber = splitString[0].Any(c => char.IsDigit(c));
            if (havePrefixNumber)
                return char.ToUpper(splitString[0][0]) + splitString[0].Substring(1);
            else
                return null;
        }

        public static string ColumnCodeNoNumber(string ColumnName)
        {
            var codeNoNumber = ColumnName.Substring(ColumnName.IndexOf("_") + 1).Replace("_"," ");
            return new CultureInfo("en-US").TextInfo.ToTitleCase(codeNoNumber);
        }

        public static string GetOrderByJavaDAO(List<ColumnModel> Columns)
        {
            string orderby = "";

            if (!Columns.Exists(x=>x.IsPrimaryKey)) {
                return "a." + GCUtil.CamelCase(Columns[0].CodeJava);
            }

            List<ColumnModel> ColumnsPrimaryKeys = Columns.Where(x => x.IsPrimaryKey).ToList();
            for (int i = 0; i < ColumnsPrimaryKeys.Count; i++)
            {
                if (i == 0)
                    orderby += "a." + GCUtil.CamelCase(Columns[i].CodeJava);
                else
                    orderby += ",a." + GCUtil.CamelCase(Columns[i].CodeJava);
            }
            return orderby + " asc";
        }

        public static string GetJavaDataType(ColumnModel ColumnModel)
        {
            if (ColumnModel.Type == "varchar" || ColumnModel.Type == "char" || ColumnModel.Type == "nvarchar" || ColumnModel.Type == "xml")
                return "String";
            if (ColumnModel.Type == "datetime" || ColumnModel.Type == "timestamp" || ColumnModel.Type == "date" || ColumnModel.Type == "time")
                return "Calendar";
            if (ColumnModel.Type == "tinyint" || ColumnModel.Type == "smallint" || ColumnModel.Type == "bit")
                return "Short";
            if (ColumnModel.Type == "int" || ColumnModel.Type == "bigint")
                return "Integer";
            if (ColumnModel.Type == "money" || ColumnModel.Type == "decimal" || ColumnModel.Type == "numeric" || ColumnModel.Type == "float" || ColumnModel.Type == "smallmoney")
                return "BigDecimal";
            if (ColumnModel.Type == "image" || ColumnModel.Type == "varbinary")
                return "byte[]";
            return null;
        }

        public static string GetNetDataType(ColumnModel ColumnModel)
        {
            if (ColumnModel.Type == "varchar" || ColumnModel.Type == "char" || ColumnModel.Type == "nvarchar" || ColumnModel.Type == "xml" || ColumnModel.Type == "text")
                return "String";
            if (ColumnModel.Type == "datetime" || ColumnModel.Type == "date" || ColumnModel.Type == "timestamp" || ColumnModel.Type == "time")
                return "DateTime";
            if (ColumnModel.Type == "bit")
                return "Boolean";
            if (ColumnModel.Type == "money" || ColumnModel.Type == "decimal" || ColumnModel.Type == "numeric" || ColumnModel.Type == "float" || ColumnModel.Type == "smallmoney")
                return "Decimal";
            if (ColumnModel.Type == "bigint")
                return "Int64";
            if (ColumnModel.Type == "int")
                return "Int32";
            if (ColumnModel.Type == "smallint")
                return "Int16";
            if (ColumnModel.Type == "tinyint")
                return "Byte";
            if (ColumnModel.Type == "image" || ColumnModel.Type == "varbinary")
                return "Byte[]";
            if (ColumnModel.Type == "uniqueidentifier")
                return "Guid";
            return null;
        }

        public static string GetNetDataTypeSimply(ColumnModel ColumnModel)
        {
            if (ColumnModel.Type == "varchar" || ColumnModel.Type == "char" || ColumnModel.Type == "nvarchar" || ColumnModel.Type == "xml" || ColumnModel.Type == "text")
                return "string";
            if (ColumnModel.Type == "datetime" || ColumnModel.Type == "timestamp" || ColumnModel.Type == "date" || ColumnModel.Type == "time")
                return "DateTime";
            if (ColumnModel.Type == "bit")
                return "bool";
            if (ColumnModel.Type == "money" || ColumnModel.Type == "decimal" || ColumnModel.Type == "numeric" || ColumnModel.Type == "float" || ColumnModel.Type == "smallmoney")
                return "decimal";
            if (ColumnModel.Type == "bigint" || ColumnModel.Type == "int")
                return "int";
            if (ColumnModel.Type == "smallint")
                return "short";
            if (ColumnModel.Type == "tinyint")
                return "byte";
            if (ColumnModel.Type == "image" || ColumnModel.Type == "varbinary")
                return "byte[]";
            return null;
        }

        public static string GetPkStringColsWhitDataType(TableModel table)
        {
            var pks = table.Columns.Where(x => x.IsPrimaryKey).ToList();
            string cols = "";
            for (int i = 0; i < pks.Count; i++)
            {
                if (i == 0)
                    cols += GetNetDataType(pks[i])  + " " + GCUtil.CamelCase(pks[i].Code);
                else
                    cols += "," + GetNetDataType(pks[i]) + " " + GCUtil.CamelCase(pks[i].Code);
            }
            return cols;
        }

        public static string GetPkStringColsAndNameModel3(TableModel table)
        {
            var pks = table.Columns.Where(x => x.IsPrimaryKey).ToList();
            string cols = "";
            for (int i = 0; i < pks.Count; i++)
            {
                if (i == 0)
                    cols += GCUtil.CamelCase((pks[i].Code)) + " = " + "Model." + (pks[i].Code) + "";
                else
                    cols += ", " + GCUtil.CamelCase((pks[i].Code)) + " = " + "Model." + (pks[i].Code) + "";
            }
            return cols;
        }

        public static string GetPkStringColsModel(TableModel table)
        {
            var pks = table.Columns.Where(x => x.IsPrimaryKey).ToList();
            string cols = "";
            for (int i = 0; i < pks.Count; i++)
            {
                if (i == 0)
                    cols += "\"" + (pks[i].Code) + "\"";
                else
                    cols += ", \"" + (pks[i].Code) + "\"";
            }
            return cols;
        }

        public static string GetPkStringCols(TableModel table)
        {
            var pks = table.Columns.Where(x => x.IsPrimaryKey).ToList();
            string cols = "";
            for (int i = 0; i < pks.Count; i++)
            {
                if (i == 0)
                    cols += GCUtil.CamelCase(pks[i].Code);
                else
                    cols += "," + GCUtil.CamelCase(pks[i].Code);
            }
            return cols;
        }

        public static string GetPkStringColsQuery(TableModel table)
        {
            var pks = table.Columns.Where(x => x.IsPrimaryKey).ToList();
            string cols = "";
            for (int i = 0; i < pks.Count; i++)
            {
                if (i == 0)
                    cols += " x => x." + pks[i].Code + " == " + GCUtil.CamelCase(pks[i].Code);
                else
                    cols += " && x." + pks[i].Code + " == " + GCUtil.CamelCase(pks[i].Code);
            }
            return cols;
        }

    }

}
