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
    /// CausaMotivoAtencion object for mapped table CausaMotivoAtencion.
    /// </summary>
    [Table("CausaMotivoAtencion")]
    public partial class CausaMotivoAtencion : BaseEntity
    {

       #region Columnas normales)

       [Column("Codigo")]
       [DDisplayName("CausaMotivoAtencion.Codigo")]
       [DRequired("CausaMotivoAtencion.Codigo")]
       [DStringLength("CausaMotivoAtencion.Codigo",5)]
       public virtual String Codigo { get; set; }

       [Column("Nombre")]
       [DDisplayName("CausaMotivoAtencion.Nombre")]
       [DRequired("CausaMotivoAtencion.Nombre")]
       [DStringLength("CausaMotivoAtencion.Nombre",255)]
       public virtual String Nombre { get; set; }

       #endregion

       #region Reglas expression

       public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<CausaMotivoAtencion, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<CausaMotivoAtencion, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<CausaMotivoAtencion, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetEliminarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<Atenciones, bool>> expression0 = entity => entity.CausaMotivoAtencionId == this.Id;
        rules.Add(new ExpRecurso(expression0.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Atenciones"), typeof(Atenciones)));

       return rules;
       }

       #endregion
    }
 }