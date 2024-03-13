using Newtonsoft.Json;
using System.Collections.Generic;

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

public class ServiciosRips
{
    [JsonProperty("procedimientos")]
    public List<ProcedimientoRips> Procedimientos { get; set; } = new List<ProcedimientoRips>();
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