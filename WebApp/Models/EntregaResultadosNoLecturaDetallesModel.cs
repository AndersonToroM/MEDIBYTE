using Blazor.Infrastructure.Entities;

namespace Blazor.WebApp.Models
{

   public partial class EntregaResultadosNoLecturaDetallesModel
   {
      public EntregaResultadosNoLecturaDetalles Entity { get; set; }

      public EntregaResultadosNoLecturaDetallesModel()
      {
         Entity = new EntregaResultadosNoLecturaDetalles();
      }

   }

}
