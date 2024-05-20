using Blazor.Infrastructure.Entities;

namespace Blazor.WebApp.Models
{

   public partial class AvisosInformativosModel
   {
      public AvisosInformativos Entity { get; set; }

      public AvisosInformativosModel()
      {
         Entity = new AvisosInformativos();
      }

   }

}
