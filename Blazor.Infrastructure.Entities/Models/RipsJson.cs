using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace Blazor.Infrastructure.Entities.Models;

public class RipsRootJson
{
    [JsonProperty("rips")]
    public RipsJson Rips { get; set; } = new RipsJson();

    [JsonProperty("xmlFevFile")]
    public string XmlFevFile { get; set; }
}

public class RipsJson
{
    [JsonProperty("numDocumentoIdObligado")]
    public string NumDocumentoIdObligado { get; set; }

    [JsonProperty("numFactura")]
    public string NumFactura { get; set; }

    [JsonProperty("numNota")]
    public string NumNota { get; set; }

    [JsonProperty("tipoNota")]
    public string TipoNota { get; set; }

    [JsonProperty("usuarios")]
    public List<UsuarioRips> Usuarios { get; set; } = new List<UsuarioRips>();
}

public class ProcedimientoRips
{
    [JsonProperty("codComplicacion")]
    public object CodComplicacion { get; set; }

    [JsonProperty("codDiagnosticoPrincipal")]
    public string CodDiagnosticoPrincipal { get; set; }

    [JsonProperty("codDiagnosticoRelacionado")]
    public object CodDiagnosticoRelacionado { get; set; }

    [JsonProperty("codPrestador")]
    public string CodPrestador { get; set; }

    [JsonProperty("codProcedimiento")]
    public string CodProcedimiento { get; set; }

    [JsonProperty("codServicio")]
    public int? CodServicio { get; set; }

    [JsonProperty("tipoPagoModerador")]
    public string TipoPagoModerador { get; set; }

    [JsonProperty("fechaInicioAtencion")]
    public string FechaInicioAtencion { get; set; }

    [JsonProperty("finalidadTecnologiaSalud")]
    public string FinalidadTecnologiaSalud { get; set; }

    [JsonProperty("grupoServicios")]
    public string GrupoServicios { get; set; }

    [JsonProperty("idMIPRES")]
    public string IdMIPRES { get; set; }

    [JsonProperty("modalidadGrupoServicioTecSal")]
    public string ModalidadGrupoServicioTecSal { get; set; }

    [JsonProperty("numAutorizacion")]
    public string NumAutorizacion { get; set; }

    [JsonProperty("numDocumentoIdentificacion")]
    public string NumDocumentoIdentificacion { get; set; }

    [JsonProperty("numFEVPagoModerador")]
    public string NumFEVPagoModerador { get; set; }

    [JsonProperty("tipoDocumentoIdentificacion")]
    public string TipoDocumentoIdentificacion { get; set; }

    [JsonProperty("valorPagoModerador")]
    public int ValorPagoModerador { get; set; }

    [JsonProperty("viaIngresoServicioSalud")]
    public string ViaIngresoServicioSalud { get; set; }

    [JsonProperty("vrServicio")]
    public int VrServicio { get; set; }

    [JsonProperty("consecutivo")]
    public int Consecutivo { get; set; }
}

public class ConsultaRips
{
    [JsonProperty("codPrestador")]
    public string CodPrestador { get; set; }

    [JsonProperty("fechaInicioAtencion")]
    public string FechaInicioAtencion { get; set; }

    [JsonProperty("numAutorizacion")]
    public string NumAutorizacion { get; set; }

    [JsonProperty("codConsulta")]
    public string CodConsulta { get; set; }

    [JsonProperty("modalidadGrupoServicioTecSal")]
    public string ModalidadGrupoServicioTecSal { get; set; }

    [JsonProperty("grupoServicios")]
    public string GrupoServicios { get; set; }

    [JsonProperty("codServicio")]
    public int? CodServicio { get; set; }

    [JsonProperty("finalidadTecnologiaSalud")]
    public string FinalidadTecnologiaSalud { get; set; }

    [JsonProperty("causaMotivoAtencion")]
    public string CausaMotivoAtencion { get; set; }

    [JsonProperty("codDiagnosticoPrincipal")]
    public string CodDiagnosticoPrincipal { get; set; }

    [JsonProperty("codDiagnosticoRelacionado1")]
    public string CodDiagnosticoRelacionado1 { get; set; }

    [JsonProperty("codDiagnosticoRelacionado2")]
    public string CodDiagnosticoRelacionado2 { get; set; }

    [JsonProperty("codDiagnosticoRelacionado3")]
    public string CodDiagnosticoRelacionado3 { get; set; }

    [JsonProperty("tipoDiagnosticoPrincipal")]
    public string TipoDiagnosticoPrincipal { get; set; }

    [JsonProperty("tipoDocumentoIdentificacion")]
    public string TipoDocumentoIdentificacion { get; set; }

    [JsonProperty("numDocumentoIdentificacion")]
    public string NumDocumentoIdentificacion { get; set; }

    [JsonProperty("vrServicio")]
    public long VrServicio { get; set; }

    [JsonProperty("tipoPagoModerador")]
    public string TipoPagoModerador { get; set; }

    [JsonProperty("valorPagoModerador")]
    public long ValorPagoModerador { get; set; }

    [JsonProperty("numFEVPagoModerador")]
    public string NumFEVPagoModerador { get; set; }

    [JsonProperty("consecutivo")]
    public int Consecutivo { get; set; }
}

public class ServiciosRips
{
    [JsonProperty("procedimientos")]
    public List<ProcedimientoRips> Procedimientos { get; set; } = new List<ProcedimientoRips>();

    [JsonProperty("consultas")]
    public List<ConsultaRips> Consultas { get; set; }
}

public class UsuarioRips
{
    [JsonProperty("tipoDocumentoIdentificacion")]
    public string TipoDocumentoIdentificacion { get; set; }

    [JsonProperty("numDocumentoIdentificacion")]
    public string NumDocumentoIdentificacion { get; set; }

    [JsonProperty("tipoUsuario")]
    public string TipoUsuario { get; set; }

    [JsonProperty("fechaNacimiento")]
    public string FechaNacimiento { get; set; }

    [JsonProperty("codSexo")]
    public string CodSexo { get; set; }

    [JsonProperty("codPaisResidencia")]
    public string CodPaisResidencia { get; set; }

    [JsonProperty("codPaisOrigen")]
    public string CodPaisOrigen { get; set; }

    [JsonProperty("codMunicipioResidencia")]
    public string CodMunicipioResidencia { get; set; }

    [JsonProperty("codZonaTerritorialResidencia")]
    public string CodZonaTerritorialResidencia { get; set; }

    [JsonProperty("incapacidad")]
    public string Incapacidad { get; set; }

    [JsonProperty("consecutivo")]
    public int Consecutivo { get; set; }

    [JsonProperty("servicios")]
    public ServiciosRips Servicios { get; set; } = new ServiciosRips();
}

public class RespuestaIntegracionRips
{
    [JsonProperty("resultState")]
    public bool ResultState { get; set; }

    [JsonProperty("procesoId")]
    public int ProcesoId { get; set; }

    [JsonProperty("numFactura")]
    public string NumFactura { get; set; }

    [JsonProperty("codigoUnicoValidacion")]
    public string CodigoUnicoValidacion { get; set; }

    [JsonProperty("codigoUnicoValidacionToShow")]
    public string CodigoUnicoValidacionToShow { get; set; }

    [JsonProperty("fechaRadicacion")]
    public DateTime FechaRadicacion { get; set; }

    [JsonProperty("rutaArchivos")]
    public object RutaArchivos { get; set; }

    [JsonProperty("resultadosValidacion")]
    public List<ResultadosValidacionRips> ResultadosValidacion { get; set; }
}

public class ResultadosValidacionRips
{
    [JsonProperty("clase")]
    public string Clase { get; set; }

    [JsonProperty("codigo")]
    public string Codigo { get; set; }

    [JsonProperty("descripcion")]
    public string Descripcion { get; set; }

    [JsonProperty("observaciones")]
    public string Observaciones { get; set; }

    [JsonProperty("pathFuente")]
    public string PathFuente { get; set; }

    [JsonProperty("fuente")]
    public string Fuente { get; set; }
}

public class LoginIntegracionRips
{
    [JsonProperty("persona")]
    public LoginPersonaRips Persona { get; set; } = new LoginPersonaRips();

    [JsonProperty("clave")]
    public string Clave { get; set; }

    [JsonProperty("nit")]
    public string Nit { get; set; }

    [JsonProperty("codigoPrestador")]
    public string CodigoPrestador { get; set; }

    [JsonProperty("nomPrestador")]
    public string NomPrestador { get; set; }

    [JsonProperty("tipoMecanismoValidacion")]
    public int TipoMecanismoValidacion { get; set; }

    [JsonProperty("token")]
    public string Token { get; set; }

    [JsonProperty("usuarioSispro")]
    public string UsuarioSispro { get; set; }

    [JsonProperty("reps")]
    public bool Reps { get; set; }
}

public class LoginIdentificacionRips
{
    [JsonProperty("tipo")]
    public string Tipo { get; set; }

    [JsonProperty("numero")]
    public string Numero { get; set; }
}

public class LoginPersonaRips
{
    [JsonProperty("identificacion")]
    public LoginIdentificacionRips Identificacion { get; set; } = new LoginIdentificacionRips();
}

public class RespuestaLoginRips
{
    [JsonProperty("token")]
    public string Token { get; set; }

    [JsonProperty("login")]
    public bool Login { get; set; }

    [JsonProperty("registrado")]
    public bool Registrado { get; set; }

    [JsonProperty("errors")]
    public object Errors { get; set; }
}