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
    /// FinalidadConsulta object for mapped table FinalidadConsulta.
    /// </summary>
    [Table("Programas")]
    public partial class Programas : BaseEntity
    {

        #region Columnas normales)

        [Column("Nombre")]
        [DDisplayName("Programas.Nombre")]
        [DRequired("Programas.Nombre")]
        [DStringLength("Programas.Nombre", 250)]
        public virtual String Nombre { get; set; }


        #endregion

        #region Reglas expression

        public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
        {
            Expression<Func<Programas, bool>> expression = entity => entity.Id == this.Id;
            return expression as Expression<Func<T, bool>>;
        }

        public override List<ExpRecurso> GetAdicionarExpression<T>()
        {
            var rules = new List<ExpRecurso>();
            Expression<Func<Programas, bool>> expression = null;

            expression = entity => entity.Nombre == this.Nombre;
            rules.Add(new ExpRecurso(expression.ToExpressionNode(), new Recurso("BLL.BUSINESS.UNIQUE", "Programas.Nombre")));

            return rules;
        }

        public override List<ExpRecurso> GetModificarExpression<T>()
        {
            var rules = new List<ExpRecurso>();
            Expression<Func<Programas, bool>> expression = null;

            expression = entity => !(entity.Id == this.Id && entity.Nombre == this.Nombre)
                                   && entity.Nombre == this.Nombre;
            rules.Add(new ExpRecurso(expression.ToExpressionNode(), new Recurso("BLL.BUSINESS.UNIQUE", "Programas.Nombre")));

            return rules;
        }

        public override List<ExpRecurso> GetEliminarExpression<T>()
        {
            var rules = new List<ExpRecurso>();
            Expression<Func<Atenciones, bool>> expression0 = entity => entity.ProgramasId == this.Id;
            rules.Add(new ExpRecurso(expression0.ToExpressionNode(), new Recurso("BLL.BUSINESS.DELETE_REL", "Atenciones"), typeof(Atenciones)));

            return rules;
        }

        #endregion
    }
}
