using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor.Infrastructure.Entities
{
    public partial class AvisosInformativos
    {
        [NotMapped]
        public bool MostrarMensaje { get; set; }
    }
}
