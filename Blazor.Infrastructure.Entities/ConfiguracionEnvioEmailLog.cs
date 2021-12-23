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
    /// ConfiguracionEnvioEmailLog object for mapped table ConfiguracionEnvioEmailLog.
    /// </summary>
    [Table("ConfiguracionEnvioEmailLog")]
    public partial class ConfiguracionEnvioEmailLog : BaseEntity
    {

       #region Columnas normales)

       [Column("Origen")]
       [DDisplayName("ConfiguracionEnvioEmailLog.Origen")]
       [DRequired("ConfiguracionEnvioEmailLog.Origen")]
       [DStringLength("ConfiguracionEnvioEmailLog.Origen",50)]
       public virtual String Origen { get; set; }

       [Column("CorreoEnvia")]
       [DDisplayName("ConfiguracionEnvioEmailLog.CorreoEnvia")]
       [DRequired("ConfiguracionEnvioEmailLog.CorreoEnvia")]
       [DStringLength("ConfiguracionEnvioEmailLog.CorreoEnvia",250)]
       public virtual String CorreoEnvia { get; set; }

       [Column("CorreosDestinatarios")]
       [DDisplayName("ConfiguracionEnvioEmailLog.CorreosDestinatarios")]
       [DRequired("ConfiguracionEnvioEmailLog.CorreosDestinatarios")]
       public virtual String CorreosDestinatarios { get; set; }

       [Column("Exitoso")]
       [DDisplayName("ConfiguracionEnvioEmailLog.Exitoso")]
       [DRequired("ConfiguracionEnvioEmailLog.Exitoso")]
       public virtual Boolean Exitoso { get; set; }

       [Column("Error")]
       [DDisplayName("ConfiguracionEnvioEmailLog.Error")]
       public virtual String Error { get; set; }

       [Column("ErrorDeDatos")]
       [DDisplayName("ConfiguracionEnvioEmailLog.ErrorDeDatos")]
       public virtual String ErrorDeDatos { get; set; }

       [Column("Asunto")]
       [DDisplayName("ConfiguracionEnvioEmailLog.Asunto")]
       [DRequired("ConfiguracionEnvioEmailLog.Asunto")]
       [DStringLength("ConfiguracionEnvioEmailLog.Asunto",300)]
       public virtual String Asunto { get; set; }

       [Column("MetodoUso")]
       [DDisplayName("ConfiguracionEnvioEmailLog.MetodoUso")]
       [DRequired("ConfiguracionEnvioEmailLog.MetodoUso")]
       [DStringLength("ConfiguracionEnvioEmailLog.MetodoUso",150)]
       public virtual String MetodoUso { get; set; }

       #endregion

       #region Reglas expression

       public override Expression<Func<T, bool>> PrimaryKeyExpression<T>()
       {
       Expression<Func<ConfiguracionEnvioEmailLog, bool>> expression = entity => entity.Id == this.Id;
       return expression as Expression<Func<T, bool>>;
       }

       public override List<ExpRecurso> GetAdicionarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<ConfiguracionEnvioEmailLog, bool>> expression = null;

       return rules;
       }

       public override List<ExpRecurso> GetModificarExpression<T>()
       {
        var rules = new List<ExpRecurso>();
        Expression<Func<ConfiguracionEnvioEmailLog, bool>> expression = null;

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
