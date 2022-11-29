using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dominus.Backend.Data;
using Dominus.Backend.DataBase;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Blazor.Infrastructure.Entities
{
    /// <summary>
    /// Audit object for mapped table Audits.
    /// </summary>
    [Table("VAudits")]
    public partial class AuditFW : BaseEntity
    {

        #region Columnas normales)

        [Column("TableName")]
        [DDisplayName("Audits.TableName")]
        [DRequired("Audits.TableName")]
        [DStringLength("Audits.TableName", 255)]
        public virtual String TableName { get; set; }

        [Column("Action")]
        [DDisplayName("Audits.Action")]
        [DRequired("Audits.Action")]
        [DStringLength("Audits.Action", 50)]
        public virtual String Action { get; set; }

        [Column("TransactionDate")]
        [DDisplayName("Audits.TransactionDate")]
        [DRequired("Audits.TransactionDate")]
        public virtual DateTime TransactionDate { get; set; }

        [Column("KeyValues")]
        [DDisplayName("Audits.KeyValues")]
        [DRequired("Audits.KeyValues")]
        [DStringLength("Audits.KeyValues", 255)]
        public virtual String KeyValues { get; set; }

        [Column("OldValues")]
        [DDisplayName("Audits.OldValues")]
        [DStringLength("Audits.OldValues", 2147483647)]
        public virtual String OldValues { get; set; }

        [Column("NewValues")]
        [DDisplayName("Audits.NewValues")]
        [DStringLength("Audits.NewValues", 2147483647)]
        public virtual String NewValues { get; set; }

        #endregion

    }
    public partial class AuditFWEntry : BaseEntity
    {
        public AuditFWEntry(EntityEntry entry)
        {
            Entry = entry;
        }


        #region Columnas normales)

        public virtual String TableName { get; set; }

        public virtual String Action { get; set; }

        public virtual DateTime TransactionDate { get; set; }

        public Dictionary<string, object> KeyValues { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; set; } = new Dictionary<string, object>();

        public EntityEntry Entry { get; }

        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        #endregion

        public AuditFW ToAuditFW()
        {
            var audit = new AuditFW();
            audit.TableName = TableName;
            audit.TransactionDate = DateTime.UtcNow;
            audit.Action = Action;
            audit.KeyValues = JsonConvert.SerializeObject(KeyValues);
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            audit.CreatedBy = CreatedBy;
            audit.CreationDate = CreationDate;
            audit.UpdatedBy = UpdatedBy;
            audit.LastUpdate = LastUpdate;
            return audit;
        }
    }
}
