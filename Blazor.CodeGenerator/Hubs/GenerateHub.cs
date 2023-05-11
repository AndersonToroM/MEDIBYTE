using CodeGenerator.Data;
using CodeGenerator.Models;
using CodeGenerator.Templates;
using CodeGenerator.Templates.Utils;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class GenerateHub : Hub
    {

        public async Task CreateUser()
        {
            await Clients.All.SendAsync("CreatedUser", Context.ConnectionId);
        }

        public async Task GenerateCode(string JsonTables, string JsonTemplates, int NumberConnection, int FrameworkActual, 
            int TipoOrigenActual,string NombreRoe,string PrefijoRoe, string Consulta, string UserId)
        {
            if (TipoOrigenActual == 1)
                await GenerateCodeTable(JsonTables,JsonTemplates,NumberConnection,FrameworkActual,UserId);
            else
                await GenerateCodeRoe(JsonTemplates, NumberConnection, FrameworkActual, NombreRoe, PrefijoRoe, Consulta, UserId);
        }

        #region GENERACION ROE

        public async Task GenerateCodeRoe(string JsonTemplates, int NumberConnection, int FrameworkActual,string NombreRoe,string PrefijoRoe, string Consulta, string UserId)
        {
            GCUtil.Reset();
            CodeGeneratorModel CodeGeneratorModel = new CodeGeneratorModel();
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (NumberConnection != 0 && FrameworkActual != 0)
            {
                GCUtil.Framework = FrameworkActual;
                if (FrameworkActual == 1)
                {
                    CodeGeneratorModel.Domain = "Blazor";
                    CodeGeneratorModel.NameSpace = "NETCoreMVCRoe";
                }
                else
                {
                    CodeGeneratorModel.Domain = "JavaHibernate";
                    CodeGeneratorModel.NameSpace = "SiesaJavaHibernate";
                }
                CodeGeneratorModel.PathGenerate += CodeGeneratorModel.Domain;
                DBSettings DBSettingsActual = GCUtil.ListDBSettings.Find(x => x.NumberConnection == NumberConnection);
                List<TemplateModel> templates = JsonConvert.DeserializeObject<List<TemplateModel>>(JsonTemplates);

                if (!string.IsNullOrWhiteSpace(PrefijoRoe) && !string.IsNullOrWhiteSpace(NombreRoe) && 
                    !string.IsNullOrWhiteSpace(Consulta) && templates.Count != 0 &&
                    !Consulta.Contains("TOP" , StringComparison.OrdinalIgnoreCase) && !Consulta.Contains("DISTINCT", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        if (Directory.Exists(CodeGeneratorModel.PathGenerate))
                        {
                            Directory.Delete(CodeGeneratorModel.PathGenerate, true);
                        }

                        List<TableModel> tables = new Scheme(DBSettingsActual).GetDataTableFromRoe(NombreRoe, PrefijoRoe, Consulta);

                        int totalGenerate = (tables.Count * templates.Count);
                        var index = 0;
                        foreach (var table in tables)
                            foreach (var template in templates)
                            {
                                CodeGeneratorModel.TableGenerated = table;
                                CodeGeneratorModel.TemplateGenerated = template;
                                new BuildTemplate().Generate(CodeGeneratorModel);
                                index++;
                                var percentage = (index * 100) / totalGenerate;
                                await Clients.Client(UserId).SendAsync("ReceiveProgressGenerate", table.Code, template.Name, percentage);
                            }

                        string folderToZip = CodeGeneratorModel.PathGenerate;
                        string nameFile = CodeGeneratorModel.Domain + "_CodeGenerate_" + DateTime.Now.ToString("yyyy-MM-dd_HH\\hmm\\mss\\s") + ".zip";
                        string zipFile = GCUtil.PathTemp + nameFile;
                        if (!Directory.Exists(folderToZip))
                            GCUtil.Errors.Add("El directorio no fue creado o no existe.");
                        else
                        {
                            ZipFile.CreateFromDirectory(folderToZip, zipFile);
                            result.Add("error", GCUtil.Errors);
                            result.Add("nameFile", nameFile);
                            result.Add("file", zipFile);
                            result.Add("success", true);
                            await Clients.Client(UserId).SendAsync("FinishGenerateCode", result);
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        GCUtil.Errors.Add(e.Message);
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(NombreRoe))
                        GCUtil.Errors.Add("El nombre del ROE es obligatorio.");
                    if (string.IsNullOrWhiteSpace(PrefijoRoe))
                        GCUtil.Errors.Add("El proyecto o area del ROE es obligatorio.");
                    if (string.IsNullOrWhiteSpace(Consulta))
                        GCUtil.Errors.Add("La consulta es obligatoria.");
                    if (templates.Count == 0)
                        GCUtil.Errors.Add("Debe seleccionar al menus una plantilla");
                    if (Consulta.Contains("TOP", StringComparison.OrdinalIgnoreCase) || Consulta.Contains("DISTINCT", StringComparison.OrdinalIgnoreCase))
                        GCUtil.Errors.Add("La consulta no debe tener clausulas TOP, DISTINCT o similares.");
                }
            }
            else
            {
                GCUtil.Errors.Add("Debe seleccionar una conexión y un framework.");
            }
            result.Add("error", GCUtil.Errors);
            result.Add("success", false);
            await Clients.Client(UserId).SendAsync("FinishGenerateCode", result);
            return;
        }

        #endregion

        public async Task GenerateCodeTable(string JsonTables, string JsonTemplates, int NumberConnection, int FrameworkActual, string UserId)
        {
            GCUtil.Reset();

            CodeGeneratorModel CodeGeneratorModel = new CodeGeneratorModel();

            Dictionary<string, object> result = new Dictionary<string, object>();
            if (NumberConnection != 0 && FrameworkActual != 0)
            {
                GCUtil.Framework = FrameworkActual;
                if (FrameworkActual == 1)
                {
                    CodeGeneratorModel.Domain = "Blazor";
                    CodeGeneratorModel.NameSpace = "NETCoreMVC";
                }
                else
                {
                    CodeGeneratorModel.Domain = "JavaHibernate";
                    CodeGeneratorModel.NameSpace = "SiesaJavaHibernate";
                }
                CodeGeneratorModel.PathGenerate += CodeGeneratorModel.Domain;
                DBSettings DBSettingsActual = GCUtil.ListDBSettings.Find(x => x.NumberConnection == NumberConnection);
                List<TableModel> tables = JsonConvert.DeserializeObject<List<TableModel>>(JsonTables);
                List<TemplateModel> templates = JsonConvert.DeserializeObject<List<TemplateModel>>(JsonTemplates);

                if (tables.Count != 0 && templates.Count != 0)
                {
                    try
                    {
                        if (Directory.Exists(CodeGeneratorModel.PathGenerate))
                        {
                            Directory.Delete(CodeGeneratorModel.PathGenerate, true);
                        }

                        if (tables.Count != 0)
                        {
                            tables.ForEach(x => x = new Scheme(DBSettingsActual).GetDataTable(x));
                        }

                        //Se verifica si la plantilla Utils se va a generar para sacarla de la lista
                        if (templates.Exists(x => x.NameClass == "Utils"))
                        {
                            templates.RemoveAll(x => x.NameClass == "Utils");
                            new Utils(tables, CodeGeneratorModel.PathGenerate);
                        }

                        int totalGenerate = (tables.Count * templates.Count);
                        var index = 0;
                        foreach (var table in tables)
                            foreach (var template in templates)
                            {
                                CodeGeneratorModel.TableGenerated = table;
                                CodeGeneratorModel.TemplateGenerated = template;
                                new BuildTemplate().Generate(CodeGeneratorModel);
                                index++;
                                var percentage = (index * 100) / totalGenerate;
                                await Clients.Client(UserId).SendAsync("ReceiveProgressGenerate", table.Code, template.Name, percentage);
                            }

                        string folderToZip = CodeGeneratorModel.PathGenerate;
                        string nameFile = CodeGeneratorModel.Domain + "_CodeGenerate_" + DateTime.Now.ToString("yyyy-MM-dd_HH\\hmm\\mss\\s") + ".zip";
                        string zipFile = GCUtil.PathTemp + nameFile;
                        if (!Directory.Exists(folderToZip))
                            GCUtil.Errors.Add("El directorio no fue creado o no existe.");
                        else
                        {
                            ZipFile.CreateFromDirectory(folderToZip, zipFile);
                            result.Add("error", GCUtil.Errors);
                            result.Add("nameFile", nameFile);
                            result.Add("file", zipFile);
                            result.Add("success", true);
                            await Clients.Client(UserId).SendAsync("FinishGenerateCode", result);
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        GCUtil.Errors.Add(e.Message);
                    }
                }
                else
                {
                    GCUtil.Errors.Add("Debe seleccionar al menos una tabla y una plantilla.");
                }
            }
            else
            {
                GCUtil.Errors.Add("Debe seleccionar una conexión y un framework.");
            }
            result.Add("error", GCUtil.Errors);
            result.Add("success", false);
            await Clients.Client(UserId).SendAsync("FinishGenerateCode", result);
            return;
        }


    }

}