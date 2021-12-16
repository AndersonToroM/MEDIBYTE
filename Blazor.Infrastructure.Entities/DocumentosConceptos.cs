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
    /// DocumentosConceptos object for mapped table DocumentosConceptos.
    /// </summary>
    [Table("DocumentosConceptos")]
    public partial class DocumentosConceptos : BaseEntity
    {

       #region Columnas normales)

       #endregion

       #region Columnas referenciales)

       [Column("EstadosConceptoId")]
       [DDisplayName("DocumentosConceptos.EstadosConceptoId")]
       [DRequired("DocumentosConceptos.EstadosConceptoId")]
       [DRequiredFK("DocumentosConceptos.EstadosConceptoId")]
       public virtual Int64 EstadosConceptoId { get; set; }

       [Column("DocumentosId")]
       [DDisplayName("DocumentosConceptos.DocumentosId")]
       [DRequired("DocumentosConceptos.DocumentosId")]
       [DRequiredFK("DocumentosConceptos.DocumentosId")]
       public virtual Int64 DocumentosId { get; set; }

       #endregion

       #region Propiedades referencias de entrada)

       [ForeignKey("DocumentosId")]
       public virtual Documentos Documentos { get; set; }

       [ForeignKey("EstadosConceptoId")]
       public virtual Estados EstadosConcepto { get; set; }

       #endregion

       #region Reglas expression

       public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<DocumentosConceptos, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<DocumentosConceptos, bool>> expression = null;

        expression = entity => entity.EstadosConceptoId == this.EstadosConceptoId && entity.DocumentosId == this.DocumentosId;
        rules.Add(new ExpRecurso(expression.ToExpressionNode() , new Recurso("BLL.BUSINESS.UNIQUE", "DocumentosConceptos.EstadosConceptoId" , "DocumentosConceptos.DocumentosId" )));

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<DocumentosConceptos, bool>> expression = null;

        expression = entity => !(entity.Id == this.Id && entity.EstadosConceptoId == this.EstadosConceptoId && entity.DocumentosId == this.DocumentosId)
                               && entity.EstadosConceptoId == this.EstadosConceptoId && entity.DocumentosId == this.DocumentosId;
        rules.Add(new ExpRecurso(expression.ToExpressionNode() , new Recurso("BLL.BUSINESS.UNIQUE", "DocumentosConceptos.EstadosConceptoId" , "DocumentosConceptos.DocumentosId" )));

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
