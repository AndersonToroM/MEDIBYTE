using Blazor.Infrastructure.Entities;

namespace Blazor.WebApp.Models
{

   public partial class FinalidadConsultaModel
   {
      public FinalidadConsulta Entity { get; set; }

      public FinalidadConsultaModel()
      {
         Entity = new FinalidadConsulta();
      }

   }

}
