using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using Dominus.Frontend.Controllers;
using Quartz;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.BusinessLogic.Jobs
{
    [DisallowConcurrentExecution]
    public class EnvioCorreoEventoAceptaJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var tenant = DApp.Tenants.FirstOrDefault(x => x.Code == context.JobDetail.JobDataMap.GetString("TenantCode"));
            try
            {
                if (tenant != null)
                {
                    var ejecuto = await new JobsBusinessLogic(tenant.DataBaseSetting).EnvioCorreoEventoAcepta();
                    if (ejecuto)
                    {
                        new JobsBusinessLogic(tenant.DataBaseSetting).SaveJobLog(nameof(EnvioCorreoEventoAceptaJob), true, "Envio Correo Accepta");
                    }
                }
            }
            catch (System.Exception ex)
            {
                new JobsBusinessLogic(tenant.DataBaseSetting).SaveJobLog(nameof(EnvioCorreoEventoAceptaJob), false, "Error Ejecutando Rutina", ex.GetFullErrorMessage());
            }
        }
    }
}
