using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Workstatus;
using SAM.Web.Common;
using SAM.Web.Shop.Models;
using SAM.Web.Shop.Resources.Models.Workstatus;
using SAM.Web.Shop.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SAM.Web.Shop.Controllers
{
    public class ShipmentOutController : AuthenticatedController
    {
        public ShipmentOutController(INavigationContext navContext) : base(navContext) { }

        #region Salida
        [HttpGet]
        public ActionResult Index(ShipmentOutModel model)
        {
            model.YardId = NavContext.GetCurrentYard().ID;
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveOut(ShipmentOutModel model)
        {
            model.YardId = NavContext.GetCurrentYard().ID;
            bool exit = false;
            if (ModelState.IsValid && ValidaModelOut(model))
            {
                try
                {
                    EmbarqueBO.Instance.GuardarFechaEmbarque(model.ShipmentId, model.DateReal, SessionFacade.UserId);
                    exit = true;
                }
                catch (ExcepcionEmbarque e)
                {
                    ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault());
                    model.Exit = exit;
                    return View("Index", model);
                }
            }

            model.Exit = exit;
            return View("Index", model);
        }

        #endregion
        private bool ValidaModelOut(ShipmentOutModel model)
        {
            bool isValid = true;
            if (model.YardId < 1)
            {
                ModelState.AddModelError(string.Empty, WorkstatusString.Yard_Required_ErrorMessage);
                isValid = false;
            }

            if (model.ShipmentId < 1)
            {
                ModelState.AddModelError(string.Empty, WorkstatusString.Shipment_Required_ErrorMessage);
                isValid = false;
            }
            return isValid;
        }

        public ActionResult DeleteShipmentResults(int id, ShipmentOutModel model)
        {
            return View("ShipmentResults", model);
        }
    }
}
