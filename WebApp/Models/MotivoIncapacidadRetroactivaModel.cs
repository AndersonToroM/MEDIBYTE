using Blazor.Infrastructure.Entities;

namespace Blazor.WebApp.Models
{

   public partial class MotivoIncapacidadRetroactivaModel
   {
      public MotivoIncapacidadRetroactiva Entity { get; set; }

      public MotivoIncapacidadRetroactivaModel()
      {
         Entity = new MotivoIncapacidadRetroactiva();
      }

   }

}
