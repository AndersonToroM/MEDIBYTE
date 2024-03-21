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
    /// HabilitacionServciosRips object for mapped table HabilitacionServciosRips.
    /// </summary>
    [Table("HabilitacionServciosRips")]
    public partial class HabilitacionServciosRips : BaseEntity
    {

       #region Columnas normales)

       [Column("Codigo")]
       [DDisplayName("HabilitacionServciosRips.Codigo")]
       [DRequired("HabilitacionServciosRips.Codigo")]
       public virtual Int32 Codigo { get; set; }

       [Column("Nombre")]
       [DDisplayName("HabilitacionServciosRips.Nombre")]
       [DRequired("HabilitacionServciosRips.Nombre")]
       [DStringLength("HabilitacionServciosRips.Nombre",255)]
       public virtual String Nombre { get; set; }

       [Column("Descripcion")]
       [DDisplayName("HabilitacionServciosRips.Descripcion")]
       [DRequired("HabilitacionServciosRips.Descripcion")]
       [DStringLength("HabilitacionServciosRips.Descripcion",255)]
       public virtual String Descripcion { get; set; }

       #endregion

       #region Reglas expression

       public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<HabilitacionServciosRips, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<HabilitacionServciosRips, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<HabilitacionServciosRips, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetEliminarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<Servicios, bool>> expression0 = entity => entity.HabilitacionServciosRipsId == this.Id;
        rules.Add(new ExpRecurso(expression0.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Servicios"), typeof(Servicios)));

       return rules;
       }

       #endregion
    }
 }
