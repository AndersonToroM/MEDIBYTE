using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Dominus.Backend.DataBase
{
    public class BaseRepository<T> where T : BaseEntity
    {
        private DbSet<T> dbSet;

        protected IUnitOfWork work;

        public BaseRepository(IUnitOfWork work)
        {
            this.work = work;
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        public virtual IQueryable<T> GetTable(bool include = false)
        {
            IQueryable<T> query = this.Entities;
            IEnumerable<INavigation> navigationProperties = work.DbContext.Model.FindEntityType(typeof(T)).GetNavigations();
            foreach (INavigation navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty.Name);
            }
           return query;
        }

        private DbSet<T> Entities
        {
            get
            {
                if (dbSet == null)
                    dbSet = work.DbContext.Set<T>();
                return dbSet;
            }
        }

        public T Add(T data)
        {
            work.DbContext.Entry(data).State = EntityState.Added;
            work.DbContext.SaveChanges();
            return data;
        }

        public async Task<T> AddAsync(T data)
        {
            return await Task.Run(() =>
            {
                return Add(data);
            });
        }

        public void AddRange(List<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");
            work.DbContext.Set<T>().AddRange(entities);
            work.DbContext.SaveChanges();
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            await Task.Run(() =>
            {
                AddRange(entities);
            });
        }

        public T Modify(T data)
        {
            work.DbContext.Entry(data).State = EntityState.Modified;
            work.DbContext.SaveChanges();
            return data;
        }

        public async Task<T> ModifyAsync(T data)
        {
            return await Task.Run(() =>
            {
                return Modify(data);
            });
        }


        public void ModifyRange(List<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");
            entities.ForEach(x => this.Modify(x));
        }

        public async Task ModifyRangeAsync(List<T> entities)
        {
            await Task.Run(() =>
            {
                ModifyRange(entities);
            });
        }

        public T Remove(T data)
        {
            work.DbContext.Entry(data).State = EntityState.Deleted;
            work.DbContext.SaveChanges();
            return data;
        }

        public async Task<T> RemoveAsync(T data)
        {
            return await Task.Run(() =>
            {
                return Remove(data);
            });
        }

        public List<T> RemoveRange(List<T> dataList)
        {
            work.DbContext.Set<T>().RemoveRange(dataList);
            work.DbContext.SaveChanges();
            return dataList;
        }

        public async void RemoveRangeAsync(List<T> entities)
        {
            await Task.Run(() =>
            {
                RemoveRange(entities);
            });
        }

        public T FindById(Expression<Func<T, bool>> predicate, bool include = false)
        {
            IQueryable<T> query = Entities;
            if (predicate != null)
                query = query.Where(predicate);
            if (!include)
                return query.FirstOrDefault();
            else
            {
                IEnumerable<INavigation> navigationProperties = work.DbContext.Model.FindEntityType(typeof(T)).GetNavigations();
                foreach (INavigation navigationProperty in navigationProperties)
                {
                    query = query.Include(navigationProperty.Name);
                }
                return query.AsNoTracking().FirstOrDefault();

            }
        }

        public async Task<T> FindByIdAsync(Expression<Func<T, bool>> predicate, bool include = false)
        {
            return await Task.Run(() =>
            {
                return FindById(predicate, include);
            });
        }

        public T FindById(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,  bool disableTracking = true)
        {
            IQueryable<T> query = Table;
            if (disableTracking)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                return orderBy(query).FirstOrDefault();
            return query.FirstOrDefault();
        }

        public List<T> FindAll(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            IQueryable<T> query = Entities;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null ? orderBy(query).ToList<T>() : query.ToList<T>();
        }

        public virtual List<T> FindAll(Expression<Func<T, bool>> predicate, bool include = false)
        {
            IQueryable<T> query = Entities;
            if (predicate != null)
                query = query.Where(predicate);
            if (!include)
                return query.ToList<T>();
            else
            {
                IEnumerable<INavigation> navigationProperties = work.DbContext.Model.FindEntityType(typeof(T)).GetNavigations();
                foreach (INavigation navigationProperty in navigationProperties)
                {
                    query = query.Include(navigationProperty.Name);
                }
                return query.AsNoTracking().Select(q => q).ToList<T>();

            }

        }

        public List<T> FindAll(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0,
            int size = 20, bool disableTracking = true)
        {
            IQueryable<T> query = Entities;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null ? orderBy(query).Skip((index - 1) * size).Take(size).ToList<T>() : query.Skip((index - 1) * size).Take(size).ToList<T>(); ;
        }

        public List<TResult> FindAll<TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true) where TResult : class
        {
            IQueryable<T> query = Entities;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null
                ? orderBy(query).Select(selector).ToList<TResult>()
                : query.Select(selector).ToList<TResult>();
        }

        public List<TResult> FindAll<TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0, int size = 20, bool disableTracking = true) where TResult : class
        {
            IQueryable<T> query = Entities;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null
                ? orderBy(query).Select(selector).Skip((index - 1) * size).Take(size).ToList<TResult>()
                : query.Select(selector).Skip((index - 1) * size).Take(size).ToList<TResult>();
        }

        public Dictionary<string, object> ExecuteStoreProcedure(string nombreProcedimiento, List<DataBaseParameter> parametros)
        {
            return ExecuteSqlCommand(CommandType.StoredProcedure, nombreProcedimiento, parametros);
        }

        public Dictionary<string, object> ExecuteStoreProcedureDictionary(string nombreProcedimiento, List<DataBaseParameter> parametros)
        {
            return ExecuteCommandSqlDictionary(CommandType.StoredProcedure, nombreProcedimiento, parametros);
        }

        /// <summary>
        /// Permite ejecutar una consulta SQL y convertirla en un diccionario de datos
        /// </summary>
        /// <param name="comandoSql"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public Dictionary<string, object> ExecuteSqlCommand(string comandoSql, List<DataBaseParameter> parametros)
        {
            return ExecuteSqlCommand(CommandType.Text, comandoSql, parametros);
        }

        /// <summary>
        /// Permite ejecutar una consulta SQL y convertirla en una lista de objetos
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public List<T1> ExecuteQuerySql<T1>(string sql, List<DataBaseParameter> parametros)
        {
            List<T1> list = new List<T1>();

            using (var conn = work.DbContext.Database.GetDbConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;

                    object[] outputparameters = ConvertParameters(parametros);

                    foreach (var item in outputparameters)
                    {
                        cmd.Parameters.Add(item);
                    }

                    DbDataReader dbDataReader = cmd.ExecuteReader();
                    list = CreateList<T1>(dbDataReader);
                    dbDataReader.Close();

                }
            }

            return list;
        }

        /// <summary>
        /// Permite ejecutar una consulta SQL y convertirla en una lista de objetos
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="sp"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public List<T1> ExecuteQuerySpSql<T1>(string sp, List<DataBaseParameter> parametros)
        {
            List<T1> list = new List<T1>();

            using (var conn = work.DbContext.Database.GetDbConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sp;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;


                    object[] outputparameters = ConvertParameters(parametros);

                    foreach (var item in outputparameters)
                    {
                        cmd.Parameters.Add(item);
                    }

                    DbDataReader dbDataReader = cmd.ExecuteReader();
                    list = CreateList<T1>(dbDataReader);
                    dbDataReader.Close();

                }
            }



            return list;
        }

        public DataTable ExecuteQueryDataTable(string sql, List<DataBaseParameter> parametros)
        {
            DataTable dtResult = new DataTable();
            using (var conn = work.DbContext.Database.GetDbConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;

                    object[] outputparameters = ConvertParameters(parametros);

                    foreach (var item in outputparameters)
                    {
                        cmd.Parameters.Add(item);
                    }

                    DbDataReader dbDataReader = cmd.ExecuteReader();
                    dtResult.Load(dbDataReader);
                    dbDataReader.Close();
                }
            }

            return dtResult;
        }

        public List<Dictionary<string, object>> ExecuteQueryDictionary(string sql, List<DataBaseParameter> parametros)
        {
            DataTable dt = ExecuteQueryDataTable(sql, parametros);
            return dt.AsEnumerable().Select(
                        // ...then iterate through the columns...
                        row => dt.Columns.Cast<DataColumn>().ToDictionary(
                            // ...and find the key value pairs for the dictionary
                            column => column.ColumnName,    // Key
                            column => row[column] as object // Value
                        )
                   ).ToList();
        }

        public List<dynamic> DataReaderAListDynamic(DbDataReader reader)
        {
            List<dynamic> data = new List<dynamic>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    int totalColumns = reader.FieldCount;
                    IDictionary<string, object> fila = new ExpandoObject();
                    for (int i = 0; i < totalColumns; i++)
                    {
                        string name = reader.GetName(i);
                        fila[name] = reader[i];

                    }
                    data.Add(fila);
                }
            }
            return data;
        }

        public List<Dictionary<string, object>> DataReaderAsDiccionario(DbDataReader reader)
        {
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    int totalColumns = reader.FieldCount;
                    Dictionary<string, object> fila = new Dictionary<string, object>();
                    for (int i = 0; i < totalColumns; i++)
                    {
                        string name = reader.GetName(i);
                        fila[name] = reader[i];

                    }
                    data.Add(fila);
                }
            }
            return data;
        }

        #region Private Methods

        /// <summary>
        /// Permite convertir los parametros de SQL y Oracle a DbParameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private object[] ConvertParameters(List<DataBaseParameter> parameters)
        {

            object[] paramse = new object[parameters.Count];
            for (int i = 0; i < parameters.Count; i++)
            {
                if (work.DbContext.Setting.DataBaseType == DataBaseType.SQLServer)
                {
                    paramse[i] = new SqlParameter(parameters[i].Name, parameters[i].Value);
                    if (parameters[i].Direcction == null || parameters[i].Direcction == Direcction.In)
                        ((SqlParameter)paramse[i]).Direction = ParameterDirection.Input;
                    else if (parameters[i].Direcction == Direcction.Out)
                        ((SqlParameter)paramse[i]).Direction = ParameterDirection.Output;
                    else
                        ((SqlParameter)paramse[i]).Direction = ParameterDirection.InputOutput;

                    if (parameters[i].Length != null && parameters[i].Length != 0)
                    {
                        ((SqlParameter)paramse[i]).Size = (int)parameters[i].Length;
                    }
                }
                if (work.DbContext.Setting.DataBaseType == DataBaseType.Oracle)
                {
                    paramse[i] = new OracleParameter(parameters[i].Name.Replace("@", ""), parameters[i].Value);
                    if (parameters[i].Direcction == null || parameters[i].Direcction == Direcction.In)
                        ((OracleParameter)paramse[i]).Direction = ParameterDirection.Input;
                    else if (parameters[i].Direcction == Direcction.Out)
                        ((OracleParameter)paramse[i]).Direction = ParameterDirection.Output;
                    else
                        ((OracleParameter)paramse[i]).Direction = ParameterDirection.InputOutput;

                }
            }
            return paramse;
        }


        private Dictionary<string, object> ExecuteSqlCommand(CommandType commandType, string commandText, List<DataBaseParameter> parametros)
        {
            Dictionary<string, object> objects = new Dictionary<string, object>();

            using (var conn = work.DbContext.Database.GetDbConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = commandText;
                    cmd.CommandType = commandType;

                    object[] outputparameters = ConvertParameters(parametros);

                    foreach (var item in outputparameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        bool nextResult = true;
                        int j = 0;
                        while (nextResult)
                        {
                            j++;
                            List<dynamic> table = new List<dynamic>();
                            table = DataReaderAListDynamic(reader);
                            if (table != null && table.Count() > 0)
                                objects.Add("resultset" + j, table);
                            nextResult = reader.NextResult();
                        }
                    }

                    if (outputparameters != null && outputparameters.Count() > 0)
                        for (int i = 0; i < outputparameters.Count(); i++)
                        {
                            if (((DbParameter)outputparameters[i]).Direction == ParameterDirection.Output || ((DbParameter)outputparameters[i]).Direction == ParameterDirection.InputOutput)
                                objects.Add(((DbParameter)outputparameters[i]).ParameterName, ((DbParameter)outputparameters[i]).Value);
                        }

                }
            }

            return objects;
        }

        private Dictionary<string, object> ExecuteCommandSqlDictionary(CommandType commandType, string commandText, List<DataBaseParameter> parametros)
        {
            Dictionary<string, object> objects = new Dictionary<string, object>();

            using (var conn = work.DbContext.Database.GetDbConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = commandText;
                    cmd.CommandType = commandType;

                    object[] outputparameters = ConvertParameters(parametros);

                    foreach (var item in outputparameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        bool nextResult = true;
                        int j = 0;
                        while (nextResult)
                        {
                            j++;
                            List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();
                            table = DataReaderAsDiccionario(reader);
                            if (table != null && table.Count() > 0)
                                objects.Add("resultset" + j, table);
                            nextResult = reader.NextResult();
                        }
                    }

                    if (outputparameters != null && outputparameters.Count() > 0)
                        for (int i = 0; i < outputparameters.Count(); i++)
                        {
                            if (((DbParameter)outputparameters[i]).Direction == ParameterDirection.Output || ((DbParameter)outputparameters[i]).Direction == ParameterDirection.InputOutput)
                                objects.Add(((DbParameter)outputparameters[i]).ParameterName, ((DbParameter)outputparameters[i]).Value);
                        }

                }
            }

            return objects;
        }

        /// <summary>
        /// Permite usar funciones lambda para convertir una datareader en una lista de objetos
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<T1> CreateList<T1>(DbDataReader reader)
        {
            var results = new List<T1>();
            Func<DbDataReader, T1> readRow = this.GetReader<T1>(reader);

            while (reader.Read())
                results.Add(readRow(reader));
            return results;
        }

        /// <summary>
        /// Atravez de reflexion permite convertir un datareader en una lista de objectos
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Func<DbDataReader, T1> GetReader<T1>(DbDataReader reader)
        {
            Delegate resDelegate;

            List<string> readerColumns = new List<string>();
            for (int index = 0; index < reader.FieldCount; index++)
                readerColumns.Add(reader.GetName(index));

            // determine the information about the reader
            var readerParam = Expression.Parameter(typeof(DbDataReader), "reader");
            var readerGetValue = typeof(DbDataReader).GetMethod("GetValue");

            // create a Constant expression of DBNull.Value to compare values to in reader
            var dbNullValue = typeof(System.DBNull).GetField("Value");
            var dbNullExp = Expression.Field(expression: null, type: typeof(DBNull), fieldName: "Value");
            //var dbNullExp = Expression.Field(Expression.Parameter(typeof(DBNull), "System.DBNull"), DBNull.Value());
            //var dbNullExp = Expression.Field(Expression.Parameter(typeof(System.DBNull), "System.DBNull"), dbNullValue);

            // loop through the properties and create MemberBinding expressions for each property
            List<MemberBinding> memberBindings = new List<MemberBinding>();
            foreach (var prop in typeof(T1).GetProperties())
            {
                // determine the default value of the property
                object defaultValue = null;
                if (prop.PropertyType.IsValueType)
                    defaultValue = Activator.CreateInstance(prop.PropertyType);
                else if (prop.PropertyType.Name.ToLower().Equals("string"))
                    defaultValue = string.Empty;

                if (readerColumns.Contains(prop.Name))
                {
                    // build the Call expression to retrieve the data value from the reader
                    var indexExpression = Expression.Constant(reader.GetOrdinal(prop.Name));
                    var getValueExp = Expression.Call(readerParam, readerGetValue, new Expression[] { indexExpression });

                    // create the conditional expression to make sure the reader value != DBNull.Value
                    var testExp = Expression.NotEqual(dbNullExp, getValueExp);
                    var ifTrue = Expression.Convert(getValueExp, prop.PropertyType);
                    var ifFalse = Expression.Convert(Expression.Constant(defaultValue), prop.PropertyType);

                    // create the actual Bind expression to bind the value from the reader to the property value
                    MemberInfo mi = typeof(T1).GetMember(prop.Name)[0];
                    MemberBinding mb = Expression.Bind(mi, Expression.Condition(testExp, ifTrue, ifFalse));
                    memberBindings.Add(mb);
                }
            }

            // create a MemberInit expression for the item with the member bindings
            var newItem = Expression.New(typeof(T1));
            var memberInit = Expression.MemberInit(newItem, memberBindings);


            var lambda = Expression.Lambda<Func<DbDataReader, T1>>(memberInit, new ParameterExpression[] { readerParam });
            resDelegate = lambda.Compile();

            return (Func<DbDataReader, T1>)resDelegate;
        }

        #endregion

        public bool Exists(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return Table.Any(predicate);
            return Table.Any();
        }
        //public bool Exists(LambdaExpression predicate)
        //{
        //    if (predicate != null)
        //        return Table.Any(predicate);
        //    return Table.Any();
        //}

    }
}