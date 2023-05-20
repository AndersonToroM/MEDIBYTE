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
                    await EjecutarRutina(tenant.DataBaseSetting);
                    new JobsBusinessLogic(tenant.DataBaseSetting).SaveJobLog(nameof(EnvioCorreoEventoAceptaJob), true, "Envio Correo Accepta");
                }
            }
            catch (System.Exception ex)
            {
                new JobsBusinessLogic(tenant.DataBaseSetting).SaveJobLog(nameof(EnvioCorreoEventoAceptaJob), false, "Error Ejecutando Rutina", ex.GetFullErrorMessage());
            }
        }

        private async Task EjecutarRutina(DataBaseSetting dataBaseSetting)
        {
            await new JobsBusinessLogic(dataBaseSetting).EnvioCorreoEventoAcepta();
        }
    }
}
