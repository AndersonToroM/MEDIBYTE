using CodeGenerator.Data;
using CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGenerator.Templates.NETCoreMVCRoe
{
    public class ProcessEntity
    {

        public string Name { get; set; } = "API - Process Entity";
        public string Description { get; set; } = "Generates a Base Class Entity for .NET";

        #region private property

        private string OutputFolder { get; set; }
        private string FullNameFile { get; set; }
        private string Project { get; set; }
        private TableModel Table { get; set; }
        private TemplateModel Template { get; set; }
        public ProcessEntity() { } // Para usarse con reflection

        #endregion

        public ProcessEntity(CodeGeneratorModel CodeGeneratorModel)
        {
            OutputFolder = CodeGeneratorModel.PathGenerate + "/API/" + CodeGeneratorModel.TableGenerated.Comment + "/Entity/Process/";
            FullNameFile = CodeGeneratorModel.TableGenerated.Code + ".cs";

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

                List<ColumnModel> pks = Table.Columns.Where(x => x.IsPrimaryKey).ToList();
                List<ColumnModel> normalColumns = Table.Columns.Where(x => !x.IsPrimaryKey && !x.IsFKIn).ToList();
                List<ColumnModel> inReferences = Table.Columns.Where(x => x.IsFKIn && !x.IsPrimaryKey).ToList();

                var unique = Table.Indexes.Where(x => x.IsUnique && !x.IsPrimaryKey).GroupBy(x => x.Name).ToList();
                var RefIn = Table.InReferences.GroupBy(x => x.InReferencesName).ToList();
                var RefOut = Table.OutReferences.GroupBy(x => x.OutReferencesName).ToList();

                for (int i = 0; i < RefIn.Count(); i++)
                {
                    if (!RefIn.ElementAt(i).All(x => x.IsRequired))
                    {
                        RefIn.Remove(RefIn.ElementAt(i));
                        i -= 1;
                    }
                }

                sw.WriteLine(@"using System;");
                sw.WriteLine(@"using System.Collections.Generic;");
                sw.WriteLine(@"using System.ComponentModel.DataAnnotations.Schema;");
                sw.WriteLine(@"using System.Linq.Expressions;");
                sw.WriteLine(@"using Serialize.Linq.Extensions;");
                sw.WriteLine(@"using Dominus.Backend.Data;");
                sw.WriteLine(@"using Dominus.Backend.DataBase;");
                sw.WriteLine();
                sw.WriteLine(@"namespace {0}.Infrastructure.Entities", Project);
                sw.WriteLine(@"{");
                sw.WriteLine(@"    /// <summary>");
                sw.WriteLine(@"    /// {0} object for mapped table {1}.", Table.Code, Table.Name);
                sw.WriteLine(@"    /// </summary>");
                sw.WriteLine(@"    [Table(""{0}"")]", Table.Name);
                sw.WriteLine(@"    public partial class {0} : BaseEntity", Table.Code);
                sw.WriteLine(@"    {");
                sw.WriteLine();
                if (normalColumns != null && normalColumns.Count > 0)
                {
                    sw.WriteLine(@"       #region Columnas normales)");
                    sw.WriteLine();
                    foreach (ColumnModel column in normalColumns)
                    {
                        if (column.Code != "LastUpdate" && column.Code != "UpdatedBy" && column.Code != "CreatedBy" && column.Code != "CreationDate" && column.Code != "Id")
                        {
                            var NetDataType = GCUtil.GetNetDataType(column);

                            if (NetDataType == "DateTime")
                                sw.WriteLine(@"       [Column(""{0}"", TypeName = ""datetime"")]", column.Name);
                            else
                                sw.WriteLine(@"       [Column(""{0}"")]", column.Name);
                            sw.WriteLine(@"       [DDisplayName(""{0}"")]", Table.Name + "." + column.Name);

                            if (column.IsRequired)
                                sw.WriteLine(@"       [DRequired(""{0}"")]", Table.Name + "." + column.Name);

                            if (NetDataType == "String")
                                if (column.Length > 0)
                                    sw.WriteLine(@"       [DStringLength(""{0}"",{1})]", Table.Name + "." + column.Name, column.Length);

                            if (NetDataType.Equals("Boolean") || NetDataType.Equals("Byte[]"))
                                sw.WriteLine(@"       public virtual " + NetDataType + " " + GCUtil.PascalCase(column.Code) + " { get; set; }");
                            else
                                sw.WriteLine(@"       public virtual " + NetDataType + ((!column.IsRequired && NetDataType != "String") ? "? " : " ") + GCUtil.PascalCase(column.Code) + " { get; set; }");
                            sw.WriteLine();
                        }
                    }
                    sw.WriteLine(@"       #endregion");
                    sw.WriteLine();
                }
                if (inReferences != null && inReferences.Count > 0)
                {
                    sw.WriteLine(@"       #region Columnas referenciales)");
                    sw.WriteLine();
                    foreach (ColumnModel column in inReferences)
                    {
                        var NetDataType = GCUtil.GetNetDataType(column);

                        if (NetDataType == "DateTime")
                            sw.WriteLine(@"       [Column(""{0}"", TypeName = ""datetime"")]", column.Name);
                        else
                            sw.WriteLine(@"       [Column(""{0}"")]", column.Name);
                        sw.WriteLine(@"       [DDisplayName(""{0}"")]", Table.Name + "." + column.Name);

                        if (column.IsRequired)
                        {
                            sw.WriteLine(@"       [DRequired(""{0}"")]", Table.Name + "." + column.Name);
                            sw.WriteLine(@"       [DRequiredFK(""{0}"")]", Table.Name + "." + column.Name);
                        }
                        if (NetDataType == "String")
                            sw.WriteLine(@"       [DStringLength(""{0}"",{1})]", Table.Name + "." + column.Name, column.Length == 0 ? int.MaxValue : column.Length);

                        if (NetDataType.Equals("Boolean") || NetDataType.Equals("Byte[]"))
                            sw.WriteLine(@"       public virtual " + NetDataType + " " + GCUtil.PascalCase(column.Code) + " { get; set; }");
                        else
                            sw.WriteLine(@"       public virtual " + NetDataType + ((!column.IsRequired && NetDataType != "String") ? "? " : " ") + GCUtil.PascalCase(column.Code) + " { get; set; }");
                        sw.WriteLine();
                    }
                    sw.WriteLine(@"       #endregion");
                    sw.WriteLine();
                }
                if (Table.InReferences != null && Table.InReferences.Count > 0)
                {
                    sw.WriteLine(@"       #region Propiedades referencias de entrada)");
                    sw.WriteLine();
                    List<string> inReferenceName = new List<string>();
                    foreach (InReferencesModel inReference in Table.InReferences)
                    {
                        string columnReference = inReference.ColumnCode.Substring(0, inReference.ColumnCode.Length - 2);
                        sw.WriteLine(@"       [ForeignKey(""{0}"")]", inReference.ColumnCode);
                        sw.WriteLine(@"       public virtual " + inReference.ParentTableCode + " " + columnReference + " { get; set; }");
                        sw.WriteLine();
                    }
                    sw.WriteLine(@"       #endregion");
                    sw.WriteLine();
                }
                sw.WriteLine(@"       #region Reglas expression");
                sw.WriteLine();

                //Llave primaria
                sw.WriteLine($@"       public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()");
                sw.WriteLine($@"       {{");
                sw.WriteLine($@"       Expression<Func<{Table.Code}, bool>> expression = entity => entity.Id == this.Id;");
                sw.WriteLine($@"       return expression as Expression<Func<T, bool>>;");
                sw.WriteLine($@"       }}");
                sw.WriteLine();

                //Adicionar
                sw.WriteLine($@"       public override List<ExpRecurso> GetAdicionarExpression<T>()");
                sw.WriteLine($@"       {{");
                sw.WriteLine($@"        var rules = new List<ExpRecurso>();");
                sw.WriteLine($@"        Expression<Func<{Table.Code}, bool>> expression = null;");
                sw.WriteLine();
                foreach (var item in unique)
                {
                    sw.WriteLine($@"        expression = entity => {GetExpresionStringCols(item.Select(x => GCUtil.CleanName(x.ColumnName, true)))};");
                    sw.WriteLine($@"        rules.Add(new ExpRecurso(expression.ToExpressionNode() , new Recurso(""BLL.BUSINESS.UNIQUE"",{ GetExpresionStringCols(item.Select(x => x)) })));");
                    sw.WriteLine();
                }
                sw.WriteLine($@"       return rules;");
                sw.WriteLine($@"       }}");
                sw.WriteLine();

                //Modificar
                sw.WriteLine($@"       public override List<ExpRecurso> GetModificarExpression<T>()");
                sw.WriteLine($@"       {{");
                sw.WriteLine($@"        var rules = new List<ExpRecurso>();");
                sw.WriteLine($@"        Expression<Func<{Table.Code}, bool>> expression = null;");
                sw.WriteLine();
                foreach (var item in unique)
                {
                    sw.WriteLine($@"        expression = entity => !({GetExpresionStringCols(pks.Select(x => x.Code))} && {GetExpresionStringCols(item.Select(x => GCUtil.CleanName(x.ColumnName, true)))})");
                    sw.WriteLine($@"                               && {GetExpresionStringCols(item.Select(x => GCUtil.CleanName(x.ColumnName, true)))};");
                    sw.WriteLine($@"        rules.Add(new ExpRecurso(expression.ToExpressionNode() , new Recurso(""BLL.BUSINESS.UNIQUE"",{ GetExpresionStringCols(item.Select(x => x)) })));");
                    sw.WriteLine();
                }
                sw.WriteLine($@"       return rules;");
                sw.WriteLine($@"       }}");
                sw.WriteLine();

                //Eliminar
                sw.WriteLine($@"       public override List<ExpRecurso> GetEliminarExpression<T>()");
                sw.WriteLine($@"       {{");
                sw.WriteLine($@"        var rules = new List<ExpRecurso>();");
                foreach (var item in RefOut)
                {
                    var id = RefOut.IndexOf(item);
                    var tableNameClass = item.Select(x => GCUtil.PascalCase(x.ParentTableName)).FirstOrDefault();
                    var tableName = item.Select(x => x.ParentTableName).FirstOrDefault();

                    sw.WriteLine($@"        Expression<Func<{GCUtil.MakeSingle(tableNameClass)}, bool>> expression{id} = entity => {GetExpresionStringCols(item.Select(x => GCUtil.CleanName(x.ColumnName, true)), item.Select(x => GCUtil.CleanName(x.ParentColumnName, true)))};");
                    sw.WriteLine($@"        rules.Add(new ExpRecurso(expression{id}.ToExpressionNode() , new Recurso(""BLL.BUSINESS.DELETE_REL"",""{ tableName }""), typeof({GCUtil.MakeSingle(tableNameClass)})));");
                    sw.WriteLine();
                }
                sw.WriteLine($@"       return rules;");
                sw.WriteLine($@"       }}");
                sw.WriteLine();
                sw.WriteLine(@"       #endregion");
                sw.WriteLine(@"    }");
                sw.WriteLine(@" }");

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


        private string GetExpresionStringCols(IEnumerable<string> cols)
        {
            string text = "";
            bool first = true;

            foreach (var item in cols)
            {
                if (first)
                {
                    text += "entity." + item + " == this." + item;
                    first = false;
                }
                else
                    text += " && entity." + item + " == this." + item;

            }

            return text;
        }

        private string GetExpresionStringCols(IEnumerable<IndexModel> cols)
        {
            string text = "";
            bool first = true;

            foreach (var item in cols)
            {
                if (first)
                {
                    text += $@" ""{item.TableName}.{item.ColumnName}"" ";
                    first = false;
                }
                else
                    text += $@", ""{item.TableName}.{item.ColumnName}"" ";

            }

            return text;
        }

        private string GetExpresionStringCols(IEnumerable<string> colsSource, IEnumerable<string> colsEntity)
        {
            string text = "";
            bool first = true;

            for (int i = 0; i < colsSource.Count(); i++)
            {
                if (first)
                {
                    text += "entity." + colsEntity.ElementAt(i) + " == this." + colsSource.ElementAt(i);
                    first = false;
                }
                else
                    text += " && entity." + colsEntity.ElementAt(i) + " == this." + colsSource.ElementAt(i);
            }

            return text;
        }

    }
}
