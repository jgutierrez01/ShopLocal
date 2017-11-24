using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web.Services;
using log4net;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Modelo;
using SAM.Entities.Cache;
using Telerik.Web.UI;
using SAM.BusinessObjects.Utilerias;
using SAM.Web.Classes;
using SAM.Entities;
using SAM.BusinessObjects.Ingenieria;
using SAM.Entities.Personalizadas;
using SAM.Entities.RadCombo;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Produccion;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Catalogos;
using System.ComponentModel;
using System.Web.Script.Services;
using SAM.BusinessObjects.Administracion;

namespace SAM.Web.Webservices
{
    /// <summary>
    /// Summary description for ComboboxWebService
    /// </summary>
    [WebService(Namespace = "http://catalogos.sam.ws/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class ComboboxWebService : WebService
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ComboboxWebService));
        private const int ELEMENTOS_POR_REQUEST = 30;

        /// <summary>
        /// Obtiene los item codes por proyecto en base a lo que el usuario va escribiendo en el combo.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod]
        public IEnumerable<RadItemCode> ListaItemCodesPorProyecto(RadComboBoxContext context)
        {
            int proyectoID = -1;

            if (context.Keys.Contains("ProyectoID"))
            {
                proyectoID = context["ProyectoID"].ToString().SafeIntParse();
            }

            return ItemCodeBO.Instance.ObtenerPorProyectoParaCombo(proyectoID, context.Text, context.NumberOfItems, ELEMENTOS_POR_REQUEST);
        }

        [WebMethod]
        public IEnumerable<RadSpool> ListaTablaSpoolPorProyecto(RadComboBoxContext context)
        {
            int proyectoID = -1;
            int ordenTrabajo = -1;

            if (context.Keys.Contains("ProyectoID"))
            {
                proyectoID = context["ProyectoID"].ToString().SafeIntParse();
            }
            if (context.Keys.Contains("OrdenTrabajoID"))
            {
                ordenTrabajo = context["OrdenTrabajoID"].ToString().SafeIntParse();
            }

            return JuntaCampoBO.Instance.ObtenerSpoolParaCombo(proyectoID, ordenTrabajo, context.Text, context.NumberOfItems, ELEMENTOS_POR_REQUEST);
        }

        /// <summary>
        /// Obtiene los tuberos por proyecto en base a lo que el usuario va escribiendo en el combo.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod]
        public IEnumerable<RadTubero> ListaTuberosPorProyecto(RadComboBoxContext context)
        {
            int proyectoID = -1;
            if (context.Keys.Contains("ProyectoID"))
            {
                proyectoID = context["ProyectoID"].ToString().SafeIntParse();
            }

            return TuberoBO.Instance.ObtenerPorProyectoParaCombo(proyectoID, context.Text, context.NumberOfItems, ELEMENTOS_POR_REQUEST);
        }

        /// <summary>
        /// Obtiene los tuberos por proyecto en base a lo que el usuario va escribiendo en el combo.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod]
        public IEnumerable<RadSoldador> ListaSoldadoresPorProyecto(RadComboBoxContext context)
        {
            int proyectoID = -1;
            if (context.Keys.Contains("ProyectoID"))
            {
                proyectoID = context["ProyectoID"].ToString().SafeIntParse();
            }


            return SoldadorBO.Instance.ObtenerPorProyectoParaCombo(proyectoID, context.Text, context.NumberOfItems, ELEMENTOS_POR_REQUEST);
        }

        /// <summary>
        /// Obtiene las coladas por proyecto en base a lo que el usuario va escribiendo en el combo.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod]
        public IEnumerable<RadColada> ListaColadasPorProyecto(RadComboBoxContext context)
        {
            int proyectoID = -1;
            if (context.Keys.Contains("ProyectoID"))
            {
                proyectoID = context["ProyectoID"].ToString().SafeIntParse();
            }

            int skip = context.NumberOfItems;
            int take = ELEMENTOS_POR_REQUEST;

            return ColadasBO.Instance.ObtenerPorProyectoParaRadCombo(proyectoID, context.Text, skip, take);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod]
        public IEnumerable<MyItemData> ObtenerSpoolsCandidatosParaArmadoCampo(RadComboBoxContext context)
        {
            int proyectoID = -1;
            int spoolID = -1;
            string etiquetaMaterial1 = "";
            string etiquetaMaterial2 = "";

            if (context.Keys.Contains("ProyectoID"))
            {
                proyectoID = context["ProyectoID"].SafeIntParse();
            }
            if (context.Keys.Contains("Spool1"))
            {
                spoolID = context["Spool1"].SafeIntParse();
            }

            if (context.Keys.Contains("EtiquetaMaterial1"))
            {
                etiquetaMaterial1 = context["EtiquetaMaterial1"].SafeStringParse();
            }

            if (context.Keys.Contains("EtiquetaMaterial2"))
            {
                etiquetaMaterial2 = context["EtiquetaMaterial2"].SafeStringParse();
            }

            int skip = context.NumberOfItems;
            int take = ELEMENTOS_POR_REQUEST;

            List<Simple> lst = JuntaCampoBO.Instance.ObtenerSpoolsCandidatosParaArmadoCampo(proyectoID, spoolID, etiquetaMaterial1, etiquetaMaterial2, context.Text, skip, take);

            return lst.ConvertAll<MyItemData>(
                new Converter<Simple, MyItemData>(
                    delegate(Simple simple)
                    {
                        return new MyItemData(simple.Valor, simple.ID.ToString());
                    }));
        }

        /// <summary>
        /// Lista los spools por proyecto
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod]
        public IEnumerable<MyItemData> ListaSpoolsPorProyecto(RadComboBoxContext context)
        {
            int proyectoID = -1;
            if (context.Keys.Contains("ProyectoID"))
            {
                proyectoID = context["ProyectoID"].ToString().SafeIntParse();
            }

            int skip = context.NumberOfItems;
            int take = ELEMENTOS_POR_REQUEST;

            List<Simple> lst = SpoolBO.Instance.ObtenerPorProyectoParaRadCombo(proyectoID, context.Text, skip, take);

            return lst.ConvertAll<MyItemData>(
                new Converter<Simple, MyItemData>(
                    delegate(Simple simple)
                    {
                        return new MyItemData(simple.Valor, simple.ID.ToString());
                    }));
        }
        /// <summary>
        /// Lista el historico spools por proyecto
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod]
        public IEnumerable<MyItemData> ListaHistoricoSpoolsPorProyecto(RadComboBoxContext context)
        {
            int proyectoID = -1;
            if (context.Keys.Contains("ProyectoID"))
            {
                proyectoID = context["ProyectoID"].ToString().SafeIntParse();
            }

            int skip = context.NumberOfItems;
            int take = ELEMENTOS_POR_REQUEST;

            List<Simple> lst = SpoolBO.Instance.ObtenerPorProyectoHistoricoSpoolsParaRadCombo(proyectoID, context.Text, skip, take);

            return lst.ConvertAll<MyItemData>(
                new Converter<Simple, MyItemData>(
                    delegate(Simple simple)
                    {
                        return new MyItemData(simple.Valor, simple.ID.ToString());
                    }));
        }

        /// <summary>
        /// Lista los consumibles por patio
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod]
        public IEnumerable<MyItemData> ListaConsumiblesPorPatio(RadComboBoxContext context)
        {
            int patioID = -1;
            if (context.Keys.Contains("PatioID"))
            {
                patioID = context["PatioID"].ToString().SafeIntParse();
            }

            int skip = context.NumberOfItems;
            int take = ELEMENTOS_POR_REQUEST;


            List<Simple> lst = ConsumiblesBO.Instance.ObtenerPorPatioParaRadCombo(patioID, context.Text, skip, take);

            return lst.ConvertAll<MyItemData>(
                new Converter<Simple, MyItemData>(
                    delegate(Simple simple)
                    {
                        return new MyItemData(simple.Valor, simple.ID.ToString());
                    }));
        }

        /// <summary>
        /// Lista Numeros de Control por ODT
        /// Y que contengan materiales afines al numero unico recibido.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod]
        public IEnumerable<MyItemData> ListaNumerosControlAfinesANumeroUnico(RadComboBoxContext context)
        {

            int numeroUnicoID = -1;
            int ordenTrabajoID = -1;
            string segmento = string.Empty;

            if (context.ContainsKey("NumeroUnicoID"))
            {
                numeroUnicoID = context["NumeroUnicoID"].SafeIntParse();
            }

            if (context.ContainsKey("OrdenTrabajoID"))
            {
                ordenTrabajoID = context["OrdenTrabajoID"].SafeIntParse();
            }

            if (context.ContainsKey("Segmento"))
            {
                segmento = context["Segmento"].ToString();
            }

            int skip = context.NumberOfItems;
            int take = ELEMENTOS_POR_REQUEST;

            List<Simple> lst = OrdenTrabajoSpoolBO.Instance.ObtenerAfinesANumeroUnico(numeroUnicoID, segmento, ordenTrabajoID, context.Text, skip, take);

            return lst.ConvertAll<MyItemData>(
                new Converter<Simple, MyItemData>(
                    delegate(Simple simple)
                    {
                        return new MyItemData(simple.Valor, simple.ID.ToString());
                    }));
        }

        #region Por User Scope
        /// <summary>
        /// Lista Numeros de Control por proyecto y/o ODT
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public IEnumerable<MyItemData> ListaNumerosControlPorUserScope(RadComboBoxContext context)
        {
            int? ordenTrabajoID = UtileriasRadCombo.ObtenId(context, "OrdenTrabajoID");
            int? proyectoID = UtileriasRadCombo.ObtenId(context, "ProyectoID");

            List<Simple> lst =
                OrdenTrabajoSpoolBO.Instance
                                   .ObtenerNumerosDeControlPorPermiso(proyectoID,
                                                                        ordenTrabajoID,
                                                                        context.NumberOfItems,
                                                                        ELEMENTOS_POR_REQUEST,
                                                                        context.Text,
                                                                        SessionFacade.EsAdministradorSistema,
                                                                        SessionFacade.UserId)
                                   .ToList();

            return lst.ConvertAll<MyItemData>(
                            new Converter<Simple, MyItemData>(
                                delegate(Simple simple)
                                {
                                    return new MyItemData(simple.Valor, simple.ID.ToString());
                                }));
        }

        [WebMethod(EnableSession = true)]
        public IEnumerable<MyItemData> ListaCuadrantesPorProyecto(RadComboBoxContext context)
        {
            int? proyectoID = UtileriasRadCombo.ObtenId(context, "ProyectoID");
            List<Simple> lst = CuadranteBO.Instance.ObtenerCuadrantesProyecto(proyectoID,context.NumberOfItems,ELEMENTOS_POR_REQUEST).ToList();
            
            return lst.ConvertAll<MyItemData>(
                new Converter<Simple,MyItemData>(
                    delegate(Simple simple)
                    {
                        return new MyItemData(simple.Valor, simple.ID.ToString());
                    }));
        }

        /// <summary>
        /// Obtiene las ordenes de trabajo por un proyecto en especifico o  de todos los proyectos a los que el usuario tiene acceso
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public IEnumerable<MyItemData> ListaOrdenTrabajoPorUserScope(RadComboBoxContext context)
        {
            int? proyectoID = UtileriasRadCombo.ObtenId(context, "ProyectoID");

            List<Simple> lst =
                OrdenTrabajoBO.Instance
                              .ObtenerOdtsPorPermiso(proyectoID,
                                                        context.NumberOfItems,
                                                        ELEMENTOS_POR_REQUEST,
                                                        context.Text,
                                                        SessionFacade.EsAdministradorSistema,
                                                        SessionFacade.UserId)
                              .ToList();

            return lst.ConvertAll<MyItemData>(
                            new Converter<Simple, MyItemData>(
                                delegate(Simple simple)
                                {
                                    return new MyItemData(simple.Valor, simple.ID.ToString());
                                }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public IEnumerable<MyItemData> ListaOrdenTrabajoPorProyecto(RadComboBoxContext context)
        {
            int? proyectoID = UtileriasRadCombo.ObtenId(context, "ProyectoID");

            List<Simple> lst =
                OrdenTrabajoBO.Instance
                              .ObtenerOdtsPorProyecto(proyectoID,
                                                        context.NumberOfItems,
                                                        ELEMENTOS_POR_REQUEST,
                                                        context.Text,                                                        
                                                        SessionFacade.UserId)
                              .ToList();

            return lst.ConvertAll<MyItemData>(
                            new Converter<Simple, MyItemData>(
                                delegate(Simple simple)
                                {
                                    return new MyItemData(simple.Valor, simple.ID.ToString());
                                }));
        }

        /// <summary>
        /// Obtiene los números unicos de los proyectos a los que el usuario tiene acceso
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public IEnumerable<MyItemData> ListaNumeroUnicoPorUserScope(RadComboBoxContext context)
        {
            int? proyectoID = UtileriasRadCombo.ObtenId(context, "ProyectoID");

            List<Simple> lst =
                NumeroUnicoBO.Instance
                             .ObtenerNumerosUnicosPorPermiso(proyectoID,
                                                                context.NumberOfItems,
                                                                ELEMENTOS_POR_REQUEST,
                                                                context.Text,
                                                                SessionFacade.EsAdministradorSistema,
                                                                SessionFacade.UserId)
                             .ToList();

            return lst.ConvertAll<MyItemData>(
                            new Converter<Simple, MyItemData>(
                                delegate(Simple simple)
                                {
                                    return new MyItemData(simple.Valor, simple.ID.ToString());
                                }));
        }

        /// <summary>
        /// Obtiene los spools a los que el usuario tiene acceso,
        /// que no tengan Orden de Trabajo asignado y que pertenezcan al proyecto
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public IEnumerable<MyItemData> ListaSpoolSinOdt(RadComboBoxContext context)
        {
            int? proyectoID = UtileriasRadCombo.ObtenId(context, "ProyectoID");

            List<Simple> lst =
                NumeroUnicoBO.Instance
                             .ObtenerSpoolSinODT(proyectoID,
                                                                context.NumberOfItems,
                                                                ELEMENTOS_POR_REQUEST,
                                                                context.Text,
                                                                SessionFacade.EsAdministradorSistema,
                                                                SessionFacade.UserId)
                             .ToList();

            return lst.ConvertAll<MyItemData>(
                            new Converter<Simple, MyItemData>(
                                delegate(Simple simple)
                                {
                                    return new MyItemData(simple.Valor, simple.ID.ToString());
                                }));
        }

        /// <summary>
        /// Obtiene los números unicos de los proyectos a los que el usuario tiene acceso
        /// y que al momento tengan inventario congelado
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public IEnumerable<MyItemData> ListaNumeroUnicoPorUserScopeCongelados(RadComboBoxContext context)
        {
            int? proyectoID = UtileriasRadCombo.ObtenId(context, "ProyectoID");
            if (proyectoID > 0)
            {
                List<Simple> lst =
                    NumeroUnicoBO.Instance
                                 .ObtenerNumerosUnicosPorPermisoCongelados(proyectoID,
                                                                    context.NumberOfItems,
                                                                    ELEMENTOS_POR_REQUEST,
                                                                    context.Text,
                                                                    SessionFacade.EsAdministradorSistema,
                                                                    SessionFacade.UserId)
                                 .ToList();

                return lst.ConvertAll<MyItemData>(
                                new Converter<Simple, MyItemData>(
                                    delegate(Simple simple)
                                    {
                                        return new MyItemData(simple.Valor, simple.ID.ToString());
                                    }));
            }
            else
                return null;
        }

        /// <summary>
        /// Regresa el listado de números únicos que hacen match con ItemCode
        /// y diámetro 1 para el número único seleccionado previamente
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public IEnumerable<MyItemData> ListaNumeroUnicoItemCodePorUserScope(RadComboBoxContext context)
        {
            int? _numeroUnico = UtileriasRadCombo.ObtenId(context, "NumeroUnicoID");
            int? _cantCongelada = UtileriasRadCombo.ObtenId(context, "CantCong");

            List<Simple> lst =
                                NumeroUnicoBO.Instance.ObtenerNumerosUnicosMatchPorItemCode(context.NumberOfItems,
                                                                    ELEMENTOS_POR_REQUEST,
                                                                    context.Text,
                                                                    _numeroUnico,
                                                                    _cantCongelada,
                                                                    true).ToList();

            return lst.ConvertAll<MyItemData>(
                            new Converter<Simple, MyItemData>(
                                delegate(Simple simple)
                                {
                                    return new MyItemData(simple.Valor, simple.ID.ToString());
                                }));
        }

        /// <summary>
        /// Regresa el listado de números únicos que hacen match con ItemCode
        /// y diámetro 1 para el Material Spool ID seleccionado previamente
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public IEnumerable<MyItemData> ListaNumeroUnicoItemCodePorMaterialSpoolID(RadComboBoxContext context)
        {
            int? _materialSpool = UtileriasRadCombo.ObtenId(context, "MaterialSpoolID");
            int? _cantCongelada = UtileriasRadCombo.ObtenId(context, "Cantidad");

            List<Simple> lst =
                                NumeroUnicoBO.Instance.ObtenerNumerosUnicosMatchPorItemCode(context.NumberOfItems,
                                                                    ELEMENTOS_POR_REQUEST,
                                                                    context.Text, 
                                                                    _materialSpool,
                                                                    _cantCongelada, 
                                                                    false).ToList();

            return lst.ConvertAll<MyItemData>(
                            new Converter<Simple, MyItemData>(
                                delegate(Simple simple)
                                {
                                    return new MyItemData(simple.Valor, simple.ID.ToString());
                                }));
        }



        #endregion

        /// <summary>
        /// Regresa una lista de spools que son candidatos a agregarse a una orden de trabajo nueva.
        /// Se utiliza en el combo de search as you type para agregar un spool a una ODT existente.
        /// </summary>
        /// <param name="context">Contexto de la llamada JSON del combo de telerik</param>
        /// <returns>Lista conteniendo los spools candidatos</returns>
        [WebMethod]
        public IEnumerable<MyItemData> SpoolsCandidatosParaOdt(RadComboBoxContext context)
        {
            int proyectoID = context["ProyectoID"].SafeIntParse();
            int skip = context.NumberOfItems;
            int take = ELEMENTOS_POR_REQUEST;

            List<Simple> lst = SpoolBO.Instance.ObtenerSpoolsCandidatosParaOdt(proyectoID, context.Text, skip, take);

            return lst.ConvertAll<MyItemData>(
                new Converter<Simple, MyItemData>(
                    delegate(Simple simple)
                    {
                        return new MyItemData(simple.Valor, simple.ID.ToString());
                    }));
        }

        /// <summary>
        /// Obtiene los numeros unicos que sean Tubos
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod]
        public IEnumerable<MyItemData> ListaNumeroUnicoTipoMaterialTubo(RadComboBoxContext context)
        {
            int proyectoID = -1;
            int skip = context.NumberOfItems;
            int take = ELEMENTOS_POR_REQUEST;

            if (context.ContainsKey("ProyectoID"))
            {
                proyectoID = context["ProyectoID"].SafeIntParse();
            }

            List<Simple> lst = NumeroUnicoBO.Instance.ObtenerSoloTubos(proyectoID, context.Text, skip, take);

            return lst.ConvertAll<MyItemData>(
                new Converter<Simple, MyItemData>(
                    delegate(Simple simple)
                    {
                        return new MyItemData(simple.Valor, simple.ID.ToString());
                    }));
        }

        /// <summary>
        /// Obtiene el listado de numeros unicos que son candidatos a una orden de trabajo en especifico
        /// y que ya han sido transferidos a corte
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod]
        public IEnumerable<RadNumeroUnico> ListaNumeroUnicoEnTrasferencia(RadComboBoxContext context)
        {
            int ordenTrabajoID = context["OrdenTrabajoID"].SafeIntParse();

            int skip = context.NumberOfItems;
            int take = ELEMENTOS_POR_REQUEST;

            IEnumerable<RadNumeroUnico> result = NumeroUnicoBO.Instance.ListaNumeroUnicoEnTrasferencia(ordenTrabajoID, context.Text, skip, take);

            return result;
        }

        [WebMethod]
        public IEnumerable<RadNumeroUnicoParaDespacho> NumerosUnicosParaDespachoDeAccesorio(RadComboBoxContext context)
        {
            int materialSpoolID = context["MaterialSpoolID"].SafeIntParse();
            int skip = context.NumberOfItems;
            int take = ELEMENTOS_POR_REQUEST;

            IEnumerable<RadNumeroUnicoParaDespacho> numerosUnicos =
                NumeroUnicoBO.Instance
                             .AccesoriosAfinesParaDespachoOAsignacion(materialSpoolID,
                                                                       context.Text,
                                                                       skip,
                                                                       take,
                                                                       false);

            return numerosUnicos;
        }

        [WebMethod]
        public IEnumerable<RadNumeroUnicoParaDespacho> NumerosUnicosParaAsignacion(RadComboBoxContext context)
        {
            int materialSpoolID = context["MaterialSpoolID"].SafeIntParse();
            int skip = context.NumberOfItems;
            int take = ELEMENTOS_POR_REQUEST;

            IEnumerable<RadNumeroUnicoParaDespacho> numerosUnicos =
                NumeroUnicoBO.Instance
                             .CandidatosParaAsignacion(materialSpoolID,
                                                       context.Text,
                                                       skip,
                                                       take);
            return numerosUnicos;
        }


        /// <summary>
        /// Obtiene los materiales de un numero de control que sean afines al numero unico recibido que aun no cuentan con corte
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [WebMethod]
        public IEnumerable<RadMaterialParaCorte> ListaMaterialesPorNumeroControl(RadComboBoxContext context)
        {
            int skip = context.NumberOfItems;
            int take = ELEMENTOS_POR_REQUEST;
            int ordenTrabajoSpoolID = context["OrdenTrabajoSpoolID"].SafeIntParse();
            int numeroUnicoID = -1;
            string segmento = string.Empty;

            if (context.ContainsKey("NumeroUnicoID"))
            {
                numeroUnicoID = context["NumeroUnicoID"].SafeIntParse();
            }

            if (context.ContainsKey("Segmento"))
            {
                segmento = context["Segmento"].ToString();
            }

            IEnumerable<RadMaterialParaCorte> result = MaterialSpoolBO.Instance.ListaMaterialesPorNumeroControl(ordenTrabajoSpoolID, numeroUnicoID, segmento, context.Text, skip, take);

            return result;
        }


        [WebMethod]
        public IEnumerable<MyItemData> ListaEmpleadosPorProyecto(RadComboBoxContext context)
        {
            int proyectoID = -1;
            int skip = context.NumberOfItems;
            int take = ELEMENTOS_POR_REQUEST;

            if (context.Keys.Contains("ProyectoID"))
            {
                proyectoID = context["ProyectoID"].ToString().SafeIntParse();
            }

            List<RadUsuario> result = UsuarioBO.Instance.ObtenerTodosPorProyecto(proyectoID, context.Text, skip, take);

            return result.ConvertAll<MyItemData>(
               new Converter<RadUsuario, MyItemData>(
                   delegate(RadUsuario usuario)
                   {
                       return new MyItemData(usuario.NombreCompleto, usuario.UsuarioID.ToString());
                   }));
        }


    }

    [Serializable]
    public class MyItemData
    {
        public MyItemData(string text, object value)
        {
            _value = value;
            Text = text;
        }

        private object _value;
        public string Text { get; set; }
        public string Value
        {
            get
            {
                return _value.ToString();
            }
            set
            {
                _value = value;
            }
        }
    }


}
