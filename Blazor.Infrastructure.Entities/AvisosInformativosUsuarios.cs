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
    /// AvisosInformativosUsuarios object for mapped table AvisosInformativosUsuarios.
    /// </summary>
    [Table("AvisosInformativosUsuarios")]
    public partial class AvisosInformativosUsuarios : BaseEntity
    {

       #region Columnas normales)

       [Column("CantidadMostroMensaje")]
       [DDisplayName("AvisosInformativosUsuarios.CantidadMostroMensaje")]
       [DRequired("AvisosInformativosUsuarios.CantidadMostroMensaje")]
       public virtual Int32 CantidadMostroMensaje { get; set; }

       [Column("AceptoMensaje")]
       [DDisplayName("AvisosInformativosUsuarios.AceptoMensaje")]
       [DRequired("AvisosInformativosUsuarios.AceptoMensaje")]
       public virtual Boolean AceptoMensaje { get; set; }

       #endregion

       #region Columnas referenciales)

       [Column("AvisosInformativosId")]
       [DDisplayName("AvisosInformativosUsuarios.AvisosInformativosId")]
       [DRequired("AvisosInformativosUsuarios.AvisosInformativosId")]
       [DRequiredFK("AvisosInformativosUsuarios.AvisosInformativosId")]
       public virtual Int64 AvisosInformativosId { get; set; }

       [Column("UserId")]
       [DDisplayName("AvisosInformativosUsuarios.UserId")]
       [DRequired("AvisosInformativosUsuarios.UserId")]
       [DRequiredFK("AvisosInformativosUsuarios.UserId")]
       public virtual Int64 UserId { get; set; }

       #endregion

       #region Propiedades referencias de entrada)

       [ForeignKey("AvisosInformativosId")]
       public virtual AvisosInformativos AvisosInformativos { get; set; }

       [ForeignKey("UserId")]
       public virtual User User { get; set; }

       #endregion

       #region Reglas expression

       public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<AvisosInformativosUsuarios, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<AvisosInformativosUsuarios, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<AvisosInformativosUsuarios, bool>> expression = null;

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
