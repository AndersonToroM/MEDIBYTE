using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Dominus.Backend.Data;
using Dominus.Backend.DataBase;

namespace Blazor.Infrastructure.Entities
{
    /// <summary>
    /// ResultadoIntegracionFE object for mapped table ResultadoIntegracionFE.
    /// </summary>
    [Table("ResultadoIntegracionFE")]
    public partial class ResultadoIntegracionFE : BaseEntity
    {

        #region Columnas normales)

        [Column("Tipo")]
        [DDisplayName("ResultadoIntegracionFE.Tipo")]
        [DRequired("ResultadoIntegracionFE.Tipo")]
        public virtual Int32 Tipo { get; set; }

        [Column("IdTipo")]
        [DDisplayName("ResultadoIntegracionFE.IdTipo")]
        [DRequired("ResultadoIntegracionFE.IdTipo")]
        public virtual Int64 IdTipo { get; set; }

        [Column("Api")]
        [DDisplayName("ResultadoIntegracionFE.Api")]
        [DStringLength("ResultadoIntegracionFE.Api", 2048)]
        public virtual String Api { get; set; }

        [Column("HuboError")]
        [DDisplayName("ResultadoIntegracionFE.HuboError")]
        [DRequired("ResultadoIntegracionFE.HuboError")]
        public virtual Boolean HuboError { get; set; }

        [Column("Error")]
        [DDisplayName("ResultadoIntegracionFE.Error")]
        public virtual String Error { get; set; }

        [Column("HttpStatus")]
        [DDisplayName("ResultadoIntegracionFE.HttpStatus")]
        public virtual Int32? HttpStatus { get; set; }

        [Column("JsonResult")]
        [DDisplayName("ResultadoIntegracionFE.JsonResult")]
        public virtual String JsonResult { get; set; }

        #endregion

        #region Reglas expression

        public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
        {
            Expression<Func<ResultadoIntegracionRips, bool>> expression = entity => entity.Id == this.Id;
            return expression as Expression<Func<T, bool>>;
        }

        public override List<ExpRecurso> GetAdicionarExpression<T>()
        {
            var rules = new List<ExpRecurso>();
            Expression<Func<ResultadoIntegracionRips, bool>> expression = null;

            return rules;
        }

        public override List<ExpRecurso> GetModificarExpression<T>()
        {
            var rules = new List<ExpRecurso>();
            Expression<Func<ResultadoIntegracionRips, bool>> expression = null;

            return rules;
        }

        public override List<ExpRecurso> GetEliminarExpression<T>()
        {
            var rules = new List<ExpRecurso>();
            return rules;
        }

        #endregion
    }
}
