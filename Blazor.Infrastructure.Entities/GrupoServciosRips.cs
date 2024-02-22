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
    /// GrupoServciosRips object for mapped table GrupoServciosRips.
    /// </summary>
    [Table("GrupoServciosRips")]
    public partial class GrupoServciosRips : BaseEntity
    {

       #region Columnas normales)

       [Column("Codigo")]
       [DDisplayName("GrupoServciosRips.Codigo")]
       [DRequired("GrupoServciosRips.Codigo")]
       [DStringLength("GrupoServciosRips.Codigo",5)]
       public virtual String Codigo { get; set; }

       [Column("Nombre")]
       [DDisplayName("GrupoServciosRips.Nombre")]
       [DRequired("GrupoServciosRips.Nombre")]
       [DStringLength("GrupoServciosRips.Nombre",255)]
       public virtual String Nombre { get; set; }

       #endregion

       #region Reglas expression

       public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<GrupoServciosRips, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<GrupoServciosRips, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<GrupoServciosRips, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetEliminarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<Servicios, bool>> expression0 = entity => entity.GrupoServciosRipsId == this.Id;
        rules.Add(new ExpRecurso(expression0.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Servicios"), typeof(Servicios)));

       return rules;
       }

       #endregion
    }
 }
