using Dominus.Backend.Data;
using Dominus.Backend.DataBase;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor.Infrastructure.Entities.Custom
{
    /// <summary>
    /// Secuences object for mapped table Secuences.
    /// </summary>
    [Table("Secuences")]
    public partial class Secuences : BaseEntity
    {
        [Column("Id")]
        [DDisplayName("Secuences.Id")]
        [DRequired("Secuences.Id")]
        public new String Id { get; set; }

        [Column("Secuence")]
        [DDisplayName("Secuences.Secuence")]
        [DRequired("Secuences.Secuence")]
        public Int32 Secuence { get; set; }

        [NotMapped]
        public new string UpdatedBy { get; set; }

        [NotMapped]
        public new string CreatedBy { get; set; }

        [NotMapped]
        public new DateTime LastUpdate { get; set; }

        [NotMapped]
        public new DateTime CreationDate { get; set; }
    }
}
