using CodeGenerator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeGenerator.Data
{

    public class Scheme
    {

        public DataBaseMapModel dataBaseMapModel { get; set; } = new DataBaseMapModel();
        private DBSettings DBSettings { get; set; }
        private DbConnection conexion { get; set; }
        public List<TableModel> tableModels { get; set; } = new List<TableModel>();
        public Scheme(DBSettings DBSettings)
        {
            this.DBSettings = DBSettings;
            conexion = new Contexto(DBSettings).Database.GetDbConnection();
        }

        public string GetWhereIn()
        {
            if (tableModels.Count <= 0)
                return null;
            string whereIn = "IN (";
            foreach (var table in tableModels)
            {
                whereIn += $"'{table.Name}',";
            }
            whereIn += "'X')"; // no es nesesario la X pero tenia pereza de pensar.
            return whereIn;
        }

        public DataBaseMapModel GetDataBaseInfo()
        {
            this.dataBaseMapModel.Tables = GetTables();
            this.dataBaseMapModel.Columns = GetColumns();
            this.dataBaseMapModel.InReferences = GetInFKs();
            this.dataBaseMapModel.OutReferences = GetOutFKs();
            this.dataBaseMapModel.Indexes = GetIndexes();
            //this.dataBaseMapModel.Tables.ForEach(x => x = SetProjectTable(x));
            this.dataBaseMapModel.NumberConnection = this.DBSettings.NumberConnection;
            return this.dataBaseMapModel;
        }

        public List<TableModel> GetTables() {
            string whereIn = GetWhereIn();
            whereIn = String.IsNullOrWhiteSpace(whereIn) ? "" : "AND t.name " + whereIn;

            //DbConnection conexion = Contexto.Database.GetDbConnection();
            conexion.Open();
            DbCommand comand = conexion.CreateCommand();
            comand.CommandText = $@"SELECT *
                                    FROM sys.tables t
                                    WHERE t.type_desc LIKE 'USER_TABLE' and t.name != 'sysdiagrams'
                                    {whereIn}
                                    ORDER BY t.name ";
            DbDataReader data = comand.ExecuteReader();
            List<TableModel> tables = new List<TableModel>();
            while (data.Read())
            {
                TableModel TableModel = new TableModel();
                TableModel.Selected = false;
                TableModel.Name = data.GetString(0);
                TableModel.Number = GCUtil.GetNumber(data.GetString(0));
                TableModel.Code = GCUtil.PascalCase(data.GetString(0));
                TableModel.Code = GCUtil.MakeSingle(TableModel.Code);
                TableModel.Comment = TableModel.Code;
                TableModel = SetProjectTable(TableModel);
                tables.Add(TableModel);
            }
            this.dataBaseMapModel.Tables = tables;
            conexion.Close();
            return tables;
        }

        public List<ColumnModel> GetColumns()
        {
            string whereIn = GetWhereIn();
            whereIn = String.IsNullOrWhiteSpace(whereIn) ? "" : " AND t.name " + whereIn;
            conexion.Open();
            DbCommand comand = conexion.CreateCommand();
            comand.CommandText = $@"select ac.name AS COLUMN_NAME
	                                    ,df.definition AS COLUMN_DEFAULT
	                                    ,CASE WHEN ac.is_nullable = 1 THEN 'false' ELSE 'true' END AS IsRequired
	                                    ,ct.name AS DATA_TYPE
	                                    ,CASE WHEN ct.name = 'nvarchar' THEN (ac.max_length / 2) 
		                                    WHEN ct.name = 'text' THEN -1 
	                                    ELSE ac.max_length END AS CHARACTER_MAXIMUM_LENGTH
	                                    ,ac.precision AS NUMERIC_PRECISION
	                                    ,ac.scale AS NUMERIC_SCALE
	                                    ,CASE WHEN ic.index_column_id IS NOT NULL THEN 'true' ELSE 'false' END AS IsPrimaryKey 
                                        ,CASE WHEN ac.is_identity = 1 THEN 'true' ELSE 'false' END AS IsIdentity  
                                        ,t.name as TABLE_NAME
                                    from  sys.all_columns ac
                                    inner join  sys.types ct on  ct.system_type_id = ac.system_type_id and ct.user_type_id = ac.user_type_id
                                    inner join	sys.tables t on  t.object_id = ac.object_id and t.name != 'sysdiagrams'
                                    inner join	sys.schemas s on  t.schema_id = s.schema_id
                                    left  join  sys.default_constraints df on  df.parent_object_id = ac.object_id and  df.parent_column_id = ac.column_id
                                    left  join  sys.indexes i on  i.object_id = t.object_id and  i.is_primary_key = 1
                                    left  join  sys.index_columns ic on  ic.object_id = t.object_id and  ac.column_id = ic.column_id and  ic.index_id = i.index_id
                                    where t.type_desc LIKE 'USER_TABLE'                                  
                                    {whereIn}
                                    order by t.name,ac.column_id ";
            DbDataReader data = comand.ExecuteReader();
            List<ColumnModel> columns = new List<ColumnModel>();
            while (data.Read())
            {
                ColumnModel ColumnModel = new ColumnModel();

                ColumnModel.Name = data.GetString(0);
                ColumnModel.Default = data.IsDBNull(1) ? null : data.GetString(1);
                ColumnModel.IsRequired = bool.Parse(data.GetString(2));
                ColumnModel.Type = data.GetString(3);
                ColumnModel.Length = data.GetInt32(4); //data.GetInt16(4);
                ColumnModel.Quantity = data.GetByte(5);
                ColumnModel.Decimals = data.GetByte(6);
                ColumnModel.Code = GCUtil.PascalCase(data.GetString(0) , true);
                ColumnModel.CodeJava = GCUtil.PascalCase(data.GetString(0));
                ColumnModel.IsPrimaryKey = bool.Parse(data.GetString(7));
                ColumnModel.IsIdentity = bool.Parse(data.GetString(8));
                ColumnModel.IsInteger = (ColumnModel.Decimals == 0 ? true : false);
                ColumnModel.IsDecimal = (ColumnModel.Decimals == 0 ? false : true);
                ColumnModel.TableName = data.GetString(9);
                ColumnModel.Number = GCUtil.GetNumber(data.GetString(0));
                columns.Add(ColumnModel);
            }
            conexion.Close();
            return columns;
        }

        public List<InReferencesModel> GetInFKs()
        {
            string whereIn = GetWhereIn();
            whereIn = String.IsNullOrWhiteSpace(whereIn) ? "" : " AND t_from.name " + whereIn;
            conexion.Open();
            DbCommand comand = conexion.CreateCommand();
            comand.CommandText = $@"SELECT fk.name AS FK_NAME 
                                    ,t_from.name AS TABLE_NAME 
                                    ,t_to.name AS TARGET_TABLE_NAME 
                                    ,ac.name AS Name 
                                    ,ac_to.name AS TARGET_Name 
                                    ,CASE WHEN ac.is_nullable = 1 THEN 'false' ELSE 'true' END AS IsRequired
                                FROM sys.foreign_key_columns fkc
                                INNER JOIN sys.foreign_keys fk ON fk.object_id = fkc.constraint_object_id
                                INNER JOIN sys.tables t_from ON t_from.object_id = fkc.parent_object_id
                                INNER JOIN sys.schemas s_from ON  t_from.schema_id = s_from.schema_id
                                INNER JOIN sys.tables t_to ON t_to.object_id = fkc.referenced_object_id
                                INNER JOIN sys.schemas s_to ON  t_to.schema_id = s_to.schema_id
                                INNER JOIN sys.all_columns ac ON  ac.object_id = t_from.object_id AND  ac.column_id = fkc.parent_column_id
                                INNER JOIN sys.all_columns ac_to ON  ac_to.object_id = t_to.object_id AND  ac_to.column_id = fkc.referenced_column_id 
                                WHERE t_from.type_desc LIKE 'USER_TABLE' 
                                {whereIn} 
                                ORDER BY t_from.name,fk.name ";
            DbDataReader data = comand.ExecuteReader();
            List<InReferencesModel> Fks = new List<InReferencesModel>();
            while (data.Read())
            {
                InReferencesModel FKModel = new InReferencesModel();
                FKModel.InReferencesName = data.GetString(0);
                FKModel.TableName = data.GetString(1);
                FKModel.ParentTableName = data.GetString(2);
                FKModel.ParentTableCode = GCUtil.PascalCase(data.GetString(2));
                FKModel.ParentTableCode = GCUtil.MakeSingle(FKModel.ParentTableCode);
                FKModel.ColumnName = data.GetString(3);
                FKModel.ColumnCode = GCUtil.PascalCase(data.GetString(3));
                FKModel.ParentColumnName = data.GetString(4);
                FKModel.ParentColumnCode = GCUtil.PascalCase(data.GetString(4));
                FKModel.IsRequired = bool.Parse(data.GetString(5));
                Fks.Add(FKModel);
            }
            conexion.Close();
            return Fks;
        }

        public List<OutReferencesModel> GetOutFKs()
        {
            string whereIn = GetWhereIn();
            whereIn = String.IsNullOrWhiteSpace(whereIn) ? "" : " AND t_to.name " + whereIn;
            conexion.Open();
            DbCommand comand = conexion.CreateCommand();
            comand.CommandText = $@"SELECT fk.name AS FK_NAME 
                                    ,t_from.name AS TABLE_NAME 
                                    ,t_to.name AS TARGET_TABLE_NAME 
                                    ,ac.name AS Name 
                                    ,ac_to.name AS TARGET_Name 
                                FROM sys.foreign_key_columns fkc
                                INNER JOIN sys.foreign_keys fk ON fk.object_id = fkc.constraint_object_id
                                INNER JOIN sys.tables t_from ON t_from.object_id = fkc.parent_object_id
                                INNER JOIN sys.schemas s_from ON  t_from.schema_id = s_from.schema_id
                                INNER JOIN sys.tables t_to ON t_to.object_id = fkc.referenced_object_id
                                INNER JOIN sys.schemas s_to ON  t_to.schema_id = s_to.schema_id
                                INNER JOIN sys.all_columns ac ON  ac.object_id = t_from.object_id AND  ac.column_id = fkc.parent_column_id
                                INNER JOIN sys.all_columns ac_to ON  ac_to.object_id = t_to.object_id AND  ac_to.column_id = fkc.referenced_column_id 
                                WHERE t_from.type_desc LIKE 'USER_TABLE' 
                                {whereIn}
                                ORDER BY t_from.name,fk.name ";

            DbDataReader data = comand.ExecuteReader();
            List<OutReferencesModel> Fks = new List<OutReferencesModel>();
            while (data.Read())
            {
                OutReferencesModel FKModel = new OutReferencesModel();
                FKModel.OutReferencesName = data.GetString(0);
                FKModel.TableName = data.GetString(2);
                FKModel.ParentTableName = data.GetString(1);
                FKModel.ParentTableCode = GCUtil.PascalCase(data.GetString(1));
                FKModel.ParentTableCode = GCUtil.MakeSingle(FKModel.ParentTableCode);
                FKModel.ColumnName = data.GetString(4);
                FKModel.ColumnCode = GCUtil.PascalCase(data.GetString(4));
                FKModel.ParentColumnName = data.GetString(3);
                FKModel.ParentColumnCode = GCUtil.PascalCase(data.GetString(3));
                Fks.Add(FKModel);
            }
            conexion.Close();
            return Fks;
        }

        public List<IndexModel> GetIndexes()
        {
            string whereIn = GetWhereIn();
            whereIn = String.IsNullOrWhiteSpace(whereIn) ? "" : " AND t.name " + whereIn;
            conexion.Open();
            DbCommand comand = conexion.CreateCommand();
            comand.CommandText = $@"SELECT ind.name AS INDEX_NAME,
	                                    col.name AS COLUMN_NAME,
	                                    t.name AS TABLE_NAME,
                                        CASE WHEN is_unique = 1 THEN 'true' ELSE 'false' END AS IsUnique,
                                        CASE WHEN is_primary_key = 1 THEN 'true' ELSE 'false' END AS IsPrimaryKey
                                    FROM sys.indexes ind 
                                    INNER JOIN sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id 
                                    INNER JOIN sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id 
                                    INNER JOIN sys.tables t ON ind.object_id = t.object_id 
                                    WHERE t.type_desc LIKE 'USER_TABLE' 
                                    {whereIn}
                                    ORDER BY ind.name";
            DbDataReader data = comand.ExecuteReader();
            List<IndexModel> Indexes = new List<IndexModel>();
            while (data.Read())
            {
                IndexModel IndexModel = new IndexModel();
                IndexModel.Name = data.GetString(0);
                IndexModel.ColumnName = data.GetString(1);
                IndexModel.ColumnCode = GCUtil.PascalCase(data.GetString(1));
                IndexModel.TableName = data.GetString(2);
                IndexModel.IsUnique = bool.Parse(data.GetString(3));
                IndexModel.IsPrimaryKey = bool.Parse(data.GetString(4));

                Indexes.Add(IndexModel);
            }
            conexion.Close();
            return Indexes;
        }

        public TableModel SetProjectTable(TableModel table)
        {
            table.Prefix = "SIISO";
            table.NameProject = "SIISO_PROJECT";
            return table;
        }

        public TableModel GetDataTable(TableModel table)
        {

            table.Columns = GCUtil.DataBaseInfo.FirstOrDefault(x => x.NumberConnection == this.DBSettings.NumberConnection).Columns.Where(x=>x.TableName == table.Name).ToList();
            table.InReferences = GCUtil.DataBaseInfo.FirstOrDefault(x => x.NumberConnection == this.DBSettings.NumberConnection).InReferences.Where(x => x.TableName == table.Name).ToList();
            table.OutReferences = GCUtil.DataBaseInfo.FirstOrDefault(x => x.NumberConnection == this.DBSettings.NumberConnection).OutReferences.Where(x => x.TableName == table.Name).ToList();
            table.Indexes = GCUtil.DataBaseInfo.FirstOrDefault(x => x.NumberConnection == this.DBSettings.NumberConnection).Indexes.Where(x => x.TableName == table.Name).ToList();

            table.Columns.ForEach(x => x.IsIndexUnique = table.Indexes.Exists(j => j.ColumnName == x.Name));
            table.Columns.ForEach(x => x.IsFKIn = table.InReferences.Exists(j => j.ColumnName == x.Name));
            table.Columns.ForEach(x => x.IsFKOut = table.OutReferences.Exists(j => j.ColumnName == x.Name));
            if (GCUtil.Framework != 1){
                //table.Columns.ForEach(x => x.Code = (x.Number + x.Code));
                table.Code = GCUtil.MakeSingle(table.Code);
            }
            return table;
        }

        public List<TableModel> GetDataTableFromRoe(string nombreRoe, string prefijoRoe, string Consulta)
        {
            Consulta = Consulta.Replace("SELECT", " SELECT TOP(1) ", StringComparison.OrdinalIgnoreCase);
            conexion.Open();
            DbCommand comand = conexion.CreateCommand();
            comand.CommandText = Consulta;
            DbDataReader data = comand.ExecuteReader();
            ReadOnlyCollection<DbColumn> scheme = data.GetColumnSchema();

            TableModel tableModel = new TableModel();
            tableModel.Name = nombreRoe;
            tableModel.Selected = true;
            tableModel.Code = GCUtil.PascalCase(nombreRoe);
            tableModel.Code = GCUtil.MakeSingle(tableModel.Code);
            tableModel.Comment = prefijoRoe;
            tableModel.Prefix = prefijoRoe;
            if(prefijoRoe.Equals("gen"))
                tableModel.NameProject = "siesaweb.general";
            else if (prefijoRoe.Equals("cco"))
                tableModel.NameProject = "siesaweb.administracion-espacios";
            else if (prefijoRoe.Equals("map"))
                tableModel.NameProject = "siesaweb.mantenimiento-preventivo";
            else if (prefijoRoe.Equals("soc"))
                tableModel.NameProject = "siesaweb.salud-ocupacional";
            else if (prefijoRoe.Equals("gpe"))
                tableModel.NameProject = "siesaweb.gestion-personal";
            else
                tableModel.NameProject = "siesaweb.nomina";

            foreach (var item in scheme)
            {
                ColumnModel columnModel = new ColumnModel();
                columnModel.Name = item.ColumnName;
                columnModel.Code = GCUtil.PascalCase(item.ColumnName);
                columnModel.CodeJava = GCUtil.PascalCase(item.ColumnName);
                columnModel.Type = item.DataTypeName;
                columnModel.Length = item.ColumnSize.GetValueOrDefault(0);
                columnModel.Decimals = item.NumericScale.GetValueOrDefault(0);
                columnModel.Quantity = item.NumericPrecision.GetValueOrDefault(0);
                tableModel.Columns.Add(columnModel);
            }
            conexion.Close();
            return new List<TableModel> { tableModel };
        }

    }

}
