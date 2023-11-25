using Blazor.Infrastructure.Entities;

namespace Blazor.WebApp.Models
{

   public partial class RecaudosDetallesModel
   {
        public RecaudosDetalles Entity { get; set; }
        public decimal SubTotalFactura { get; set; }

        public RecaudosDetallesModel()
      {
         Entity = new RecaudosDetalles();
      }

    }

}
