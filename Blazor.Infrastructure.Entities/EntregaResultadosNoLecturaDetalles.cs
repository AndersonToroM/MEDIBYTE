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
    /// EntregaResultadosNoLecturaDetalles object for mapped table EntregaResultadosNoLecturaDetalles.
    /// </summary>
    [Table("EntregaResultadosNoLecturaDetalles")]
    public partial class EntregaResultadosNoLecturaDetalles : BaseEntity
    {

       #region Columnas normales)

       #endregion

       #region Columnas referenciales)

       [Column("EntregaResultadosNoLecturaId")]
       [DDisplayName("EntregaResultadosNoLecturaDetalles.EntregaResultadosNoLecturaId")]
       [DRequired("EntregaResultadosNoLecturaDetalles.EntregaResultadosNoLecturaId")]
       [DRequiredFK("EntregaResultadosNoLecturaDetalles.EntregaResultadosNoLecturaId")]
       public virtual Int64 EntregaResultadosNoLecturaId { get; set; }

       [Column("AdmisionesServiciosPrestadosId")]
       [DDisplayName("EntregaResultadosNoLecturaDetalles.AdmisionesServiciosPrestadosId")]
       [DRequired("EntregaResultadosNoLecturaDetalles.AdmisionesServiciosPrestadosId")]
       [DRequiredFK("EntregaResultadosNoLecturaDetalles.AdmisionesServiciosPrestadosId")]
       public virtual Int64 AdmisionesServiciosPrestadosId { get; set; }

       #endregion

       #region Propiedades referencias de entrada)

       [ForeignKey("AdmisionesServiciosPrestadosId")]
       public virtual AdmisionesServiciosPrestados AdmisionesServiciosPrestados { get; set; }

       [ForeignKey("EntregaResultadosNoLecturaId")]
       public virtual EntregaResultadosNoLectura EntregaResultadosNoLectura { get; set; }

       #endregion

       #region Reglas expression

       public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<EntregaResultadosNoLecturaDetalles, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<EntregaResultadosNoLecturaDetalles, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<EntregaResultadosNoLecturaDetalles, bool>> expression = null;

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
