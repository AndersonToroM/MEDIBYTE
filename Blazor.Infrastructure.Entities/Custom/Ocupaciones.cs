using Dominus.Backend.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor.Infrastructure.Entities
{

    public partial class Ocupaciones
    {
        [NotMapped]
        [DDisplayName("Ocupaciones.CodigoDescripcion")]
        public string DescripcionCompleta
        {
            get
            {
                return Codigo + " - " + Descripcion;
            }
            private set
            {
            }
        }

    }
}
