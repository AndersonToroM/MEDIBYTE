using Blazor.Infrastructure.Entities;

namespace Blazor.WebApp.Models
{

   public partial class DocumentosConceptosModel
   {
      public DocumentosConceptos Entity { get; set; }

      public DocumentosConceptosModel()
      {
         Entity = new DocumentosConceptos();
      }

   }

}
