using CodeGenerator.Data;
using CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeGenerator.Templates
{
    public class BuildTemplate
    {
        #region Generate

        public List<TemplateModel> GetTemplates(string NameSpace)
        {
            List<Type> TemplateTypes = Assembly.GetExecutingAssembly().GetTypes()
                              .Where(t => t.Namespace == "CodeGenerator.Templates." + NameSpace && !t.Name.Contains("<>"))
                              .ToList();

            List<TemplateModel> templates = new List<TemplateModel>();
            foreach (var TemplateType in TemplateTypes)
            {
                object obj = Activator.CreateInstance(TemplateType);
                try
                {
                    TemplateModel TemplateModel = new TemplateModel();
                    TemplateModel.Name = TemplateType.GetProperty("Name").GetValue(obj).ToString();
                    TemplateModel.NameClass = TemplateType.Name;
                    TemplateModel.Description = TemplateType.GetProperty("Description").GetValue(obj).ToString();
                    templates.Add(TemplateModel);
                }
                catch (Exception e) {
                    GCUtil.Errors.Add("Las propiedades de la "+ TemplateType.Name + " no estan bien declaradas. | Error: " + e.Message);
                }
            }

            ///Se agrega la platilla Utils por defecto
            templates.Add(new TemplateModel { Name = "Utils", Description = "Genera utilidades para la aplicacion.", NameClass = "Utils" });

            return templates;
        }

        
        public void Generate(CodeGeneratorModel CodeGeneratorModel)
        {
            Type type = Type.GetType("CodeGenerator.Templates." + CodeGeneratorModel.NameSpace + "." + CodeGeneratorModel.TemplateGenerated.NameClass);
            if (type != null && !GCUtil.TemplateWithError.Contains(type.Name))
            {
                Activator.CreateInstance(type, new object[] { CodeGeneratorModel });
            }
                
        }
        #endregion
    }
}
