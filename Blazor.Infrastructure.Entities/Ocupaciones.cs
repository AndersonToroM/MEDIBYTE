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
    /// Generos object for mapped table Generos.
    /// </summary>
    [Table("Ocupaciones")]
    public partial class Ocupaciones : BaseEntity
    {

       #region Columnas normales)

       [Column("Codigo")]
       [DDisplayName("Ocupaciones.Codigo")]
       [DRequired("Ocupaciones.Codigo")]
       [DStringLength("Ocupaciones.Codigo", 25)]
       public virtual String Codigo { get; set; }

        [Column("Descripcion")]
        [DDisplayName("Ocupaciones.Descripcion")]
        [DRequired("Ocupaciones.Descripcion")]
        [DStringLength("Ocupaciones.Descripcion", 255)]
        public virtual String Descripcion { get; set; }

        #endregion

        #region Reglas expression

        public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<Ocupaciones, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<Ocupaciones, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<Ocupaciones, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetEliminarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<Admisiones, bool>> expression0 = entity => entity.OcupacionesId == this.Id;
        rules.Add(new ExpRecurso(expression0.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Diagnosticos"), typeof(Diagnosticos)));

       return rules;
       }

       #endregion
    }
 }
