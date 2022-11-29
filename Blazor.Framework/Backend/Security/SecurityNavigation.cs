using System.Collections.Generic;

namespace Dominus.Backend.Security
{
    public class SecurityNavigation
    {
        public long ProfileId { get; set; }
        public bool SecurityPolicy { get; set; }
        public List<Navegation> ListNavegation { get; set; } = new List<Navegation>();

        public static List<string> ControllerExclude = new List<string>()
        {
            "/Login/","/Error/"
        };

    }

    /// <summary>
    /// Rutas de las aciones permitidas segun su perfil
    /// </summary>
    public class Navegation
    {
        //public long MenuId { get; set; }
        //public long MenuActionId { get; set; }
        public string MenuName { get; set; }
        public string ActionName { get; set; }
        public string Path { get; set; }

    }
}
