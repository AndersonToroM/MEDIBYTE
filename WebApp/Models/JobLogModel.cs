using Blazor.Infrastructure.Entities;

namespace Blazor.WebApp.Models
{

   public partial class JobLogModel
   {
      public JobLog Entity { get; set; }

      public JobLogModel()
      {
         Entity = new JobLog();
      }

   }

}
