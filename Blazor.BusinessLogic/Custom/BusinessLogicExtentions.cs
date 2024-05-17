using Blazor.Infrastructure.Entities;
using Dominus.Backend.Application;
using Dominus.Backend.Data;
using Dominus.Backend.DataBase;
using System.Collections.Generic;

namespace Blazor.BusinessLogic
{
    public static class BusinessLogicExtentions
    {
        public static GenericBusinessLogic<T> GetBusinessLogic<T>(this Dominus.Backend.DataBase.BusinessLogic logic) where T : BaseEntity
        {
            if (typeof(T) == typeof(User))
                return new UserBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(Empleados))
                return new EmpleadosBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(ListaPrecios))
                return new ListaPreciosLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(LiquidacionHonorarios))
                return new LiquidacionHonorariosLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(Empresas))
                return new EmpresasBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(RadicacionCuentas))
                return new RadicacionCuentasBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(Admisiones))
                return new AdmisionesBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(Atenciones))
                return new AtencionesBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(ProgramacionAgenda))
                return new ProgramacionAgendaBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(ProgramacionCitas))
                return new ProgramacionCitasBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(Archivos))
                return new ArchivosBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(HistoriasClinicas))
                return new HistoriasClinicasBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(EntregaResultados))
                return new EntregaResultadosBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(EntregaResultadosNoLectura))
                return new EntregaResultadosNoLecturaBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(AdmisionesServiciosPrestados))
                return new AdmisionesServiciosPrestadosBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(Notas))
                return new NotasBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(NotasDetalles))
                return new NotasDetallesBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            if (typeof(T) == typeof(Rips))
                return new RipsBusinessLogic(logic.settings) as GenericBusinessLogic<T>;
            return new GenericBusinessLogic<T>(logic.settings);

        }

        public static UserBusinessLogic UserBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new UserBusinessLogic(logic.settings);
        }
        public static EmpleadosBusinessLogic EmpleadosBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new EmpleadosBusinessLogic(logic.settings);
        }
        public static PacientesBusinessLogic PacientesBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new PacientesBusinessLogic(logic.settings);
        }
        public static ListaPreciosLogic ListaPreciosLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new ListaPreciosLogic(logic.settings);
        }
        public static LiquidacionHonorariosLogic LiquidacionHonorariosLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new LiquidacionHonorariosLogic(logic.settings);
        }
        public static FacturasBusinessLogic FacturasBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new FacturasBusinessLogic(logic.settings);
        }
        public static EmpresasBusinessLogic EmpresasBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new EmpresasBusinessLogic(logic.settings);
        }
        public static AdmisionesBusinessLogic AdmisionesBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new AdmisionesBusinessLogic(logic.settings);
        }
        public static AtencionesBusinessLogic AtencionesBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new AtencionesBusinessLogic(logic.settings);
        }
        public static PreciosServiciosLogic PreciosServiciosLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new PreciosServiciosLogic(logic.settings);
        }

        public static ProgramacionAgendaBusinessLogic ProgramacionAgendaBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new ProgramacionAgendaBusinessLogic(logic.settings);
        }

        public static ProgramacionCitasBusinessLogic ProgramacionCitasBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new ProgramacionCitasBusinessLogic(logic.settings);
        }

        public static ImagenesDiagnosticasDetalleBusinessLogic ImagenesDiagnosticasDetalleBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new ImagenesDiagnosticasDetalleBusinessLogic(logic.settings);
        }

        public static AtencionesResultadoBusinessLogic AtencionesResultadoBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new AtencionesResultadoBusinessLogic(logic.settings);
        }

        public static BancosBusinessLogic BancosBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new BancosBusinessLogic(logic.settings);
        }

        public static RipsBusinessLogic RipsBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new RipsBusinessLogic(logic.settings);
        }

        public static ConfiguracionEnvioEmailBusinessLogic ConfiguracionEnvioEmailBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new ConfiguracionEnvioEmailBusinessLogic(logic.settings);
        }

        public static FacturasGeneracionBusinessLogic FacturasGeneracionBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new FacturasGeneracionBusinessLogic(logic.settings);
        }

        public static NotasBusinessLogic NotasBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new NotasBusinessLogic(logic.settings);
        }

        public static ConveniosServiciosBusinessLogic ConveniosServiciosBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new ConveniosServiciosBusinessLogic(logic.settings);
        }

        public static ServiciosConsultoriosBusinessLogic ServiciosConsultoriosBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new ServiciosConsultoriosBusinessLogic(logic.settings);
        }
        public static JobsBusinessLogic JobsBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new JobsBusinessLogic(logic.settings);
        }

        public static DocumentosBusinessLogic DocumentosBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new DocumentosBusinessLogic(logic.settings);
        }

        public static HistoriasClinicasBusinessLogic HistoriasClinicasBusinessLogic(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new HistoriasClinicasBusinessLogic(logic.settings);
        }

        #region Identificadores estaticos

        public static List<KeyValue> GetIdentificationTypes(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new List<KeyValue>
            {
                new KeyValue { Key = 0, Id = "CC", Value = "Cedula de ciudadania" },
                new KeyValue { Key = 1, Id = "CE", Value = "Cedula de extranjeria" },
                new KeyValue { Key = 2, Id = "TI", Value = "Tarjeta de identidad" },
                new KeyValue { Key = 3, Id = "RC", Value = "Registro civil" },
                new KeyValue { Key = 4, Id = "NIT", Value = "Numero de identificaión tributaria" },
                new KeyValue { Key = 5, Id = "PA", Value = "Pasaporte" },
                new KeyValue { Key = 6, Id = "NU", Value = "No. Unico de Id. Personal" },
                new KeyValue { Key = 7, Id = "AS", Value = "Adulto sin identificación" },
                new KeyValue { Key = 8, Id = "MS", Value = "Menor sin Identificación" },
                new KeyValue { Key = 9, Id = "CD", Value = "Carnet diplomático" },
                new KeyValue { Key = 10, Id = "CN", Value = "Certificado Nacido Vivo" },
                new KeyValue { Key = 11, Id = "SC", Value = "Salvo Conducto" },
                new KeyValue { Key = 12, Id = "PE", Value = "Per Especial Permanencia" },
                new KeyValue { Key = 13, Id = "XX", Value = "Otro" }
            };
        }

        public static List<KeyValue> GetGenders(this Dominus.Backend.DataBase.BusinessLogic logic)
        {
            return new List<KeyValue>
            {
                new KeyValue { Id = "M", Value = "Masculino" },
                new KeyValue { Id = "F", Value = "Femenino" }
            };
        }

        #endregion
    }
}