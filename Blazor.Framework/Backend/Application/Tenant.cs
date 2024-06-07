using Dominus.Backend.DataBase;
using System;
using System.Collections.Generic;

namespace Dominus.Backend.Application
{
    public class Tenant
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public bool SendNotifications { get; set; }

        public DataBaseSetting DataBaseSetting { get; set; } = new DataBaseSetting();

        public Dictionary<string, string> Services { get; set; } = new Dictionary<string, string>();

        public bool LoadDefaultData { get; set; }

        public string Environment { get; set; }


    }
}
