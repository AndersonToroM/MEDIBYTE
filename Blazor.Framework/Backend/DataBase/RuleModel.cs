using System.Runtime.Serialization;

namespace Dominus.Backend.DataBase
{
    public class RuleModel
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public RuleType RuleType { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Rule { get; set; }

        [DataMember]
        public int ProfileId { get; set; }

        [DataMember]
        public string ResourceId { get; set; }
    }

    public enum RuleType
    {
        AddRules = 1,
        MofidyRules = 2,
        CommonRules = 3,
        RemoveRules = 4,
        FindByIdRules = 5,
        CalculationRules = 6,
        CustomRules = 7
    }
}
