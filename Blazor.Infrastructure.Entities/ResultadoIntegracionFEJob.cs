using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Dominus.Backend.Data;
using Dominus.Backend.DataBase;

namespace Blazor.Infrastructure.Entities
{
    /// <summary>
    /// ConfiguracionEnvioEmailJob object for mapped table ConfiguracionEnvioEmailJob.
    /// </summary>
    [Table("ResultadoIntegracionFEJob")]
    public partial class ResultadoIntegracionFEJob : BaseEntity
    {

        #region Columnas normales)

        [Column("Host")]
        [DDisplayName("ResultadoIntegracionFEJob.Host")]
        [DStringLength("ResultadoIntegracionFEJob.Host", 250)]
        public virtual String Host { get; set; }

        [Column("Tipo")]
        [DDisplayName("ResultadoIntegracionFEJob.Tipo")]
        [DRequired("ResultadoIntegracionFEJob.Tipo")]
        public virtual Int32 Tipo { get; set; }

        [Column("IdTipo")]
        [DDisplayName("ResultadoIntegracionFEJob.IdTipo")]
        [DRequired("ResultadoIntegracionFEJob.IdTipo")]
        public virtual Int64 IdTipo { get; set; }

        [Column("Ejecutado")]
        [DDisplayName("ResultadoIntegracionFEJob.Ejecutado")]
        [DRequired("ResultadoIntegracionFEJob.Ejecutado")]
        public virtual Boolean Ejecutado { get; set; }

        [Column("Exitoso")]
        [DDisplayName("ResultadoIntegracionFEJob.Exitoso")]
        [DRequired("ResultadoIntegracionFEJob.Exitoso")]
        public virtual Boolean Exitoso { get; set; }

        [Column("Detalle")]
        [DDisplayName("ResultadoIntegracionFEJob.Detalle")]
        public virtual String Detalle { get; set; }

        [Column("Intentos")]
        [DDisplayName("ResultadoIntegracionFEJob.Intentos")]
        [DRequired("ResultadoIntegracionFEJob.Intentos")]
        public virtual Int32 Intentos { get; set; }

        #endregion

        #region Reglas expression

        public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
        {
            Expression<Func<ConfiguracionEnvioEmailJob, bool>> expression = entity => entity.Id == this.Id;
            return expression as Expression<Func<T, bool>>;
        }

        public override List<ExpRecurso> GetAdicionarExpression<T>()
        {
            var rules = new List<ExpRecurso>();
            Expression<Func<ConfiguracionEnvioEmailJob, bool>> expression = null;

            return rules;
        }

        public override List<ExpRecurso> GetModificarExpression<T>()
        {
            var rules = new List<ExpRecurso>();
            Expression<Func<ConfiguracionEnvioEmailJob, bool>> expression = null;

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
