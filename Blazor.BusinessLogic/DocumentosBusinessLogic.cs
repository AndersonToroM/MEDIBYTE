using Blazor.BusinessLogic.Models;
using Blazor.BusinessLogic.ServiciosExternos;
using Blazor.Infrastructure;
using Blazor.Infrastructure.Entities;
using Blazor.Infrastructure.Entities.Models;
using DevExpress.Xpo;
using Dominus.Backend.DataBase;
using Dominus.Frontend.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.BusinessLogic
{
    public class DocumentosBusinessLogic : GenericBusinessLogic<Documentos>
    {
        public DocumentosBusinessLogic(IUnitOfWork unitWork) : base(unitWork)
        {
        }

        public DocumentosBusinessLogic(DataBaseSetting configuracionBD) : base(configuracionBD)
        {
        }

        public async Task<IntegracionSeriesFEModel> ObtenerDatosSeries(long idDocumento, string host)
        {
            IntegracionSeriesFEModel integracionSeriesFEModel = new IntegracionSeriesFEModel();
            try
            {
                BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
                var parametros = unitOfWork.Repository<ParametrosGenerales>().Table.FirstOrDefault();
                var documento = unitOfWork.Repository<Documentos>().Table.FirstOrDefault(x => x.Id == idDocumento);

                if (documento == null)
                {
                    throw new Exception($"El documento con el id {idDocumento} no existe o no se encuentra registrado en nuestro sistema.");
                }

                IntegracionFE integracionRips = new IntegracionFE(parametros, host);

                integracionSeriesFEModel = await integracionRips.GetResultadoSeries();
                if (!integracionSeriesFEModel.HuboErrorFE && !integracionSeriesFEModel.HuboErrorIntegracion &&
                    integracionSeriesFEModel.ResultadoSeries != null && integracionSeriesFEModel.ResultadoSeries.Any())
                {
                    var docSerie = integracionSeriesFEModel.ResultadoSeries.FirstOrDefault(x=> string.Equals(x.Prefix,documento.Prefijo, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(x.Status, "Active", StringComparison.OrdinalIgnoreCase));

                    if (docSerie == null)
                    {
                        throw new Exception($"No se encontro una serie en estado activo para el documento con el prefijo {documento.Prefijo} en el portal de facturacion electronica.");
                    }

                    integracionSeriesFEModel.ExternalKey = docSerie.ExternalKey;
                    integracionSeriesFEModel.TechnicalKey = docSerie.TechnicalKey;
                }
            }
            catch (Exception ex)
            {
                integracionSeriesFEModel.HuboErrorIntegracion = true;
                integracionSeriesFEModel.ErrorIntegracion = ex.GetFullErrorMessage();
            }

            integracionSeriesFEModel.ResultadoSeries = null;

            return integracionSeriesFEModel;
        }
    }

}

