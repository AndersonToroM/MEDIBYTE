using Dominus.Backend.Security;
using System.Collections.Generic;

namespace Dominus.Backend.DataBase
{
    public class DataBaseSetting
    {
        public virtual DataBaseType DataBaseType { get; set; }

        public string Provider
        {
            get
            {
                if (DataBaseType == DataBaseType.SQLServer)
                    return "System.Data.SqlClient";
                else if (DataBaseType == DataBaseType.Oracle)
                    return "Oracle.ManagedDataAccess.Client";
                else if (DataBaseType == DataBaseType.MySql)
                    return "MySql.Data.MySqlClient";
                else if (DataBaseType == DataBaseType.PostgreSQL)
                    return "Npgsql";
                else
                    return "";
            }
        }

        public virtual string DataSource { get; set; }

        public virtual string InitialCatalog { get; set; }

        public virtual string UserId { get; set; }

        public virtual string Password { get; set; }

        public virtual bool TurnOnJobs { get; set; }

        public List<string> MenuActionName { get; set; }

        public List<SecurityNavigation> ListSecurityNavigation { get; set; } = new List<SecurityNavigation>();
    }

    public enum DataBaseType
    {
        SQLServer,
        Oracle,
        MySql,
        PostgreSQL
    }
}
