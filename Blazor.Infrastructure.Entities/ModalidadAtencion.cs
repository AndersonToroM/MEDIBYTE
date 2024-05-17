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
    /// ModalidadAtencion object for mapped table ModalidadAtencion.
    /// </summary>
    [Table("ModalidadAtencion")]
    public partial class ModalidadAtencion : BaseEntity
    {

       #region Columnas normales)

       [Column("Codigo")]
       [DDisplayName("ModalidadAtencion.Codigo")]
       [DRequired("ModalidadAtencion.Codigo")]
       [DStringLength("ModalidadAtencion.Codigo",5)]
       public virtual String Codigo { get; set; }

       [Column("Nombre")]
       [DDisplayName("ModalidadAtencion.Nombre")]
       [DRequired("ModalidadAtencion.Nombre")]
       [DStringLength("ModalidadAtencion.Nombre",255)]
       public virtual String Nombre { get; set; }

       #endregion

       #region Reglas expression

       public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<ModalidadAtencion, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<ModalidadAtencion, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<ModalidadAtencion, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetEliminarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<Admisiones, bool>> expression0 = entity => entity.ModalidadAtencionId == this.Id;
        rules.Add(new ExpRecurso(expression0.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Admisiones"), typeof(Admisiones)));

       return rules;
       }

       #endregion
    }
 }
