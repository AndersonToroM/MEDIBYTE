using Blazor.BusinessLogic;
using Blazor.BusinessLogic.Models;
using Blazor.Infrastructure.Entities;
using Blazor.WebApp.Models;
using Dominus.Backend.Application;
using Dominus.Frontend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WidgetGallery;

namespace Blazor.WebApp.Controllers
{
    [Authorize]
    public class HomeController : BaseAppController
    {
        public HomeController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
        }

        public IActionResult Index()
        {
            ViewBag.VersionApp = DApp.InfoApp.VersionApp;
            ViewBag.ParcheApp = DApp.InfoApp.ParcheApp;
            return View();
        }

        public IActionResult DicomViewer()
        {
            DicomViewerModel dicomViewer = new DicomViewerModel();
            return View("_ViewerDicom", dicomViewer);
        }

        public IActionResult DicomViewerImageDisco(long id)
        {
            DicomViewerModel dicomViewer = new DicomViewerModel();
            var data = Manager().GetBusinessLogic<ImagenesDiagnosticasDetalle>().FindById(x => x.Id == id, false);
            var db = DApp.GetTenantConnection(Request.Host.Value);
            if (data != null)
            {
                dicomViewer.EsDesdeDisco = true;
                dicomViewer.TieneImagen = true;
                dicomViewer.Contenedor = db.InitialCatalog.ToLower();
                dicomViewer.NombreArchivoAzureBlob = data.NombreArchivoAzureBlob;
            }
            return View("_ViewerDicom", dicomViewer);
        }

        public IActionResult DicomViewerImageAzure(long id)
        {
            DicomViewerModel dicomViewer = new DicomViewerModel();
            var data = Manager().GetBusinessLogic<ImagenesDiagnosticasDetalle>().FindById(x => x.Id == id, false);
            var db = DApp.GetTenantConnection(Request.Host.Value);
            if (data != null)
            {
                dicomViewer.TieneImagen = true;
                dicomViewer.Contenedor = db.InitialCatalog.ToLower();
                dicomViewer.NombreArchivoAzureBlob = data.NombreArchivoAzureBlob;
            }
            return View("_ViewerDicom", dicomViewer);
        }

        [HttpPost]
        public Dictionary<string, object> ObtenerPreferenciasUsuario(bool bloqueoPantalla)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("BloqueoPantalla", bloqueoPantalla);
            return data;
        }

        [HttpPost]
        public bool DesBloqueoPantalla(string password)
        {
            var usuario = Manager().UserBusinessLogic().SingIn(User.Identity.Name, password, Request.Host.Value);
            if (usuario == null)
                return false;
            else
            {
                return true;
            }
        }

        [HttpGet]
        public IActionResult LogFileView()
        {
            return View("_LogFileView");
        }

        [HttpPost]
        public IActionResult GetFilesLog()
        {
            List<ArchivoDescargaModel> logsFiles = new List<ArchivoDescargaModel>();
            if (Directory.Exists(DApp.PathDirectoryLogs))
            {                
                DirectoryInfo dirLog = new DirectoryInfo(DApp.PathDirectoryLogs);
                dirLog.GetFiles().ToList().ForEach(x =>
                {
                    logsFiles.Add(new ArchivoDescargaModel
                    {
                        Nombre = x.Name,
                        Extension = x.Extension
                    });
                });
            }
            return new OkObjectResult(logsFiles);
        }

        [HttpGet]
        public IActionResult DownloadLogFile(string fileName)
        {
            try
            {
                string pathFile = Path.Combine(DApp.PathDirectoryLogs, fileName);
                if (System.IO.File.Exists(pathFile))
                {
                    FileInfo file = new FileInfo(pathFile);
                    return File(System.IO.File.ReadAllBytes(pathFile), DApp.Util.ObtenerContentTypePorExtension(file.Extension), $"{DateTime.Now:yyyyMMddHHmm}_{fileName}");
                }
                else throw new Exception($"Archivo {fileName} no encontrado");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("Error en servidor. " + e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public IActionResult DeleteLogFile(string fileName)
        {
            try
            {
                string pathFile = Path.Combine(DApp.PathDirectoryLogs, fileName);
                if (System.IO.File.Exists(pathFile))
                {
                    System.IO.File.Delete(pathFile);
                    return Ok();
                }
                else throw new Exception($"Archivo {fileName} no encontrado");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("Error en servidor. " + e.GetFullErrorMessage());
            }
        }
    }
}
