using Dominus.Backend.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor.Infrastructure.Entities
{

    public partial class ProgramacionCitas
    {
        #region Referencia de salida

        public virtual List<Admisiones> Admisiones { get; set; } = new List<Admisiones>();

        #endregion

        [NotMapped]
        [DDisplayName("ProgramacionCitas.HoraInicio")]
        public DateTime HoraInicio { get; set; }

        [NotMapped]
        [DDisplayName("ProgramacionCitas.HoraFinal")]
        public DateTime HoraFinal { get; set; }

    }
}
