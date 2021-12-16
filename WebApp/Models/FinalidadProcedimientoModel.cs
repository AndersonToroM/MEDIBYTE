using Blazor.Infrastructure.Entities;

namespace Blazor.WebApp.Models
{

   public partial class FinalidadProcedimientoModel
   {
      public FinalidadProcedimiento Entity { get; set; }

      public FinalidadProcedimientoModel()
      {
         Entity = new FinalidadProcedimiento();
      }

   }

}
