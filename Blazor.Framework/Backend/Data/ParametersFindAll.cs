using System.Collections.Generic;

namespace Dominus.Backend.Data
{
    public class ParametersFindAll<T>
    {
        public string Expression { get; set; }
        public string LoadOptions { get; set; }
        public List<T> Models { get; set; }
    }
}