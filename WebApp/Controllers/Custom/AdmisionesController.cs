using Blazor.BusinessLogic;
using Blazor.Infrastructure.Entities;
using Blazor.Reports.EntregaAdmisiones;
using Blazor.Reports.Facturas;
using Blazor.WebApp.Models;
using Dominus.Backend.Application;
using Dominus.Frontend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WidgetGallery;

namespace Blazor.WebApp.Controllers
{

    [Authorize]
    public partial class AdmisionesController
    {
        private AdmisionesModel EditModel(long Id)
        {
            AdmisionesModel model = new AdmisionesModel();
            model.Entity = Manager().GetBusinessLogic<Admisiones>().FindById(x => x.Id == Id, true);
            model.EntidadesConvenio = GetEntidadesConvenios(model.Entity.ConveniosId);
            model.EmpleadosId = model.Entity.ProgramacionCitas.EmpleadosId;
            model.ConsultoriosId = model.Entity.ProgramacionCitas.ConsultoriosId;
            model.Entity.IsNew = false;

            var serviciosDetalle = Manager().GetBusinessLogic<AdmisionesServiciosPrestados>().FindById(x => x.AdmisionesId == Id && x.Facturado, false);
            if (serviciosDetalle != null)
                model.TieneServiciosFacturadosAEntidad = true;

            model.TieneServiciosFacturadosAEntidad = false;

            if (model.Entity.ExoneracionArchivo == null)
                model.Entity.ExoneracionArchivo = new Archivos();
            else
                model.Entity.ExoneracionArchivo.StringToBase64 = DApp.Util.ArrayBytesToString(model.Entity.ExoneracionArchivo.Archivo);

            var atention = Manager().GetBusinessLogic<Atenciones>().FindById(x => x.AdmisionesId == Id && x.EstadosId == 10077, false);
            if (atention != null)
            {
                model.MotivoAnulacion = atention.DetalleAnulacion;
            }

            return model;
        }
        private AdmisionesModel EditModel(AdmisionesModel model)
        {
            ViewBag.Accion = "Save";
            var OnState = model.Entity.IsNew;
            var citaSeleccionada = Manager().GetBusinessLogic<ProgramacionCitas>().FindById(x => x.Id == model.Entity.ProgramacionCitasId, true);
            var citaAdmitida = Manager().GetBusinessLogic<Admisiones>()
                .FindById(
                    x => x.ProgramacionCitasId == citaSeleccionada.Id &&
                    x.EstadosId != 72 &&
                    x.EstadosId != 10079
                , true);

            if (citaAdmitida != null && OnState)
            {
                ModelState.AddModelError("Entity.Id", $"La cita No. {citaSeleccionada.Consecutivo} ya cuenta con la Admisión No. {citaAdmitida.Consecutivo} en proceso. Por favor verifique en el listado de admisiones.");
            }

            if (!string.IsNullOrWhiteSpace(model.Entity.NroAutorizacion))
            {
                List<long> estadosAdmision = new List<long> { 72, 10079 };

                var citaSeleccion = Manager().GetBusinessLogic<ProgramacionCitas>().FindById(x => x.Id == model.Entity.ProgramacionCitasId, true);
                var admisionautorizacion = Manager().GetBusinessLogic<Admisiones>()
                    .FindById(
                        x => x.NroAutorizacion == model.Entity.NroAutorizacion &&
                        !estadosAdmision.Contains(x.EstadosId) &&
                        x.Id != model.Entity.Id &&
                        x.ProgramacionCitas.ServiciosId == citaSeleccion.ServiciosId &&
                        x.PacientesId == citaSeleccion.PacientesId
                    , true);
                if (admisionautorizacion != null)
                {
                    ModelState.AddModelError("Entity.Id", $"El número de autorización {model.Entity.NroAutorizacion} ha sido registrado en la admisión consecutivo {admisionautorizacion.Consecutivo}, para el servicio {citaSeleccion.Servicios.Nombre} y el paciente {citaSeleccion.Pacientes.NombreCompleto}.");
                }
            }

            if (model.Entity.FechaAutorizacion > DateTime.Now)
            {
                ModelState.AddModelError("Entity.Id", "La fecha de autorización no puede ser mayor que la fecha actual.");
            }

            if (!OnState)
            {
                var admisionDB = Manager().GetBusinessLogic<Admisiones>().FindById(x => x.Id == model.Entity.Id, true);
                var estadosId = new List<long> { 62, 72, 10068, 10079 };
                if (estadosId.Contains(admisionDB.EstadosId))
                {
                    ModelState.AddModelError("Entity.Id", $"La admisiÃ³n no puede ser modificada ya que se encuentra en estado {admisionDB.Estados.Nombre}");
                }
            }

            ModelState.Remove("CategoriasIngresosDetalles.CategoriasIngresosId");

            var llaves = ModelState.Where(x => x.Key.Contains("Entity.ProgramacionCitas")).Select(x => x.Key).ToList();
            foreach (var key in llaves)
            {
                ModelState.Remove(key);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.Entity.LastUpdate = DateTime.Now;
                    model.Entity.UpdatedBy = User.Identity.Name;

                    if (model.Entity.IsNew)
                    {
                        if (model.Entity.PorcDescAutorizado == 0)
                            model.Entity.EstadosId = 10066;
                        if (model.Entity.PorcDescAutorizado > 0)
                        {
                            Manager().AdmisionesBusinessLogic().EnvioAutorizacionAdmisiones(model.Entity.PacientesId);
                            model.Entity.EstadosId = 61;
                        }

                        model.Entity.CreationDate = DateTime.Now;
                        model.Entity.CreatedBy = User.Identity.Name;
                        model.Entity = Manager().GetBusinessLogic<Admisiones>().Add(model.Entity);
                        model.Entity.IsNew = false;
                    }
                    else
                    {
                        var admision = Manager().GetBusinessLogic<Admisiones>().FindById(x => x.Id == model.Entity.Id, true);
                        List<long> estados = new List<long> { 62, 72 };
                        if (estados.Contains(admision.EstadosId))
                            model.Entity.EstadosId = admision.EstadosId;
                        model.Entity = Manager().GetBusinessLogic<Admisiones>().Modify(model.Entity);
                    }

                    return EditModel(model.Entity.Id);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Entity.Id", e.GetFullErrorMessage());
                }
            }
            else
            {
                ModelState.AddModelError("Entity.Id", $"Error en vista, diferencia con base de datos. | " + ModelState.GetFullErrorMessage());
            }
            return model;
        }

        [HttpPost]
        public String GetTipoId(int id)
        {
            return Manager().GetBusinessLogic<TiposIdentificacion>().FindById(x => x.Id == id, false).Codigo;
        }

        [HttpPost]
        public long? GetConvenio(long id)
        {
            return Manager().GetBusinessLogic<ProgramacionCitas>().FindById(x => x.Id == id, false).ConveniosId;
        }
        [HttpPost]
        public long? GetEntidadesConvenios(long? id)
        {
            if (id == null)
                return null;
            else
                return Manager().GetBusinessLogic<Convenios>().FindById(x => x.Id == id, false).EntidadesId; ;
        }

        public long GetCitaCercana(long IdPaciente)
        {
            var dateActual = DateTime.Now;
            var citas = Manager().GetBusinessLogic<ProgramacionCitas>().FindAll(x => x.FechaInicio <= dateActual && x.PacientesId == IdPaciente, false).OrderByDescending(x => x.FechaInicio).FirstOrDefault();
            if (citas != null)
                return citas.Id;
            else
                return 0;
        }

        public AdmisionesModel GetValorCategoriaIngreso(long PacienteId, long CitasId)
        {
            AdmisionesModel admisiones = new AdmisionesModel();
            Pacientes paciente = Manager().GetBusinessLogic<Pacientes>().FindById(x => x.Id == PacienteId, true);
            if (paciente != null)
            {
                admisiones.CategoriasIngresosDetalles = Manager().GetBusinessLogic<CategoriasIngresosDetalles>().FindById(x => x.CategoriasIngresosId == paciente.CategoriasIngresosId && x.FechaInicial <= DateTime.Now && x.FechaFinal >= DateTime.Now, false);
                admisiones.valorServicio = ObtenerValorServicio(paciente, admisiones.CategoriasIngresosDetalles, CitasId);
            }
            return admisiones;
        }

        private decimal ObtenerValorServicio(Pacientes paciente, CategoriasIngresosDetalles categoriasIngresosDetalles, long CitasId)
        {
            try
            {
                if (paciente == null || categoriasIngresosDetalles == null || CitasId <= 0)
                    return 0;

                if (paciente.TiposRegimenes != null)
                {
                    if (paciente.TiposAfiliados != null)
                    {
                        if (paciente.TiposRegimenes.Codigo.Equals("CON") && paciente.TiposAfiliados.Codigo.Equals("COT"))
                            return 0;
                        else if (paciente.TiposRegimenes.Codigo.Equals("SUB") && paciente.CategoriasIngresos != null && paciente.CategoriasIngresos.Codigo.Equals("1"))
                            return 0;
                        else
                            return Manager().AdmisionesBusinessLogic().ValorCopago(categoriasIngresosDetalles.PorcentajeCopago, categoriasIngresosDetalles.CopagoMaximoEvento, CitasId);
                    }
                }
            }
            catch (Exception) { }

            return 0;
        }

        [HttpPost]
        public IActionResult FacturaIndividual(AdmisionesModel model)
        {
            try
            {
                ViewBag.FacturaConsecutivo = Manager().FacturasBusinessLogic().FacturaIndividual(model.Entity.Id, this.ActualEmpresaId(), this.ActualUsuarioId());
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Entity.Id", e.GetFullErrorMessage());
                return PartialView("Edit", model);
            }

            model = EditModel(model.Entity.Id);
            ViewBag.Accion = "Facturado";

            return PartialView("Edit", model);
        }

        [HttpGet]
        public IActionResult ImprimirFacturaIndividual(long Id)
        {
            try
            {
                var report = Manager().Report<FacturasParticularReporte>(Id, User.Identity.Name);
                return PartialView("_ViewerReport", report);

            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public IActionResult AnularAdmision(long admisionId, string detalleAnulacion)
        {
            try
            {
                Manager().AdmisionesBusinessLogic().AnularAtencion(admisionId, detalleAnulacion);
                return New();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }

        }

        [HttpPost]
        public IActionResult ImprimirReporteEntregaProduccion(long sedeId, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                if (sedeId <= 0 || fechaDesde.Year < 1900 || fechaHasta.Year < 1900)
                    throw new Exception("Los parámetros Fecha Desde, Fecha Hasta y Sede no fueron enviados correctamente al servidor.");

                var parametros = new Dictionary<string, object>
                {
                    { "p_FechaDesde", new DateTime(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day, 0, 0, 0)},
                    { "p_FechaHasta", new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day, 23, 59, 59)},
                    {"p_SedeId", sedeId }
                };
                var report = Manager().Report<EntregaAdmisionesReporte>(User.Identity.Name, parametros);
                return PartialView("_ViewerReport", report);

            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public IActionResult XLSXReporteEntregaProduccion(long sedeId, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                if (sedeId <= 0 || fechaDesde.Year < 1900 || fechaHasta.Year < 1900)
                    throw new Exception("Los parámetros Fecha Desde, Fecha Hasta y Sede no fueron enviados correctamente al servidor.");

                fechaDesde = new DateTime(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day, 0, 0, 0);
                fechaHasta = new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day, 23, 59, 59);
                byte[] book = Manager().AdmisionesBusinessLogic().ExcelEntregaAdmisiones(sedeId, fechaDesde, fechaHasta);
                return File(book, "application/octet-stream", $"Admisiones_{sedeId}_{fechaDesde.ToString("ddMMyyyy")}_{fechaHasta.ToString("ddMMyyyy")}.xlsx");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

    }
}
