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
    /// MotivoIncapacidadRetroactiva object for mapped table MotivoIncapacidadRetroactiva.
    /// </summary>
    [Table("MotivoIncapacidadRetroactiva")]
    public partial class MotivoIncapacidadRetroactiva : BaseEntity
    {

       #region Columnas normales)

       [Column("Codigo")]
       [DDisplayName("MotivoIncapacidadRetroactiva.Codigo")]
       [DRequired("MotivoIncapacidadRetroactiva.Codigo")]
       [DStringLength("MotivoIncapacidadRetroactiva.Codigo",5)]
       public virtual String Codigo { get; set; }

       [Column("Nombre")]
       [DDisplayName("MotivoIncapacidadRetroactiva.Nombre")]
       [DRequired("MotivoIncapacidadRetroactiva.Nombre")]
       [DStringLength("MotivoIncapacidadRetroactiva.Nombre",255)]
       public virtual String Nombre { get; set; }

       #endregion

       #region Reglas expression

       public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<MotivoIncapacidadRetroactiva, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<MotivoIncapacidadRetroactiva, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<MotivoIncapacidadRetroactiva, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetEliminarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<Incapacidades, bool>> expression0 = entity => entity.MotivoIncapacidadRetroactivaId == this.Id;
        rules.Add(new ExpRecurso(expression0.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Incapacidades"), typeof(Incapacidades)));

       return rules;
       }

       #endregion
    }
 }
