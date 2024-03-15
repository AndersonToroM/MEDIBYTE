using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Serialize.Linq.Extensions;
using Dominus.Backend.Data;
using Dominus.Backend.DataBase;

namespace Blazor.Infrastructure.Entities
{
    /// <summary>
    /// ResultadoIntegracionRips object for mapped table ResultadoIntegracionRips.
    /// </summary>
    [Table("ResultadoIntegracionRips")]
    public partial class ResultadoIntegracionRips : BaseEntity
    {

       #region Columnas normales)

       [Column("HuboError")]
       [DDisplayName("ResultadoIntegracionRips.HuboError")]
       [DRequired("ResultadoIntegracionRips.HuboError")]
       public virtual Boolean HuboError { get; set; }

       [Column("Error")]
       [DDisplayName("ResultadoIntegracionRips.Error")]
       public virtual String Error { get; set; }

       [Column("HttpStatus")]
       [DDisplayName("ResultadoIntegracionRips.HttpStatus")]
       public virtual Int32? HttpStatus { get; set; }

       [Column("JsonResult")]
       [DDisplayName("ResultadoIntegracionRips.JsonResult")]
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
