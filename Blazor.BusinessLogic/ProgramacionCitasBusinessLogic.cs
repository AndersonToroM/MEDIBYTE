using Blazor.Infrastructure;
using Blazor.Infrastructure.Entities;
using Blazor.Infrastructure.Models;
using DevExpress.Spreadsheet;
using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using Dominus.Backend.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Blazor.BusinessLogic
{
    public class ProgramacionCitasBusinessLogic : GenericBusinessLogic<ProgramacionCitas>
    {
        public ProgramacionCitasBusinessLogic(IUnitOfWork unitWork) : base(unitWork)
        {
        }

        public ProgramacionCitasBusinessLogic(DataBaseSetting configuracionBD) : base(configuracionBD)
        {
        }

        public void EnviarCorreoCancelacionCita(long citaId, string server)
        {
            var cita = new GenericBusinessLogic<ProgramacionCitas>(this.UnitOfWork).Tabla()
                .Include(x => x.Empresas)
                .Include(x => x.Empresas.LogoArchivos)
                .Include(x => x.Pacientes)
                .Include(x => x.Empleados)
                .Include(x => x.Servicios)
                .Include(x => x.Sedes)
                .FirstOrDefault(x => x.Id == citaId);

            EmailModelConfig envioEmailConfig = new EmailModelConfig();
            envioEmailConfig.Origen = "POR DEFECTO";
            envioEmailConfig.MetodoUso = "Cancelacion de citas";
            envioEmailConfig.Asunto = $"Cancelación de cita en {cita.Empresas.RazonSocial}";
            envioEmailConfig.Template = "EmailCancelacionCita";
            envioEmailConfig.Destinatarios.Add(cita.Pacientes.CorreoElectronico);
            envioEmailConfig.Datos = new Dictionary<string, string>
                {
                    {"server",$"{server}" },
                    {"nombreEmpresa",$"{cita.Empresas.RazonSocial}" },
                    {"paginaWebEmpresa",$"{(string.IsNullOrWhiteSpace(cita.Empresas.PaginaWeb) ? "#":cita.Empresas.PaginaWeb)}" },
                    {"paciente",$"{cita.Pacientes.NombreCompleto}" },
                    {"movito",$"{cita.MotivoCancelacion}" },
                    {"medico",$"{(cita.Empleados != null ? cita.Empleados.NombreCompleto : "Técnico Especializado")}" },
                    {"servicio",$"{cita.Servicios.Nombre}" },
                    {"sede",$"{cita.Sedes.Nombre}" },
                    {"direccion",$"{cita.Sedes.Direccion}" },
                    {"telefono",$"{cita.Empresas.Telefono} - {cita.Empresas.Celular}" },
                    {"fecha",$"{cita.FechaInicio:D}" },
                    {"hora",$"{cita.FechaInicio:t}" },
                    {"consecutivo",$"{cita.Consecutivo}" }
                };
            new ConfiguracionEnvioEmailBusinessLogic(this.UnitOfWork).EnviarEmail(envioEmailConfig);
        }

        public void EnviarCorreoCitaProgramada(long citaId, string server)
        {
            var cita = new GenericBusinessLogic<ProgramacionCitas>(this.UnitOfWork).Tabla()
                .Include(x => x.Empresas)
                .Include(x => x.Empresas.LogoArchivos)
                .Include(x => x.Pacientes)
                .Include(x => x.Empleados)
                .Include(x => x.Servicios)
                .Include(x => x.Sedes)
                .FirstOrDefault(x => x.Id == citaId);

            if (!DApp.Util.EsEmailValido(cita.Pacientes.CorreoElectronico))
                throw new Exception("El paciente no tiene un correo electronico valido.");

            EmailModelConfig envioEmailConfig = new EmailModelConfig();
            envioEmailConfig.Origen = "POR DEFECTO";
            envioEmailConfig.MetodoUso = "Programacion de citas";
            envioEmailConfig.Asunto = $"Programación de nueva cita en {cita.Empresas.RazonSocial}";
            envioEmailConfig.Template = "EmailProgramacionCita";
            envioEmailConfig.Destinatarios.Add(cita.Pacientes.CorreoElectronico);
            envioEmailConfig.Datos = new Dictionary<string, string>
                {
                    {"server",$"{server}" },
                    {"nombreEmpresa",$"{cita.Empresas.RazonSocial}" },
                    {"paginaWebEmpresa",$"{(string.IsNullOrWhiteSpace(cita.Empresas.PaginaWeb) ? "#":cita.Empresas.PaginaWeb)}" },
                    {"paciente",$"{cita.Pacientes.NombreCompleto}" },
                    {"medico",$"{(cita.Empleados != null ? cita.Empleados.NombreCompleto : "Técnico Especializado")}" },
                    {"servicio",$"{cita.Servicios.Nombre}" },
                    {"sede",$"{cita.Sedes.Nombre}" },
                    {"direccion",$"{cita.Sedes.Direccion}" },
                    {"telefono",$"{cita.Empresas.Telefono} - {cita.Empresas.Celular}" },
                    {"fecha",$"{cita.FechaInicio:D}" },
                    {"hora",$"{cita.FechaInicio:t}" },
                    {"consecutivo",$"{cita.Consecutivo}" },
                    {"preparacion",$"{cita.Servicios.Preparacion}" }
                };
            new ConfiguracionEnvioEmailBusinessLogic(this.UnitOfWork).EnviarEmail(envioEmailConfig);
        }

        public byte[] DescargarXLSX0256(long sedeId, DateTime fechaDesde, DateTime fechaHasta)
        {
            Workbook workbook = new Workbook();
            workbook.CreateNewDocument();
            Worksheet worksheet = workbook.Worksheets.ActiveWorksheet as Worksheet;
            worksheet.Name = "0256";
            List<ProgramacionCitas> data = new GenericBusinessLogic<ProgramacionCitas>(this.UnitOfWork).Tabla()
                .Include(x => x.Estados)
                .Include(x => x.Pacientes)
                .Include(x => x.Pacientes.Generos)
                .Include(x => x.Pacientes.TiposIdentificacion)
                .Include(x => x.Pacientes.GenerosIdentidad)
                .Include(x => x.Pacientes.TiposRegimenes)
                .Include(x => x.Entidades)
                .Include(x => x.Empresas.Ciudades)
                .Include(x => x.Empleados)
                .Include(x => x.Servicios)
                .Include(x => x.Admisiones).ThenInclude(x => x.Diagnosticos)
                .Include(x => x.Admisiones).ThenInclude(x => x.TiposUsuarios)
                .Include(x => x.Admisiones).ThenInclude(x => x.Atenciones.DiagnosticosPrincipalHC)
                .Include(x => x.Admisiones).ThenInclude(x => x.Atenciones.EnfermedadesHuerfanas)
                .Include(x => x.Admisiones).ThenInclude(x => x.Atenciones.Programas)
                .Include(x => x.Admisiones).ThenInclude(x => x.Atenciones).ThenInclude(x => x.HistoriasClinicas)
                .Include(x => x.Admisiones).ThenInclude(x => x.Atenciones).ThenInclude(x => x.Empleados)
                .Where(x => x.CreationDate.Date >= fechaDesde && x.CreationDate.Date <= fechaHasta && x.SedesId == sedeId)
                //.Where(x => x.Id == 126923)
                .OrderBy(x => x.CreationDate).ToList();

            //Titulos
            int column = 0;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.Entidad")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.NIT")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.REPS")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.CodigoMunicipio")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.DocProfesional")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.Profesional")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.PacienteNombre1")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.PacienteNombre2")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.PacienteApellido1")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.PacienteApellido2")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.PacienteNombreCompleto")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.CodigoDocumento")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.DescDocumento")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.NumeroDocumento")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.IdentidadGenero")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.Sexo")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.Telefono")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.Edad")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.Regimen")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.Esttado0256")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.CUPS")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.Servicio")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.FechaSolicitud")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.FechaDeseada")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.FechaCita")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.DuracionCita")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.DuracionAtencion")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.TipoConsulta")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.PertenecePrograma")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.NombrePrograma")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.CodigoAutorizacion")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.CodCIE10")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.DescCIE10")); column++;
            worksheet.Rows[0][column].SetValue(DApp.GetResource("XLSX0256.EnfermedadHuerfana")); column++;
#if DEBUG
            worksheet.Rows[0][column].SetValue(DApp.GetResource("Id CITA")); column++;
#endif

            var row = 0;
            foreach (var cita in data)
            {
                column = 0;
                row++;
                worksheet.Rows[row][column].SetValue(cita.Entidades?.Nombre); column++; //Entidad
                worksheet.Rows[row][column].SetValue(cita.Empresas?.NumeroIdentificacion); column++; //NIT
                worksheet.Rows[row][column].SetValue(cita.Empresas?.CodigoReps); column++; // REPS
                worksheet.Rows[row][column].SetValue(cita.Empresas?.Ciudades?.Codigo); column++; //CodigoMunicipio

                var admision = cita.Admisiones.FirstOrDefault(x => x.EstadosId != 72);

                var docProfesional = string.Empty;
                var profesional = string.Empty;
                if (cita.Servicios.RequiereProfesional == true)
                {
                    docProfesional = cita?.Empleados?.NumeroIdentificacion;
                    profesional = cita?.Empleados?.NombreCompleto;
                }
                else
                {
                    docProfesional = admision?.Atenciones?.Empleados?.NumeroIdentificacion;
                    profesional = admision?.Atenciones?.Empleados?.NombreCompleto;
                }

                worksheet.Rows[row][column].SetValue(docProfesional); column++; //DocProfesional
                worksheet.Rows[row][column].SetValue(profesional); column++; //Profesional
                worksheet.Rows[row][column].SetValue(cita.Pacientes?.PrimerNombre); column++; //PacienteNombre1
                worksheet.Rows[row][column].SetValue(cita.Pacientes?.SegundoNombre); column++; //PacienteNombre2
                worksheet.Rows[row][column].SetValue(cita.Pacientes?.PrimerApellido); column++; //PacienteApellido1
                worksheet.Rows[row][column].SetValue(cita.Pacientes?.SegundoApellido); column++; //PacienteApellido2
                worksheet.Rows[row][column].SetValue(cita.Pacientes?.NombreCompleto); column++; //PacienteNombreCompleto
                worksheet.Rows[row][column].SetValue(cita.Pacientes?.TiposIdentificacion.Codigo); column++; //CodigoDocumento
                worksheet.Rows[row][column].SetValue(cita.Pacientes?.TiposIdentificacion.Nombre); column++; //DescDocumento
                worksheet.Rows[row][column].SetValue(cita.Pacientes?.NumeroIdentificacion); column++; //NumeroDocumento
                worksheet.Rows[row][column].SetValue(cita.Pacientes?.GenerosIdentidad.Nombre); column++; //IdentidadGenero
                worksheet.Rows[row][column].SetValue(cita.Pacientes?.Generos.Nombre); column++; //Sexo
                worksheet.Rows[row][column].SetValue(cita.Pacientes?.Telefono); column++; //Telefono

                var edad = DApp.Util.CalcularEdad(cita.Pacientes?.FechaNacimiento, admision?.Atenciones?.FechaAtencion);
                worksheet.Rows[row][column].SetValue(edad); column++; //Edad

                var regimen = admision?.TiposUsuarios?.Codigo;
                if (admision == null || regimen == null)
                {
                    regimen = cita.Pacientes?.TiposRegimenes?.CodigoIndicadores;
                    if (regimen == null)
                    {
                        regimen = "S/R";
                    }
                }

                worksheet.Rows[row][column].SetValue(regimen); column++; //Regimen

                var estado = string.Empty;
                var fechaInicio = new DateTime(cita.FechaInicio.Year, cita.FechaInicio.Month, 1);
                var fechaCreacion = new DateTime(cita.CreationDate.Year, cita.CreationDate.Month, 1);
                if (cita.EstadosId == 3)
                {
                    if (admision == null && (fechaInicio > fechaCreacion
                         || (cita.FechaInicio.Year > cita.CreationDate.Year)))
                    {
                        estado = "ASIGNADA";
                    }
                    else
                    {
                        estado = "INCUMPLIDA";
                    }
                }
                else if (cita.EstadosId == 6 && (fechaInicio == fechaCreacion))
                {
                    estado = "CUMPLIDA";
                }
                else if (cita.EstadosId == 6 && (fechaInicio > fechaCreacion))
                {
                    estado = "ASIGNADA";
                }
                else if (cita.EstadosId == 8 && fechaInicio == fechaCreacion)
                {
                    estado = "CANCELADA";
                }
                else if (cita.EstadosId == 8 || cita.EstadosId == 4 || cita.EstadosId == 5 || cita.EstadosId == 10078
                   && (fechaInicio > fechaCreacion))
                {
                    estado = "ASIGNADA";
                }
                else
                {
                    estado = cita.Estados.Nombre;
                }

                worksheet.Rows[row][column].SetValue(estado); column++; //Estado0256
                worksheet.Rows[row][column].SetValue(cita.Servicios?.Codigo); column++; //CUPS
                worksheet.Rows[row][column].SetValue(cita.Servicios?.Nombre); column++; //Servicio
                worksheet.Rows[row][column].SetValue($"{cita.CreationDate:dd/MM/yyyy}"); column++; //FechaSolicitud
                worksheet.Rows[row][column].SetValue($"{cita.FechaDeseada:dd/MM/yyyy}"); column++; //FechaDeseada
                worksheet.Rows[row][column].SetValue($"{cita.FechaInicio:dd/MM/yyyy}"); column++; //FechaCita

                var duracionServicio = cita.EstadosIdTipoDuracion == 10080 ? cita.Duracion * 60 : cita.Duracion;
                worksheet.Rows[row][column].SetValue(duracionServicio); column++; //DuracionCita

                var duracionAtencion = 0;
                if (admision != null && admision.Atenciones != null && admision.Atenciones.HistoriasClinicas != null &&
                    admision.Atenciones.HistoriasClinicas.FechaCierre.HasValue && admision.Atenciones.FechaFinAtencion.HasValue)
                {
                    duracionAtencion = (int)(admision.Atenciones.HistoriasClinicas.FechaCierre.Value - admision.Atenciones.FechaAtencion).TotalMinutes;
                }
                else if (admision != null && admision.Atenciones != null && admision.Atenciones.FechaFinAtencion.HasValue)
                {
                    duracionAtencion = (int)(admision.Atenciones.FechaFinAtencion.Value - admision.Atenciones.FechaAtencion).TotalMinutes;
                }
                worksheet.Rows[row][column].SetValue(duracionAtencion); column++; //DuracionAtencion

                var tipoConsulta = "Primera Vez";
                if (admision != null && admision.EsControl)
                {
                    tipoConsulta = "Control";
                }
                worksheet.Rows[row][column].SetValue(tipoConsulta); column++; //TipoConsulta

                var pertenecePrograma = "No";
                var nombrePrograma = string.Empty;
                if (admision != null && admision.Atenciones != null && admision.Atenciones.ProgramasId.HasValue)
                {
                    pertenecePrograma = "Si";
                    nombrePrograma = admision?.Atenciones?.Programas?.Nombre;
                }

                worksheet.Rows[row][column].SetValue(pertenecePrograma); column++; //PertenecePrograma
                worksheet.Rows[row][column].SetValue(nombrePrograma); column++; //NombrePrograma
                worksheet.Rows[row][column].SetValue(admision?.NroAutorizacion); column++; //CodigoAutorizacion

                var codCIE10 = string.Empty;
                var descCIE10 = string.Empty;
                if (cita.Servicios.TiposServiciosId == 1)
                {
                    codCIE10 = admision?.Atenciones?.DiagnosticosPrincipalHC?.Codigo;
                    descCIE10 = admision?.Atenciones?.DiagnosticosPrincipalHC?.Descripcion;
                }
                else
                {
                    codCIE10 = admision?.Diagnosticos?.Codigo;
                    descCIE10 = admision?.Diagnosticos?.Descripcion;
                }
                worksheet.Rows[row][column].SetValue(codCIE10); column++; //CodCIE10
                worksheet.Rows[row][column].SetValue(descCIE10); column++; //DescCIE10

                var enfermedadHuerfana = string.Empty;
                if (admision != null && admision.Atenciones != null && admision.Atenciones.EnfermedadesHuerfanasId.HasValue)
                {
                    enfermedadHuerfana = admision?.Atenciones?.EnfermedadesHuerfanas?.NombreV4;
                }
                worksheet.Rows[row][column].SetValue(enfermedadHuerfana); column++; //Enfermedad Huérfana
#if DEBUG
                worksheet.Rows[row][column].SetValue(cita.Id); column++; //Id CITA
#endif

            }

            worksheet.Columns.AutoFit(0, column);

            byte[] book = workbook.SaveDocument(DocumentFormat.Xlsx);
            workbook.Dispose();
            return book;
        }

        public bool VerificarAgendaDisponiblePorServicio(long servicioId)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            int result = (from programacionDisponible in unitOfWork.Repository<ProgramacionDisponible>().Table.Where(x => x.ServiciosId == servicioId)
                          join programacionAgenda in unitOfWork.Repository<ProgramacionAgenda>().Table
                             on programacionDisponible.ProgramacionAgendaId equals programacionAgenda.Id
                          where (programacionAgenda.FechaInicio.Date >= DateTime.Now.Date
                          || (DateTime.Now.Date >= programacionAgenda.FechaInicio.Date && DateTime.Now.Date <= programacionAgenda.FechaFinal.Date))
                          select programacionAgenda.Id
            ).Count();

            return (result > 0);
        }

        public SchedulerModel VerDisponibilidadProfesional(long profesionalId)
        {
            SchedulerModel schedulerModel = new SchedulerModel();

            if (profesionalId == 0)
            {
                schedulerModel.Habilitado = false;
                return schedulerModel;
            }

            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            List<ProgramacionAgenda> result = new List<ProgramacionAgenda>(); // Este dato es para sacar los parametros desde la programacion de la agenda

            List<long> estados = new List<long> { 3, 4, 5, 6 };
            result = (from programacionDisponible in unitOfWork.Repository<ProgramacionDisponible>().GetTable(true).Where(x => x.EmpleadosId == profesionalId)
                      join programacionAgenda in unitOfWork.Repository<ProgramacionAgenda>().Table
                         on programacionDisponible.ProgramacionAgendaId equals programacionAgenda.Id
                      where (programacionAgenda.FechaInicio.Date >= DateTime.Now.Date
                      || (DateTime.Now.Date >= programacionAgenda.FechaInicio.Date && DateTime.Now.Date <= programacionAgenda.FechaFinal.Date))
                      select programacionAgenda).ToList();
            schedulerModel.Data = (unitOfWork.Repository<ProgramacionCitas>().GetTable(true).Where(x => x.FechaInicio.Date >= DateTime.Now.AddDays(-1).Date && x.EmpleadosId == profesionalId && estados.Contains(x.EstadosId))).AsEnumerable<ProgramacionCitas>();

            if (result != null && result.Count > 0)
            {
                var fechaInicio = result.Min(x => x.FechaInicio);

                schedulerModel.FechaInicial = DateTime.Now;
                schedulerModel.FechaMinima = DateTime.Now;
                schedulerModel.FechaMaxima = result.Max(x => x.FechaFinal);

                int horaMinima = result.Min(x => x.FechaInicio.Hour) < result.Min(x => x.FechaFinal.Hour) ? result.Min(x => x.FechaInicio.Hour) : result.Min(x => x.FechaFinal.Hour);
                int horaMaxima = result.Max(x => x.FechaInicio.Hour) > result.Max(x => x.FechaFinal.Hour) ? result.Max(x => x.FechaInicio.Hour) : result.Max(x => x.FechaFinal.Hour);
                int minMaxima = result.Max(x => x.FechaInicio.Minute) > result.Max(x => x.FechaFinal.Minute) ? result.Max(x => x.FechaInicio.Minute) : result.Max(x => x.FechaFinal.Minute);

                schedulerModel.HoraMinima = horaMinima;
                schedulerModel.HoraMaxima = horaMaxima + (double)minMaxima / 100;

                schedulerModel.IntervaloCelda = 10;
            }
            else
            {
                schedulerModel.Habilitado = false;
            }

            schedulerModel.FechaInicial = DateTime.Now;

            return schedulerModel;
        }

        public SchedulerModel ObtenerSchedulerAgendaDisponible(long servicioId, long consultorioId, long? empleadoId, long estadosIdTipoDuracion, long duracion, DateTime? fechaScheduler)
        {
            SchedulerModel schedulerModel = new SchedulerModel();

            if (servicioId == 0 || consultorioId == 0)
            {
                schedulerModel.Habilitado = false;
                return schedulerModel;
            }

            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            List<ProgramacionAgenda> result = new List<ProgramacionAgenda>(); // Este dato es para sacar los parametros desde la programacion de la agenda
            var servicio = unitOfWork.Repository<Servicios>().FindById(x => x.Id == servicioId, true);

            List<long> estados = new List<long> { 3, 4, 5, 6 };
            if (servicio.RequiereProfesional && empleadoId != null)
            {
                result = (from programacionDisponible in unitOfWork.Repository<ProgramacionDisponible>().GetTable(true).Where(x => x.ServiciosId == servicioId && x.EmpleadosId == empleadoId)
                          join programacionAgenda in unitOfWork.Repository<ProgramacionAgenda>().Table
                             on programacionDisponible.ProgramacionAgendaId equals programacionAgenda.Id
                          where (programacionAgenda.FechaInicio.Date >= DateTime.Now.Date
                          || (DateTime.Now.Date >= programacionAgenda.FechaInicio.Date && DateTime.Now.Date <= programacionAgenda.FechaFinal.Date))
                          select programacionAgenda).ToList();
            }
            else
            {
                result = (from programacionDisponible in unitOfWork.Repository<ProgramacionDisponible>().GetTable(true).Where(x => x.ServiciosId == servicioId && x.ConsultoriosId == consultorioId)
                          join programacionAgenda in unitOfWork.Repository<ProgramacionAgenda>().Table
                             on programacionDisponible.ProgramacionAgendaId equals programacionAgenda.Id
                          where (programacionAgenda.FechaInicio.Date >= DateTime.Now.Date
                          || (DateTime.Now.Date >= programacionAgenda.FechaInicio.Date && DateTime.Now.Date <= programacionAgenda.FechaFinal.Date))
                          select programacionAgenda).ToList();
            }
            schedulerModel.Data = (unitOfWork.Repository<ProgramacionCitas>().GetTable(true).Where(x => x.FechaInicio.Date >= DateTime.Now.AddDays(-1).Date && x.ConsultoriosId == consultorioId && estados.Contains(x.EstadosId))).AsEnumerable<ProgramacionCitas>();

            if (result != null && result.Count > 0)
            {
                var fechaInicio = result.Min(x => x.FechaInicio);
                var fechaHoy = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                schedulerModel.FechaInicial = (fechaInicio < fechaHoy ? fechaHoy : fechaInicio);
                schedulerModel.FechaMinima = (fechaInicio < fechaHoy ? fechaHoy : fechaInicio);
                schedulerModel.FechaMaxima = result.Max(x => x.FechaFinal);

                int horaMinima = result.Min(x => x.FechaInicio.Hour) < result.Min(x => x.FechaFinal.Hour) ? result.Min(x => x.FechaInicio.Hour) : result.Min(x => x.FechaFinal.Hour);
                int horaMaxima = result.Max(x => x.FechaInicio.Hour) > result.Max(x => x.FechaFinal.Hour) ? result.Max(x => x.FechaInicio.Hour) : result.Max(x => x.FechaFinal.Hour);
                int minMaxima = result.Max(x => x.FechaInicio.Minute) > result.Max(x => x.FechaFinal.Minute) ? result.Max(x => x.FechaInicio.Minute) : result.Max(x => x.FechaFinal.Minute);

                schedulerModel.HoraMinima = horaMinima;
                schedulerModel.HoraMaxima = horaMaxima + (double)minMaxima / 100;

                long minutos = (estadosIdTipoDuracion == 10080 ? (duracion * 60) : duracion);
                schedulerModel.IntervaloCelda = Convert.ToInt32(minutos);
            }
            else
            {
                schedulerModel.Habilitado = false;
            }

            if (fechaScheduler != null)
            {
                schedulerModel.FechaInicial = fechaScheduler.GetValueOrDefault();
            }

            return schedulerModel;
        }

        public Dictionary<string, object> ConsultarDisponibilidad(long servicioId, long consultorioId, long? empleadoId, DateTime fechaInicio, long duracion, long tipoDuracion)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            List<ProgramacionAgenda> result = new List<ProgramacionAgenda>();
            var servicio = unitOfWork.Repository<Servicios>().FindById(x => x.Id == servicioId, true);
            ProgramacionCitas citaCruce = null;
            List<long> estados = new List<long> { 3, 4, 5, 6 };

            long minutos = (tipoDuracion == 10080 ? (duracion * 60) : duracion);
            DateTime fechaFinal = fechaInicio.AddMinutes(minutos);

            if (servicio.RequiereProfesional && empleadoId != null)
            {
                result = (from programacionDisponible in unitOfWork.Repository<ProgramacionDisponible>().GetTable(true).Where(x => x.EmpleadosId == empleadoId)
                          join programacionAgenda in unitOfWork.Repository<ProgramacionAgenda>().Table
                             on programacionDisponible.ProgramacionAgendaId equals programacionAgenda.Id
                          where fechaInicio >= programacionAgenda.FechaInicio && fechaInicio <= programacionAgenda.FechaFinal
                          select programacionAgenda).ToList();
                citaCruce = unitOfWork.Repository<ProgramacionCitas>().GetTable(true)
                    .Where(x => x.EmpleadosId == empleadoId && estados.Contains(x.EstadosId)
                    && ((x.FechaInicio >= fechaInicio && x.FechaInicio < fechaFinal)
                        || (x.FechaFinal > fechaInicio && x.FechaFinal < fechaFinal)
                        || (fechaInicio >= x.FechaInicio && fechaInicio < x.FechaFinal)
                        || (fechaFinal > x.FechaInicio && fechaFinal < x.FechaFinal)
                    )).FirstOrDefault();
            }
            else
            {
                result = (from programacionDisponible in unitOfWork.Repository<ProgramacionDisponible>().GetTable(true).Where(x => x.ConsultoriosId == consultorioId)
                          join programacionAgenda in unitOfWork.Repository<ProgramacionAgenda>().Table
                             on programacionDisponible.ProgramacionAgendaId equals programacionAgenda.Id
                          where fechaInicio >= programacionAgenda.FechaInicio && fechaInicio <= programacionAgenda.FechaFinal
                          select programacionAgenda).ToList();
                citaCruce = unitOfWork.Repository<ProgramacionCitas>().GetTable(true)
                    .Where(x => x.ConsultoriosId == consultorioId && estados.Contains(x.EstadosId)
                    && ((x.FechaInicio >= fechaInicio && x.FechaInicio < fechaFinal)
                        || (x.FechaFinal > fechaInicio && x.FechaFinal < fechaFinal)
                        || (fechaInicio >= x.FechaInicio && fechaInicio < x.FechaFinal)
                        || (fechaFinal > x.FechaInicio && fechaFinal < x.FechaFinal)
                    )).FirstOrDefault();
            }

            Dictionary<string, object> data = new Dictionary<string, object>();
            if (citaCruce != null)
            {
                data.Add("Disponible", false);
                data.Add("CitaCruce", citaCruce);
            }
            else
            {
                var disponible = EstaDisponible(result, fechaInicio);
                data.Add("Disponible", disponible);
                if (result != null && result.Count > 0 && disponible)
                {
                    var agenda = result.FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(agenda.Observaciones))
                        data.Add("ObservacionesAgenda", agenda.Observaciones);
                }
            }
            data.Add("DiaSemanaFecha", GetDiaSemanaEsp(fechaInicio));
            data.Add("Festivo", BuscarFestivo(fechaInicio));
            data.Add("FechaInicio", fechaInicio);
            data.Add("FechaFinal", fechaFinal);

            return data;
        }

        private Festivos BuscarFestivo(DateTime fechainicio)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            var soloFecha = fechainicio.Date;
            var festivo = unitOfWork.Repository<Festivos>().GetTable(false).FirstOrDefault(x => x.Dia == soloFecha);
            return festivo;
        }

        private string GetDiaSemanaEsp(DateTime fechaInicio)
        {
            DayOfWeek diaSemanaFecha = fechaInicio.DayOfWeek;

            if (diaSemanaFecha == DayOfWeek.Monday) return "Lunes";
            if (diaSemanaFecha == DayOfWeek.Tuesday) return "Martes";
            if (diaSemanaFecha == DayOfWeek.Wednesday) return "Miercoles";
            if (diaSemanaFecha == DayOfWeek.Thursday) return "Jueves";
            if (diaSemanaFecha == DayOfWeek.Friday) return "Viernes";
            if (diaSemanaFecha == DayOfWeek.Saturday) return "Sabado";
            if (diaSemanaFecha == DayOfWeek.Sunday) return "Domingo";

            return "";
        }

        private bool EstaDisponible(List<ProgramacionAgenda> result, DateTime fechaInicio)
        {
            if (fechaInicio < DateTime.Now)
                return false;

            DayOfWeek diaSemanaFecha = fechaInicio.DayOfWeek;

            foreach (var programacionAgenda in result)
            {
                if (programacionAgenda.Lunes || programacionAgenda.Martes || programacionAgenda.Miercoles
                    || programacionAgenda.Jueves || programacionAgenda.Viernes || programacionAgenda.Sabado || programacionAgenda.Domingo)
                {
                    if (programacionAgenda.Lunes && diaSemanaFecha == DayOfWeek.Monday) return true;
                    if (programacionAgenda.Martes && diaSemanaFecha == DayOfWeek.Tuesday) return true;
                    if (programacionAgenda.Miercoles && diaSemanaFecha == DayOfWeek.Wednesday) return true;
                    if (programacionAgenda.Jueves && diaSemanaFecha == DayOfWeek.Thursday) return true;
                    if (programacionAgenda.Viernes && diaSemanaFecha == DayOfWeek.Friday) return true;
                    if (programacionAgenda.Sabado && diaSemanaFecha == DayOfWeek.Saturday) return true;
                    if (programacionAgenda.Domingo && diaSemanaFecha == DayOfWeek.Sunday) return true;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public IQueryable<Convenios> GetConveniosByEntidad(long Id, long ServicioId)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            var convenios = unitOfWork.Repository<Convenios>().GetTable(false).Where(x => x.EntidadesId == Id);
            var conveniosServicios = unitOfWork.Repository<ConveniosServicios>().GetTable(false).Where(x => x.ServiciosId == ServicioId);

            var result = from convenio in convenios
                         join conveniServi in conveniosServicios on
                            convenio.Id equals conveniServi.ConveniosId
                         select convenio;
            return result;

        }

        public IQueryable<Consultorios> GetConsultoriosPorServicio(long servicioId, long sedesId)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            var serviciosconsultorios = unitOfWork.Repository<ServiciosConsultorios>().GetTable(false).Where(x => x.ServiciosId == servicioId);
            var consultorios = unitOfWork.Repository<Consultorios>().GetTable(true);

            var result = from consultorio in consultorios
                         join servicioConsultorio in serviciosconsultorios on
                         consultorio.Id equals servicioConsultorio.ConsultoriosId
                         where consultorio.SedesId == sedesId
                         select consultorio;
            return result;
        }

        public IQueryable<Empleados> GetEmpleadosPorServicio(long servicioId)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            var servicio = unitOfWork.Repository<Servicios>().FindById(x => x.Id == servicioId, false);
            if (servicio != null && servicio.RequiereProfesional)
            {
                var empleadosServicios = unitOfWork.Repository<ServiciosEmpleados>().GetTable(false).Where(x => x.ServiciosId == servicioId);
                var empleados = unitOfWork.Repository<Empleados>().GetTable(true);

                var result = from empleado in empleados
                             join empleadoServicio in empleadosServicios on
                             empleado.Id equals empleadoServicio.EmpleadosId
                             select empleado;
                return result;
            }
            else
            {
                var result = from empleado in unitOfWork.Repository<Empleados>().GetTable(false)
                             where empleado.Id == 0
                             select empleado;
                return result;
            }
        }

        public IQueryable<Entidades> GetEntidadesByPaciente(long Id, long entidadId)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            var pacientes = unitOfWork.Repository<PacientesEntidades>().GetTable(false).Where(x => x.PacientesId == Id && x.Activo);
            var entidades = unitOfWork.Repository<Entidades>().GetTable(true);

            var result = from entidad in entidades
                         join paciente in pacientes on
                         entidad.Id equals paciente.EntidadesId
                         select entidad;

            if (entidadId != 0)
                result.Where(x => x.Id == entidadId);

            return result;

        }

        public long GetSiguienteConsecutivo()
        {
            try
            {
                BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
                return unitOfWork.Repository<ProgramacionCitas>().Table.Max(x => x.Consecutivo) + 1;
            }
            catch (Exception ex)
            {
                DApp.LogException(ex);
                return 1;
            }
        }

        public override ProgramacionCitas Add(ProgramacionCitas data)
        {
            if (data.TiposCitasId == 1)
            {
                var disponibilidad = ConsultarDisponibilidad(data.ServiciosId, data.ConsultoriosId, data.EmpleadosId, data.FechaInicio, data.Duracion, data.EstadosIdTipoDuracion);
                if (disponibilidad.ContainsKey("Disponible"))
                {
                    bool estaDisponible = Convert.ToBoolean(disponibilidad["Disponible"]);
                    if (!estaDisponible)
                        throw new Exception("El espacio seleccionado ya no se encuentra disponible, por favor verifique nuevamente la agenda.");
                }
            }

            data.Consecutivo = GetSiguienteConsecutivo();
            data.EstadosId = 3;
            return base.Add(data);
        }

        public decimal ObtenerValorTarifaConvenio(long convenioId, long servicioId)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            var result = (from convenio in unitOfWork.Repository<Convenios>().Table
                          join listaPrecio in unitOfWork.Repository<ListaPrecios>().Table on convenio.ListaPreciosId equals listaPrecio.Id
                          join precioServicio in unitOfWork.Repository<PreciosServicios>().Table on listaPrecio.Id equals precioServicio.ListaPreciosId
                          where convenio.Id == convenioId && precioServicio.ServiciosId == servicioId
                          select new { listaPrecio = listaPrecio, precioServicio = precioServicio }).FirstOrDefault();
            decimal tarifaConvenio = 0;
            if (result != null)
            {
                if (result.listaPrecio.TipoEstadosId != 54)
                {
                    tarifaConvenio = result.precioServicio.Precio + result.listaPrecio.Valor;
                }
                else
                {
                    tarifaConvenio = result.precioServicio.Precio + (result.precioServicio.Precio * (result.listaPrecio.Porcentaje / 100));
                }
            }

            return tarifaConvenio;
        }

        public Dictionary<string, object> ConsultarDisponibilidadCitaAdicional(DateTime fechaInicio, long consultorioId)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            List<long> estados = new List<long> { 3, 4, 5, 6 };
            var totalCitas = unitOfWork.Repository<ProgramacionCitas>().Table
                .Count(x => x.FechaInicio == fechaInicio && estados.Contains(x.EstadosId) && x.ConsultoriosId == consultorioId);
            if (totalCitas >= 2)
            {
                result.Add("Disponible", false);
                result.Add("Error", "Ya existe una cita adicional programada en este horario.");
            }
            else
            {
                result.Add("Disponible", true);
            }
            return result;
        }

    }
}
