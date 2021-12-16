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
    /// EntregaResultadosNoLectura object for mapped table EntregaResultadosNoLectura.
    /// </summary>
    [Table("EntregaResultadosNoLectura")]
    public partial class EntregaResultadosNoLectura : BaseEntity
    {

       #region Columnas normales)

       [Column("Fecha", TypeName = "datetime")]
       [DDisplayName("EntregaResultadosNoLectura.Fecha")]
       [DRequired("EntregaResultadosNoLectura.Fecha")]
       public virtual DateTime Fecha { get; set; }

       [Column("NumeroIdentificacion")]
       [DDisplayName("EntregaResultadosNoLectura.NumeroIdentificacion")]
       [DRequired("EntregaResultadosNoLectura.NumeroIdentificacion")]
       [DStringLength("EntregaResultadosNoLectura.NumeroIdentificacion",20)]
       public virtual String NumeroIdentificacion { get; set; }

       [Column("Nombres")]
       [DDisplayName("EntregaResultadosNoLectura.Nombres")]
       [DRequired("EntregaResultadosNoLectura.Nombres")]
       [DStringLength("EntregaResultadosNoLectura.Nombres",60)]
       public virtual String Nombres { get; set; }

       [Column("Apellidos")]
       [DDisplayName("EntregaResultadosNoLectura.Apellidos")]
       [DRequired("EntregaResultadosNoLectura.Apellidos")]
       [DStringLength("EntregaResultadosNoLectura.Apellidos",60)]
       public virtual String Apellidos { get; set; }

       [Column("Telefono")]
       [DDisplayName("EntregaResultadosNoLectura.Telefono")]
       [DStringLength("EntregaResultadosNoLectura.Telefono",20)]
       public virtual String Telefono { get; set; }

       [Column("Observaciones")]
       [DDisplayName("EntregaResultadosNoLectura.Observaciones")]
       [DStringLength("EntregaResultadosNoLectura.Observaciones",1024)]
       public virtual String Observaciones { get; set; }

       [Column("Email")]
       [DDisplayName("EntregaResultadosNoLectura.Email")]
       [DStringLength("EntregaResultadosNoLectura.Email",255)]
       public virtual String Email { get; set; }

       #endregion

       #region Columnas referenciales)

       [Column("TiposIdentificacionid")]
       [DDisplayName("EntregaResultadosNoLectura.TiposIdentificacionid")]
       [DRequired("EntregaResultadosNoLectura.TiposIdentificacionid")]
       [DRequiredFK("EntregaResultadosNoLectura.TiposIdentificacionid")]
       public virtual Int64 TiposIdentificacionid { get; set; }

       [Column("ParentescosId")]
       [DDisplayName("EntregaResultadosNoLectura.ParentescosId")]
       [DRequired("EntregaResultadosNoLectura.ParentescosId")]
       [DRequiredFK("EntregaResultadosNoLectura.ParentescosId")]
       public virtual Int64 ParentescosId { get; set; }

       [Column("MediosEntragasId")]
       [DDisplayName("EntregaResultadosNoLectura.MediosEntragasId")]
       [DRequired("EntregaResultadosNoLectura.MediosEntragasId")]
       [DRequiredFK("EntregaResultadosNoLectura.MediosEntragasId")]
       public virtual Int64 MediosEntragasId { get; set; }

       [Column("ContanciaArchivosId")]
       [DDisplayName("EntregaResultadosNoLectura.ContanciaArchivosId")]
       public virtual Int64? ContanciaArchivosId { get; set; }

       [Column("PacientesId")]
       [DDisplayName("EntregaResultadosNoLectura.PacientesId")]
       [DRequired("EntregaResultadosNoLectura.PacientesId")]
       [DRequiredFK("EntregaResultadosNoLectura.PacientesId")]
       public virtual Int64 PacientesId { get; set; }

       #endregion

       #region Propiedades referencias de entrada)

       [ForeignKey("ContanciaArchivosId")]
       public virtual Archivos ContanciaArchivos { get; set; }

       [ForeignKey("MediosEntragasId")]
       public virtual MediosEntregas MediosEntragas { get; set; }

       [ForeignKey("PacientesId")]
       public virtual Pacientes Pacientes { get; set; }

       [ForeignKey("ParentescosId")]
       public virtual Parentescos Parentescos { get; set; }

       [ForeignKey("TiposIdentificacionid")]
       public virtual TiposIdentificacion TiposIdentificacion { get; set; }

       #endregion

       #region Reglas expression

       public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<EntregaResultadosNoLectura, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<EntregaResultadosNoLectura, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<EntregaResultadosNoLectura, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetEliminarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<EntregaResultadosNoLecturaDetalles, bool>> expression0 = entity => entity.EntregaResultadosNoLecturaId == this.Id;
        rules.Add(new ExpRecurso(expression0.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","EntregaResultadosNoLecturaDetalles"), typeof(EntregaResultadosNoLecturaDetalles)));

       return rules;
       }

       #endregion
    }
 }
