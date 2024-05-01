using Dominus.Backend.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor.Infrastructure.Entities
{
    public partial class Notas 
    {
        [NotMapped]
        public List<NotasDetalles> NotasDetalles { get; set; }

        [NotMapped]
        [DDisplayName("Notas.EsFacturaInstitucional")]
        public bool EsNotaInstitucional
        {
            get
            {
                return EntidadesId.HasValue;
            }
        }

    }
}
