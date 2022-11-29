using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominus.Backend.Application;
using RuleType =  Dominus.Backend.DataBase.RuleType;

namespace Dominus.Backend.DataBase
{
    public class IDomainLogic<T> where T : BaseEntity
    {
        public virtual T Add(T data)
        {
            try
            {
                BeginTransaction();
                CommonRules(data);
                AddRules(data);
                AddingRule(data);
                if (!HasErrors)
                    data = UnitOfWork.Repository<T>().Add(data);
                else
                    throw GetBusinessExeption();
                return data;
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                CommitTransaction();
            }
        }

        public virtual T Modify(T data)
        {
            try
            {
                BeginTransaction();
                CommonRules(data);
                ModifyRules(data);
                ModifyingRule(data);
                if (!HasErrors)
                    data = UnitOfWork.Repository<T>().Modify(data);
                else
                    throw GetBusinessExeption();
                return data;
            }
            catch (Exception e)
            {
                RollbackTransaction();
                throw e;
            }
            finally
            {
                CommitTransaction();
            }
        }

        public virtual T Remove(T data)
        {
            try
            {
                BeginTransaction();
                RemoveRules(data);
                DeletingRule(data);
                if (!HasErrors)
                    data = UnitOfWork.Repository<T>().Remove(data);
                else
                    throw GetBusinessExeption();
                return data;
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                CommitTransaction();
            }
        }

        public virtual T FindById(Expression<Func<T, bool>> predicate, bool include = false)
        {
            try
            {
                T data;
                if (!HasErrors)
                    data = UnitOfWork.Repository<T>().FindById(predicate, include);
                else
                    throw GetBusinessExeption();
                return data;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public virtual T FindById(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            try
            {
                T data;
                if (!HasErrors)
                    data = UnitOfWork.Repository<T>().FindById(predicate, orderBy, include, disableTracking);
                else
                    throw GetBusinessExeption();
                return data;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public virtual List<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return UnitOfWork.Repository<T>().FindAll(predicate, null, null, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual List<T> FindAll(Expression<Func<T, bool>> predicate, bool include = false)
        {
            return UnitOfWork.Repository<T>().FindAll(predicate, include);
        }

        public virtual List<T> FindAll(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            try
            {
                return UnitOfWork.Repository<T>().FindAll(predicate, null, include, false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual List<T> FindAll(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            try
            {
                return UnitOfWork.Repository<T>().FindAll(predicate, orderBy, include, disableTracking);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual List<TResult> FindAll<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true) where TResult : class
        {
            try
            {
                return UnitOfWork.Repository<T>().FindAll(selector, predicate, orderBy, include, disableTracking);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual List<TResult> FindAll<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0, int size = 20, bool disableTracking = true) where TResult : class
        {
            try
            {
                return UnitOfWork.Repository<T>().FindAll(selector, predicate, orderBy, include, disableTracking);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<T1> ExecuteQuerySql<T1>(string sql, List<DataBaseParameter> parametros)
        {
            return UnitOfWork.Repository<T>().ExecuteQuerySql<T1>(sql, parametros);
        }

        /// <summary>
        /// Permite ejecutar una consulta SQL y convertirla en una lista de objetos
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="sp"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public List<T1> ExecuteQuerySpSql<T1>(string sql, List<DataBaseParameter> parametros)
        {
            return UnitOfWork.Repository<T>().ExecuteQuerySpSql<T1>(sql, parametros);
        }

        public Dictionary<string, object> ExecuteSqlCommand(string comandoSql, List<DataBaseParameter> parametros)
        {
            return UnitOfWork.Repository<T>().ExecuteSqlCommand(comandoSql, parametros);
        }

        public Dictionary<string, object> ExecuteStoreProcedure(string nombreProcedimiento, List<DataBaseParameter> parametros)
        {
            return UnitOfWork.Repository<T>().ExecuteStoreProcedure(nombreProcedimiento, parametros);
        }

        public Dictionary<string, object> ExecuteStoreProcedureDictionary(string nombreProcedimiento, List<DataBaseParameter> parametros)
        {
            return UnitOfWork.Repository<T>().ExecuteStoreProcedureDictionary(nombreProcedimiento, parametros);
        }

        public IQueryable<T> Tabla(bool include = false)
        {
            return UnitOfWork.Repository<T>().GetTable(include);
        }

        public List<Dictionary<string, object>> EjecutarConsultaDiccionario(string sql, List<DataBaseParameter> parametros)
        {
            

            return UnitOfWork.Repository<T>().ExecuteQueryDictionary(sql, parametros);
        }

        #region Business Rules

        public virtual void CommonRules(T data)
        {
            List<string> errors = DApp.BusinessRules.ValidateRules(DApp.BusinessRules.GetRules<T>(RuleType.CommonRules), data);
            if (errors != null && errors.Count > 0)
            {
                foreach (string error in errors)
                {
                    AddExeptionMessages(error);
                }
            }
        }

        public virtual void AddRules(T data)
        {
            List<string> errors = DApp.BusinessRules.ValidateRules(DApp.BusinessRules.GetRules<T>(RuleType.AddRules), data);
            if(errors!=null && errors.Count > 0)
            {
                foreach (string error in errors)
                {
                    AddExeptionMessages(error);
                }
            }
        }

        public virtual void ModifyRules(T data)
        {
            List<string> errors = DApp.BusinessRules.ValidateRules(DApp.BusinessRules.GetRules<T>(RuleType.MofidyRules), data);
            if (errors != null && errors.Count > 0)
            {
                foreach (string error in errors)
                {
                    AddExeptionMessages(error);
                }
            }
        }

        public virtual void RemoveRules(T data)
        {
            List<string> errors = DApp.BusinessRules.ValidateRules(DApp.BusinessRules.GetRules<T>(RuleType.RemoveRules), data);
            if (errors != null && errors.Count > 0)
            {
                foreach (string error in errors)
                {
                    AddExeptionMessages(error);
                }
            }
        }

        public virtual void FindByIdRules(T data)
        {
            List<string> errors = DApp.BusinessRules.ValidateRules(DApp.BusinessRules.GetRules<T>(RuleType.FindByIdRules), data);
            if (errors != null && errors.Count > 0)
            {
                foreach (string error in errors)
                {
                    AddExeptionMessages(error);
                }
            }
        }

        #endregion

        #region Error Properties and Methods

        private List<string> exeptionMessages;

        public List<string> ExeptionMessages
        {
            get
            {
                if (exeptionMessages == null)
                    exeptionMessages = new List<string>();
                return exeptionMessages;
            }
            set
            {
                exeptionMessages = value;
            }
        }

        public bool HasErrors
        {
            get
            {
                return ExeptionMessages.Count > 0;
            }
        }

        public void AddExeptionMessages(string message, params string[] args)
        {
            ExeptionMessages.Add(String.Format(message, args));
        }

        public string GetFOrmatMessage(string message, params string[] args)
        {
            return string.Format(message, args);
        }

        public Exception GetBusinessExeption()
        {
            string mensajesError = "Regla: \n";
            foreach (string mensaje in ExeptionMessages)
                mensajesError += mensaje + " \n";
            return new Exception(mensajesError);
        }

        #endregion

        #region Transactional Properties and Methods


        public bool CommitTheTransaction { get; set; }

        public IUnitOfWork UnitOfWork { get; protected set; }

        public void CommitTransaction()
        {
            if (CommitTheTransaction)
                UnitOfWork.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            if (CommitTheTransaction)
                UnitOfWork.RollbackTransaction();
        }

        public void BeginTransaction()
        {
            UnitOfWork.BeginTransaction();
        }

        #endregion

        #region Framework Bussiness Rules

        private void AddingRule(T entity)
        {

            bool exits = false;
            var valor = entity.PrimaryKeyExpression<T>();

            if (valor != null)
                exits = UnitOfWork.Repository<T>().Table.Any(valor);

            if (exits)
            {
                AddExeptionMessages(DApp.GetResource("BLL.BUSINESS.UNREGISTEREDROW"));
                throw GetBusinessExeption();
            }
            else
            {
                var rules = entity.GetAdicionarExpression<T>();
                dynamicRules(rules);
            }
        }

        private void ModifyingRule(T entity)
        {
            int count = -1;
            var valor = entity.PrimaryKeyExpression<T>();

            if (valor != null)
                count = UnitOfWork.Repository<T>().Table.Count(valor);

            if (count == 0)
            {
                AddExeptionMessages(DApp.GetResource("BLL.BUSINESS.UNREGISTEREDROW"));
                throw GetBusinessExeption();
            }
            //else if (count == 1)
            //{
            //    var property = entity.GetType().GetProperty("LastUpdate");
            //    if (property != null)
            //    {
            //        DateTime entityDate = (DateTime)(property.GetValue(entity, null));
            //        T savedEntity = UnitOfWork.Repository<T>().Table.FirstOrDefault(valor);
            //        string savedUser = (string)(savedEntity.GetType().GetProperty("UpdatedBy").GetValue(savedEntity, null));
            //        DateTime savedDate = (DateTime)(savedEntity.GetType().GetProperty("LastUpdate").GetValue(savedEntity, null));
            //        if (savedDate >= entityDate)
            //        {
            //            AddExeptionMessages(string.Format(DApp.GetResource("BLL.BUSINESS.MODIFIEDROW"), savedUser, savedDate.ToString("dd/MM/yyyy HH:mm:ss")));
            //            throw GetBusinessExeption();
            //        }
            //    }
            //}
            var rules = entity.GetModificarExpression<T>();
            dynamicRules(rules);
        }

        private void DeletingRule(T entity)
        {

            bool exits = false;
            var valor = entity.PrimaryKeyExpression<T>();

            if (valor != null)
                exits = UnitOfWork.Repository<T>().Table.Any(valor);

            if (!exits)
            {
                AddExeptionMessages(DApp.GetResource("BLL.BUSINESS.UNREGISTEREDROW"));
                throw GetBusinessExeption();
            }
            else
            {
                var rules = entity.GetEliminarExpression<T>();
                dynamicRules(rules);
            }

        }

        //private T DeletingRule(Expression<Func<T, bool>> predicate)
        //{
        //    var data = UnitOfWork.Repository<T>().FindById(predicate,false);

        //    if (data == null)
        //    {
        //        var entity = Activator.CreateInstance<T>();
        //        AddExeptionMessages(DApp.GetResource("BLL.BUSINESS.UNREGISTEREDROW"));
        //        return entity;
        //    }
        //    else
        //    {
        //        var rules = data.GetEliminarExpression<T>();
        //        dynamicRules(rules);
        //    }

        //    return data;
        //}

        private void dynamicRules(List<ExpRecurso> rules)
        {

            if (rules != null)
            {
                foreach (var item in rules)
                {
                    if (item.type != null)
                    {
                        item.unidad ??= UnitOfWork;

                        var expr = item.exp.GetType().GetMethod("ToBooleanExpression");
                        var methodGen = expr.MakeGenericMethod(item.type);
                        var resultexpr = methodGen.Invoke(item.exp, new object[] { null });

                        Type repositoryType = typeof(BaseRepository<>).MakeGenericType(new[] { item.type });
                        object repository = Activator.CreateInstance(repositoryType, item.unidad);

                        var method = repository.GetType().GetMethod("Exists", new Type[] { resultexpr.GetType() });
                        var result = method.Invoke(repository, new object[] { resultexpr });

                        if (Convert.ToBoolean(result) == item.alError)
                        {
                            AddExeptionMessages(item.Recurso.text);
                            throw GetBusinessExeption();
                        }

                    }
                    else if (UnitOfWork.Repository<T>().Table.Any(item.exp.ToBooleanExpression<T>()) == item.alError)
                    {
                        AddExeptionMessages(item.Recurso.text);
                        throw GetBusinessExeption();
                    }
                }
            }
        }
        #endregion Framework Bussiness Rules

    }
}
