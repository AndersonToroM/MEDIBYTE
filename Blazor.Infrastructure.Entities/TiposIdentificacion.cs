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
    /// TiposIdentificacion object for mapped table TiposIdentificacion.
    /// </summary>
    [Table("TiposIdentificacion")]
    public partial class TiposIdentificacion : BaseEntity
    {

       #region Columnas normales)

       [Column("Nombre")]
       [DDisplayName("TiposIdentificacion.Nombre")]
       [DRequired("TiposIdentificacion.Nombre")]
       [DStringLength("TiposIdentificacion.Nombre",150)]
       public virtual String Nombre { get; set; }

       [Column("Codigo")]
       [DDisplayName("TiposIdentificacion.Codigo")]
       [DStringLength("TiposIdentificacion.Codigo",5)]
       public virtual String Codigo { get; set; }

       [Column("CodigoAlterno")]
       [DDisplayName("TiposIdentificacion.CodigoAlterno")]
       [DStringLength("TiposIdentificacion.CodigoAlterno",5)]
       public virtual String CodigoAlterno { get; set; }

        [Column("CodigoFE")]
        [DDisplayName("TiposIdentificacion.CodigoFE")]
        [DStringLength("TiposIdentificacion.CodigoFE", 5)]
        public virtual String CodigoFE { get; set; }

        #endregion

        #region Reglas expression

        public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<TiposIdentificacion, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<TiposIdentificacion, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<TiposIdentificacion, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetEliminarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<Atenciones, bool>> expression0 = entity => entity.TiposIdentificacionPacienteAtencionesAperturaId == this.Id;
        rules.Add(new ExpRecurso(expression0.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Atenciones"), typeof(Atenciones)));

        Expression<Func<Empleados, bool>> expression1 = entity => entity.TiposIdentificacionId == this.Id;
        rules.Add(new ExpRecurso(expression1.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Empleados"), typeof(Empleados)));

        Expression<Func<Empresas, bool>> expression2 = entity => entity.TiposIdentificacionId == this.Id;
        rules.Add(new ExpRecurso(expression2.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Empresas"), typeof(Empresas)));

        Expression<Func<Empresas, bool>> expression3 = entity => entity.TiposIdentificacionRepresentanteLegalId == this.Id;
        rules.Add(new ExpRecurso(expression3.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Empresas"), typeof(Empresas)));

        Expression<Func<Entidades, bool>> expression4 = entity => entity.TiposIdentificacionId == this.Id;
        rules.Add(new ExpRecurso(expression4.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Entidades"), typeof(Entidades)));

        Expression<Func<EntregaResultados, bool>> expression5 = entity => entity.TiposIdentificacionid == this.Id;
        rules.Add(new ExpRecurso(expression5.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","EntregaResultados"), typeof(EntregaResultados)));

        Expression<Func<EntregaResultadosNoLectura, bool>> expression6 = entity => entity.TiposIdentificacionid == this.Id;
        rules.Add(new ExpRecurso(expression6.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","EntregaResultadosNoLectura"), typeof(EntregaResultadosNoLectura)));

        Expression<Func<Pacientes, bool>> expression7 = entity => entity.TiposIdentificacionId == this.Id;
        rules.Add(new ExpRecurso(expression7.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Pacientes"), typeof(Pacientes)));

        Expression<Func<User, bool>> expression8 = entity => entity.IdentificationTypeId == this.Id;
        rules.Add(new ExpRecurso(expression8.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Users"), typeof(User)));

       return rules;
       }

       #endregion
    }
 }
