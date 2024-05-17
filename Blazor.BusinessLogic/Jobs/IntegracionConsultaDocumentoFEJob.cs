using Dominus.Backend.Application;
using Dominus.Frontend.Controllers;
using Quartz;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.BusinessLogic.Jobs
{
    [DisallowConcurrentExecution]
    public class IntegracionConsultaDocumentoFEJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var tenant = DApp.Tenants.FirstOrDefault(x => x.Code == context.JobDetail.JobDataMap.GetString("TenantCode"));
            try
            {
                if (tenant != null)
                {
                    var ejecuto = await new JobsBusinessLogic(tenant.DataBaseSetting).IntegracionConsultaDocumentoFEJob();
                    if (ejecuto)
                    {
                        new JobsBusinessLogic(tenant.DataBaseSetting).SaveJobLog(nameof(IntegracionConsultaDocumentoFEJob), true, "Integracion Consulta Documento FE");
                    }
                }
            }
            catch (System.Exception ex)
            {
                new JobsBusinessLogic(tenant.DataBaseSetting).SaveJobLog(nameof(IntegracionConsultaDocumentoFEJob), false, "Error Ejecutando Rutina", ex.GetFullErrorMessage());
            }
        }
    }
}
