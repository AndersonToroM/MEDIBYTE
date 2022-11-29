using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dominus.Backend.Application;
using Dominus.Backend.DataBase;

namespace Dominus.Backend.DataBase
{
    public class BusinessRule
    {
        public List<RuleModel> Rules { get; set; }

        public List<CalculationModel> Calculates { get; set; }

        private ScriptOptions GetScriptOptions()
        {
            var scriptOptions = ScriptOptions.Default;

            var mscorlib = typeof(object).GetTypeInfo().Assembly;
            var systemCore = typeof(System.Linq.Enumerable).GetTypeInfo().Assembly;

            var references = new[] { mscorlib, systemCore };

            scriptOptions = scriptOptions.AddReferences(references);

            scriptOptions = scriptOptions.AddImports("System");
            scriptOptions = scriptOptions.AddImports("System.Linq");
            scriptOptions = scriptOptions.AddImports("System.Collections.Generic");
            return scriptOptions;

        }

        public void CompileRules()
        {
            if (Calculates != null && Calculates.Count > 0)
            {
                Parallel.ForEach(Calculates, (currentRule) =>
                {
                    Type rntituType = Type.GetType(currentRule.Type);
                    var script = CSharpScript.Create(currentRule.Calculation, GetScriptOptions(), globalsType: rntituType);
                    script.Compile();
                    currentRule.ComíleCalculation = script;
                });

            }
        }

        public List<RuleModel> GetRules<TInput>(RuleType ruleType)
        {
            return Rules.Where(x => x.Type == typeof(TInput).FullName && x.RuleType== ruleType).ToList();
        }

        public List<RuleModel> GetRules<T>(RuleType ruleType, int profileId)
        {
            return Rules.Where(x => x.Type == typeof(T).FullName && x.RuleType == ruleType && x.ProfileId == profileId).ToList();
        }

        public string ValidateRules<T>(RuleModel rule, T data) where T : BaseEntity
        {

            var ruleEngine = new RuleRunner();
            bool isTrue = ruleEngine.IsTrue(rule.Rule, data);
            if (!isTrue)
                return DApp.DefaultLanguage.GetResource(rule.ResourceId);
            else
                return "";
        }

        public List<string> ValidateRules<T>(List<RuleModel> rules, T data) where T : BaseEntity
        {
            List<string> errors = new List<string>();

            if (rules != null && rules.Count > 0 && data != null)
            {
                foreach (RuleModel rule in rules)
                {
                    string error = ValidateRules<T>(rule, data);
                    if (!string.IsNullOrWhiteSpace(error))
                        errors.Add(error);
                }
            }
            return errors;
        }
    }
}
