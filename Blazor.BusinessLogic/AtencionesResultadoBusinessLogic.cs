using Blazor.Infrastructure;
using Blazor.Infrastructure.Entities;
using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using Dominus.Backend.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.BusinessLogic
{
    public class AtencionesResultadoBusinessLogic : GenericBusinessLogic<Atenciones>
    {
        public AtencionesResultadoBusinessLogic(IUnitOfWork unitWork) : base(unitWork)
        {
        }

        public AtencionesResultadoBusinessLogic(DataBaseSetting configuracionBD) : base(configuracionBD)
        {
        }

        public void CambiarProfesional(long empleadoId, List<long> resultadosSelected, string userName)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            unitOfWork.BeginTransaction();
            try
            {
                foreach (var item in resultadosSelected)
                {
                    AtencionesResultado atencionesResultado = unitOfWork.Repository<AtencionesResultado>().FindById(x => x.Id == item, false);
                    atencionesResultado.IsNew = false;
                    atencionesResultado.UpdatedBy = userName;
                    atencionesResultado.LastUpdate = DateTime.Now;
                    atencionesResultado.EmpleadoId = empleadoId;
                    atencionesResultado = unitOfWork.Repository<AtencionesResultado>().Modify(atencionesResultado);
                }
                unitOfWork.CommitTransaction();
            }
            catch
            {
                unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public void MarcarLeidos(long empleadoId, List<long> admisionesServiciosPrestadosId, string userName)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            unitOfWork.BeginTransaction();
            try
            {
                foreach (var item in admisionesServiciosPrestadosId)
                {
                    AtencionesResultado atencionesResultado = new AtencionesResultado();
                    atencionesResultado.IsNew = true;
                    atencionesResultado.CreatedBy = userName;
                    atencionesResultado.CreationDate = DateTime.Now;
                    atencionesResultado.UpdatedBy = userName;
                    atencionesResultado.LastUpdate = DateTime.Now;
                    atencionesResultado.EstadosId = 66;
                    atencionesResultado.ResultadoAudio = null;
                    atencionesResultado.AdmisionesServiciosPrestadosId = item;
                    atencionesResultado.FechaLectura = DateTime.Now;
                    atencionesResultado.EmpleadoId = empleadoId;

                    var existeLectura = unitOfWork.Repository<AtencionesResultado>().Table.Any(x => x.AdmisionesServiciosPrestadosId == item);
                    if (!existeLectura)
                        atencionesResultado = unitOfWork.Repository<AtencionesResultado>().Add(atencionesResultado);
                    else
                    {
                        var servicio = unitOfWork.Repository<AdmisionesServiciosPrestados>().FindById(x => x.Id == item, true).Servicios;
                        throw new Exception($"Ya existe una lectura realizada para el servicio {servicio.DescripcionCompleta}");
                    }

                }

                var admisionesServiciosPrestados = unitOfWork.Repository<AdmisionesServiciosPrestados>()
                    .FindAll(x => admisionesServiciosPrestadosId.Contains(x.Id), false);
                admisionesServiciosPrestados.ForEach(x => { x.LecturaRealizada = true; x.UpdatedBy = userName; x.LastUpdate = DateTime.Now; });
                unitOfWork.Repository<AdmisionesServiciosPrestados>().ModifyRange(admisionesServiciosPrestados);

                unitOfWork.CommitTransaction();
            }
            catch
            {
                unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public AtencionesResultado SubirAudioAResultado(byte[] mp3Bytes, long admisionesServiciosPrestadosId, long id, string userName, long userId)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            unitOfWork.BeginTransaction();
            try
            {
                AtencionesResultado atencionesResultado = new AtencionesResultado();
                atencionesResultado.IsNew = true;
                atencionesResultado.CreatedBy = userName;
                atencionesResultado.CreationDate = DateTime.Now;
                if (id != 0)
                {
                    atencionesResultado = unitOfWork.Repository<AtencionesResultado>().FindById(x => x.Id == id, false);
                    atencionesResultado.IsNew = false;
                }
                atencionesResultado.UpdatedBy = userName;
                atencionesResultado.LastUpdate = DateTime.Now;
                atencionesResultado.EstadosId = 66;
                atencionesResultado.ResultadoAudio = mp3Bytes;
                atencionesResultado.AdmisionesServiciosPrestadosId = admisionesServiciosPrestadosId;
                atencionesResultado.FechaLectura = DateTime.Now;
                var empleado = unitOfWork.Repository<Empleados>().FindById(x => x.UserId == userId, false);
                if (empleado != null)
                {
                    atencionesResultado.EmpleadoId = empleado.Id;
                }
                else
                {
                    throw new Exception("El usuario actual no tiene un empleado asociado.");
                }


                if (atencionesResultado.IsNew)
                {
                    atencionesResultado = unitOfWork.Repository<AtencionesResultado>().Add(atencionesResultado);
                }
                else
                {
                    atencionesResultado = unitOfWork.Repository<AtencionesResultado>().Modify(atencionesResultado);
                }
                atencionesResultado.IsNew = false;

                AdmisionesServiciosPrestados admisionesServiciosPrestados = unitOfWork.Repository<AdmisionesServiciosPrestados>().FindById(x => x.Id == admisionesServiciosPrestadosId, false);
                admisionesServiciosPrestados.LecturaRealizada = true;
                admisionesServiciosPrestados.UpdatedBy = userName;
                admisionesServiciosPrestados.LastUpdate = DateTime.Now;
                unitOfWork.Repository<AdmisionesServiciosPrestados>().Modify(admisionesServiciosPrestados);
                unitOfWork.CommitTransaction();

                return atencionesResultado;
            }
            catch
            {
                unitOfWork.RollbackTransaction();
                throw;
            }

        }

    }
}
