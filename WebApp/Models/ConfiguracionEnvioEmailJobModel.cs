using Blazor.Infrastructure.Entities;

namespace Blazor.WebApp.Models
{

   public partial class ConfiguracionEnvioEmailJobModel
   {
      public ConfiguracionEnvioEmailJob Entity { get; set; }

      public ConfiguracionEnvioEmailJobModel()
      {
         Entity = new ConfiguracionEnvioEmailJob();
      }

   }

}
