using Blazor.BusinessLogic;
using Blazor.Infrastructure.Entities;
using Blazor.WebApp.Models;
using Dominus.Frontend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Blazor.WebApp.Controllers
{

    [Authorize]
    public partial class HistoriasClinicasController
    {
        private HistoriasClinicas GetEntityData(long Id)
        {
            return Manager().GetBusinessLogic<HistoriasClinicas>().Tabla(true)
                .Include(x => x.HCTipos.Especialidades)

                .FirstOrDefault(x => x.Id == Id);
        }

        protected Estados GetEstado()
        {
            return Manager().GetBusinessLogic<Estados>().FindById(x => x.Tipo == "HISTORIA CLINICA" && x.Nombre == "ABIERTA", false);
        }

        [HttpGet]
        public IActionResult OpenHC(int atentionId, int hcTipoId, long CausaMotivoAtencionId, long finalidadconsultaId, long? programasId, long enfermedadesHuerfanasId, bool pertenecePrograma)
        {
            HistoriasClinicasModel model = new HistoriasClinicasModel();
            model.Entity = Manager().GetBusinessLogic<HistoriasClinicas>().FindById(x => x.AtencionesId == atentionId, false);
            Atenciones aten = Manager().GetBusinessLogic<Atenciones>().Tabla(true).Include(x => x.Admisiones.Pacientes).FirstOrDefault(x => x.Id == atentionId);
            if (model.Entity == null)
            {
                Estados est = GetEstado();
                model.Entity = new HistoriasClinicas();
                model.Entity.AtencionesId = atentionId;
                model.Entity.Atenciones = aten;
                model.Entity.EstadosId = est.Id;
                model.Entity.Consecutivo = $"{aten.Admisiones.Pacientes.NumeroIdentificacion}_{DateTime.Now.ToString("yyyyMMddHH24mmss")}";
                model.Entity.FechaApertura = DateTime.Now;
                model.Entity.PacientesId = aten.Admisiones.PacientesId;
                model.Entity.ProfesionalId = aten.EmpleadosId;
                model.Entity.HCTiposId = hcTipoId;
                model.Entity.LastUpdate = DateTime.Now;
                model.Entity.UpdatedBy = User.Identity.Name;
                model.Entity.CreationDate = DateTime.Now;
                model.Entity.CreatedBy = User.Identity.Name;
                model.Entity.EsControl = aten.Admisiones.EsControl;
                model.Entity.IsNew = true;
                model.Entity.Id = 0;
                model.Entity = Manager().GetBusinessLogic<HistoriasClinicas>().Add(model.Entity);
                var tiposPreguntas = Manager().GetBusinessLogic<HCTiposPreguntas>().FindAll(x => x.HCTiposId == model.Entity.HCTiposId, false).Select(x => x.HCPreguntasId).ToList();

                var respuestas = Manager().GetBusinessLogic<HCRespuestas>().FindAll(x => tiposPreguntas.Contains(x.HCPreguntaId), true).ToList();
                if (respuestas != null && respuestas.Count > 0)
                    foreach (var item in respuestas)
                    {
                        HistoriasClinicasRespuestas reps = new HistoriasClinicasRespuestas();
                        reps.HCRespuestasId = item.Id;
                        reps.HIstoriasClinicasId = model.Entity.Id;
                        reps.LastUpdate = DateTime.Now;
                        reps.UpdatedBy = User.Identity.Name;
                        reps.CreationDate = DateTime.Now;
                        reps.CreatedBy = User.Identity.Name;
                        Manager().GetBusinessLogic<HistoriasClinicasRespuestas>().Add(reps);
                    }

            }

            aten.CausaMotivoAtencionId = CausaMotivoAtencionId;
            aten.FinalidadConsultaId = finalidadconsultaId;
            aten.ProgramasId = programasId;
            aten.EnfermedadesHuerfanasId = enfermedadesHuerfanasId;
            aten.PertenecePrograma = pertenecePrograma;
            Manager().GetBusinessLogic<Atenciones>().Modify(aten);

            model.Entity = Manager().GetBusinessLogic<HistoriasClinicas>().Tabla(true).Include(x => x.HCTipos.Especialidades).FirstOrDefault(x => x.Id == model.Entity.Id);
            model.Preguntas = Manager().GetBusinessLogic<HCTiposPreguntas>().FindAll(x => x.HCTiposId == model.Entity.HCTiposId, true).Select(x => x.HCPreguntas).ToList();
            model.Entity.IsNew = false;
            model.EsMismoUsuario = true;
            return PartialView("Edit", model);
        }

        [HttpPost]
        public IActionResult CerrarHC(HistoriasClinicasModel model)
        {
            try
            {
                model.Entity = Manager().GetBusinessLogic<HistoriasClinicas>().Modify(model.Entity);
            }
            catch { }

            var totalDiagnosticosHC = Manager().GetBusinessLogic<HistoriasClinicasDiagnosticos>().Tabla(true).Any(x => x.HistoriasClinicasId == model.Entity.Id);
            if (!totalDiagnosticosHC)
            {
                ModelState.AddModelError("Entity.Id", "Para cerrar la historia clinica debe tener al menos un diagnóstico registrado.");
            }
            var diagnosticoPrincipal = Manager().GetBusinessLogic<HistoriasClinicasDiagnosticos>().Tabla(true).FirstOrDefault(x => x.HistoriasClinicasId == model.Entity.Id && x.Principal != false);
            if (diagnosticoPrincipal is null)
            {
                ModelState.AddModelError("Entity.Id", "Para cerrar la historia clinica se debe marcar un diagnóstico como principal.");
            }
            if (model.Entity.Peso <= 0)
            {
                ModelState.AddModelError("Entity.Peso", "Para cerrar la historia clinica debe registrar un peso mayor a cero (0).");
            }
            if (model.Entity.Altura <= 0)
            {
                ModelState.AddModelError("Entity.Altura", "Para cerrar la historia clinica debe registrar una altura mayor a cero (0).");
            }
            if (string.IsNullOrWhiteSpace(model.Entity.AntecedentesFamiliar))
            {
                ModelState.AddModelError("Entity.AntecedentesFamiliar", "Para cerrar la historia clinica debe registrar el antecedente familiar.");
            }
            if (string.IsNullOrWhiteSpace(model.Entity.AntecedentesPersonal))
            {
                ModelState.AddModelError("Entity.AntecedentesPersonal", "Para cerrar la historia clinica debe registrar el antecedente personal.");
            }
            if (string.IsNullOrWhiteSpace(model.Entity.MotivoConsulta))
            {
                ModelState.AddModelError("Entity.MotivoConsulta", "Para cerrar la historia clinica debe registrar el motivo de la consulta.");
            }
            if (string.IsNullOrWhiteSpace(model.Entity.EnfermedadActual))
            {
                ModelState.AddModelError("Entity.EnfermedadActual", "Para cerrar la historia clinica debe registrar la enfermedad actual.");
            }
            if (string.IsNullOrWhiteSpace(model.Entity.PlanManejo))
            {
                ModelState.AddModelError("Entity.PlanManejo", "Para cerrar la historia clinica debe registrar el plan de manejo.");
            }
            if (string.IsNullOrWhiteSpace(model.Entity.Hallazgos))
            {
                ModelState.AddModelError("Entity.Hallazgos", "Para cerrar la historia clinica debe registrar el hallazgos.");
            }
            if (!model.Entity.PresionDiastolica.HasValue || model.Entity.PresionDiastolica.Value == 0)
            {
                ModelState.AddModelError("Entity.PresionDiastolica", "Para cerrar la historia clinica debe registrar la presión diastolica.");
            }
            if (!model.Entity.PresionSistolica.HasValue || model.Entity.PresionSistolica.Value == 0)
            {
                ModelState.AddModelError("Entity.PresionSistolica", "Para cerrar la historia clinica debe registrar la presión sistolica.");
            }
            if (!model.Entity.FrecuenciaCardiaca.HasValue || model.Entity.FrecuenciaCardiaca.Value == 0)
            {
                ModelState.AddModelError("Entity.FrecuenciaCardiaca", "Para cerrar la historia clinica debe registrar la frecuencia cardiaca.");
            }
            if (!model.Entity.FrecuenciaRespiratoria.HasValue || model.Entity.FrecuenciaRespiratoria.Value == 0)
            {
                ModelState.AddModelError("Entity.FrecuenciaRespiratoria", "Para cerrar la historia clinica debe registrar la frecuencia respiratoria.");
            }
            if (!model.Entity.Temperatura.HasValue || model.Entity.Temperatura.Value == 0)
            {
                ModelState.AddModelError("Entity.Temperatura", "Para cerrar la historia clinica debe registrar la temperatura.");
            }
            if (model.Entity.DominanciaEstadosId == null)
            {
                ModelState.AddModelError("Entity.DominanciaEstadosId", "Para cerrar la historia clinica debe registrar la dominancia.");
            }
            if (string.IsNullOrWhiteSpace(model.Entity.Analisis))
            {
                ModelState.AddModelError("Entity.Analisis", "Para cerrar la historia clinica debe registrar el análisis.");
            }

            model.Entity = Manager().GetBusinessLogic<HistoriasClinicas>().Tabla(true)
                .Include(x => x.HCTipos.Especialidades)
                .Include(x => x.Atenciones.Admisiones)
                .FirstOrDefault(x => x.Id == model.Entity.Id);
            model.Preguntas = Manager().GetBusinessLogic<HCTiposPreguntas>().FindAll(x => x.HCTiposId == model.Entity.HCTiposId, true).Select(x => x.HCPreguntas).ToList();
            model.Entity.IsNew = false;
            model.EsMismoUsuario = true;

            if (model.Entity.Atenciones.EstadosId == 10077)
            {
                ModelState.AddModelError("Entity", "No es posible cerrar esta historia clínica porque la atención se encuentra ANULADA. Contacte con el servicio de soporte");
            }

            var keyPacientes = ModelState.Where(x => x.Key.StartsWith("Entity.Atenciones")).Select(x => x.Key).ToList();
            foreach (var key in keyPacientes)
            {
                ModelState.Remove(key);
            }

            if (ModelState.IsValid)
            {
                var estadoIncial = model.Entity.EstadosId;
                try
                {
                    model.Entity.LastUpdate = DateTime.Now;
                    model.Entity.UpdatedBy = User.Identity.Name;
                    model.Entity.EstadosId = 19;
                    model.Entity.FechaCierre = DateTime.Now;
                    model.Entity = Manager().GetBusinessLogic<HistoriasClinicas>().Modify(model.Entity);

                    var atencion = model.Entity.Atenciones;
                    atencion.DiagnosticosPrincipalHCId = diagnosticoPrincipal.DiagnosticosId;
                    atencion.LastUpdate = DateTime.Now;
                    atencion.UpdatedBy = User.Identity.Name;
                    Manager().GetBusinessLogic<Atenciones>().Modify(atencion);
                }
                catch (Exception e)
                {
                    model.Entity.EstadosId = estadoIncial;
                    ModelState.AddModelError("Entity.Id", e.GetFullErrorMessage());
                    return PartialView("Edit", model);
                }

                Manager().AtencionesBusinessLogic().AddAtencion(model.Entity.Atenciones);
                ViewBag.Accion = "CerrarHC";
            }

            return PartialView("Edit", model);
        }
    }
}
