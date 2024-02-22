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
    /// ViaIngresoServicioSalud object for mapped table ViaIngresoServicioSalud.
    /// </summary>
    [Table("ViaIngresoServicioSalud")]
    public partial class ViaIngresoServicioSalud : BaseEntity
    {

       #region Columnas normales)

       [Column("Codigo")]
       [DDisplayName("ViaIngresoServicioSalud.Codigo")]
       [DRequired("ViaIngresoServicioSalud.Codigo")]
       [DStringLength("ViaIngresoServicioSalud.Codigo",5)]
       public virtual String Codigo { get; set; }

       [Column("Nombre")]
       [DDisplayName("ViaIngresoServicioSalud.Nombre")]
       [DRequired("ViaIngresoServicioSalud.Nombre")]
       [DStringLength("ViaIngresoServicioSalud.Nombre",255)]
       public virtual String Nombre { get; set; }

       #endregion

       #region Reglas expression

       public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<ViaIngresoServicioSalud, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<ViaIngresoServicioSalud, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<ViaIngresoServicioSalud, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetEliminarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<Admisiones, bool>> expression0 = entity => entity.ViaIngresoServicioSaludId == this.Id;
        rules.Add(new ExpRecurso(expression0.ToExpressionNode() , new Recurso("BLL.BUSINESS.DELETE_REL","Admisiones"), typeof(Admisiones)));

       return rules;
       }

       #endregion
    }
 }
