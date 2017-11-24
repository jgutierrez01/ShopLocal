using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SAM.Web.Classes
{
    public static class JsUtils
    {

        /// <summary>
        /// Emite javascript a la página para forzar un logout.
        /// Manda llamar a la función de la página padre Sam.Usuarios.Logout() de JS.
        /// </summary>
        /// <param name="pagina"></param>
        public static void RegistraScriptLogoutDesdePopup(Page pagina)
        {
            pagina.ClientScript.RegisterStartupScript(  typeof(Page), 
                                                        "ScriptLogout",
                                                        "$(function(){Sam.Popup.VentanaPadre().Sam.Usuarios.Logout();});",
                                                        true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagina"></param>
        public static void RegistraScriptActualizaGridCruce(Page pagina)
        {
            pagina.ClientScript.RegisterStartupScript(typeof(Page),
                                                      "ScriptActualizaGridCruce",
                                                      "$(function(){Sam.Popup.VentanaPadre().Sam.Produccion.OrdenDeTrabajoGenerada();});",
                                                      true);                    
        }

        public static void RegistraScriptActualizaGridCruceRevisiones(Page pagina)
        {
            pagina.ClientScript.RegisterStartupScript(typeof(Page),
                                                      "ScriptActualizaGridCruce",
                                                      "$(function(){Sam.Popup.VentanaPadre().Sam.Produccion.OrdenDeTrabajoGenerada();});",
                                                      true);
        }

        public static void RegistrarScriptActualizarGridListadoDespachos(Page pagina)
        {
            pagina.ClientScript.RegisterStartupScript(typeof(Page),
                                                        "ScriptActualizaGridListadoDespacho",
                                                        "$(function(){Sam.Popup.VentanaPadre().Sam.Workstatus.EdicionEspecialDespachoTerminada();});",
                                                        true);
        }

        /// <summary>
        /// Redirecciona a la alta de los números unicos recién generados.
        /// </summary>
        /// <param name="pagina"></param>
        public static void RegistraScriptRedirectAltaNumUnico(Page pagina, int numeroUnicoID, int cantidadNumUnicos)
        {
            string parametros = string.Format("{0},{1}", numeroUnicoID, cantidadNumUnicos);
            pagina.ClientScript.RegisterStartupScript(typeof(Page),
                                                      "RedirectAltaNumUnico",
                                                      "$(function(){Sam.Popup.VentanaPadre().Sam.Materiales.AltaNumerosUnicos("+parametros+");});",
                                                      true);
        }

        public static void RegistraScriptActualizaGridNumeroUnico(Page pagina)
        {
            pagina.ClientScript.RegisterStartupScript(typeof(Page),
                                                      "ActualizaGridNumeroUnico",
                                                      "$(function(){Sam.Popup.VentanaPadre().Sam.Materiales.NumeroUnicoEditado();});",
                                                      true);
        }

        public static void RegistraScriptActualizaHoldIngenieria(Page pagina)
        {
            pagina.ClientScript.RegisterStartupScript(typeof(Page),
                                                      "ActualizaHoldIngenieria",
                                                      "$(function(){Sam.Popup.VentanaPadre().Sam.Ingenieria.HoldIngenieria();});",
                                                      true);
        }

        public static void RegistraScriptActualizaRequisicionPinturaNumUnico(Page pagina)
        {
            pagina.ClientScript.RegisterStartupScript(typeof(Page),
                                                       "ActualizaReqPinturaNumUnico",
                                                        "$(function(){Sam.Popup.VentanaPadre().Sam.Materiales.ActualizaReqPinturaNumUnico();});",
                                                        true);
        }

        public static void RegistraScriptActualizaPinturaNumUnico(Page pagina)
        {
            pagina.ClientScript.RegisterStartupScript(typeof(Page),
            "ActualizaPinturaNumeroUnico",
            "$(function(){Sam.Popup.VentanaPadre().Sam.Materiales.ActualizaPinturaNumUnico();});",
                                                        true);
        }

        public static void RegistraScriptActualizaCortesDeAjuste(Page pagina)
        {
            pagina.ClientScript.RegisterStartupScript(typeof(Page),
                                                      "ActualizaCortesDeAjuste",
                                                      "$(function(){Sam.Popup.VentanaPadre().Sam.Ingenieria.HoldIngenieria();});",
                                                      true);
        }

       
        public static void RegistraScriptActualizaGridGenerico(Page pagina)
        {
            pagina.ClientScript.RegisterStartupScript(typeof(Page),
                                                      "ScriptActualizaGridGenerico",
                                                      "$(function(){Sam.Popup.VentanaPadre().Sam.Workstatus.ActualizaGrid();});",
                                                      true);
        }

        public static void RegistrarScriptActualizarGridPerzonalizado(Page pagina)
        {
            pagina.ClientScript.RegisterStartupScript(typeof(Page),
                                                        "ScriptActualizarPerzonalizado",
                                                        "$(function(){Sam.Popup.VentanaPadre().Sam.Workstatus.ActualizaGridPersonalizado();});",
                                                        true);
        }

        public static void RegistraScriptActualizayCierraJuntaAdicional(Page pagina)
        {
            pagina.ClientScript.RegisterStartupScript(typeof(Page),
                                                       "ScriptCierraVentana",
                                                       "$(function(){Sam.Popup.VentanaPadre().Sam.Ingenieria.ActualizayCierra();});",
                                                       true);
        }

        public static void RegistraScriptCierraVentana(Page pagina)
        {
            pagina.ClientScript.RegisterStartupScript(typeof(Page),
                                                       "ScriptCierraVentana",
                                                       "$(function(){Sam.Popup.VentanaPadre().Sam.Ingenieria.Cierra();});",
                                                       true);
        }

        public static void RegistraScriptActualizayCierraVentana(Page pagina)
        {
           pagina.ClientScript.RegisterStartupScript(typeof(Page),
                                                      "ScriptCierraVentana",
                                                      "$(function(){Sam.Popup.VentanaPadre().Sam.Workstatus.ActualizayCierra();});",
                                                      true);
        }

        //Se utiliza para abrir el popUp en caso de que no se hayan encontrado
        //los valores de Peq, KgTeorico, Esp que se esperaban actualizar
        public static void RegistraScriptAbrePopUpDatosNoEncontrados(Page pagina)
        {
            pagina.ClientScript.RegisterStartupScript(typeof(Page),
                                                        "AbrePopUpDatosNoEncontrados",
                                                        "$(function(){Sam.Ingenieria.AbrePopUpDatosNoEncontrados();};)",
                                                        true);
        }

        //public static void RegistraScriptActualizaGridPendientes(Page pagina)
        //{
        //    pagina.ClientScript.RegisterStartupScript(typeof(Page),
        //                                              "ScriptActualizaGridPendientes",
        //                                              "$(function(){Sam.Popup.VentanaPadre().Sam.Administracion.ActualizaGrdPendientes();});",
        //                                              true);
        //}
    }
}