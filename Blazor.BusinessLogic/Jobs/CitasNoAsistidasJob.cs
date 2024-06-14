using Dominus.Backend.Application;
using Quartz;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.BusinessLogic.Jobs
{
    [DisallowConcurrentExecution]
    public class CitasNoAsistidasJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var tenant = DApp.Tenants.FirstOrDefault(x => x.Code == context.JobDetail.JobDataMap.GetString("TenantCode"));
            try
            {
                if (tenant != null)
                {
                    var ejecuto = await new JobsBusinessLogic(tenant.DataBaseSetting).CitasNoAsistidasJob();
                    if (ejecuto)
                    {
                        new JobsBusinessLogic(tenant.DataBaseSetting).SaveJobLog(nameof(CitasNoAsistidasJob), true, "Cita no asistida");
                    }
                }
            }
            catch (System.Exception ex)
            {
                new JobsBusinessLogic(tenant.DataBaseSetting).SaveJobLog(nameof(CitasNoAsistidasJob), false, "Error Ejecutando Rutina", ex.GetBackFullErrorMessage());
            }
        }
    }

}
