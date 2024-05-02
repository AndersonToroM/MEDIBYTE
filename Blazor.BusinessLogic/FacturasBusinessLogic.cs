using Blazor.BusinessLogic.Models;
using Blazor.BusinessLogic.Models.Enums;
using Blazor.BusinessLogic.ServiciosExternos;
using Blazor.Infrastructure;
using Blazor.Infrastructure.Entities;
using Blazor.Infrastructure.Entities.Models;
using Blazor.Reports.Facturas;
using DevExpress.Compression;
using DevExpress.Spreadsheet;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using Dominus.Frontend.Controllers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.BusinessLogic
{
    public class FacturasBusinessLogic : GenericBusinessLogic<Facturas>
    {
        public FacturasBusinessLogic(IUnitOfWork unitWork) : base(unitWork)
        {
        }

        public FacturasBusinessLogic(DataBaseSetting configuracionBD) : base(configuracionBD)
        {
        }

        public async Task<IntegracionEnviarFEModel> EnviarFacturaDian(long facturaId, string user, string host)
        {
            ResultadoIntegracionFE enviarDocumento_FE = new ResultadoIntegracionFE();
            enviarDocumento_FE.CreatedBy = user;
            enviarDocumento_FE.UpdatedBy = user;
            enviarDocumento_FE.CreationDate = DateTime.Now;
            enviarDocumento_FE.LastUpdate = DateTime.Now;

            IntegracionEnviarFEModel enviarDocumento_IFE = new IntegracionEnviarFEModel();
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            unitOfWork.BeginTransaction();
            try
            {
                var parametros = unitOfWork.Repository<ParametrosGenerales>().Table.FirstOrDefault();
                var fac = unitOfWork.Repository<Facturas>().Table.FirstOrDefault(x => x.Id == facturaId);

                if (fac.IdDocumentoFE.HasValue &&
                    !string.IsNullOrWhiteSpace(fac.DIANResponse) &&
                    fac.DIANResponse.Equals(DApp.Util.Dian.StatusStaged, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Esta factura ya fue enviada a la DIAN pero no ha sido ceritifcada. Por favor consulte su estado.");
                }

                if (fac.IdDocumentoFE.HasValue &&
                    !string.IsNullOrWhiteSpace(fac.DIANResponse) &&
                    fac.DIANResponse.Equals(DApp.Util.Dian.StatusCertified, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Esta factura ya fue enviada a la DIAN.");
                }

                IntegracionFE integracionFE = new IntegracionFE(parametros, host);

                enviarDocumento_FE.Tipo = (int)TipoDocumento.Factura;
                enviarDocumento_FE.IdTipo = facturaId;

                var json = GetFEJson(facturaId);
                enviarDocumento_IFE = await integracionFE.EnviarDocumento(json, TipoEnvioDocumentoDian.Factura);

                enviarDocumento_FE.Api = enviarDocumento_IFE.Api;
                enviarDocumento_FE.HttpStatus = enviarDocumento_IFE.HttpStatus;
                enviarDocumento_FE.JsonResult = enviarDocumento_IFE.JsonResult;
                enviarDocumento_FE.HuboError = enviarDocumento_IFE.HuboErrorFE || enviarDocumento_IFE.HuboErrorIntegracion;
                enviarDocumento_FE.Error = string.Join(", ", enviarDocumento_IFE.Errores);

                if (!enviarDocumento_FE.HuboError)
                {
                    fac.IdDocumentoFE = enviarDocumento_IFE.IdDocumentFE;
                    fac.UpdatedBy = user;
                    unitOfWork.Repository<Facturas>().Modify(fac);
                    unitOfWork.CommitTransaction();
                    enviarDocumento_IFE.IdDocumentFE = fac.IdDocumentoFE;

                    enviarDocumento_IFE = await ConsultarEstadoDocumento(fac.Id, user, host);
                }
            }
            catch (Exception ex)
            {
                enviarDocumento_FE.HuboError = true;
                enviarDocumento_FE.Error = ex.GetFullErrorMessage();
                enviarDocumento_IFE.Errores.Add(ex.GetFullErrorMessage());
                enviarDocumento_IFE.HuboErrorFE = true;
            }

            unitOfWork.Repository<ResultadoIntegracionFE>().Add(enviarDocumento_FE);
            unitOfWork.CommitTransaction();
            return enviarDocumento_IFE;
        }

        public async Task<IntegracionEnviarFEModel> ConsultarEstadoDocumento(long facturaId, string user, string host, ResultadoIntegracionFEJob job = null)
        {
            ResultadoIntegracionFE consultarEstado_FE = new ResultadoIntegracionFE();
            consultarEstado_FE.CreatedBy = user;
            consultarEstado_FE.UpdatedBy = user;
            consultarEstado_FE.CreationDate = DateTime.Now;
            consultarEstado_FE.LastUpdate = DateTime.Now;
            IntegracionEnviarFEModel consultaEstaod_IFE = new IntegracionEnviarFEModel();
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            unitOfWork.BeginTransaction();
            try
            {
                var parametros = unitOfWork.Repository<ParametrosGenerales>().Table.FirstOrDefault();
                var fac = unitOfWork.Repository<Facturas>().Table.FirstOrDefault(x => x.Id == facturaId);

                if (string.Equals(DApp.Util.Dian.StatusCertified, fac.DIANResponse, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Ya el documento fue certificado por la dian.");
                }

                IntegracionFE integracionFE = new IntegracionFE(parametros, host);

                consultarEstado_FE.Tipo = (int)TipoDocumento.Factura;
                consultarEstado_FE.IdTipo = facturaId;

                consultaEstaod_IFE = await integracionFE.ConsultarEstadoDocumento(fac.IdDocumentoFE.Value);
                consultaEstaod_IFE.IdDocumentFE = fac.IdDocumentoFE;
                consultarEstado_FE.Api = consultaEstaod_IFE.Api;
                consultarEstado_FE.HttpStatus = consultaEstaod_IFE.HttpStatus;
                consultarEstado_FE.JsonResult = consultaEstaod_IFE.JsonResult;
                consultarEstado_FE.HuboError = consultaEstaod_IFE.HuboErrorFE || consultaEstaod_IFE.HuboErrorIntegracion;
                consultarEstado_FE.Error = string.Join(", ", consultaEstaod_IFE.Errores);

                if (!string.IsNullOrWhiteSpace(consultaEstaod_IFE.Status))
                {
                    fac.DIANResponse = consultaEstaod_IFE.Status;
                    unitOfWork.Repository<Facturas>().Modify(fac);
                    unitOfWork.CommitTransaction();
                }

                bool isCertified = consultaEstaod_IFE.Status.Equals(DApp.Util.Dian.StatusCertified, StringComparison.OrdinalIgnoreCase);
                bool isWithErrors = consultaEstaod_IFE.Status.Equals(DApp.Util.Dian.StatusWithErrors, StringComparison.OrdinalIgnoreCase);
                if (!consultarEstado_FE.HuboError)
                {
                    if (isCertified)
                    {
                        consultaEstaod_IFE = await ConsultarDatosDocumento(fac, integracionFE, user, host);
                    }
                }

                if (job != null)
                {
                    job.Ejecutado = true;
                    job.LastUpdate = DateTime.Now;
                    job.UpdatedBy = user;
                    job.Intentos++;
                    job.Detalle += $"| Intento {job.Intentos}: {consultaEstaod_IFE.Status} | ";
                    if (isCertified || isWithErrors)
                    {
                        job.Exitoso = true;
                    }
                    else
                    {
                        job.Exitoso = false;
                    }
                    unitOfWork.Repository<ResultadoIntegracionFEJob>().Modify(job);
                }
                else
                {
                    if (!isCertified && !isWithErrors)
                    {
                        ResultadoIntegracionFEJob resultadoIntegracionFEJob = new ResultadoIntegracionFEJob
                        {
                            Id = 0,
                            Host = host,
                            Tipo = (int)TipoDocumento.Factura,
                            IdTipo = fac.Id,
                            Ejecutado = false,
                            Exitoso = false,
                            Intentos = 1,
                            Detalle = $"| Intento 1: {consultaEstaod_IFE.Status} | ",
                            CreationDate = DateTime.Now,
                            LastUpdate = DateTime.Now,
                            CreatedBy = user,
                            UpdatedBy = user
                        };
                        unitOfWork.Repository<ResultadoIntegracionFEJob>().Add(resultadoIntegracionFEJob);

                        throw new Exception("El documento se envió satisfactoriamente y esta pendiente de certificación.");
                    }
                }
            }
            catch (Exception ex)
            {
                consultarEstado_FE.HuboError = true;
                consultarEstado_FE.Error = ex.GetFullErrorMessage();
                consultaEstaod_IFE.Errores.Add(ex.GetFullErrorMessage());
                consultaEstaod_IFE.HuboErrorFE = true;
            }

            unitOfWork.Repository<ResultadoIntegracionFE>().Add(consultarEstado_FE);
            unitOfWork.CommitTransaction();
            return consultaEstaod_IFE;
        }

        private async Task<IntegracionEnviarFEModel> ConsultarDatosDocumento(Facturas fac, IntegracionFE integracionFE, string user, string host)
        {
            ResultadoIntegracionFE consultarDatosDoc_FE = new ResultadoIntegracionFE();
            consultarDatosDoc_FE.CreatedBy = user;
            consultarDatosDoc_FE.UpdatedBy = user;
            consultarDatosDoc_FE.CreationDate = DateTime.Now;
            consultarDatosDoc_FE.LastUpdate = DateTime.Now;
            IntegracionEnviarFEModel consultarDatosDoc_IFE = new IntegracionEnviarFEModel();
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            unitOfWork.BeginTransaction();
            try
            {
                consultarDatosDoc_FE.Tipo = (int)TipoDocumento.Factura;
                consultarDatosDoc_FE.IdTipo = fac.Id;

                consultarDatosDoc_IFE = await integracionFE.ConsultarDatosDocumento(fac.IdDocumentoFE.Value);
                consultarDatosDoc_IFE.IdDocumentFE = fac.IdDocumentoFE;
                consultarDatosDoc_FE.Api = consultarDatosDoc_IFE.Api;
                consultarDatosDoc_FE.HttpStatus = consultarDatosDoc_IFE.HttpStatus;
                consultarDatosDoc_FE.JsonResult = consultarDatosDoc_IFE.JsonResult;
                consultarDatosDoc_FE.HuboError = consultarDatosDoc_IFE.HuboErrorFE || consultarDatosDoc_IFE.HuboErrorIntegracion;
                consultarDatosDoc_FE.Error = string.Join(", ", consultarDatosDoc_IFE.Errores);
                fac.DIANResponse = consultarDatosDoc_IFE.Status;

                bool isCertified = consultarDatosDoc_IFE.Status.Equals(DApp.Util.Dian.StatusCertified, StringComparison.OrdinalIgnoreCase);

                if (!consultarDatosDoc_FE.HuboError)
                {
                    fac.CUFE = consultarDatosDoc_IFE.Cufe;
                    fac.IssueDate = consultarDatosDoc_IFE.IssueDate;
                    fac.UpdatedBy = user;
                }

                unitOfWork.Repository<Facturas>().Modify(fac);
                unitOfWork.CommitTransaction();

                if (isCertified)
                {
                    var envioCorreo = await EnviarEmail(fac.Id, "Envío automático SIISO", user, host);
                    if (!envioCorreo)
                    {
                        ConfiguracionEnvioEmailJob configuracionEnvioEmailJob = new ConfiguracionEnvioEmailJob
                        {
                            Id = 0,
                            Host = host,
                            Tipo = (int)TipoDocumento.Factura,
                            IdTipo = fac.Id,
                            Ejecutado = false,
                            Exitoso = false,
                            Intentos = 1,
                            Detalle = $"| Intento 1: {string.Join(", ", consultarDatosDoc_IFE.Errores)} | ",
                            CreationDate = DateTime.Now,
                            LastUpdate = DateTime.Now,
                            CreatedBy = user,
                            UpdatedBy = user
                        };
                        unitOfWork.Repository<ConfiguracionEnvioEmailJob>().Add(configuracionEnvioEmailJob);
                    }
                }

            }
            catch (Exception ex)
            {
                consultarDatosDoc_FE.HuboError = true;
                consultarDatosDoc_FE.Error = ex.GetFullErrorMessage();
                consultarDatosDoc_IFE.Errores.Add(ex.GetFullErrorMessage());
                consultarDatosDoc_IFE.HuboErrorFE = true;
            }

            unitOfWork.Repository<ResultadoIntegracionFE>().Add(consultarDatosDoc_FE);
            unitOfWork.CommitTransaction();
            return consultarDatosDoc_IFE;
        }

        public async Task<IntegracionXmlFEModel> GetArchivoXmlDIAN(long facturaId, string user, string host)
        {
            ResultadoIntegracionFE resultadoIntegracionFE = new ResultadoIntegracionFE();
            IntegracionXmlFEModel integracionXmlFEModel = new IntegracionXmlFEModel();
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            try
            {
                var parametros = unitOfWork.Repository<ParametrosGenerales>().Table.FirstOrDefault();
                var fac = unitOfWork.Repository<Facturas>().Table.FirstOrDefault(x => x.Id == facturaId);

                if (!fac.IdDocumentoFE.HasValue ||
                    !fac.IssueDate.HasValue ||
                    string.IsNullOrWhiteSpace(fac.DIANResponse) || !fac.DIANResponse.Equals(DApp.Util.Dian.StatusCertified, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Esta factura no esta validada en la DIAN.");
                }

                IntegracionFE integracionRips = new IntegracionFE(parametros, host);

                resultadoIntegracionFE.CreatedBy = user;
                resultadoIntegracionFE.UpdatedBy = user;
                resultadoIntegracionFE.CreationDate = DateTime.Now;
                resultadoIntegracionFE.LastUpdate = DateTime.Now;
                resultadoIntegracionFE.Tipo = (int)TipoDocumento.Factura;
                resultadoIntegracionFE.IdTipo = facturaId;

                integracionXmlFEModel = await integracionRips.GetXmlFile(fac.IdDocumentoFE.Value);

                resultadoIntegracionFE.HttpStatus = integracionXmlFEModel.HttpStatus;
                resultadoIntegracionFE.JsonResult = integracionXmlFEModel.JsonResult;
                resultadoIntegracionFE.HuboError = integracionXmlFEModel.HuboErrorFE || integracionXmlFEModel.HuboErrorIntegracion;
                resultadoIntegracionFE.Error = string.Join(", ", integracionXmlFEModel.Errores);

                if (!resultadoIntegracionFE.HuboError)
                {
                    string pathXml = Path.Combine(Path.GetTempPath(), integracionXmlFEModel.FileName);
                    File.WriteAllBytes(pathXml, integracionXmlFEModel.ContentBytes);
                    integracionXmlFEModel.PathFile = pathXml;
                }
            }
            catch (Exception ex)
            {
                resultadoIntegracionFE.HuboError = true;
                resultadoIntegracionFE.Error = ex.GetFullErrorMessage();
                integracionXmlFEModel.Errores.Add(ex.GetFullErrorMessage());
                integracionXmlFEModel.HuboErrorFE = true;
            }

            unitOfWork.Repository<ResultadoIntegracionFE>().Add(resultadoIntegracionFE);
            unitOfWork.CommitTransaction();
            return integracionXmlFEModel;
        }

        /// <summary>
        /// https://localhost:44333/empresas/ObtenerJsonFacturaFE?id=68282
        /// </summary>
        /// <param name="idFactura"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetFEJson(long facturaId)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            var fac = unitOfWork.Repository<Facturas>().Table
                .Include(x => x.Empresas.TiposIdentificacion)
                .Include(x => x.Empresas.Ciudades.Departamentos.Paises)
                .Include(x => x.Documentos)
                .Include(x => x.Entidades)
                .Include(x => x.Entidades.TiposPersonas)
                .Include(x => x.Entidades.Ciudades.Departamentos.Paises)
                .Include(x => x.Entidades.TiposIdentificacion)
                .Include(x => x.Entidades.EntidadesResponsabilidadesFiscales).ThenInclude(x => x.ResponsabilidadesFiscales)
                .Include(x => x.Pacientes)
                .Include(x => x.Pacientes.Ciudades.Departamentos.Paises)
                .Include(x => x.Pacientes.TiposIdentificacion)
                .Include(x => x.Convenio.ModalidadesContratacion)
                .Include(x => x.MediosPago)
                .Include(x => x.FormasPagos)
                .FirstOrDefault(x => x.Id == facturaId);

            if (fac == null)
            {
                throw new Exception($"La factura con el Id {facturaId} no se encuentra registrada en el sistema.");
            }

            var parametros = unitOfWork.Repository<ParametrosGenerales>().Table.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(parametros.EmailRecepcionFE))
            {
                throw new Exception($"El email para recepcionar los documentos de la facturación electrónica no se encuentra parametrizado correctamente.");
            }
            if (!DApp.Util.EsEmailValido(parametros.EmailRecepcionFE))
            {
                throw new Exception($"El email para recepcionar los documentos de la facturación electrónica no es válido.");
            }

            var facDetalles = unitOfWork.Repository<FacturasDetalles>().Table
                .Include(x => x.Servicios)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.FacturaCopagoCuotaModeradora)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Pacientes)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.CoberturaPlanBeneficios)
                .Where(x => x.FacturasId == fac.Id)
                .ToList();

            FeFacturaJson feRootJson = new FeFacturaJson();
            feRootJson.Currency = DApp.Util.Dian.Currency;
            feRootJson.SeriePrefix = fac.Documentos.Prefijo;
            feRootJson.SerieNumber = fac.NroConsecutivo.ToString();
            if (fac.EsCopagoModeradora)
            {
                feRootJson.OperationType = DApp.Util.Dian.OperationTypeSSRecaudo;
            }
            else
            {
                feRootJson.OperationType = DApp.Util.Dian.OperationTypeSSCUFE;
            }
            feRootJson.IssueDate = fac.Fecha;
            feRootJson.DeliveryDate = fac.Fecha;
            feRootJson.DueDate = fac.ConvenioId.HasValue ? (fac.Convenio.PeriodicidadPago.HasValue ? fac.Fecha.AddDays(fac.Convenio.PeriodicidadPago.Value).ToString("yyyy-MM-dd") : fac.Fecha.ToString("yyyy-MM-dd")) : fac.Fecha.ToString("yyyy-MM-dd");
            feRootJson.CorrelationDocumentId = fac.ConsecutivoFE;
            feRootJson.SerieExternalKey = fac.Documentos.ExternalKey;
            feRootJson.IssuerParty.Identification.DocumentNumber = fac.Empresas.NumeroIdentificacion;
            feRootJson.IssuerParty.Identification.DocumentType = fac.Empresas.TiposIdentificacion.CodigoFE;
            feRootJson.IssuerParty.Identification.CountryCode = fac.Empresas.Ciudades.Departamentos.Paises.Codigo;
            feRootJson.IssuerParty.Identification.CheckDigit = fac.Empresas.DV;

            FePaymentMean fEPaymentMeans = new FePaymentMean();
            fEPaymentMeans.Code = fac.MediosPago.Codigo;
            fEPaymentMeans.Mean = fac.FormasPagos.Codigo;
            fEPaymentMeans.DueDate = fac.ConvenioId.HasValue ? (fac.Convenio.PeriodicidadPago.HasValue ? fac.Fecha.AddDays(fac.Convenio.PeriodicidadPago.Value).ToString("yyyy-MM-dd") : fac.Fecha.ToString("yyyy-MM-dd")) : fac.Fecha.ToString("yyyy-MM-dd");
            feRootJson.PaymentMeans.Add(fEPaymentMeans);

            feRootJson.BillingPeriod.From = fac.FehaInicial.ToString("yyyy-MM-dd");
            feRootJson.BillingPeriod.To = fac.FechaFinal.ToString("yyyy-MM-dd");

            feRootJson.Total.GrossAmount = fac.ValorSubtotal.ToString(CultureInfo.InvariantCulture);
            feRootJson.Total.TotalBillableAmount = fac.ValorSubtotal.ToString(CultureInfo.InvariantCulture);
            feRootJson.Total.PayableAmount = fac.ValorTotal.ToString(CultureInfo.InvariantCulture);
            feRootJson.Total.TaxableAmount = "0";

            if (!string.IsNullOrWhiteSpace(fac.Observaciones))
                feRootJson.Notes.Add(fac.Observaciones);
            else
                feRootJson.Notes = null;

            feRootJson.CustomerParty.Email = parametros.EmailRecepcionFE; // Es el correo al que van a llegar las notificaciones del provedor de FE
            if (fac.EsFacturaInstitucional)
            {
                feRootJson.CustomerParty.LegalType = fac.Entidades.TiposPersonas.NombreFE;
                feRootJson.CustomerParty.TaxScheme = "ZZ"; //Identificador del Régimen Fiscal del adquirente ???
                feRootJson.CustomerParty.ResponsabilityTypes.AddRange(fac.Entidades.EntidadesResponsabilidadesFiscales.Select(x => x.ResponsabilidadesFiscales.Codigo));
                feRootJson.CustomerParty.Identification.DocumentNumber = fac.Entidades.NumeroIdentificacion;
                feRootJson.CustomerParty.Identification.DocumentType = fac.Empresas.TiposIdentificacion.CodigoFE;
                feRootJson.CustomerParty.Identification.CountryCode = fac.Entidades.Ciudades.Departamentos.Paises.Codigo;
                feRootJson.CustomerParty.Identification.CheckDigit = fac.Entidades.DV;
                feRootJson.CustomerParty.Name = fac.Entidades.Nombre;
                feRootJson.CustomerParty.Address.DepartmentCode = fac.Entidades.Ciudades.Departamentos.Codigo;
                feRootJson.CustomerParty.Address.CityCode = fac.Entidades.Ciudades.Codigo;
                feRootJson.CustomerParty.Address.AddressLine = fac.Entidades.Direccion;
                feRootJson.CustomerParty.Address.Country = fac.Entidades.Ciudades.Departamentos.Paises.Codigo;
                feRootJson.Total.PrePaidTotalAmount = fac.ValorCopagoCuotaModeradora.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                feRootJson.CustomerParty.LegalType = "Natural"; // agregar tipopersona a la tabla pacientes
                feRootJson.CustomerParty.TaxScheme = "ZZ";
                feRootJson.CustomerParty.ResponsabilityTypes.Add("R-99-PN");
                feRootJson.CustomerParty.Identification.DocumentNumber = fac.Pacientes.NumeroIdentificacion;
                feRootJson.CustomerParty.Identification.DocumentType = fac.Pacientes.TiposIdentificacion.CodigoFE;
                feRootJson.CustomerParty.Identification.CountryCode = fac.Pacientes.Ciudades.Departamentos.Paises.Codigo;
                feRootJson.CustomerParty.Person.FirstName = fac.Pacientes.PrimerNombre;
                feRootJson.CustomerParty.Person.MiddleName = fac.Pacientes.SegundoNombre;
                feRootJson.CustomerParty.Person.FamilyName = fac.Pacientes.PrimerApellido;
                feRootJson.CustomerParty.Address.DepartmentCode = fac.Pacientes.Ciudades.Departamentos.Codigo;
                feRootJson.CustomerParty.Address.CityCode = fac.Pacientes.Ciudades.Codigo;
                feRootJson.CustomerParty.Address.AddressLine = fac.Pacientes.Direccion;
                feRootJson.CustomerParty.Address.Country = fac.Pacientes.Ciudades.Departamentos.Paises.Codigo;
                feRootJson.Total.PrePaidTotalAmount = "0";
                feRootJson.PrepaidPayments = null;
            }

            if (!string.IsNullOrWhiteSpace(fac.ReferenciaFactura))
            {
                feRootJson.DocumentReferences.Add(new FeDocumentReference
                {
                    DocumentReferred = fac.ReferenciaFactura,
                    IssueDate = fac.Fecha.ToString("yyyy-MM-dd"),
                    Type = DApp.Util.Dian.OtherReference
                });
            }

            if (!string.IsNullOrWhiteSpace(fac.OrdenCompra))
            {
                feRootJson.DocumentReferences.Add(new FeDocumentReference
                {
                    DocumentReferred = fac.OrdenCompra,
                    IssueDate = fac.Fecha.ToString("yyyy-MM-dd"),
                    Type = DApp.Util.Dian.OrderReference
                });
            }

            int numberLine = 1;
            foreach (var facDetalle in facDetalles)
            {
                FeLine feLine = new FeLine();
                feLine.Number = numberLine.ToString();
                feLine.Quantity = facDetalle.Cantidad.ToString("F2", CultureInfo.InvariantCulture);
                feLine.QuantityUnitOfMeasure = DApp.Util.Dian.QuantityUnitOfMeasure;
                feLine.ExcludeVat = "true";
                feLine.UnitPrice = facDetalle.ValorServicio.ToString(CultureInfo.InvariantCulture);
                feLine.GrossAmount = facDetalle.SubTotal.ToString(CultureInfo.InvariantCulture);
                feLine.NetAmount = facDetalle.ValorTotal.ToString(CultureInfo.InvariantCulture);
                feLine.Item.Gtin = facDetalle.Servicios.Codigo;
                feLine.Item.Description = facDetalle.Servicios.Nombre;

                if (fac.EsFacturaInstitucional)
                {
                    feLine.WithholdingTaxSubTotals.Add(new FeWithholdingTaxSubTotal
                    {
                        WithholdingTaxCategory = DApp.Util.Dian.Reterenta,
                        TaxPercentage = fac.Entidades.PorcentajeRetefuente.ToString(CultureInfo.InvariantCulture),
                        TaxableAmount = fac.ValorTotal.ToString(CultureInfo.InvariantCulture),
                        TaxAmount = (fac.ValorTotal * (fac.Entidades.PorcentajeRetefuente / 100)).ToString("F2", CultureInfo.InvariantCulture)
                    });
                    feLine.WithholdingTaxSubTotals.Add(new FeWithholdingTaxSubTotal
                    {
                        WithholdingTaxCategory = DApp.Util.Dian.Reteica,
                        TaxPercentage = fac.Entidades.PorcentajeReteIca.ToString(CultureInfo.InvariantCulture),
                        TaxableAmount = fac.ValorTotal.ToString(CultureInfo.InvariantCulture),
                        TaxAmount = (fac.ValorTotal * (fac.Entidades.PorcentajeReteIca / 100)).ToString("F2", CultureInfo.InvariantCulture)
                    });
                    feLine.WithholdingTaxTotals.Add(new FeWithholdingTaxTotal
                    {
                        WithholdingTaxCategory = DApp.Util.Dian.Reterenta,
                        TaxAmount = (fac.ValorTotal * (fac.Entidades.PorcentajeRetefuente / 100)).ToString("F2", CultureInfo.InvariantCulture)
                    });
                    feLine.WithholdingTaxTotals.Add(new FeWithholdingTaxTotal
                    {
                        WithholdingTaxCategory = DApp.Util.Dian.Reteica,
                        TaxAmount = (fac.ValorTotal * (fac.Entidades.PorcentajeReteIca / 100)).ToString("F2", CultureInfo.InvariantCulture)
                    });

                    if (facDetalle.AdmisionesServiciosPrestados.Atenciones.Admisiones.ValorPagoEstadosId == 58 || facDetalle.AdmisionesServiciosPrestados.Atenciones.Admisiones.ValorPagoEstadosId == 59)
                    {
                        FePrepaidPayment fePrepaidPayment = new FePrepaidPayment();
                        var facturaCopagoCuotaModeradora = facDetalle.AdmisionesServiciosPrestados.Atenciones.Admisiones.FacturaCopagoCuotaModeradora;
                        if (facturaCopagoCuotaModeradora == null)
                        {
                            throw new Exception($"La admisión numero {facDetalle.AdmisionesServiciosPrestados.Atenciones.Admisiones.Consecutivo} no ha sido facturada.");
                        }
                        fePrepaidPayment.PaidDate = facturaCopagoCuotaModeradora.Fecha;
                        fePrepaidPayment.PaidAmount = facturaCopagoCuotaModeradora.ValorTotal.ToString(CultureInfo.InvariantCulture);
                        feRootJson.PrepaidPayments.Add(fePrepaidPayment);
                    }

                    FeCollection feCollection = new FeCollection();
                    feCollection.Name = DApp.Util.Dian.FeCollectionName;
                    feCollection.NameValues.Add(new FeNameValue
                    {
                        Name = DApp.Util.Dian.CodigoPrestador,
                        Value = fac.Empresas.CodigoReps
                    });
                    feCollection.NameValues.Add(new FeNameValue
                    {
                        Name = DApp.Util.Dian.ModalidadPago,
                        Value = fac.Convenio.ModalidadesContratacion.Descripcion,
                        CodeListName = DApp.Util.Dian.CodeListName,
                        CodeListCode = fac.Convenio.ModalidadesContratacion.CodigoRips
                    });
                    feCollection.NameValues.Add(new FeNameValue
                    {
                        Name = DApp.Util.Dian.OberturaPlanBeneficios,
                        Value = facDetalle.AdmisionesServiciosPrestados.Atenciones.Admisiones.CoberturaPlanBeneficios.Descripcion,
                        CodeListName = DApp.Util.Dian.CodeListName,
                        CodeListCode = facDetalle.AdmisionesServiciosPrestados.Atenciones.Admisiones.CoberturaPlanBeneficios.CodigoRips
                    });
                    feCollection.NameValues.Add(new FeNameValue
                    {
                        Name = DApp.Util.Dian.NumeroContrato,
                        Value = fac.Convenio.NroContrato
                    });
                    feCollection.NameValues.Add(new FeNameValue
                    {
                        Name = DApp.Util.Dian.NumeroPoliza,
                        Value = fac.Convenio.NroPoliza
                    });
                    var admision = facDetalle.AdmisionesServiciosPrestados.Atenciones.Admisiones;
                    feCollection.NameValues.Add(new FeNameValue
                    {
                        Name = DApp.Util.Dian.Copago,
                        Value = admision.ValorPagoEstadosId == 58 ? admision.ValorCopago.ToString(CultureInfo.InvariantCulture) : "0"
                    });
                    feCollection.NameValues.Add(new FeNameValue
                    {
                        Name = DApp.Util.Dian.CuotaModeradora,
                        Value = admision.ValorPagoEstadosId == 59 ? admision.ValorCopago.ToString(CultureInfo.InvariantCulture) : "0"
                    });
                    feCollection.NameValues.Add(new FeNameValue
                    {
                        Name = DApp.Util.Dian.CuotaRecuperacion,
                        Value = admision.ValorPagoEstadosId == 68 ? admision.ValorCopago.ToString(CultureInfo.InvariantCulture) : "0"
                    });
                    feCollection.NameValues.Add(new FeNameValue
                    {
                        Name = DApp.Util.Dian.PagosCompartidos,
                        Value = admision.ValorPagoEstadosId == 69 ? admision.ValorCopago.ToString(CultureInfo.InvariantCulture) : "0"
                    });

                    feRootJson.HealthcareData.Collections.Add(feCollection);
                }
                else
                {
                    FeCollection feCollection = new FeCollection();
                    feCollection.Name = DApp.Util.Dian.FeCollectionName;
                    feCollection.NameValues.Add(new FeNameValue
                    {
                        Name = "NA",
                        Value = "NA"
                    });

                    feRootJson.HealthcareData.Collections.Add(feCollection);
                }

                feRootJson.Lines.Add(feLine);
                numberLine++;
            }

            return JsonConvert.SerializeObject(feRootJson, Newtonsoft.Json.Formatting.Indented);
        }

        public async Task<IntegracionRipsModel> EnviarRips(long facturaId, string user, string host)
        {
            ResultadoIntegracionRips resultadoIntegracionRips = new ResultadoIntegracionRips();
            IntegracionRipsModel integracionRipsModel = new IntegracionRipsModel();
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            try
            {
                var empresa = unitOfWork.Repository<Empresas>().Table.FirstOrDefault();
                var usuario = unitOfWork.Repository<User>().Table
                    .Include(x => x.IdentificationType)
                    .FirstOrDefault(x => x.UserName == user);

                var fac = unitOfWork.Repository<Facturas>().Table.FirstOrDefault(x => x.Id == facturaId);

                IntegracionRips integracionRips = new IntegracionRips(empresa, usuario, host);

                resultadoIntegracionRips.CreatedBy = user;
                resultadoIntegracionRips.UpdatedBy = user;
                resultadoIntegracionRips.CreationDate = DateTime.Now;
                resultadoIntegracionRips.LastUpdate = DateTime.Now;
                resultadoIntegracionRips.Tipo = (int)TipoDocumento.Factura;
                resultadoIntegracionRips.IdTipo = facturaId;

                var json = await GetRipsJson(facturaId, user, host);
                integracionRipsModel = await integracionRips.EnviarRipsFactura(json);

                resultadoIntegracionRips.HttpStatus = integracionRipsModel.HttpStatus;
                resultadoIntegracionRips.JsonResult = integracionRipsModel.JsonResult;
                resultadoIntegracionRips.HuboError = integracionRipsModel.HuboErrorRips || integracionRipsModel.HuboErrorIntegracion;
                resultadoIntegracionRips.Error = integracionRipsModel.Error;

                if (!resultadoIntegracionRips.HuboError)
                {
                    fac.FechaRadicacionRips = integracionRipsModel.Respuesta.FechaRadicacion;
                    fac.CodigoUnicoValidacionRips = integracionRipsModel.Respuesta.CodigoUnicoValidacion;
                    fac.UpdatedBy = user;
                    fac.LastUpdate = DateTime.Now;
                    unitOfWork.Repository<Facturas>().Modify(fac);
                }
            }
            catch (Exception ex)
            {
                resultadoIntegracionRips.HuboError = true;
                resultadoIntegracionRips.Error = ex.GetFullErrorMessage();
            }

            unitOfWork.Repository<ResultadoIntegracionRips>().Add(resultadoIntegracionRips);

            return integracionRipsModel;
        }

        public ArchivoDescargaModel GetJsonRipsFile(long facturaId)
        {
            ArchivoDescargaModel archivoDescargaModel = new ArchivoDescargaModel();
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            var fac = unitOfWork.Repository<Facturas>().Table
                .Include(x => x.Documentos)
                .FirstOrDefault(x => x.Id == facturaId);

            var query = unitOfWork.Repository<ResultadoIntegracionRips>().Table
                .OrderBy(x => x.Id)
                .Where(x => x.Tipo == (int)TipoDocumento.Factura && x.IdTipo == facturaId);

            if (!string.IsNullOrWhiteSpace(fac.CodigoUnicoValidacionRips) && fac.FechaRadicacionRips.HasValue)
            {
                query.Where(x => x.HttpStatus == (int)HttpStatusCode.OK);
            }

            var resultadoRips = query.OrderBy(x => x.Id).LastOrDefault();

            if (resultadoRips == null)
            {
                throw new Exception("No se ha realizado ninguna validación de los RIPS de esta factura.");
            }

            archivoDescargaModel.Nombre = $"{DateTime.Now:yyyyMMddHHmmss}-JsonRips-{fac.Documentos.Prefijo}{fac.NroConsecutivo}.json";
            archivoDescargaModel.ContentType = "application/json";
            archivoDescargaModel.Archivo = Encoding.ASCII.GetBytes(resultadoRips.JsonResult);

            return archivoDescargaModel;
        }

        /// <summary>
        /// https://localhost:44333/empresas/ObtenerJsonRips?id=68282
        /// </summary>
        /// <param name="idFactura"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GetRipsJson(long facturaId, string user, string host)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            var fac = unitOfWork.Repository<Facturas>().Table
                .Include(x => x.Empresas)
                .Include(x => x.Documentos)
                .FirstOrDefault(x => x.Id == facturaId);

            if (fac == null)
            {
                throw new Exception($"La factura con el Id {facturaId} no se encuentra registrada en el sistema.");
            }

            if (!fac.EsFacturaInstitucional)
            {
                throw new Exception("La factura no es de tipo institucional.");
            }

            if (string.IsNullOrWhiteSpace(fac.XmlUrl))
            {
                throw new Exception("La factura no tiene registro en la DIAN o no se le ha generado XML.");
            }

            var facturasDetalles = unitOfWork.Repository<FacturasDetalles>().Table
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Empleados)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Empleados.TiposIdentificacion)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.FinalidadConsulta)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.CausasExternas)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.DiagnosticosPrincipalHC)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Pacientes)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Pacientes.ZonaTerritorialResidencia)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Pacientes.TiposIdentificacion)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Pacientes.Generos)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Pacientes.Ciudades.Departamentos.Paises)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Diagnosticos)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.ModalidadAtencion)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.FacturaCopagoCuotaModeradora)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.FacturaCopagoCuotaModeradora.Documentos)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.CoberturaPlanBeneficios)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.ViaIngresoServicioSalud)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.ModalidadAtencion)
                .Include(x => x.Servicios.Cups)
                .Include(x => x.Servicios.HabilitacionServciosRips)
                .Include(x => x.Servicios.GrupoServciosRips)
                .Include(x => x.Facturas)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.Pacientes.PaisesOrigen)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones.ValorPagoEstados)
                .Include(x => x.AdmisionesServiciosPrestados.Atenciones.FinalidadProcedimiento)
                .Where(x => x.FacturasId == fac.Id)
                .ToList();

            if (facturasDetalles == null || !facturasDetalles.Any())
            {
                throw new Exception("La factura no tiene servicios facturados.");
            }

            var admisiones = facturasDetalles.Select(x => x.AdmisionesServiciosPrestados.Atenciones.Admisiones).Distinct().ToList();

            RipsRootJson ripsRootJson = new RipsRootJson();

            var xmlfile = await GetArchivoXmlDIAN(fac.Id, user, host);
            ripsRootJson.XmlFevFile = xmlfile.ContentBase64;

            ripsRootJson.Rips.NumDocumentoIdObligado = fac.Empresas.NumeroIdentificacion;
            ripsRootJson.Rips.NumFactura = $"{fac.Documentos.Prefijo}{fac.NroConsecutivo}";

            int consecutivoUsuario = 1;
            foreach (var admision in admisiones)
            {
                UsuarioRips usuarioRips = new UsuarioRips();
                usuarioRips.Consecutivo = consecutivoUsuario;
                usuarioRips.TipoDocumentoIdentificacion = admision.Pacientes.TiposIdentificacion.Codigo;
                usuarioRips.NumDocumentoIdentificacion = admision.Pacientes.NumeroIdentificacion;
                usuarioRips.TipoUsuario = admision.Pacientes.NumeroIdentificacion;
                usuarioRips.FechaNacimiento = admision.Pacientes.FechaNacimiento.ToString("yyyy-MM-dd");
                usuarioRips.CodPaisOrigen = admision.Pacientes.PaisesOrigen?.CodigoISO3166Num;
                usuarioRips.CodSexo = admision.Pacientes.Generos?.Codigo;
                usuarioRips.CodPaisResidencia = admision.Pacientes.Ciudades?.Departamentos?.Paises?.CodigoISO3166Num;
                usuarioRips.CodMunicipioResidencia = admision.Pacientes.Ciudades?.Codigo;
                usuarioRips.CodZonaTerritorialResidencia = admision.Pacientes.ZonaTerritorialResidencia?.Codigo;
                usuarioRips.TipoUsuario = admision.CoberturaPlanBeneficios.CodigoRips;

                var tieneIncapacidad = unitOfWork.Repository<Incapacidades>().Table.Any(a => a.HIstoriasClinicas.AtencionesId == admision.Atenciones.Id);
                usuarioRips.Incapacidad = tieneIncapacidad ? "SI" : "NO";

                int consecutivoProcedimiento = 1;
                int consecutivoConsulta = 1;
                var serviciosFacturados = facturasDetalles.Where(x => x.AdmisionesServiciosPrestados.Atenciones.AdmisionesId == admision.Id).ToList();
                foreach (var servicio in serviciosFacturados)
                {
                    if (servicio.Servicios.TiposServiciosId == 1) // Consulta
                    {
                        ConsultaRips consultaRips = new ConsultaRips();

                        consultaRips.Consecutivo = consecutivoConsulta;
                        consultaRips.CodPrestador = fac.Empresas?.CodigoReps;
                        consultaRips.FechaInicioAtencion = servicio.AdmisionesServiciosPrestados?.Atenciones?.FechaAtencion.ToString("yyyy-MM-dd");
                        consultaRips.NumAutorizacion = servicio.AdmisionesServiciosPrestados?.Atenciones?.Admisiones.NroAutorizacion;
                        if (servicio.Servicios.CupsId != null)
                        {
                            consultaRips.CodConsulta = servicio.Servicios?.Cups?.Codigo;
                        }
                        else
                        {
                            consultaRips.CodConsulta = servicio.Servicios?.Codigo;
                        }
                        consultaRips.ModalidadGrupoServicioTecSal = servicio.AdmisionesServiciosPrestados.Atenciones?.Admisiones?.ModalidadAtencion?.Codigo;
                        consultaRips.GrupoServicios = servicio.Servicios?.GrupoServciosRips?.Codigo;
                        consultaRips.CodServicio = servicio.Servicios?.HabilitacionServciosRips?.Codigo;
                        consultaRips.FinalidadTecnologiaSalud = servicio.AdmisionesServiciosPrestados?.Atenciones?.FinalidadConsulta?.CodigoRips;
                        consultaRips.CausaMotivoAtencion = servicio.AdmisionesServiciosPrestados?.Atenciones?.CausasExternas?.CodigoRips;

                        var diagonosticos = unitOfWork.Repository<HistoriasClinicasDiagnosticos>().Table
                            .Include(x => x.Diagnosticos)
                            .Where(a => a.HistoriasClinicas.AtencionesId == admision.Atenciones.Id)
                            .OrderBy(x => x.Id).ToList();
                        if (diagonosticos == null || !diagonosticos.Any())
                        {
                            consultaRips.CodDiagnosticoPrincipal = null;
                            consultaRips.CodDiagnosticoRelacionado1 = null;
                            consultaRips.CodDiagnosticoRelacionado2 = null;
                            consultaRips.CodDiagnosticoRelacionado3 = null;
                        }
                        else
                        {
                            var diagnosticoPrincipal = diagonosticos.FirstOrDefault(x => x.Principal);
                            if (diagnosticoPrincipal != null)
                            {
                                consultaRips.CodDiagnosticoPrincipal = diagnosticoPrincipal.Diagnosticos?.Codigo;
                                consultaRips.TipoDiagnosticoPrincipal = diagnosticoPrincipal.TiposDiagnosticos?.CodigoRips;
                            }

                            var otrosDiagnosticos = diagonosticos.Where(x => x.Id != diagnosticoPrincipal.Id).OrderBy(x => x.Id).ToList();
                            if (otrosDiagnosticos != null && otrosDiagnosticos.Any())
                            {
                                consultaRips.CodDiagnosticoRelacionado1 = otrosDiagnosticos[0]?.Diagnosticos?.Codigo;
                                if (otrosDiagnosticos.Count > 1)
                                {
                                    consultaRips.CodDiagnosticoRelacionado2 = otrosDiagnosticos[1]?.Diagnosticos?.Codigo;
                                }
                                if (otrosDiagnosticos.Count > 2)
                                {
                                    consultaRips.CodDiagnosticoRelacionado3 = otrosDiagnosticos[2]?.Diagnosticos?.Codigo;
                                }
                            }
                        }

                        consultaRips.NumDocumentoIdentificacion = servicio.AdmisionesServiciosPrestados.Atenciones?.Empleados?.NumeroIdentificacion;
                        consultaRips.TipoDocumentoIdentificacion = servicio.AdmisionesServiciosPrestados.Atenciones.Empleados?.TiposIdentificacion?.Codigo;
                        consultaRips.VrServicio = Convert.ToInt32(servicio.ValorServicio);
                        consultaRips.ValorPagoModerador = Convert.ToInt32(servicio.Facturas.ValorTotal);

                        if (servicio.AdmisionesServiciosPrestados.Atenciones.Admisiones.ValorPagoEstadosId == 58 || servicio.AdmisionesServiciosPrestados.Atenciones.Admisiones.ValorPagoEstadosId == 59)
                        {
                            var facturaCopagoCuotaModeradora = servicio.AdmisionesServiciosPrestados.Atenciones.Admisiones.FacturaCopagoCuotaModeradora;
                            consultaRips.NumFEVPagoModerador = $"{facturaCopagoCuotaModeradora.Documentos.Prefijo}{facturaCopagoCuotaModeradora.NroConsecutivo}";
                        }

                        consecutivoConsulta++;
                        usuarioRips.Servicios.Consultas.Add(consultaRips);
                    }

                    if (servicio.Servicios.TiposServiciosId == 2) // Procedimiento
                    {
                        ProcedimientoRips procedimientoRips = new ProcedimientoRips();

                        procedimientoRips.Consecutivo = consecutivoProcedimiento;
                        procedimientoRips.CodPrestador = fac.Empresas?.CodigoReps;
                        procedimientoRips.CodComplicacion = null;
                        procedimientoRips.CodDiagnosticoPrincipal = servicio.AdmisionesServiciosPrestados.Atenciones.Admisiones?.Diagnosticos?.Codigo;
                        procedimientoRips.CodDiagnosticoRelacionado = null;
                        if (servicio.Servicios.CupsId != null)
                        {
                            procedimientoRips.CodProcedimiento = servicio.Servicios?.Cups?.Codigo;
                        }
                        else
                        {
                            procedimientoRips.CodProcedimiento = servicio.Servicios?.Codigo;
                        }
                        procedimientoRips.CodServicio = servicio.Servicios?.HabilitacionServciosRips?.Codigo;
                        procedimientoRips.TipoPagoModerador = servicio.AdmisionesServiciosPrestados.Atenciones?.Admisiones?.ValorPagoEstados?.CodigoRips;
                        procedimientoRips.FechaInicioAtencion = servicio.AdmisionesServiciosPrestados.Atenciones?.FechaAtencion.ToString("yyyy-MM-dd HH:mm");
                        procedimientoRips.FinalidadTecnologiaSalud = servicio.AdmisionesServiciosPrestados.Atenciones?.FinalidadProcedimiento?.CodigoRips;
                        procedimientoRips.GrupoServicios = servicio.Servicios?.GrupoServciosRips?.Codigo;
                        procedimientoRips.IdMIPRES = null;
                        procedimientoRips.ModalidadGrupoServicioTecSal = servicio.AdmisionesServiciosPrestados.Atenciones?.Admisiones?.ModalidadAtencion?.Codigo;
                        procedimientoRips.NumAutorizacion = servicio.AdmisionesServiciosPrestados.Atenciones?.Admisiones?.NroAutorizacion;
                        procedimientoRips.NumDocumentoIdentificacion = servicio.AdmisionesServiciosPrestados.Atenciones?.Empleados?.NumeroIdentificacion;
                        procedimientoRips.TipoDocumentoIdentificacion = servicio.AdmisionesServiciosPrestados.Atenciones.Empleados?.TiposIdentificacion?.Codigo;
                        procedimientoRips.ValorPagoModerador = Convert.ToInt32(servicio.Facturas.ValorTotal);
                        procedimientoRips.ViaIngresoServicioSalud = servicio.AdmisionesServiciosPrestados.Atenciones?.Admisiones?.ViaIngresoServicioSalud?.Codigo;
                        procedimientoRips.VrServicio = Convert.ToInt32(servicio.ValorServicio);
                        procedimientoRips.NumFEVPagoModerador = null;

                        if (servicio.AdmisionesServiciosPrestados.Atenciones.Admisiones.ValorPagoEstadosId == 58 || servicio.AdmisionesServiciosPrestados.Atenciones.Admisiones.ValorPagoEstadosId == 59)
                        {
                            var facturaCopagoCuotaModeradora = servicio.AdmisionesServiciosPrestados.Atenciones.Admisiones.FacturaCopagoCuotaModeradora;
                            procedimientoRips.NumFEVPagoModerador = $"{facturaCopagoCuotaModeradora.Documentos.Prefijo}{facturaCopagoCuotaModeradora.NroConsecutivo}";
                        }

                        consecutivoProcedimiento++;
                        usuarioRips.Servicios.Procedimientos.Add(procedimientoRips);
                    }

                }

                consecutivoUsuario++;
                ripsRootJson.Rips.Usuarios.Add(usuarioRips);
            }

            return JsonConvert.SerializeObject(ripsRootJson, Newtonsoft.Json.Formatting.Indented);
        }

        private string GetPdfFacturaReporte(Facturas factura, string nameFile, string user)
        {
            XtraReport xtraReport = null;
            if (factura.AdmisionesId != null)
            {
                xtraReport = ReportExtentions.Report<FacturasParticularReporte>(this.BusinessLogic, factura.Id, user);
            }
            else
            {
                xtraReport = ReportExtentions.Report<FacturasReporte>(this.BusinessLogic, factura.Id, user);
            }

            string pathPdf = Path.Combine(Path.GetTempPath(), $"fv{nameFile}.pdf");
            PdfExportOptions pdfOptions = new PdfExportOptions();
            pdfOptions.ConvertImagesToJpeg = false;
            pdfOptions.ImageQuality = PdfJpegImageQuality.Medium;
            pdfOptions.PdfACompatibility = PdfACompatibility.PdfA2b;
            xtraReport.ExportToPdf(pathPdf, pdfOptions);
            return pathPdf;
        }

        public async Task<bool> EnviarEmail(long facturaId, string eventoEnvio, string user, string host)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);

            var factura = unitOfWork.Repository<Facturas>().Table
                .Include(x => x.Pacientes)
                .Include(x => x.Entidades)
                .Include(x => x.Empresas)
                .FirstOrDefault(x => x.Id == facturaId);

            if (string.IsNullOrWhiteSpace(factura.CUFE) ||
                !factura.IssueDate.HasValue ||
                !string.Equals(factura.DIANResponse, DApp.Util.Dian.StatusCertified, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("La factura no ha sido aceptada por la DIAN.");
            }

            try
            {
                string correo = null;
                if (factura.AdmisionesId != null)
                    correo = unitOfWork.Repository<Pacientes>().GetTable().FirstOrDefault(x => x.Id == factura.PacientesId)?.CorreoElectronico;
                else
                    correo = unitOfWork.Repository<Entidades>().GetTable().FirstOrDefault(x => x.Id == factura.EntidadesId)?.CorreoElectronico;

                var xmlDian = await GetArchivoXmlDIAN(factura.Id, user, host);

                ZipArchive archive = new ZipArchive();
                archive.FileName = $"z{factura.ConsecutivoFE}.zip";
                archive.AddFile(GetPdfFacturaReporte(factura, factura.ConsecutivoFE, user), "/");
                archive.AddFile(xmlDian.PathFile, "/");
                MemoryStream msZip = new MemoryStream();
                archive.Save(msZip);
                msZip = new MemoryStream(msZip.ToArray());

                EmailModelConfig envioEmailConfig = new EmailModelConfig();
                envioEmailConfig.Origen = DApp.Util.EmailOrigen_Facturacion;
                envioEmailConfig.Asunto = $"{factura.Empresas.NumeroIdentificacion};{factura.Empresas.RazonSocial};{factura.Documentos.Prefijo}{factura.NroConsecutivo};01;{factura.Empresas.RazonSocial}";
                envioEmailConfig.MetodoUso = eventoEnvio;
                envioEmailConfig.Template = "EmailEnvioFacturaElectronica";
                envioEmailConfig.Destinatarios.Add(correo);
                envioEmailConfig.ArchivosAdjuntos.Add($"z{factura.ConsecutivoFE}.zip", msZip);
                envioEmailConfig.Datos = new Dictionary<string, string>
                {
                    {"nombreCia",$"{factura.Empresas.RazonSocial}" }
                };

                new ConfiguracionEnvioEmailBusinessLogic(this.UnitOfWork).EnviarEmail(envioEmailConfig);

                var job = unitOfWork.Repository<ConfiguracionEnvioEmailJob>().Table
                    .OrderBy(c => c.CreationDate)
                    .FirstOrDefault(x => x.Tipo == (int)TipoDocumento.Factura && x.IdTipo == factura.Id && !x.Exitoso);
                if (job != null)
                {
                    job.Ejecutado = true;
                    job.Exitoso = true;
                    job.LastUpdate = DateTime.Now;
                    job.UpdatedBy = user;
                    job.Intentos++;
                    job.Detalle += $"| Intento {job.Intentos}: {eventoEnvio} | ";
                    unitOfWork.Repository<ConfiguracionEnvioEmailJob>().Modify(job);
                }

                return true;
            }
            catch (Exception e)
            {
                DApp.LogToFile($"{e.GetFullErrorMessage()} | {e.StackTrace}");
                return false;
            }
        }

        public void DisminuirSaldo(long idFactura, decimal valor)
        {
            var factura = new Dominus.Backend.DataBase.BusinessLogic(this.UnitOfWork.Settings).GetBusinessLogic<Facturas>().FindById(x => x.Id == idFactura, false);
            factura.Saldo = factura.Saldo - valor;
            new Dominus.Backend.DataBase.BusinessLogic(this.UnitOfWork.Settings).GetBusinessLogic<Facturas>().Modify(factura);
        }

        public string FacturaIndividual(long admisionId, long empresaId, long userId)
        {
            BlazorUnitWork unitOfWork = new BlazorUnitWork(UnitOfWork.Settings);
            Admisiones admision = unitOfWork.Repository<Admisiones>().Table
                .Include(x => x.FormasPagos)
                .Include(x => x.FacturaCopagoCuotaModeradora)
                .Include(x => x.FacturaCopagoCuotaModeradora.Documentos)
                .Include(x => x.ProgramacionCitas)
                .FirstOrDefault(x => x.Id == admisionId);

            User usuario = unitOfWork.Repository<User>().FindById(x => x.Id == userId, false);
            CiclosCajas cicloCaja = unitOfWork.Repository<CiclosCajas>().FindById(x => x.OpenUsersId == userId && x.CloseUsersId == null, false);

            if (admision.Facturado)
                throw new Exception($"Esta admisión ya fue facturada. Factura {admision.FacturaCopagoCuotaModeradora?.Documentos?.Prefijo}-{admision.FacturaCopagoCuotaModeradora?.NroConsecutivo}");

            if (admision.FormasPagosId == null)
                throw new Exception($"La forma de pago es obligatoria.");
            if (admision.MedioPagosId == null)
                throw new Exception($"El medio de pago es obligatorio.");
            if (admision.DocumentosId == null)
                throw new Exception($"El documento es obligatorio.");

            if (admision.FormasPagos.Codigo == "1")
            {
                if (cicloCaja == null)
                {
                    throw new Exception($"No existe un ciclo de caja abierto para el usuario {usuario.NombreCompleto}.");
                }
            }

            if (admision.ValorPagoEstadosId == 10067)
            {
                return FacturaIndividualParticular(admision, empresaId, cicloCaja.Id, usuario.UserName, unitOfWork);
            }
            else if (admision.ValorPagoEstadosId == 58 || admision.ValorPagoEstadosId == 59 || admision.ValorPagoEstadosId == 68 | admision.ValorPagoEstadosId == 69)
            {
                return FacturaIndividualCopagoCuotaModeradora(admision, empresaId, cicloCaja.Id, usuario.UserName, unitOfWork);
            }
            else
            {
                throw new Exception($"Solo se permite facturar valores de COPAGO, CUOTA MODERADORA, CUOTA DE RECUPERACIÓN, PAGOS COMPARTIDOS o PARTICULAR.");
            }

        }

        private string FacturaIndividualCopagoCuotaModeradora(Admisiones admision, long empresaId, long cicloCajaId, string userName, BlazorUnitWork unitOfWork)
        {
            try
            {
                unitOfWork.BeginTransaction();

                if (admision.ValorCopago <= 0)
                {
                    throw new Exception($"El valor a facturar de COPAGO o CUOTA MODERADORA debe ser superiores a cero (0)");
                }

                Facturas factura = new Facturas();
                factura.EmpresasId = empresaId;
                factura.SedesId = admision.ProgramacionCitas.SedesId;
                factura.PacientesId = admision.PacientesId;
                factura.EntidadesId = null;
                factura.ConvenioId = null;
                factura.OrdenCompra = null;
                factura.ReferenciaFactura = null;
                factura.EsCopagoModeradora = true;
                factura.AdmisionesId = admision.Id;
                factura.Fecha = DateTime.Now;
                factura.DocumentosId = admision.DocumentosId.GetValueOrDefault(0);
                factura.ConsecutivoFE = new GenericBusinessLogic<Facturas>(unitOfWork).GetConsecutivoParaEnvioFE();

                var documento = unitOfWork.Repository<Documentos>().FindById(x => x.Id == factura.DocumentosId, false);
                long consecutivo = 0;
                try
                {
                    consecutivo = new GenericBusinessLogic<Documentos>(unitOfWork).GetSecuence($"{documento.Prefijo}");
                }
                catch (Exception e)
                {
                    throw new Exception($"Error obteniendo consecutivo para {factura.SedesId}-{documento.Prefijo}. | {e.Message}");
                }

                if (documento.ResolucionDian != 0)
                {
                    if (consecutivo < documento.ConsecutivoDesde || consecutivo > documento.ConsecutivoHasta)
                    {
                        throw new Exception($"El consecutivo superó la resolución DIAN del documento {documento.Prefijo}.");
                    }
                    if (DateTime.Now < documento.FechaDesde || DateTime.Now > documento.FechaHasta)
                    {
                        throw new Exception($"La fecha superó la resolución DIAN del documento {documento.Prefijo}.");
                    }
                }

                factura.FormasPagosId = admision.FormasPagosId.GetValueOrDefault(0);
                factura.Observaciones = admision.ObservacionFactura;
                factura.NroConsecutivo = consecutivo;
                factura.LastUpdate = DateTime.Now;
                factura.UpdatedBy = userName;
                factura.CreationDate = DateTime.Now;
                factura.CreatedBy = userName;

                factura.ValorCopagoCuotaModeradora = 0;
                factura.ValorSubtotal = admision.ValorCopago;
                factura.ValorDescuentos = 0;
                factura.ValorImpuestos = 0;
                factura.ValorTotal = admision.ValorCopago;
                factura.Saldo = 0;
                factura.FehaInicial = DateTime.Now;
                factura.FechaFinal = DateTime.Now;
                factura.MontoEscrito = DApp.Util.NumeroEnLetras(factura.ValorTotal);
                if (admision.FormasPagos.Codigo == "1")
                    factura.Estadosid = 16;
                else
                    factura.Estadosid = 14;

                factura.MediosPagoId = admision.MedioPagosId;

                if (factura.ValorTotal < 0)
                {
                    throw new Exception("Valor total a pagar es negativo. Por favor verificar saldos.");
                }

                var newFact = unitOfWork.Repository<Facturas>().Add(factura);

                //Detalle si es copago o cuota
                if (!admision.Facturado)
                {
                    FacturasDetalles facturasDetalles = new FacturasDetalles();
                    facturasDetalles.Id = 0;
                    facturasDetalles.LastUpdate = DateTime.Now;
                    facturasDetalles.UpdatedBy = userName;
                    facturasDetalles.CreationDate = DateTime.Now;
                    facturasDetalles.CreatedBy = userName;
                    facturasDetalles.FacturasId = newFact.Id;
                    facturasDetalles.NroLinea = 1;
                    facturasDetalles.Cantidad = 1;
                    facturasDetalles.AdmisionesServiciosPrestadosId = null;
                    facturasDetalles.PorcDescuento = 0;
                    facturasDetalles.PorcImpuesto = 0;
                    facturasDetalles.ValorServicio = admision.ValorCopago;
                    facturasDetalles.SubTotal = facturasDetalles.Cantidad * facturasDetalles.ValorServicio;
                    facturasDetalles.ValorTotal = facturasDetalles.ValorServicio;

                    if (admision.ValorPagoEstadosId == 58)
                    {
                        var serv = unitOfWork.Repository<Servicios>().FindById(x => x.Codigo.Equals("COP"), true);
                        if (serv == null)
                        {
                            throw new Exception($"No existe el servicio creado COPAGO con codigo COP.");
                        }
                        facturasDetalles.ServiciosId = serv.Id;
                    }
                    else if (admision.ValorPagoEstadosId == 59)
                    {
                        var serv = unitOfWork.Repository<Servicios>().FindById(x => x.Codigo.Equals("CM"), true);
                        if (serv == null)
                        {
                            throw new Exception($"No existe el servicio creado CUOTA MODERADORA con codigo CM.");
                        }
                        facturasDetalles.ServiciosId = serv.Id;
                    }
                    else if (admision.ValorPagoEstadosId == 68)
                    {
                        var serv = unitOfWork.Repository<Servicios>().FindById(x => x.Codigo.Equals("CR"), true);
                        if (serv == null)
                        {
                            throw new Exception($"No existe el servicio creado CUOTA DE RECUPERACIÓN con codigo CR.");
                        }
                        facturasDetalles.ServiciosId = serv.Id;
                    }
                    else if (admision.ValorPagoEstadosId == 69)
                    {
                        var serv = unitOfWork.Repository<Servicios>().FindById(x => x.Codigo.Equals("PC"), true);
                        if (serv == null)
                        {
                            throw new Exception($"No existe el servicio creado PAGOS COMPARTIDOS con codigo PC.");
                        }
                        facturasDetalles.ServiciosId = serv.Id;
                    }
                    else
                    {
                        throw new Exception($"Admisión sin estado de pago registrado en el sistema. Comuniquese con el administrador.");
                    }

                    unitOfWork.Repository<FacturasDetalles>().Add(facturasDetalles);
                }

                if (admision.FormasPagos.Codigo == "1")
                {
                    Recaudos recaudos = new Recaudos();
                    recaudos.Id = 0;
                    try
                    {
                        recaudos.Consecutivo = unitOfWork.Repository<Recaudos>().Table.Max(x => x.Consecutivo) + 1;
                    }
                    catch
                    {
                        recaudos.Consecutivo = 1;
                    }
                    recaudos.IsNew = true;
                    recaudos.LastUpdate = DateTime.Now;
                    recaudos.UpdatedBy = userName;
                    recaudos.CreationDate = DateTime.Now;
                    recaudos.CreatedBy = userName;

                    recaudos.FechaRecaudo = DateTime.Now;
                    recaudos.CiclosCajasId = cicloCajaId;
                    recaudos.MediosPagoId = admision.MedioPagosId.GetValueOrDefault(0);
                    recaudos.EmpresasId = empresaId;
                    recaudos.PacientesId = factura.PacientesId;
                    recaudos.EntidadesId = factura.EntidadesId;
                    recaudos.SedesId = factura.SedesId;
                    recaudos.ValorTotalRecibido = factura.ValorTotal;
                    recaudos = unitOfWork.Repository<Recaudos>().Add(recaudos);

                    RecaudosDetalles recaudosDetalles = new RecaudosDetalles();
                    recaudosDetalles.Id = 0;
                    recaudosDetalles.IsNew = true;
                    recaudosDetalles.LastUpdate = DateTime.Now;
                    recaudosDetalles.UpdatedBy = userName;
                    recaudosDetalles.CreationDate = DateTime.Now;
                    recaudosDetalles.CreatedBy = userName;

                    recaudosDetalles.RecaudosId = recaudos.Id;
                    recaudosDetalles.ValorAplicado = factura.ValorTotal;
                    recaudosDetalles.ValorReteIca = 0;
                    recaudosDetalles.ValorRetencion = 0;
                    recaudosDetalles.FacturasId = factura.Id;
                    recaudosDetalles = unitOfWork.Repository<RecaudosDetalles>().Add(recaudosDetalles);
                }

                admision.Facturado = true;
                admision.FacturaCopagoCuotaModeradoraId = newFact.Id;
                admision = unitOfWork.Repository<Admisiones>().Modify(admision);

                unitOfWork.CommitTransaction();
                return $"{documento.Prefijo}-{newFact.NroConsecutivo}";
            }
            catch (Exception e)
            {
                unitOfWork.RollbackTransaction();
                throw (e);
            }
        }

        private string FacturaIndividualParticular(Admisiones admision, long empresaId, long cicloCajaId, string userName, BlazorUnitWork unitOfWork)
        {
            try
            {
                unitOfWork.BeginTransaction();

                var serviciosAFacturar = unitOfWork.Repository<ServiciosFacturar>().Table.Where(x => x.AdmisionesId == admision.Id).ToList();
                if (serviciosAFacturar == null)
                {
                    throw new Exception($"No existen servicios a facturar en esa admisión.");
                }

                if (serviciosAFacturar.Exists(x => x.AdmisionFacturada))
                {
                    throw new Exception($"Esta admisión ya fue facturada.");
                }

                Facturas factura = new Facturas();
                factura.EmpresasId = empresaId;
                factura.SedesId = admision.ProgramacionCitas.SedesId;
                factura.PacientesId = admision.PacientesId;
                factura.EntidadesId = null;
                factura.ConvenioId = null;
                factura.OrdenCompra = null;
                factura.ReferenciaFactura = null;
                factura.EsCopagoModeradora = false;
                factura.AdmisionesId = admision.Id;
                factura.Fecha = DateTime.Now;
                factura.DocumentosId = admision.DocumentosId.GetValueOrDefault(0);
                factura.ConsecutivoFE = new GenericBusinessLogic<Facturas>(unitOfWork).GetConsecutivoParaEnvioFE();

                var documento = unitOfWork.Repository<Documentos>().FindById(x => x.Id == factura.DocumentosId, false);
                long consecutivo = 0;
                try
                {
                    consecutivo = new GenericBusinessLogic<Documentos>(unitOfWork).GetSecuence($"{documento.Prefijo}");
                }
                catch (Exception e)
                {
                    throw new Exception($"Error obteniendo consecutivo para {factura.SedesId}-{documento.Prefijo}. | {e.Message}");
                }

                if (documento.ResolucionDian != 0)
                {
                    if (consecutivo < documento.ConsecutivoDesde || consecutivo > documento.ConsecutivoHasta)
                    {
                        throw new Exception($"El consecutivo superó la resolución DIAN del documento {documento.Prefijo}.");
                    }
                    if (DateTime.Now < documento.FechaDesde || DateTime.Now > documento.FechaHasta)
                    {
                        throw new Exception($"La fecha superó la resolución DIAN del documento {documento.Prefijo}.");
                    }
                }

                factura.FormasPagosId = admision.FormasPagosId.GetValueOrDefault(0);
                factura.Observaciones = admision.ObservacionFactura;
                factura.NroConsecutivo = consecutivo;
                factura.LastUpdate = DateTime.Now;
                factura.UpdatedBy = userName;
                factura.CreationDate = DateTime.Now;
                factura.CreatedBy = userName;
                factura.ServiciosAfacturar.AddRange(serviciosAFacturar);

                factura.ValorCopagoCuotaModeradora = 0;
                factura.ValorSubtotal = factura.ServiciosAfacturar.Sum(x => x.SubTotal);
                factura.ValorDescuentos = factura.ServiciosAfacturar.Sum(x => x.ValorDescuento);
                factura.ValorImpuestos = factura.ServiciosAfacturar.Sum(x => x.ValorImpuesto);
                factura.ValorTotal = ((factura.ValorSubtotal - factura.ValorDescuentos) + factura.ValorImpuestos) - factura.ValorCopagoCuotaModeradora;
                factura.Saldo = 0;
                factura.FehaInicial = DateTime.Now;
                factura.FechaFinal = DateTime.Now;
                factura.MontoEscrito = DApp.Util.NumeroEnLetras(factura.ValorTotal);

                if (admision.FormasPagos.Codigo == "1")
                    factura.Estadosid = 16;
                else
                    factura.Estadosid = 14;

                factura.MediosPagoId = admision.MedioPagosId;
                var newFact = unitOfWork.Repository<Facturas>().Add(factura);

                List<long> idsServ = factura.ServiciosAfacturar.Select(x => x.Id).ToList();
                factura.ServicioFacturados = unitOfWork.Repository<AdmisionesServiciosPrestados>().FindAll(x => idsServ.Contains(x.Id), false);
                if (factura.ServicioFacturados != null && factura.ServicioFacturados.Count > 0)
                {
                    for (int i = 0; i < factura.ServicioFacturados.Count; i++)
                    {
                        if (!admision.Facturado)
                        {
                            FacturasDetalles facturasDetalles = new FacturasDetalles();
                            facturasDetalles.Id = 0;
                            facturasDetalles.LastUpdate = DateTime.Now;
                            facturasDetalles.UpdatedBy = userName;
                            facturasDetalles.CreationDate = DateTime.Now;
                            facturasDetalles.CreatedBy = userName;
                            facturasDetalles.FacturasId = newFact.Id;
                            facturasDetalles.NroLinea = i + 1;
                            facturasDetalles.AdmisionesServiciosPrestadosId = factura.ServicioFacturados[i].Id;
                            facturasDetalles.ServiciosId = factura.ServicioFacturados[i].ServiciosId;
                            facturasDetalles.Cantidad = factura.ServicioFacturados[i].Cantidad;
                            facturasDetalles.PorcDescuento = admision.PorcDescAutorizado;
                            facturasDetalles.PorcImpuesto = 0;
                            facturasDetalles.ValorServicio = factura.ServicioFacturados[i].ValorServicio;
                            facturasDetalles.SubTotal = facturasDetalles.Cantidad * facturasDetalles.ValorServicio;
                            facturasDetalles.ValorTotal = facturasDetalles.SubTotal - (factura.ServicioFacturados[i].ValorServicio * (facturasDetalles.PorcDescuento / 100));
                            unitOfWork.Repository<FacturasDetalles>().Add(facturasDetalles);
                        }
                        factura.ServicioFacturados[i].FacturasGeneracionId = null;
                        factura.ServicioFacturados[i].FacturasId = newFact.Id;
                        factura.ServicioFacturados[i].Facturado = true;
                        unitOfWork.Repository<AdmisionesServiciosPrestados>().Modify(factura.ServicioFacturados[i]);
                    }
                }

                if (admision.FormasPagos.Codigo == "1")
                {
                    Recaudos recaudos = new Recaudos();
                    recaudos.Id = 0;
                    try
                    {
                        recaudos.Consecutivo = unitOfWork.Repository<Recaudos>().Table.Max(x => x.Consecutivo) + 1;
                    }
                    catch
                    {
                        recaudos.Consecutivo = 1;
                    }
                    recaudos.IsNew = true;
                    recaudos.LastUpdate = DateTime.Now;
                    recaudos.UpdatedBy = userName;
                    recaudos.CreationDate = DateTime.Now;
                    recaudos.CreatedBy = userName;

                    recaudos.FechaRecaudo = DateTime.Now;
                    recaudos.CiclosCajasId = cicloCajaId;
                    recaudos.MediosPagoId = admision.MedioPagosId.GetValueOrDefault(0);
                    recaudos.EmpresasId = empresaId;
                    recaudos.PacientesId = factura.PacientesId;
                    recaudos.EntidadesId = factura.EntidadesId;
                    recaudos.SedesId = factura.SedesId;
                    recaudos.ValorTotalRecibido = factura.ValorTotal;

                    if (factura.ValorTotal < 0)
                    {
                        throw new Exception("Valor total a pagar es negativo. Por favor verificar saldos.");
                    }

                    recaudos = unitOfWork.Repository<Recaudos>().Add(recaudos);

                    RecaudosDetalles recaudosDetalles = new RecaudosDetalles();
                    recaudosDetalles.Id = 0;
                    recaudosDetalles.IsNew = true;
                    recaudosDetalles.LastUpdate = DateTime.Now;
                    recaudosDetalles.UpdatedBy = userName;
                    recaudosDetalles.CreationDate = DateTime.Now;
                    recaudosDetalles.CreatedBy = userName;

                    recaudosDetalles.RecaudosId = recaudos.Id;
                    recaudosDetalles.ValorAplicado = factura.ValorTotal;
                    recaudosDetalles.ValorReteIca = 0;
                    recaudosDetalles.ValorRetencion = 0;
                    recaudosDetalles.FacturasId = factura.Id;
                    recaudosDetalles = unitOfWork.Repository<RecaudosDetalles>().Add(recaudosDetalles);
                }

                admision.Facturado = true;
                admision.FacturaCopagoCuotaModeradoraId = newFact.Id;
                admision = unitOfWork.Repository<Admisiones>().Modify(admision);

                unitOfWork.CommitTransaction();
                return $"{documento.Prefijo}-{newFact.NroConsecutivo}";
            }
            catch (Exception e)
            {
                unitOfWork.RollbackTransaction();
                throw (e);
            }
        }

        public string GenerateFileToCobol(List<long> ids)
        {
            string path = Path.Combine(Path.GetTempPath(), "FilesToCobol");
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);

            List<VContabilizacionRegistro> documentos = new GenericBusinessLogic<VContabilizacionRegistro>(this.UnitOfWork).FindAll(x => ids.Contains(x.Id)).OrderBy(x => x.Id).ToList();
            if (documentos.Count <= 0)
                throw new Exception("No existen datos a generar con las facturas seleccionadas.");

            //List<IGrouping<string, VContabilizacionRegistro>> result = documentos.GroupBy(x => x.Documento).ToList();
            //int i = 0;
            //foreach (var registro in result)
            //{
            //    string nombreArchivo = $"FIBATCH.{i.ToString("D3")}";
            //    List<string> data = registro.Select(x => Regex.Replace(x.Registro.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "") ).ToList();
            //    File.WriteAllLines(Path.Combine(path, nombreArchivo), data);
            //    i++;
            //}
            string nombreArchivo = $"FIBATCH.{documentos.Count.ToString("D3")}";
            List<string> data = new List<string>();
            //foreach (var item in documentos)
            //{
            //    data.Add(item.Registro);
            //}
            for (int i = 0; i < documentos.Count; i++)
            {
                var datareg = (i + 1).ToString("D9") + documentos[i].Registro.Substring(9);
                data.Add(DApp.Util.QutarTildes(datareg));
            }
            File.WriteAllLines(Path.Combine(path, nombreArchivo), data);

            ZipArchive archive = new ZipArchive();
            archive.AddDirectory(path, "/");
            string pathZip = Path.GetTempFileName();
            archive.Save(pathZip);
            return pathZip;
        }

        public byte[] ExcelInformeCartera(long entidadId, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.CreateNewDocument();
                Worksheet worksheet = workbook.Worksheets.ActiveWorksheet as Worksheet;
                worksheet.Name = "Informe de cartera por entidad";
                List<VReporteCartera> data = new GenericBusinessLogic<VReporteCartera>(this.UnitOfWork).Tabla()
                    .Where(x => x.EntidadId == entidadId && x.Fecha_Emision >= fechaDesde && x.Fecha_Emision <= fechaHasta)
                    .OrderBy(x => x.Fecha_Emision).ToList();


                //Titulos
                int column = 0;
                worksheet.Rows[0][column].SetValue("RAZON SOCIAL ENTIDAD"); column++;
                worksheet.Rows[0][column].SetValue("TIPO DOCUMENTO"); column++;
                worksheet.Rows[0][column].SetValue("NUMERO DE IDENTIFICACION"); column++;
                worksheet.Rows[0][column].SetValue("PREFIJO"); column++;
                worksheet.Rows[0][column].SetValue("CONSECUTIVO"); column++;
                worksheet.Rows[0][column].SetValue("FECHA EMISION"); column++;
                worksheet.Rows[0][column].SetValue("FECHA RADICACION"); column++;
                worksheet.Rows[0][column].SetValue("PLAZO"); column++;
                worksheet.Rows[0][column].SetValue("FECHA VENCIMIENTO"); column++;
                worksheet.Rows[0][column].SetValue("DIAS MORA"); column++;
                worksheet.Rows[0][column].SetValue("SUBTOTAL FACTURA"); column++;
                worksheet.Rows[0][column].SetValue("COPAGOS Y CUOTAS MODERADORAS"); column++;
                worksheet.Rows[0][column].SetValue("TOTAL FACTURA"); column++;
                worksheet.Rows[0][column].SetValue("PORCENTAJE RETEFUENTE"); column++;
                worksheet.Rows[0][column].SetValue("VALOR RETEFUENTE"); column++;
                worksheet.Rows[0][column].SetValue("PORCENTAJE RETEICA"); column++;
                worksheet.Rows[0][column].SetValue("VALOR RETEICA"); column++;
                worksheet.Rows[0][column].SetValue("VALOR GLOSA"); column++;
                worksheet.Rows[0][column].SetValue("VALOR ACEPTADO GLOSA"); column++;
                worksheet.Rows[0][column].SetValue("SALDO POR COBRAR (Aplicados todos los recaudos)"); column++;
                worksheet.Rows[0][column].SetValue("PAGOS RECIBIDOS"); column++;

                for (int row = 0; row < data.Count; row++)
                {
                    column = 0;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Razon_Social_Entidad); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Tipo_Documento); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].NumeroIdentificacion); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Prefijo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Consecutivo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Fecha_Emision); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Fecha_Radicacion); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Plazo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].FechaVencimiento); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].DiasMora); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Subtotal_Factura); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Copagos_CuotasModeradoras); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Total_Factura); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Porcentaje_ReteFuente); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Retefuente); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Porcentaje_ReteICA); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_ReteICA); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Glosa); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Aceptado_Glosa); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Saldo_Por_Cobrar); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Pagos_Recibidos); column++;
                }
                worksheet.Columns.AutoFit(0, column);

                byte[] book = workbook.SaveDocument(DocumentFormat.Xlsx);
                workbook.Dispose();
                return book;
            }
            catch
            {
                throw;
            }
        }

        public byte[] ExcelInformeGeneralCartera(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.CreateNewDocument();
                Worksheet worksheet = workbook.Worksheets.ActiveWorksheet as Worksheet;
                worksheet.Name = "Informe de cartera general";
                List<VReporteCartera> data = new GenericBusinessLogic<VReporteCartera>(this.UnitOfWork).Tabla()
                    .Where(x => x.Fecha_Emision >= fechaDesde && x.Fecha_Emision <= fechaHasta)
                    .OrderBy(x => x.Fecha_Emision).ToList();


                //Titulos
                int column = 0;
                worksheet.Rows[0][column].SetValue("RAZON SOCIAL ENTIDAD"); column++;
                worksheet.Rows[0][column].SetValue("TIPO DOCUMENTO"); column++;
                worksheet.Rows[0][column].SetValue("NUMERO DE IDENTIFICACION"); column++;
                worksheet.Rows[0][column].SetValue("PREFIJO"); column++;
                worksheet.Rows[0][column].SetValue("CONSECUTIVO"); column++;
                worksheet.Rows[0][column].SetValue("FECHA EMISION"); column++;
                worksheet.Rows[0][column].SetValue("FECHA RADICACION"); column++;
                worksheet.Rows[0][column].SetValue("PLAZO"); column++;
                worksheet.Rows[0][column].SetValue("FECHA VENCIMIENTO"); column++;
                worksheet.Rows[0][column].SetValue("DIAS MORA"); column++;
                worksheet.Rows[0][column].SetValue("SUBTOTAL FACTURA"); column++;
                worksheet.Rows[0][column].SetValue("COPAGOS Y CUOTAS MODERADORAS"); column++;
                worksheet.Rows[0][column].SetValue("TOTAL FACTURA"); column++;
                worksheet.Rows[0][column].SetValue("PORCENTAJE RETEFUENTE"); column++;
                worksheet.Rows[0][column].SetValue("VALOR RETEFUENTE"); column++;
                worksheet.Rows[0][column].SetValue("PORCENTAJE RETEICA"); column++;
                worksheet.Rows[0][column].SetValue("VALOR RETEICA"); column++;
                worksheet.Rows[0][column].SetValue("VALOR GLOSA"); column++;
                worksheet.Rows[0][column].SetValue("VALOR ACEPTADO GLOSA"); column++;
                worksheet.Rows[0][column].SetValue("SALDO POR COBRAR (Aplicados todos los recaudos)"); column++;
                worksheet.Rows[0][column].SetValue("PAGOS RECIBIDOS"); column++;

                for (int row = 0; row < data.Count; row++)
                {
                    column = 0;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Razon_Social_Entidad); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Tipo_Documento); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].NumeroIdentificacion); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Prefijo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Consecutivo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Fecha_Emision); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Fecha_Radicacion); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Plazo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].FechaVencimiento); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].DiasMora); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Subtotal_Factura); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Copagos_CuotasModeradoras); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Total_Factura); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Porcentaje_ReteFuente); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Retefuente); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Porcentaje_ReteICA); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_ReteICA); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Glosa); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Aceptado_Glosa); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Saldo_Por_Cobrar); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Pagos_Recibidos); column++;
                }
                worksheet.Columns.AutoFit(0, column);

                byte[] book = workbook.SaveDocument(DocumentFormat.Xlsx);
                workbook.Dispose();
                return book;
            }
            catch
            {
                throw;
            }
        }

        public byte[] ExcelExportarSiigo(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.CreateNewDocument();
                Worksheet worksheet = workbook.Worksheets.ActiveWorksheet as Worksheet;
                worksheet.Name = "Exportar SIIGO";
                List<VExportarSiigo> data = new GenericBusinessLogic<VExportarSiigo>(this.UnitOfWork).Tabla()
                    .Where(x => x.Fecha_Emision >= fechaDesde && x.Fecha_Emision <= fechaHasta)
                    .OrderBy(x => x.Fecha_Emision).ToList();


                //Titulos
                int column = 0;
                worksheet.Rows[0][column].SetValue("Tipo de comprobante"); column++;
                worksheet.Rows[0][column].SetValue("Consecutivo"); column++;
                worksheet.Rows[0][column].SetValue("Tipo identificación"); column++;
                worksheet.Rows[0][column].SetValue("Numero Identificación"); column++;
                worksheet.Rows[0][column].SetValue("Razón social (Obligatorio)"); column++;
                worksheet.Rows[0][column].SetValue("Nombres del tercero (Obligatorio)"); column++;
                worksheet.Rows[0][column].SetValue("Apellidos del tercero (Obligatorio)"); column++;
                worksheet.Rows[0][column].SetValue("Nit Entidad copago"); column++;
                worksheet.Rows[0][column].SetValue("Fecha de elaboración"); column++;
                worksheet.Rows[0][column].SetValue("Email Contacto"); column++;
                worksheet.Rows[0][column].SetValue("Código producto"); column++;
                worksheet.Rows[0][column].SetValue("Descripción producto"); column++;
                worksheet.Rows[0][column].SetValue("Cantidad producto"); column++;
                worksheet.Rows[0][column].SetValue("Valor unitario"); column++;
                worksheet.Rows[0][column].SetValue("Valor Descuento"); column++;
                worksheet.Rows[0][column].SetValue("Valor Copago"); column++;
                worksheet.Rows[0][column].SetValue("Valor Forma de Pago"); column++;
                worksheet.Rows[0][column].SetValue("Fecha Vencimiento"); column++;
                worksheet.Rows[0][column].SetValue("Observaciones"); column++;

                for (int row = 0; row < data.Count; row++)
                {
                    column = 0;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Prefijo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Consecutivo); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Tipo_Documento); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Numero_Identificacion); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Razon_Social); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Nombres_Tercero); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Apellidos_Tercero); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Nit_Entidad_Copago); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Fecha_Emision); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Email_Contacto); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Codigo_Producto); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Descripcion_Producto); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Cantidad_Producto); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Unitario); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Descuento); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Valor_Copago); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Subtotal_Factura); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].FechaVencimiento); column++;
                    worksheet.Rows[row + 1][column].SetValue(data[row].Observaciones); column++;
                }
                worksheet.Columns.AutoFit(0, column);

                byte[] book = workbook.SaveDocument(DocumentFormat.Xlsx);
                workbook.Dispose();
                return book;
            }
            catch
            {
                throw;
            }
        }

    }
}