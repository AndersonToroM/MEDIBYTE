using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace CodeGenerator.Data
{
    public class Contexto : DbContext
    {
        public DBSettings DBData { get; set; }

        public Contexto(DBSettings DBData)
        {
            this.DBData = DBData;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(GetConnectionString(DBData));
        }

        private string GetConnectionString(DBSettings DBSettings)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ApplicationName = DBSettings.Name;
            builder.DataSource = DBSettings.DataSource;
            builder.InitialCatalog = DBSettings.InitialCatalog;
            builder.UserID = DBSettings.UserId;
            builder.Password = DBSettings.Password;
            builder.IntegratedSecurity = false;
            builder.MultipleActiveResultSets = true;
            builder.UserInstance = false;
            builder.ConnectTimeout = 120;
            builder.Pooling = true;
            //builder.Encrypt = true;
            string urlConexion = builder.ConnectionString;
            return builder.ConnectionString;
        }

    }

    public class DBSettings
    {
        public virtual string Name { get; set; }
        public virtual string DataSource { get; set; }
        public virtual string InitialCatalog { get; set; }
        public virtual string UserId { get; set; }
        public virtual string Password { get; set; }
        public virtual int NumberConnection { get; set; }
    }
}
