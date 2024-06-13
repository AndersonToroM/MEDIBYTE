using Blazor.Infrastructure;
using Blazor.Infrastructure.Entities;
using Dominus.Backend.DataBase;
using System;

namespace Blazor.BusinessLogic
{
    public class HistoriasClinicasBusinessLogic : GenericBusinessLogic<HistoriasClinicas>
    {
        public HistoriasClinicasBusinessLogic(IUnitOfWork unitWork) : base(unitWork)
        {
        }

        public HistoriasClinicasBusinessLogic(DataBaseSetting configuracionBD) : base(configuracionBD)
        {
        }

        public HistoriasClinicas AnularHC(HistoriasClinicas data)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            unitOfWork.BeginTransaction();
            try
            {
                data.EstadosId = 81;// Estado anulada
                data.FechaAnulacion = DateTime.Now;
                data = unitOfWork.Repository<HistoriasClinicas>().Modify(data);
                unitOfWork.CommitTransaction();
                return data;
            }
            catch (Exception)
            {
                unitOfWork.RollbackTransaction();
                throw;
            }

        }

    }
}
