using Dominus.Backend.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor.Infrastructure.Entities
{

    public partial class HabilitacionServciosRips
    {

        [NotMapped]
        [DDisplayName("HabilitacionServciosRips.DescripcionCompleta")]
        public string DescripcionCompleta
        {
            get
            {
                return Codigo + " | " + Nombre + " | " + Descripcion;
            }
            private set
            {
            }
        }

    }
}
