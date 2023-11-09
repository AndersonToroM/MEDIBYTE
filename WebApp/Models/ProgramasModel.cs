using Blazor.Infrastructure.Entities;

namespace Blazor.WebApp.Models
{

   public partial class ProgramasModel
   {
      public Programas Entity { get; set; }

      public ProgramasModel()
      {
         Entity = new Programas();
      }

   }

}
