using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Dominus.Frontend.Controllers;
using Blazor.Infrastructure.Entities;
using Blazor.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using Newtonsoft.Json;
using Blazor.BusinessLogic;
using Dominus.Backend.Security;
using Blazor.Infrastructure;
using DevExpress.Xpo;

namespace Blazor.WebApp.Controllers
{

    [Authorize]
    public partial class ParametrosGeneralesController : BaseAppController
    {

        //private const string Prefix = "ParametrosGenerales"; 

        public ParametrosGeneralesController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
        }

        #region Functions Master

        //[HttpPost]
        //public LoadResult Get(DataSourceLoadOptions loadOptions)
        //{
        //    return DataSourceLoader.Load(Manager().GetBusinessLogic<ParametrosGenerales>().Tabla(true), loadOptions);
        //}

        public IActionResult List()
        {
            return View("Edit", EditModel());
            //return View("List");
        }

        public IActionResult ListPartial()
        {
            return PartialView("Edit", EditModel());
            //return PartialView("List");
        }

        //[HttpGet]
        //public IActionResult New()
        //{
        //    return PartialView("Edit", NewModel());
        //}

        //private ParametrosGeneralesModel NewModel() 
        //{ 
        //    ParametrosGeneralesModel model = new ParametrosGeneralesModel();
        //    model.Entity.IsNew = true;
        //    return model; 
        //} 

        [HttpGet]
        public IActionResult Edit()
        {
            return PartialView("Edit", EditModel());
        }

        private ParametrosGeneralesModel EditModel()
        {
            ParametrosGeneralesModel model = new ParametrosGeneralesModel();
            model.Entity = Manager().GetBusinessLogic<ParametrosGenerales>().FindById(x => true, false);
            model.Entity.IsNew = false;
            return model;
        }

        [HttpPost]
        public IActionResult Edit(ParametrosGeneralesModel model)
        {
            return PartialView("Edit", EditModel(model));
        }

        private ParametrosGeneralesModel EditModel(ParametrosGeneralesModel model)
        {
            ViewBag.Accion = "Save";
            var OnState = model.Entity.IsNew;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Entity.LastUpdate = DateTime.Now;
                    model.Entity.UpdatedBy = User.Identity.Name;
                    if (model.Entity.IsNew)
                    {
                        model.Entity.CreationDate = DateTime.Now;
                        model.Entity.CreatedBy = User.Identity.Name;
                        model.Entity = Manager().GetBusinessLogic<ParametrosGenerales>().Add(model.Entity);
                        model.Entity.IsNew = false;
                    }
                    else
                    {
                        if (model.ModifyPasswordIntegracionFE)
                            model.Entity.PasswordIntegracionFE = Cryptography.Encrypt(model.Entity.PasswordIntegracionFE);

                        model.Entity = Manager().GetBusinessLogic<ParametrosGenerales>().Modify(model.Entity);
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Entity.Id", e.GetFrontFullErrorMessage());
                }
            }
            else
            {
                ModelState.AddModelError("Entity.Id", "Error de codigo, el objeto a guardar tiene campos diferentes a los de la entidad.");
            }
            return model;
        }

        //[HttpPost]
        //public IActionResult Delete(ParametrosGeneralesModel model)
        //{
        //    return PartialView("Edit", DeleteModel(model));
        //}

        //private ParametrosGeneralesModel DeleteModel(ParametrosGeneralesModel model)
        //{ 
        //    ViewBag.Accion = "Delete"; 
        //    ParametrosGeneralesModel newModel = NewModel(); 
        //    if (ModelState.IsValid) 
        //    { 
        //        try 
        //        { 
        //            model.Entity = Manager().GetBusinessLogic<ParametrosGenerales>().FindById(x => x.Id == model.Entity.Id, false); 
        //            Manager().GetBusinessLogic<ParametrosGenerales>().Remove(model.Entity); 
        //            return newModel;
        //        } 
        //        catch (Exception e) 
        //        { 
        //            ModelState.AddModelError("Entity.Id", e.GetFullErrorMessage()); 
        //        } 
        //    } 
        //    return model; 
        //} 

        #endregion

        #region Functions Detail 
        /*

        [HttpGet]
        public IActionResult NewDetail(long IdFather)
        {
            return PartialView("EditDetail", NewModelDetail(IdFather));
        }

        private ParametrosGeneralesModel NewModelDetail(long IdFather) 
        { 
            ParametrosGeneralesModel model = new ParametrosGeneralesModel(); 
            model.Entity.IdFather = IdFather; 
            model.Entity.IsNew = true; 
            return model; 
        } 

        [HttpGet]
        public IActionResult EditDetail(long Id)
        {
            return PartialView("EditDetail", EditModel(Id));
        }

        [HttpPost]
        public IActionResult EditDetail(ParametrosGeneralesModel model)
        {
            return PartialView("EditDetail",EditModel(model));
        }

        [HttpPost]
        public IActionResult DeleteDetail(ParametrosGeneralesModel model)
        {
            return PartialView("EditDetail", DeleteModelDetail(model));
        }

        private ParametrosGeneralesModel DeleteModelDetail(ParametrosGeneralesModel model)
        { 
            ViewBag.Accion = "Delete"; 
            ParametrosGeneralesModel newModel = NewModelDetail(model.Entity.IdFather); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<ParametrosGenerales>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<ParametrosGenerales>().Remove(model.Entity); 
                    return newModel;
                } 
                catch (Exception e) 
                { 
                    ModelState.AddModelError("Entity.Id", e.GetFullErrorMessage()); 
                } 
            } 
            return model; 
        } 

        #endregion 

        #region Funcions Detail Edit in Grid 

        [HttpPost] 
        public IActionResult AddInGrid(string values) 
        { 
             ParametrosGenerales entity = new ParametrosGenerales(); 
             JsonConvert.PopulateObject(values, entity); 
             ParametrosGeneralesModel model = new ParametrosGeneralesModel(); 
             model.Entity = entity; 
             model.Entity.IsNew = true; 
             this.EditModel(model); 
             if(ModelState.IsValid) 
                 return Ok(ModelState); 
             else 
                 return BadRequest(ModelState); 
        } 

        [HttpPost] 
        public IActionResult ModifyInGrid(int key, string values) 
        { 
             ParametrosGenerales entity = Manager().GetBusinessLogic<ParametrosGenerales>().FindById(x => x.Id == key, false); 
             JsonConvert.PopulateObject(values, entity); 
             ParametrosGeneralesModel model = new ParametrosGeneralesModel(); 
             model.Entity = entity; 
             model.Entity.IsNew = false; 
             this.EditModel(model); 
             if(ModelState.IsValid) 
                 return Ok(ModelState); 
             else 
                 return BadRequest(ModelState); 
        } 

        [HttpPost]
        public void DeleteInGrid(int key)
        { 
             ParametrosGenerales entity = Manager().GetBusinessLogic<ParametrosGenerales>().FindById(x => x.Id == key, false); 
             Manager().GetBusinessLogic<ParametrosGenerales>().Remove(entity); 
        } 

        */
        #endregion


        [HttpPost]
        public IActionResult CambiarEstadoAtencion(long consecutivoCita, long consecutivoAdmision, string detalleAnulacion)
        {
            try
            {
                if (consecutivoCita <= 0 || consecutivoAdmision <= 0)
                    throw new Exception($"Los consecutivos no son correctos. Cita: {consecutivoCita} - Admision: {consecutivoAdmision}");
                if (string.IsNullOrWhiteSpace(detalleAnulacion))
                    throw new Exception($"El motivo de anulaciÛn es obligatorio.");

                var admision = Manager().GetBusinessLogic<Admisiones>().FindById(x => x.Consecutivo == consecutivoAdmision, true);
                if (admision == null)
                    throw new Exception($"La admision No. {consecutivoAdmision} no existe.");
                if (admision.ProgramacionCitas.Consecutivo != consecutivoCita)
                    throw new Exception($"La admision No. {consecutivoAdmision} no corresponde a la cita No. {consecutivoCita}. Por favor verifique los consecutivos.");

                var atencion = Manager().GetBusinessLogic<Atenciones>().FindById(x => x.Admisiones.Consecutivo == consecutivoAdmision, true);
                if (atencion == null)
                    throw new Exception($"La admision No. {consecutivoAdmision} no tiene una atencion realizada aun.");
                if (atencion.EstadosId == 10077 && admision.EstadosId == 10079 && admision.ProgramacionCitas.EstadosId == 10078)
                    throw new Exception($"La atenciÛn, admisiÛn y cita ya se encuentra en estado \"Anulada en AtenciÛn\".");

                atencion.DetalleAnulacion = detalleAnulacion;
                atencion.LastUpdate = DateTime.Now;
                atencion.UpdatedBy = User.Identity.Name;
                Manager().AtencionesBusinessLogic().AnularAtencion(atencion);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.GetFrontFullErrorMessage());
            }
        }

        [HttpPost]
        public IActionResult CambiarEstadoHC(string consecutivoHC, string detalleAnulacionHC)
        {
            try
            {
                if (consecutivoHC == "" || consecutivoHC == null)
                    throw new Exception($"El consecutivo no es correcto. Historia cl√≠nica Nro.: {consecutivoHC}");
                if (string.IsNullOrWhiteSpace(detalleAnulacionHC))
                    throw new Exception($"El motivo de anulaci√≥n es obligatorio.");

                var historiaClinica = Manager().GetBusinessLogic<HistoriasClinicas>().FindById(x => x.Consecutivo == consecutivoHC, true);
                if (historiaClinica == null)
                    throw new Exception($"La historia cl√≠nica No. {consecutivoHC} no existe.");

                if (historiaClinica.EstadosId == 19)
                    throw new Exception($"No es posible anular la historia cl√≠nica porque se encuentra en estado \"Cerrada\".");

                if (historiaClinica.EstadosId == 81)
                    throw new Exception($"La historia cl√≠nica No. {consecutivoHC} ya se encuentra anulada.");

                if (historiaClinica.Atenciones.EstadosId != 10077)
                    throw new Exception($"Para anular la historia cl√≠nica No. {consecutivoHC} debe anular la atenci√≠n del paciente desde Atenciones.");

                historiaClinica.DetalleAnulacion = detalleAnulacionHC;
                historiaClinica.LastUpdate = DateTime.Now;
                historiaClinica.UpdatedBy = User.Identity.Name;
                Manager().HistoriasClinicasBusinessLogic().AnularHC(historiaClinica);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.GetFrontFullErrorMessage());
            }
        }
    }
}
