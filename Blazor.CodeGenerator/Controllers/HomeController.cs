using CodeGenerator.Data;
using CodeGenerator.Models;
using CodeGenerator.Templates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalRChat.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGenerator.Controllers
{
    public class HomeController : Controller
    {
        public CodeGeneratorModel CodeGeneratorModel = new CodeGeneratorModel();

        public IActionResult Index()
        {
            var context = HttpContext.Connection.Id;
            GCUtil.Reset();
            CodeGeneratorModel.ConnectionId = HttpContext.Connection.Id;
            CodeGeneratorModel.Conexiones = GCUtil.ListDBSettings;
            CodeGeneratorModel.ConexionActual = 1;
            CodeGeneratorModel.FrameworkActual = 1;
            CodeGeneratorModel.TipoOrigenActual = 1;
            CodeGeneratorModel.Tables = GetTables(CodeGeneratorModel.ConexionActual).Tables;
            var TemplatesResult = GetTemplates(CodeGeneratorModel.FrameworkActual, CodeGeneratorModel.TipoOrigenActual);
            CodeGeneratorModel.Templates = TemplatesResult.Templates;
            CodeGeneratorModel.ProyectoArea = TemplatesResult.ProyectoArea;
            return View(CodeGeneratorModel);
        }

        [HttpGet]
        public CodeGeneratorModel GetTables(int NumberConnection)
        {
            if (NumberConnection != 0)
            {
                DBSettings DBSettingsActual = GCUtil.ListDBSettings.Find(x => x.NumberConnection == NumberConnection);
                try
                {
                    CodeGeneratorModel.Tables = GCUtil.DataBaseInfo.FirstOrDefault(x => x.NumberConnection == NumberConnection).Tables.OrderBy(x=>x.Name).ToList();
                }
                catch (Exception Ex)
                {
                    CodeGeneratorModel.Errores.Add("Error en la conexión con la base de datos " + DBSettingsActual.Name);
                    CodeGeneratorModel.Errores.Add(Ex.Message);
                }
            }
            return CodeGeneratorModel;
        }

        [HttpGet]
        public CodeGeneratorModel GetTemplates(int Framework , int TipoOrigen)
        {
            try
            {
                if (Framework == 0)
                    CodeGeneratorModel.Errores.Add("Error en el framework seleccionado. ");
                else if (Framework == 1)
                {
                    //CodeGeneratorModel.ProyectoArea = new List<string> { "UNONOM", "UNOGP", "UNOSST", "UNOEE", "UNOMNTO", "UNOCCO" };
                    CodeGeneratorModel.ProyectoArea = new List<string> { "PROCESS" };
                    if (TipoOrigen == 1)
                        CodeGeneratorModel.NameSpace = "NETCoreMVC";
                    else 
                        CodeGeneratorModel.NameSpace = "NETCoreMVCRoe";
                }
                else
                {
                    CodeGeneratorModel.ProyectoArea = new List<string> { "nom", "gpe", "soc", "gen", "map", "cco" };
                    CodeGeneratorModel.NameSpace = "SiesaJavaHibernate";
                }
                CodeGeneratorModel.FrameworkActual = Framework;
                CodeGeneratorModel.Templates = new BuildTemplate().GetTemplates(CodeGeneratorModel.NameSpace).OrderBy(x => x.Name).ToList();
            }
            catch (Exception Ex)
            {
                CodeGeneratorModel.Errores.Add("Error en el framework " + CodeGeneratorModel.NameSpace);
                CodeGeneratorModel.Errores.Add(Ex.Message);
            }

            
            return CodeGeneratorModel;
        }

        [HttpGet]
        public ActionResult DownloadFile(string path,string name)
        {
            return File(System.IO.File.ReadAllBytes(path), "application/octet-stream", name);
        }

        //[HttpPost]
        //public JsonResult GenerateCode(string JsonTables, string JsonTemplates, int NumberConnection, int FrameworkActual, string UserId)
        //{
        //    GCUtil.Reset();

        //    CodeGeneratorModel.ConexionActual = 1;
        //    CodeGeneratorModel.FrameworkActual = 1;

        //    Dictionary<string, object> result = new Dictionary<string, object>();
        //    if (NumberConnection != 0 && FrameworkActual != 0)
        //    {
        //        GCUtil.Framework = FrameworkActual;
        //        if (FrameworkActual == 1)
        //        {
        //            CodeGeneratorModel.Domain = "Siesa.Enterprise";
        //            CodeGeneratorModel.NameSpace = "SiesaNetCore";
        //        }
        //        else
        //        {
        //            CodeGeneratorModel.Domain = "JavaHibernate";
        //            CodeGeneratorModel.NameSpace = "SiesaJavaHibernate";
        //        }
        //        CodeGeneratorModel.PathGenerate += CodeGeneratorModel.Domain;
        //        DBSettings DBSettingsActual = GCUtil.ListDBSettings.Find(x => x.NumberConnection == NumberConnection);
        //        List<TableModel> tables = JsonConvert.DeserializeObject<List<TableModel>>(JsonTables);
        //        List<TemplateModel> templates = JsonConvert.DeserializeObject<List<TemplateModel>>(JsonTemplates);

        //        if (tables.Count != 0 && templates.Count != 0)
        //        {
        //            try
        //            {
        //                if (Directory.Exists(CodeGeneratorModel.PathGenerate))
        //                {
        //                    Directory.Delete(CodeGeneratorModel.PathGenerate, true);
        //                }

        //                if (tables.Count != 0)
        //                {
        //                    tables.ForEach(x => x = new Scheme(DBSettingsActual).GetDataTable(x));
        //                }
        //                //var hubContext = GlobalHost.ConnectionManager.GetHubContext("ChatHub");
        //                foreach (var template in templates)
        //                    foreach (var table in tables)
        //                    {
        //                        CodeGeneratorModel.TableGenerated = table;
        //                        CodeGeneratorModel.TemplateGenerated = template;
        //                        new BuildTemplate().Generate(CodeGeneratorModel);
        //                    }

        //                string folderToZip = CodeGeneratorModel.PathGenerate;
        //                string nameFile = CodeGeneratorModel.Domain + "_CodeGenerate_" + DateTime.Now.ToFileTime().ToString() + ".zip";
        //                string zipFile = GCUtil.PathTemp + nameFile;
        //                if (!Directory.Exists(folderToZip))
        //                    GCUtil.Errors.Add("El directorio no fue creado o no existe.");
        //                else
        //                {
        //                    ZipFile.CreateFromDirectory(folderToZip, zipFile);
        //                    byte[] file = System.IO.File.ReadAllBytes(zipFile);
        //                    result.Add("Error", GCUtil.Errors);
        //                    result.Add("nameFile", nameFile);
        //                    result.Add("file", file);
        //                    result.Add("success", true);
        //                    return Json(result);
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                GCUtil.Errors.Add(e.Message);
        //            }
        //        }
        //        else
        //        {
        //            GCUtil.Errors.Add("Debe seleccionar al menos una tabla y una plantilla.");
        //        }
        //    }
        //    else
        //    {
        //        GCUtil.Errors.Add("Debe seleccionar una conexión y un framework.");
        //    }
        //    result.Add("Error", GCUtil.Errors);
        //    result.Add("success", false);
        //    return Json(result);
        //}

        [HttpGet]
        public JsonResult RefreshTable(string TableName, int NumberConnection)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (NumberConnection != 0)
            {
                DBSettings DBSettingsActual = GCUtil.ListDBSettings.Find(x => x.NumberConnection == NumberConnection);
                try
                {
                    Scheme scheme = new Scheme(DBSettingsActual);
                    scheme.tableModels.Add(new TableModel { Name = TableName });

                    int ubicacion = GCUtil.DataBaseInfo.FindIndex(x => x.NumberConnection == DBSettingsActual.NumberConnection);

                    List<ColumnModel> columns = scheme.GetColumns();
                    if (columns.Count > 0)
                    {
                        var asd = GCUtil.DataBaseInfo[ubicacion].Columns.RemoveAll(x=>x.TableName == TableName);
                        GCUtil.DataBaseInfo[ubicacion].Columns.AddRange(columns);
                    }

                    List<InReferencesModel> inReferencesModel = scheme.GetInFKs();
                    if (inReferencesModel.Count > 0)
                    {
                        GCUtil.DataBaseInfo[ubicacion].InReferences.RemoveAll(x => x.TableName == TableName);
                        GCUtil.DataBaseInfo[ubicacion].InReferences.AddRange(inReferencesModel);
                    }

                    List<OutReferencesModel> outReferencesModel = scheme.GetOutFKs();
                    if (outReferencesModel.Count > 0)
                    {
                        GCUtil.DataBaseInfo[ubicacion].OutReferences.RemoveAll(x => x.TableName == TableName);
                        GCUtil.DataBaseInfo[ubicacion].OutReferences.AddRange(outReferencesModel);
                    }

                    List<IndexModel> indexModel = scheme.GetIndexes();
                    if (outReferencesModel.Count > 0)
                    {
                        GCUtil.DataBaseInfo[ubicacion].Indexes.RemoveAll(x => x.TableName == TableName);
                        GCUtil.DataBaseInfo[ubicacion].Indexes.AddRange(indexModel);
                    }

                    result.Add("Columns", columns.Select(x => x.Name)); ;
                    result.Add("InReferences", inReferencesModel.Select(x => x.InReferencesName).Distinct());
                    result.Add("OutReferences", outReferencesModel.Select(x => x.OutReferencesName).Distinct());
                    result.Add("Indexes", indexModel.Select(x => x.Name).Distinct());
                }
                catch (Exception Ex)
                {
                    result.Add("Errores", new List<string> {
                        "Error  actualizando datos de la tabla " + TableName + "en la base de datos" + DBSettingsActual.Name,
                        Ex.Message
                    });
                }
            }
            return Json(result);
        }

        [HttpGet]
        public JsonResult SearchNewTables(int NumberConnection)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (NumberConnection != 0)
            {
                DBSettings DBSettingsActual = GCUtil.ListDBSettings.Find(x => x.NumberConnection == NumberConnection);
                try
                {
                    int ubicacion = GCUtil.DataBaseInfo.FindIndex(x => x.NumberConnection == NumberConnection);
                    Scheme scheme = new Scheme(DBSettingsActual);
                    List<TableModel> tablesBD = scheme.GetTables();
                    List<TableModel> tablesNow = GCUtil.DataBaseInfo[ubicacion].Tables;
                    scheme.tableModels = tablesBD.Where(x => !tablesNow.Exists(j => j.Name == x.Name)).ToList();
                    if (scheme.tableModels.Count > 0) {
                        GCUtil.DataBaseInfo[ubicacion].Tables.AddRange(scheme.tableModels);
                        GCUtil.DataBaseInfo[ubicacion].Columns.AddRange(scheme.GetColumns());
                        GCUtil.DataBaseInfo[ubicacion].InReferences.AddRange(scheme.GetInFKs());
                        GCUtil.DataBaseInfo[ubicacion].OutReferences.AddRange(scheme.GetOutFKs());
                        GCUtil.DataBaseInfo[ubicacion].Indexes.AddRange(scheme.GetIndexes());

                        result.Add("NewTables", scheme.tableModels.Select(x => x.Name));
                        result.Add("DatasourceTables", GCUtil.DataBaseInfo[ubicacion].Tables.OrderBy(x => x.Name).ToList());
                    }
                }
                catch (Exception Ex)
                {
                    result.Add("Errores", new List<string> {
                        "Error consultando tablas en la base de datos " + DBSettingsActual.Name,
                        Ex.Message
                    });
                }
            }
            return Json(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
