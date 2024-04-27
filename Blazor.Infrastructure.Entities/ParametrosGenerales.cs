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
    /// ParametrosGenerales object for mapped table ParametrosGenerales.
    /// </summary>
    [Table("ParametrosGenerales")]
    public partial class ParametrosGenerales : BaseEntity
    {

        #region Columnas normales)

        [Column("HabilitarAnteciones")]
        [DDisplayName("ParametrosGenerales.HabilitarAnteciones")]
        [DRequired("ParametrosGenerales.HabilitarAnteciones")]
        public virtual Boolean HabilitarAnteciones { get; set; }

        [Column("FechaDesdeAtenciones", TypeName = "datetime")]
        [DDisplayName("ParametrosGenerales.FechaDesdeAtenciones")]
        [DRequired("ParametrosGenerales.FechaDesdeAtenciones")]
        public virtual DateTime FechaDesdeAtenciones { get; set; }

        [Column("FechaHastaAtenciones", TypeName = "datetime")]
        [DDisplayName("ParametrosGenerales.FechaHastaAtenciones")]
        [DRequired("ParametrosGenerales.FechaHastaAtenciones")]
        public virtual DateTime FechaHastaAtenciones { get; set; }

        [Column("EsObligatorioAudioLectura")]
        [DDisplayName("ParametrosGenerales.EsObligatorioAudioLectura")]
        [DRequired("ParametrosGenerales.EsObligatorioAudioLectura")]
        public virtual Boolean EsObligatorioAudioLectura { get; set; }

        [Column("UsuarioIntegracionFE")]
        [DDisplayName("ParametrosGenerales.UsuarioIntegracionFE")]
        [DStringLength("ParametrosGenerales.UsuarioIntegracionFE", 255)]
        public virtual String UsuarioIntegracionFE { get; set; }

        [Column("PasswordIntegracionFE")]
        [DDisplayName("ParametrosGenerales.PasswordIntegracionFE")]
        [DStringLength("ParametrosGenerales.PasswordIntegracionFE", 255)]
        public virtual String PasswordIntegracionFE { get; set; }

        [Column("OperadorFE")]
        [DDisplayName("ParametrosGenerales.OperadorFE")]
        [DStringLength("ParametrosGenerales.OperadorFE", 30)]
        public virtual String OperadorFE { get; set; }

        [Column("CompanyIdFE")]
        [DDisplayName("ParametrosGenerales.CompanyIdFE")]
        public virtual Guid? CompanyIdFE { get; set; }

        [Column("EmailRecepcionFE")]
        [DDisplayName("ParametrosGenerales.EmailRecepcionFE")]
        [DStringLength("ParametrosGenerales.EmailRecepcionFE", 250)]
        public virtual String EmailRecepcionFE { get; set; }

        #endregion

        #region Reglas expression

        public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
        {
            Expression<Func<ParametrosGenerales, bool>> expression = entity => entity.Id == this.Id;
            return expression as Expression<Func<T, bool>>;
        }

        public override List<ExpRecurso> GetAdicionarExpression<T>()
        {
            var rules = new List<ExpRecurso>();
            Expression<Func<ParametrosGenerales, bool>> expression = null;

            return rules;
        }

        public override List<ExpRecurso> GetModificarExpression<T>()
        {
            var rules = new List<ExpRecurso>();
            Expression<Func<ParametrosGenerales, bool>> expression = null;

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
