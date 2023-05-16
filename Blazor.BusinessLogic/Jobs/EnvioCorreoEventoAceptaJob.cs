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
        #region IniciarJob

        public async Task Execute(IJobExecutionContext context)
        {
            var tenant = DApp.Tenants.FirstOrDefault(x => x.Code == context.JobDetail.JobDataMap.GetString("TenantCode"));
            try
            {
                if (tenant != null)
                {
                    await EjecutarTarea(tenant.DataBaseSetting);
                    new JobsBusinessLogic(tenant.DataBaseSetting).SaveJobLog(nameof(EnvioCorreoEventoAceptaJob), true);
                }
            }
            catch (System.Exception ex)
            {
                new JobsBusinessLogic(tenant.DataBaseSetting).SaveJobLog(nameof(EnvioCorreoEventoAceptaJob), false, ex.GetFullErrorMessage());
            }
        }

        #endregion

        private async Task EjecutarTarea(DataBaseSetting dataBaseSetting)
        {
            await new JobsBusinessLogic(dataBaseSetting).EnvioCorreoEventoAcepta();
        }
    }
}
