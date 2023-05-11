using Dominus.Backend.Application;
using Dominus.Frontend.Controllers;
using Quartz;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.BusinessLogic.Jobs
{
    [DisallowConcurrentExecution]
    public class InformeEmpresaJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var tenant = DApp.Tenants.FirstOrDefault(x => x.Code == context.JobDetail.JobDataMap.GetString("TenantCode"));
            try
            {
                if (tenant != null)
                {
                    new JobsBusinessLogic(tenant.DataBaseSetting).PruebaDeRutina();
                    new JobsBusinessLogic(tenant.DataBaseSetting).SaveJobLog(nameof(InformeEmpresaJob), true);
                }
            }
            catch (System.Exception ex)
            {
                new JobsBusinessLogic(tenant.DataBaseSetting).SaveJobLog(nameof(InformeEmpresaJob), false, ex.GetFullErrorMessage());
            }
            return Task.CompletedTask;
        }
    }
}
