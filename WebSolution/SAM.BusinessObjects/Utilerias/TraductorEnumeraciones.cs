using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using Mimo.Framework.Common;

namespace SAM.BusinessObjects.Utilerias
{
    public static class TraductorEnumeraciones
    {
        public static string TextoTecnicaSoldador(int tecnica)
        {
            if(tecnica == (int)TecnicaSoldadorEnum.Raiz)
            {
                return TextoTecnicaSoldador(TecnicaSoldadorEnum.Raiz);
            }
            return TextoTecnicaSoldador(TecnicaSoldadorEnum.Relleno);
        }

        public static string TextoTecnicaSoldador(TecnicaSoldadorEnum tecnica)
        {
            if (tecnica == TecnicaSoldadorEnum.Raiz)
            {
                return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Root" : "Raíz";
            }

            return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Fill" : "Relleno";
        }

        /// <summary>
        /// En base a la enumeración de los estatus posibles para la orden de trabajo
        /// regresa el texto en inglés o en español dependiendo del idioma seleccionado por el usuario.
        /// </summary>
        /// <param name="estatus">Estatus de la ODT</param>
        /// <returns>Texto internacionalizado</returns>
        public static string TextoEstatusOrdenDeTrabajo(EstatusOrdenDeTrabajo estatus)
        {
            if (estatus == EstatusOrdenDeTrabajo.Activa)
            {
                return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Active" : "Activa";
            }

            return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Cancelled" : "Cancelada";
        }

        /// <summary>
        /// En base a la enumeración de los estatus posibles para un despacho
        /// regresa el texto en inglés o en español dependiendo del idioma seleccionado por el usuario.
        /// </summary>
        /// <param name="estatus">Estatus del despacho</param>
        /// <returns>Texto internacionalizado</returns>
        public static string TextoEstatusDespacho(EstatusDespachoOdt estatus)
        {
            switch (estatus)
            {
                case EstatusDespachoOdt.Despachada:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Yes" : "Sí";
                case EstatusDespachoOdt.Parcial:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Partial" : "Parcial";
                default:
                    return "No";
            }
        }

        /// <summary>
        /// Regresa Sí/No en inglés o en español para un valor booleano.
        /// </summary>
        /// <param name="valor">Valor booleano a traducir</param>
        /// <returns>Sí/No internacionalizado</returns>
        public static string TextoSiNo(bool valor)
        {
            if (valor == null)
            { valor = false; }

            if(LanguageHelper.CustomCulture == LanguageHelper.INGLES)
            {
                return  valor ? "Yes" : "No";
            }
            return valor ? "Sí" : "No";
        }

        /// <summary>
        /// Regresa el texto internacionalizado del estatus de un número único.
        /// Se basa en unas constantes que a su vez pertecen a una estructura y hacen match con un check-constraint
        /// en la BD.
        /// </summary>
        /// <param name="estatus">Estatus del número único como viene de BD</param>
        /// <returns>Estatus traducido al inglés/español</returns>
        public static string TextoEstatusNumeroUnico(string estatus)
        {
            switch (estatus)
            {
                case EstatusNumeroUnico.CONDICIONADO:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Conditioned" : "Condicionado";
                case EstatusNumeroUnico.RECHAZADO:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Rejected" : "Rechazado";
                default:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Approved" : "Aprobado";
            }
        }

        public static string TextoAprobadoRechazado(bool estatus)
        {
            switch (estatus)
            {
                case true:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Approved" : "Aprobado";
                case false:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Rejected" : "Rechazado";
                default:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Approved" : "Aprobado";
            }
        }

        /// <summary>
        /// Regresa el texto internacionalidado al tipo de material
        /// </summary>
        /// <param name="tipoMaterial">ID del tipo de material</param>
        /// <returns>Tipo de material traducido al ingles/español</returns>
        public static string TextoTipoMaterial(int tipoMaterial)
        {
            switch (tipoMaterial)
            {
                case (int)TipoMaterialEnum.Accessorio:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Accessory" : "Accesorio";
                case (int)TipoMaterialEnum.Tubo:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Pipe" : "Tubo";
                default:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Pipe" : "Tubo";
            }
        }

        /// <summary>
        /// El texto no es 100% igual al estatus, está pensado para el Grid de despachos.
        /// </summary>
        /// <param name="estatus">Valor de la enumeración de despachos para un material</param>
        /// <returns>Texto en inglés o en español dependiendo de la preferencia del usuario</returns>
        public static string TextoEstatusMaterialDespacho(EstatusMaterialDespacho estatus)
        {
            switch (estatus)
            {
                case EstatusMaterialDespacho.SinOdt:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Not included" : "No incluído";
                
                case EstatusMaterialDespacho.Despachado:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Dispatched" : "Despachado";
                
                case EstatusMaterialDespacho.AccesorioCongelado:
                case EstatusMaterialDespacho.TuboCongelado:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Frozen" : "Congelado";
                
                case EstatusMaterialDespacho.AccesorioNoCongelado:
                case EstatusMaterialDespacho.TuboNoCongelado:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Not frozen" : "No congelado";
                
                default:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Cut" : "Cortado";
            }
        }

        /// <summary>
        /// Pensado para los resultados de las pruebas/reportes
        /// </summary>
        /// <param name="aprobado">boleano que indica si se aprobo la prueba</param>
        /// <returns></returns>
        public static string TextoAprobadoONoAprobado(bool aprobado)
        {
            if (aprobado)
            {
                return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Passed" : "Aprobado";
            }
            
            return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Not Passed" : "No Aprobado";
            
        }

        /// <summary>
        /// Pensado para los resultados de las pruebas/reportes
        /// </summary>
        /// <param name="aprobado">boleano que indica si se aprobo la prueba</param>
        /// <returns></returns>
        public static string TextoAprobadoONoAprobado(bool? aprobado)
        {
            return !aprobado.HasValue ? string.Empty : TextoAprobadoONoAprobado(aprobado.Value);
        }

        /// <summary>
        /// Regresa Aprobado / Cancelado en inglés o en español para un valor booleano.
        /// </summary>
        /// <param name="valor">Valor booleano a traducir</param>
        /// <returns>Aprobado / Cancelado internacionalizado</returns>
        public static string TextoCanceladoAprobado(bool valor)
        {
            if (valor)
            {
                return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Approved" : "Aprobado";
            }
            else
            {
                return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Cancelled" : "Cancelado";
            }
        }

        /// <summary>
        /// Regresa Activo / Cancelado en inglés o en español para un valor booleano.
        /// </summary>
        /// <param name="valor">Valor booleano a traducir</param>
        /// <returns>Activo / Cancelado internacionalizado</returns>
        public static string TextoCanceladoActivo(bool valor)
        {
            if (valor)
            {
                return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Active" : "Activo";
            }
            else
            {
                return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Cancelled" : "Cancelado";
            }
        }

        /// <summary>
        /// Regresa activo/inactivo en el idioma correspondiente en base al valor booleano.
        /// </summary>
        /// <param name="valor">activo = 1, inactivo = 0</param>
        /// <returns>Activo/Inactivo en el idioma correspondiente</returns>
        public static string TextoActivoInactivo(bool valor)
        {
            if (valor)
            {
                return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Active" : "Activo";
            }
            else
            {
                return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Inactive" : "Inactivo";
            }
        }


        public static string TextoTipoHold(string tipoHold)
        {
            switch (tipoHold.ToUpper())
            {
                case TipoHoldSpool.CALIDAD:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Quality Hold" : "Hold Calidad";
                case TipoHoldSpool.INGENIERIA:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Engineer Hold" : "Hold Ingeniería";
                case TipoHoldSpool.CONFINADO:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Confined" : "Confinado";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Regresa el texto internacionalizado del tipo de reportes de la inspeccion dimensional
        /// Se basa en unas constantes que a su vez pertecen a una estructura y hacen match con un check-constraint
        /// en la BD.
        /// </summary>
        /// <param name="tipoReporteID">ID del tipo de reporte</param>
        /// <returns>Tipo de reporte traducido al inglés/español</returns>
        public static string TextoTipoReporteDimensional(int tipoReporteID)
        {
            switch (tipoReporteID)
            {
                case (int)TipoReporteDimensionalEnum.Dimensional:
                    return "Dimensional";
                case (int)TipoReporteDimensionalEnum.Espesores:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Thickness" : "Espesores";
                default:
                    return "Dimensional";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="esTubero"></param>
        /// <returns></returns>
        public static string TextoCategoriaPuesto(bool esTubero)
        {
            if (esTubero)
            {
                return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Fitter" : "Tubero"; 
            }

            return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Welder" : "Soldador"; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string TextoEstatusDestajoPersona(bool aprobado)
        {
            if (aprobado)
            {
                return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Approved" : "Aprobado";
            }

            return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Pending" : "Pendiente"; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="estaCerrado"></param>
        /// <returns></returns>
        public static string AbiertoOCerrado(bool estaCerrado)
        {
            if (estaCerrado)
            {
                return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Closed" : "Cerrado";
            }

            return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Open" : "Abierto";
        }

        /// <summary>
        /// En base a la enumeración de los estatus posibles para armado
        /// regresa el texto en inglés o en español dependiendo del idioma seleccionado por el usuario.
        /// </summary>
        /// <param name="estatus">Estatus del armado</param>
        /// <returns>Texto internacionalizado</returns>
        public static string TextoEstatusArmado(EstatusArmado estatus)
        {
            switch (estatus)
            {
                case EstatusArmado.SinODT:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Without Job Order" : "Sin Orden de Trabajo";
                case EstatusArmado.Despachado:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Dispatched" : "Despachado";
                case EstatusArmado.SinDespacho:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Not Dispatched" : "Sin Despacho";
                default:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Fitted" : "Armado";;
            }
        }

        /// <summary>
        /// En base al id de los estatus posibles para armado
        /// regresa el texto en inglés o en español dependiendo del idioma seleccionado por el usuario.
        /// </summary>
        /// <param name="estatus">Estatus del armado</param>
        /// <returns>Texto internacionalizado</returns>
        public static string TextoEstatusArmado(int estatusID)
        {
            switch (estatusID)
            {
                case (int)EstatusArmado.SinODT:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Without Job Order" : "Sin Orden de Trabajo";
                case (int)EstatusArmado.Despachado:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Dispatched" : "Despachado";
                case (int)EstatusArmado.SinDespacho:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Not Dispatched" : "Sin Despacho";
                default:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Fitted" : "Armado"; ;
            }
        }

        /// <summary>
        /// En base al id de los estatus posibles para soldadura
        /// regresa el texto en inglés o en español dependiendo del idioma seleccionado por el usuario.
        /// </summary>
        /// <param name="estatus">Estatus de soldadura</param>
        /// <returns>Texto internacionalizado</returns>
        public static string TextoEstatusSoldadura(int estatusID)
        {
            switch (estatusID)
            {
                case (int)EstatusSoldadura.SinODT:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Without Job Order" : "Sin Orden de Trabajo";
                case (int)EstatusSoldadura.Armado:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Fitted" : "Armado";
                case (int)EstatusSoldadura.SinArmado:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Not fitted" : "Sin Armado";
                case (int)EstatusSoldadura.SinDespacho:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Not Dispatched" : "Sin Despacho";
                default:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Welded" : "Soldado"; ;
            }
        }


        public static string TextoEstatusPeriodoDestajo(EstatusPeriodoDestajo estatus)
        {
            switch (estatus)
            {
                case EstatusPeriodoDestajo.Pendiente:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Pending approvals" : "Aprobaciones pendientes";
                case EstatusPeriodoDestajo.Cerrado:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Closed" : "Cerrado";
                case EstatusPeriodoDestajo.ListoParaCierre:
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Ready for closure" : "Listo para cierre";
                default:
                    return string.Empty;
            }
        }
       
    }
}
