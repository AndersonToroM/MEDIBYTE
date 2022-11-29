using Dominus.Backend.Application;
using Dominus.Backend.Data;
using Serialize.Linq.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Dominus.Backend.DataBase
{
    [KnownType(typeof(List<Int32>))]
    [KnownType(typeof(List<string>))]
    [KnownType(typeof(List<Int64>))]
    [KnownType(typeof(List<Int16>))]
    [ApiEntityAttribute]
    public class BaseEntity : IDisposable
    {
        public BaseEntity()
        {
        }

        public BaseEntity(BaseEntity data)
        {
            this.IsNew = data.IsNew;
            this.Id = data.Id;
            this.CreatedBy = data.CreatedBy;
            this.CreationDate = data.CreationDate;
            this.LastUpdate = data.LastUpdate;
            this.UpdatedBy = data.UpdatedBy;
        }

        public virtual BaseEntity SetData(BaseEntity data)
        {
            this.IsNew = data.IsNew;
            this.Id = data.Id;
            this.CreatedBy = data.CreatedBy;
            this.CreationDate = data.CreationDate;
            this.LastUpdate = data.LastUpdate;
            this.UpdatedBy = data.UpdatedBy;
            return this;
        }

        [DDisplayName("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("UpdatedBy")]
        [DDisplayName("UpdatedBy")]
        public string UpdatedBy { get; set; }

        [Column("CreatedBy")]
        [DDisplayName("CreatedBy")]
        public string CreatedBy { get; set; }

        [Column("LastUpdate", TypeName = "datetime")]
        [DDisplayName("LastUpdate")]
        public DateTime LastUpdate { get; set; }

        [Column("CreationDate", TypeName = "datetime")]
        [DDisplayName("CreationDate")]
        public DateTime CreationDate { get; set; }


        [NotMapped]
        public bool IsNew { get; set; }

        [NotMapped]
        public Dictionary<int, string> Base64Files { get; set; } = new Dictionary<int, string>() { { 0, "" } };



        #region Compare methods

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;
            if (other.Id == this.Id)
                return true;
            if (ReferenceEquals(this, other))
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }

        #endregion

        public virtual Expression<Func<T, bool>> PrimaryKeyExpression<T>() where T : BaseEntity
        {
            return null;
        }

        public virtual List<ExpRecurso> GetAdicionarExpression<T>() where T : BaseEntity
        {
            return null;
        }

        public virtual List<ExpRecurso> GetModificarExpression<T>() where T : BaseEntity
        {
            return null;
        }

        public virtual List<ExpRecurso> GetEliminarExpression<T>() where T : BaseEntity
        {
            return null;
        }

        #region Disposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            // free native resources if there are any.
        }

        public bool IsNullOrWhiteSpace(string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        #endregion

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class ApiEntityAttribute : Attribute
        {
        }
    }

    public class ExpRecurso
    {
        public ExpressionNode exp { get; set; }
        public Recurso Recurso { get; set; }
        public IUnitOfWork unidad { get; set; }
        public Type type { get; set; }
        public bool alError { get; set; }


        public ExpRecurso(ExpressionNode exp, Recurso Recurso, bool alError = true)
        {
            this.exp = exp;
            this.Recurso = Recurso;
            this.alError = alError;
        }

        public ExpRecurso(ExpressionNode exp, Recurso Recurso, Type type, bool alError = true, IUnitOfWork unidad = null)
        {
            this.exp = exp;
            this.Recurso = Recurso;
            this.unidad = unidad;
            this.type = type;
            this.alError = alError;
            this.unidad = unidad;

        }
    }

    public class Recurso
    {

        public string text { get; set; }
        private string texts { get; set; }

        public Recurso(string textFormat, params string[] texts)
        {

            this.texts = "";
            texts ??= new string[] { };
            bool first = true;

            foreach (var item in texts)
            {
                this.texts += $"{(!first ? "," : "")} '{ DApp.DefaultLanguage.GetResource(item)}' ";
                first = false;
            }

            text = string.Format(DApp.DefaultLanguage.GetResource(textFormat), this.texts);
        }

    }
}