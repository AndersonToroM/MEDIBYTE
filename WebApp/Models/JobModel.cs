using Blazor.Infrastructure.Entities;

namespace Blazor.WebApp.Models
{

   public partial class JobModel
   {
      public Job Entity { get; set; }

      public JobModel()
      {
         Entity = new Job();
      }

   }

}
