using Microsoft.CodeAnalysis.Scripting;
using System.Runtime.Serialization;

namespace Dominus.Backend.DataBase
{
    public class CalculationModel
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Calculation { get; set; }

        [DataMember]
        public int ProfileId { get; set; }

        [DataMember]
        public string ResourceId { get; set; }

        public Script ComíleCalculation { get; set; }
    }
}
