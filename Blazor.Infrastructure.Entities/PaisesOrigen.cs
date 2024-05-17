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
    /// PaisesOrigen object for mapped table PaisesOrigen.
    /// </summary>
    [Table("PaisesOrigen")]
    public partial class PaisesOrigen : BaseEntity
    {

       #region Columnas normales)

       [Column("Codigo")]
       [DDisplayName("PaisesOrigen.Codigo")]
       [DRequired("PaisesOrigen.Codigo")]
       [DStringLength("PaisesOrigen.Codigo",5)]
       public virtual String Codigo { get; set; }

       [Column("CodigoISO3166Num")]
       [DDisplayName("PaisesOrigen.CodigoISO3166Num")]
       [DRequired("PaisesOrigen.CodigoISO3166Num")]
       [DStringLength("PaisesOrigen.CodigoISO3166Num",5)]
       public virtual String CodigoISO3166Num { get; set; }

       [Column("Nombre")]
       [DDisplayName("PaisesOrigen.Nombre")]
       [DRequired("PaisesOrigen.Nombre")]
       [DStringLength("PaisesOrigen.Nombre",255)]
       public virtual String Nombre { get; set; }

       #endregion

       #region Reglas expression

       public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<PaisesOrigen, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<PaisesOrigen, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<PaisesOrigen, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetEliminarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<Pacientes, bool>> expression0 = entity => entity.PaisesOrigenId == this.Id;
        rules.Add(new ExpRecurso(expression0.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Pacientes"), typeof(Pacientes)));

       return rules;
       }

       #endregion
    }
 }
