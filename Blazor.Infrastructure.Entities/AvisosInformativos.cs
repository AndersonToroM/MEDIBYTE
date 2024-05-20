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
    /// AvisosInformativos object for mapped table AvisosInformativos.
    /// </summary>
    [Table("AvisosInformativos")]
    public partial class AvisosInformativos : BaseEntity
    {

       #region Columnas normales)

       [Column("Titulo")]
       [DDisplayName("AvisosInformativos.Titulo")]
       [DRequired("AvisosInformativos.Titulo")]
       [DStringLength("AvisosInformativos.Titulo",500)]
       public virtual String Titulo { get; set; }

       [Column("Contenido")]
       [DDisplayName("AvisosInformativos.Contenido")]
       [DRequired("AvisosInformativos.Contenido")]
       public virtual String Contenido { get; set; }

       [Column("MostrarHasta", TypeName = "datetime")]
       [DDisplayName("AvisosInformativos.MostrarHasta")]
       [DRequired("AvisosInformativos.MostrarHasta")]
       public virtual DateTime MostrarHasta { get; set; }

       [Column("Activo")]
       [DDisplayName("AvisosInformativos.Activo")]
       [DRequired("AvisosInformativos.Activo")]
       public virtual Boolean Activo { get; set; }

       #endregion

       #region Reglas expression

       public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<AvisosInformativos, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<AvisosInformativos, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<AvisosInformativos, bool>> expression = null;

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
