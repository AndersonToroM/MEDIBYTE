using Blazor.Infrastructure;
using Blazor.Infrastructure.Entities;
using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using Dominus.Backend.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.BusinessLogic
{
    public class EntregaResultadosNoLecturaBusinessLogic : GenericBusinessLogic<EntregaResultadosNoLectura>
    {
        public EntregaResultadosNoLecturaBusinessLogic(IUnitOfWork unitWork) : base(unitWork)
        {
        }

        public EntregaResultadosNoLecturaBusinessLogic(DataBaseSetting configuracionBD) : base(configuracionBD)
        {
        }

        public override EntregaResultadosNoLectura Add(EntregaResultadosNoLectura data)
        {
            var logicaData = new GenericBusinessLogic<EntregaResultadosNoLectura>(this.UnitOfWork.Settings);
            var logicaArchivo = new GenericBusinessLogic<Archivos>(logicaData.UnitOfWork);
            var logicaEntregaResultadosNoLecturaDetalles = new GenericBusinessLogic<EntregaResultadosNoLecturaDetalles>(logicaData.UnitOfWork);
            var logicaAdmisionesServiciosPrestados = new GenericBusinessLogic<AdmisionesServiciosPrestados>(logicaData.UnitOfWork);
            logicaData.BeginTransaction();
            try
            {
                if (data.ContanciaArchivos != null)
                {
                    data.ContanciaArchivosId = ManageArchivo(data.ContanciaArchivos, data.ContanciaArchivosId, logicaArchivo);
                    data.ContanciaArchivos.Id = data.ContanciaArchivosId.GetValueOrDefault(0);
                }

                data = logicaData.Add(data);

                foreach (var item in data.ListAdmisionesServiciosPrestadosId)
                {
                    EntregaResultadosNoLecturaDetalles EntregaResultadosNoLecturaDetalles = new EntregaResultadosNoLecturaDetalles();
                    EntregaResultadosNoLecturaDetalles.Id = 0;
                    EntregaResultadosNoLecturaDetalles.CreatedBy =data.CreatedBy;
                    EntregaResultadosNoLecturaDetalles.UpdatedBy =data.UpdatedBy;
                    EntregaResultadosNoLecturaDetalles.LastUpdate =data.LastUpdate;
                    EntregaResultadosNoLecturaDetalles.CreationDate =data.CreationDate;

                    EntregaResultadosNoLecturaDetalles.EntregaResultadosNoLecturaId = data.Id;
                    EntregaResultadosNoLecturaDetalles.AdmisionesServiciosPrestadosId = item;
                    logicaEntregaResultadosNoLecturaDetalles.Add(EntregaResultadosNoLecturaDetalles);

                    AdmisionesServiciosPrestados admisionesServiciosPrestados = logicaAdmisionesServiciosPrestados.FindById(x => x.Id == item, false);
                    admisionesServiciosPrestados.EntregaNoLectura = true;
                    logicaAdmisionesServiciosPrestados.Modify(admisionesServiciosPrestados);
                }

                
                logicaData.CommitTransaction();
                return data;
            }
            catch
            {
                logicaData.RollbackTransaction();
                throw;
            }
        }

        public override EntregaResultadosNoLectura Modify(EntregaResultadosNoLectura data)
        {
            var logicaData = new GenericBusinessLogic<EntregaResultadosNoLectura>(this.UnitOfWork.Settings);
            var logicaArchivo = new GenericBusinessLogic<Archivos>(logicaData.UnitOfWork);
            logicaData.BeginTransaction();
            try
            {
                if (data.ContanciaArchivos != null)
                {
                    data.ContanciaArchivosId = ManageArchivo(data.ContanciaArchivos, data.ContanciaArchivosId, logicaArchivo);
                    data.ContanciaArchivos.Id = data.ContanciaArchivosId.GetValueOrDefault(0);
                }

                data = logicaData.Modify(data);
                logicaData.CommitTransaction();
                return data;
            }
            catch
            {
                logicaData.RollbackTransaction();
                throw;
            }
        }

        public override EntregaResultadosNoLectura Remove(EntregaResultadosNoLectura data)
        {
            var unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            unitOfWork.BeginTransaction();
            try
            {
                var detalles = unitOfWork.Repository<EntregaResultadosNoLecturaDetalles>().FindAll(x => x.EntregaResultadosNoLecturaId == data.Id, false);
                unitOfWork.Repository<EntregaResultadosNoLecturaDetalles>().RemoveRange(detalles);

                data = unitOfWork.Repository<EntregaResultadosNoLectura>().Remove(data);
                EliminarArchivoDeMaestro(data.ContanciaArchivosId, unitOfWork);

                var existeOtrasLecturas = unitOfWork.Repository<EntregaResultadosNoLecturaDetalles>().Table
                    .Include(x => x.AdmisionesServiciosPrestados)
                    .Any(x => detalles.Select(j => j.AdmisionesServiciosPrestadosId).Contains(x.AdmisionesServiciosPrestadosId) && x.AdmisionesServiciosPrestados.EntregaNoLectura == true);

                if (!existeOtrasLecturas)
                {
                    var logicaAdmisionesServiciosPrestados = unitOfWork.Repository<AdmisionesServiciosPrestados>();
                    foreach (var item in detalles)
                    {
                        AdmisionesServiciosPrestados admisionesServiciosPrestados = logicaAdmisionesServiciosPrestados.FindById(x => x.Id == item.AdmisionesServiciosPrestadosId, false);
                        admisionesServiciosPrestados.EntregaNoLectura = false;
                        logicaAdmisionesServiciosPrestados.Modify(admisionesServiciosPrestados);
                    }
                }

                unitOfWork.CommitTransaction();
                return data;
            }
            catch
            {
                unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
