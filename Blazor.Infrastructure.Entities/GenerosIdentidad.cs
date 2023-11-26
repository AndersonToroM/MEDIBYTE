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
    [Table("GenerosIdentidad")]
    public partial class GenerosIdentidad : BaseEntity
    {

        #region Columnas normales)

        [Column("Nombre")]
        [DDisplayName("GenerosIdentidad.Nombre")]
        [DRequired("GenerosIdentidad.Nombre")]
        [DStringLength("GenerosIdentidad.Nombre", 45)]
        public virtual String Nombre { get; set; }

        #endregion

        #region Reglas expression

        public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
        {
            Expression<Func<Generos, bool>> expression = entity => entity.Id == this.Id;
            return expression as Expression<Func<T, bool>>;
        }

        public override List<ExpRecurso> GetAdicionarExpression<T>()
        {
            var rules = new List<ExpRecurso>();
            Expression<Func<Generos, bool>> expression = null;

            return rules;
        }

        public override List<ExpRecurso> GetModificarExpression<T>()
        {
            var rules = new List<ExpRecurso>();
            Expression<Func<Generos, bool>> expression = null;

            return rules;
        }

        public override List<ExpRecurso> GetEliminarExpression<T>()
        {
            var rules = new List<ExpRecurso>();

            Expression<Func<Pacientes, bool>> expression2 = entity => entity.GenerosIdentidadId == this.Id;
            rules.Add(new ExpRecurso(expression2.ToExpressionNode(), new Recurso("BLL.BUSINESS.DELETE_REL", "Pacientes"), typeof(Pacientes)));

            return rules;
        }

        #endregion
    }
}
