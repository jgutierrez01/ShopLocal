using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SAM.Entities.Cache;
using SAM.Web.Common;
using SAM.Web.Shop.Models;
using SAM.Web.Shop.Utils;
using SAM.BusinessObjects.Workstatus;
using System;
using SAM.BusinessLogic.Utilerias;
using System.Net.Mail;
using SAM.Entities.Busqueda;
using System.Collections.Generic;
using SAM.Web.Shop.Resources.Views.WorkStatus;
using SAM.Common;
using SAM.Entities;
using SAM.BusinessObjects.Administracion;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities.Personalizadas;
using SAM.Entities.Personalizadas.Shop;
using SAM.BusinessObjects.Produccion;
using Newtonsoft.Json;

namespace SAM.Web.Shop.Controllers
{
    public class ControlsController : AuthenticatedController
    {
        public ControlsController(INavigationContext navContext) : base(navContext) { }


        [ChildActionOnly]
        public ActionResult ProjectDropDown(DropDownConfiguration config)
        {
            PatioCache yard = NavContext.GetCurrentYard();
            DropDownItem[] items =
                (from project in UserScope.MisProyectos
                 orderby project.Text
                 where project.PatioID == yard.ID
                 select new DropDownItem
                 {
                     Text = project.Nombre,
                     Id = project.ID.ToString(CultureInfo.CurrentCulture),
                     Attributes = new
                     {
                         WorkOrderPrefix = project.PrefijoOdt,
                         UniqueNumberPrefix = project.PrefijoNumeroUnico
                     }
                 }).ToArray();
            DropDownModel model = new DropDownModel(config)
            {
                Items = items
            };
            if (config.HasErrors)
            {
                config.Errors.ForEach(e => ModelState.AddModelError(config.Name, e.ErrorMessage));
            }
            return PartialView("_DropDown", model);
        }


        [ChildActionOnly]
        public ActionResult YardBreadcrumb()
        {
            PatioCache yard = NavContext.GetCurrentYard();
            BreadcrumbModel model = new BreadcrumbModel
            {
                Items = new[]
                {
                    new BreadcrumbItem
                    {
                        DisplayText = yard.Nombre
                    }
                }
            };


            return PartialView("_Breadcrumb", model);
        }


        [ChildActionOnly]
        public ActionResult ProjectBreadcrumb()
        {
            PatioCache yard = NavContext.GetCurrentYard();
            ProyectoCache project = NavContext.GetCurrentProject();


            BreadcrumbModel model = new BreadcrumbModel
            {
                Items = new[]
                {
                    new BreadcrumbItem { DisplayText = yard.Nombre, Link = Url.Action("Index", "Yard", new { yardId = yard.ID})},
                    new BreadcrumbItem { DisplayText = project.Nombre}
                }
            };


            return PartialView("_Breadcrumb", model);
        }


        [ChildActionOnly]
        public ActionResult ControlNumberBreadcrumb()
        {
            PatioCache yard = NavContext.GetCurrentYard();
            ProyectoCache project = NavContext.GetCurrentProject();
            ControlNumberCache controlNumber = NavContext.GetCurrentControlNumber();


            BreadcrumbModel model = new BreadcrumbModel
            {
                Items = new[]
                {
                    new BreadcrumbItem { DisplayText = yard.Nombre, Link = Url.Action("Index", "Yard", new { yardId = yard.ID}),Value = yard.ID},
                    new BreadcrumbItem { DisplayText = project.Nombre, Link = Url.Action("Index", "Project", new { projectId = project.ID}), Value= project.ID},
                    new BreadcrumbItem { DisplayText = controlNumber.ControlNumber, Value= controlNumber.ControlNumberId}
                }
            };


            return PartialView("_Breadcrumb", model);
        }


        [ChildActionOnly]
        public ActionResult ProjectBreadcrumbLabel()
        {
            PatioCache yard = NavContext.GetCurrentYard();
            ProyectoCache project = NavContext.GetCurrentProject();
            ControlNumberCache controlNumber = NavContext.GetCurrentControlNumber();

            BreadcrumbModel model = new BreadcrumbModel
            {
                Items = new[]
                {
                    new BreadcrumbItem { DisplayText = yard.Nombre, Value=yard.ID},
                    new BreadcrumbItem { DisplayText = project.Nombre, Link = Url.Action("Index","Yard", new {UpdateYardId= yard.ID, projectId = project.ID}), Value=project.ID},
                    new BreadcrumbItem { Value= controlNumber.ControlNumberId}                 
                }
            };

            return PartialView("_Breadcrumb", model);
        }

        [ChildActionOnly]
        public ActionResult ShipmentDropDown(DropDownConfiguration config)
        {
            PatioCache yard = NavContext.GetCurrentYard();


            DropDownItem[] items =
                (from shipment in EmbarqueBO.Instance.ObtenerNumerosDeEmbarquePorPatio(yard.ID)
                 select new DropDownItem
                 {
                     Text = shipment.NumeroEmbarque,
                     Id = shipment.EmbarqueID.ToString(CultureInfo.CurrentCulture)


                 }).ToArray();


            DropDownModel model = new DropDownModel(config)
            {
                Items = items
            };


            if (config.HasErrors)
            {
                config.Errors.ForEach(e => ModelState.AddModelError(config.Name, e.ErrorMessage));
            }


            return PartialView("_DropDown", model);
        }


        [ChildActionOnly]
        public ActionResult TypeReportDropDown(DropDownConfiguration config)
        {



            DropDownItem[] items =
                (from type in TypeReport.GetTypesReport()
                 select new DropDownItem
                 {
                     Text = type.Valor,
                     Id = type.ID.ToString()


                 }).ToArray();


            DropDownModel model = new DropDownModel(config)
            {
                Items = items
            };



            return PartialView("_DropDown", model);
        }


        [ChildActionOnly]
        public ActionResult QuadrantsDropDown(DropDownConfiguration config)
        {
            int patioId;


            try
            {
                patioId = NavContext.GetCurrentYard().ID;
            }
            catch (Exception e)
            {
                patioId = 0;
            }


            DropDownItem[] items =
                (from type in CuadranteBO.Instance.ObtenerCuadrantesPorPatio(patioId)
                 select new DropDownItem
                 {
                     Text = type.Nombre,
                     Id = type.CuadranteID.ToString()


                 }).ToArray();


            DropDownModel model = new DropDownModel(config)
            {
                Items = items
            };




            return PartialView("_DropDown", model);
        }


        [HttpGet]
        public JsonResult UpdateProjetId(int ID)
        {
            NavContext.SetProject(ID);
            NavContext.SetProjectEdit(ID);
            var myData = new[] { new { result = "OK" } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UpdateCuadranteID(string CuadranteID)
        {
            NavContext.setCuadranteID(CuadranteID);
            var myData = new[] { new { result = "OK" } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }
     
        [HttpGet]
        public JsonResult LimpiarGrid(int ProyectoID)
        {            
            NavContext.SetNumbersControl("");
            NavContext.SetNumbersControlCuadranteSQ("");
            NavContext.SetNumbersControlCuadranteSQEditar("");
            NavContext.SetProjectEdit(ProyectoID);
            var mydata = new[] { new { result = "OK" } };
            return Json(mydata, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LimpiarGridAdd(int ProyectoID)
        {
            NavContext.SetNumbersControl("");
            NavContext.SetNumbersControlCuadranteSQ("");
            NavContext.SetNumbersControlCuadranteSQEditar("");
            NavContext.SetProject(ProyectoID);
            var mydata = new[] { new { result = "OK" } };
            return Json(mydata, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarSpoolAdd(string NumeroControl, string ProyectoID, string SQ)
        {
            List<LayoutGridSQ> listaNcSQ = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd"));
            LayoutGridSQ ncActualSQ = listaNcSQ.Where(x => x.NumeroControl == NumeroControl).FirstOrDefault();
            if (ncActualSQ != null)
            {
                listaNcSQ.Remove(ncActualSQ);
                if(ProyectoID != null)
                {
                    OrdenTrabajoSpoolBO.Instance.EliminarSpool(NumeroControl, int.Parse(ProyectoID), SQ);
                }                
            }
            NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", JsonConvert.SerializeObject(listaNcSQ));
            var mydata = new[] { new { result = "OK" } };
            return Json(mydata, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarSpoolEdit(string NumeroControl, string ProyectoID, string SQ)
        {
            List<LayoutGridSQ> listaNcSQ = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit"));
            LayoutGridSQ ncActualSQ = listaNcSQ.Where(x => x.NumeroControl == NumeroControl).FirstOrDefault();
            if (ncActualSQ != null)
            {
                listaNcSQ.Remove(ncActualSQ);
                if (ProyectoID != null)
                {
                    OrdenTrabajoSpoolBO.Instance.EliminarSpool(NumeroControl, int.Parse(ProyectoID), SQ);
                }
            }
            NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", JsonConvert.SerializeObject(listaNcSQ));
            var mydata = new[] { new { result = "OK" } };
            return Json(mydata, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult UpdateYardId(int ID)
        {
            NavContext.SetYard(ID);
            var myData = new[] { new { result = "OK" } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SendEmail(string process)
        {
            string msg = WorkStatusStrings.EmailSend;

            string detalle = process + Environment.NewLine + Environment.NewLine;
            detalle += GetDetailReport();

            MailMessage mmsg = new MailMessage();
            Usuario usu = UsuarioBO.Instance.Obtener(SessionFacade.UserId);
            //mmsg.To.Add("genoveva.torres@definityfirst.com");                     
            mmsg.To.Add(usu.Email);

            mmsg.Subject = WorkStatusStrings.SubjectSendReport;
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;
            mmsg.Body = detalle;
            mmsg.BodyEncoding = System.Text.Encoding.UTF8;
            mmsg.IsBodyHtml = false;

            mmsg.From = new System.Net.Mail.MailAddress(Configuracion.CuentaCorreo);
            SmtpClient cliente = new SmtpClient();
            cliente.Credentials = new System.Net.NetworkCredential(Configuracion.UsuarioCorreo, Configuracion.PasswordCorreo);
            //cliente.UseDefaultCredentials = true;
            cliente.Port = Configuracion.PuertoCorreo;
            cliente.EnableSsl = true;

            cliente.Host = Configuracion.HostCorreo;

            try
            {
                cliente.Send(mmsg);

            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        cliente.Send(mmsg);
                    }
                    else
                    {
                        Console.WriteLine("Failed to deliver message to {0}",
                        ex.InnerExceptions[i].FailedRecipient);
                    }
                }
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Console.WriteLine("error: " + ex);
                msg = ex.Message;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in RetryIfBusy(): {0}",
                        ex.ToString());
            }

            var myData = new[] { new { result = msg } };

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        private String GetDetailReport()
        {
            String detail = string.Empty;

            string spools = NavContext.GetCurrentNumbersControl();
            List<NumeroControlBusqueda> numberControlNotProcessed = Helps.GeControlNumbersStringToNCB(spools);

            numberControlNotProcessed = numberControlNotProcessed.Where(x => x.TipoNC == TipoNumeroControlEnum.NoCumple).ToList();

            foreach (NumeroControlBusqueda ncb in numberControlNotProcessed)
            {
                detail += ncb.NumeroControl + Environment.NewLine;
            }
            
            return detail;
        }
    }
}
