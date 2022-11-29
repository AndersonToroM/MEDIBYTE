using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Dominus.Backend.Application
{
    public class Language
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Culture { get; set; }
        public string Name { get; set; }

        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string DateTimeFormat { get; set; }

        //private Dictionary<string, Dictionary<string, string>> resources;

        private List<LanguageResource> Resources { get; set; } = new List<LanguageResource>();

        //public void Languaje()
        //{
        //    resources = new Dictionary<string, Dictionary<string, string>>();
        //}

        public List<LanguageResource> GetAllResources()
        {
            return Resources;
        }

        public void SetAllResources()
        {
            try
            {
                var stringResoures = File.ReadAllText(Path.Combine("Utils", $"resources{DApp.DefaultLanguage.Code}.json"));
                var resources = JsonConvert.DeserializeObject<List<LanguageResource>>(stringResoures);
                Resources = resources;

                var duplicates = Resources.GroupBy(x => x.Id)
                      .Where(g => g.Count() > 1)
                      .Select(y => new { id = y.Key })
                      .ToList();

                if (duplicates.Count > 0)
                {
                    foreach (var element in duplicates)
                    {
                        var resource = resources.FirstOrDefault(x => x.Id == element.id);
                        Resources.RemoveAll(x => x.Id == resource.Id);
                        Resources.Add(resource);
                    }
                    WriteAllResources();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error en cargar recursos. | {e.Message}", Console.ForegroundColor = ConsoleColor.Red);
                throw;
            }
        }

        public void WriteAllResources()
        {
            try
            {
                var resources = Resources.OrderBy(x => x.Id);
                var serializedResources = JsonConvert.SerializeObject(resources, Formatting.Indented);
                File.WriteAllText(Path.Combine("Utils", $"resources{DApp.DefaultLanguage.Code}.json"), serializedResources);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error en guardar recursos. | {e.Message}", Console.ForegroundColor = ConsoleColor.Red);
                throw;
            }
        }

        public void AddResource(string id, string description)
        {
            lock (Resources)
            {
                var resoure = Resources.FirstOrDefault(x => x.Id == id);
                if (resoure != null)
                {
                    Resources[Resources.IndexOf(resoure)].Description = description;
                }
                else
                {
                    Resources.Add(new LanguageResource { Id = id, Description = description });
                }
            }
        }

        public void DeleteResource(string id)
        {
            lock (Resources)
            {
                var resoure = Resources.FirstOrDefault(x => x.Id == id);
                if (resoure != null)
                {
                    Resources.RemoveAt(Resources.IndexOf(resoure));
                }
            }
        }

        public string GetResource(string llave)
        {
            var recurso = Resources.FirstOrDefault(x => x.Id == llave);
            if (recurso != null)
            {
                return recurso.Description;
            }
            else
            {
                return llave;
            }
        }

        public class LanguageResource
        {
            public string Id { get; set; }
            public string Description { get; set; }
        }
    }
}
