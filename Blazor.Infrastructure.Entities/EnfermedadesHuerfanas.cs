using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Serialize.Linq.Extensions;
using Dominus.Backend.Data;
using Dominus.Backend.DataBase;

namespace Blazor.Infrastructure.Entities
{

    [Table("EnfermedadesHuerfanas")]
    public partial class EnfermedadesHuerfanas : BaseEntity
    {

        #region Columnas normales)

        [Column("NombreV4")]
        [DDisplayName("EnfermedadesHuerfanas.NombreV4")]
        [DRequired("EnfermedadesHuerfanas.NombreV4")]
        [DStringLength("EnfermedadesHuerfanas.NombreV4", 250)]
        public virtual String NombreV4 { get; set; }

        [Column("CIE10")]
        [DDisplayName("EnfermedadesHuerfanas.CIE10")]
        [DRequired("EnfermedadesHuerfanas.CIE10")]
        [DStringLength("EnfermedadesHuerfanas.CIE10", 50)]
        public virtual String CIE10 { get; set; }


        #endregion


        #region Reglas expression

        public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<EnfermedadesHuerfanas, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<EnfermedadesHuerfanas, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<EnfermedadesHuerfanas, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetEliminarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<Atenciones, bool>> expression0 = entity => entity.EnfermedadesHuerfanasId == this.Id;
        rules.Add(new ExpRecurso(expression0.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Atenciones"), typeof(Atenciones)));

       return rules;
       }

       #endregion
    }
 }