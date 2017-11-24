using System;
using SAM.Web.Classes;
using SAM.BusinessLogic.Excel;
using Mimo.Framework.Extensions;
using SAM.Entities;
using System.Globalization;
using Mimo.Framework.Common;
using log4net;

namespace SAM.Web.Produccion
{
    public partial class ExportaExcel : SamPaginaConSeguridad
    {

        private static readonly ILog _logger = LogManager.GetLogger(typeof(ExportaExcel));

        protected override void OnLoad(EventArgs e)
        {
            //todos pueden entrar a esta página siempre y cuando estén loggeados
            RevisarSeguridad = false;
            base.OnLoad(e);
        }

        #region parametros
        private string OrdenDeTrabajo
        {
            get
            {
                return Request.Params["Ot"];
            }
        }

        private string NumeroControl
        {
            get
            {
                return Request.Params["Nc"];
            }
        }

        private bool Emb
        {
            get
            {
                return Request.Params["Emb"].SafeBoolParse();
            }
        }

        private bool Rep
        {
            get
            {
                return Request.Params["Rep"].SafeBoolParse();
            }
        }

        private int TJunta
        {
            get
            {
                return Request.Params["Tj"].SafeIntParse();
            }
        }

        private int FAcero
        {
            get
            {
                return Request.Params["Fa"].SafeIntParse();
            }
        }

        private int PsoID
        {
            get
            {
                return Request.Params["PrID"].SafeIntParse();
            }
        }

        private string PsoValor
        {
            get
            {
                return Request.Params["PrValor"];
            }
        }

        private string pytoValor
        {
            get
            {
                return Request.Params["pytoValor"];
            }
        }

        private string Filtros
        {
            get
            {
                return Request.Params["Filtros"];
            }
        }

        private string Columnas
        {
            get
            {
                return Request.Params["Columnas"];
            }
        }

        /*
         *  Parametros de LstNumeroUnico
         */
        private string Colada
        {
            get { return Request.Params["Co"]; }
        }
        private string IC
        {
            get { return Request.Params["Ic"]; }
        }
        private string NuI
        {
            get { return Request.Params["NuI"]; }
        }
        private string NuF
        {
            get { return Request.Params["NuF"]; }
        }
        #endregion        


        protected void Page_Load(object sender, EventArgs e)
        {
            TipoArchivoExcel tipoExcel = (TipoArchivoExcel)Request.QueryString["Type"].SafeIntParse();
            byte[] archivo = null;
            string nombreArchivo = "";

            _logger.Info("crea excel");
            switch (tipoExcel)
            {
                case TipoArchivoExcel.EstimacionJuntas:
                    archivo = ExcelEstimacionJuntas.Instance.ObtenerExcelPorEstimacionID(EntityID.Value);
                    nombreArchivo = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "WeldInvoicing.xlsx" : "EstimadoJuntas.xlsx";
                    break;
                case TipoArchivoExcel.EstimacionSpools:
                    archivo = ExcelEstimacionSpool.Instance.ObtenerExcelPorEstimacionID(EntityID.Value);
                    nombreArchivo = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "SpoolInvoincing.xlsx" : "EstimadoSpools.xlsx";
                    break;
                case TipoArchivoExcel.SeguimientoJuntas:
                    archivo = ExcelSeguimientoJuntas.Instance.ObtenerCsvPorIDs(EntityID.Value, OrdenDeTrabajo, NumeroControl, Rep, Emb, Columnas, Filtros);
                    nombreArchivo = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "WorkstatusWeld.zip" : "SeguimientoJuntas.zip";
                    break;
                case TipoArchivoExcel.SeguimientoSpools:
                    archivo = ExcelSeguimientoSpool.Instance.ObtenerExcelPorIDs(EntityID.Value, OrdenDeTrabajo, NumeroControl, Emb, Filtros);
                    nombreArchivo = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "WorkstatusSpool.xlsx" : "SeguimientoSpools.xlsx";
                    break;
                case TipoArchivoExcel.Destajos:
                    archivo = ExcelDestajos.Instance.ObtenerExcelPorIDs(EntityID.Value, TJunta, FAcero, PsoID, PsoValor, pytoValor);
                    nombreArchivo = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Payroll.xlsx" : "Destajos.xlsx";
                    break;
                case TipoArchivoExcel.LstNumeroUnico:
                    archivo = ExcelLstNumeroUnico.Instance.ObtenerExcelPorIDs(EntityID.Value, Colada, IC, NuI, NuF);
                    nombreArchivo = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "ListUniqueNumber.xlsx" : "LstNumeroUnico.xlsx";
                    break;
                case TipoArchivoExcel.LstItemCode:
                    archivo = ExcelLstItemCode.Instance.ObtenerExcelPorIDs(EntityID.Value);
                    nombreArchivo = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "ListItemcode.xlsx" : "LstItemCode.xlsx";
                    break;
                case TipoArchivoExcel.ItemCodePeso:
                    archivo = ExcelItemCodePeso.Instance.ObtenerExcelPorIDs(EntityID.Value);
                    nombreArchivo = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "ListItemcode.xlsx" : "LstItemCode.xlsx";
                    break;
            }

            if (archivo != null)
            {
                _logger.Info("bajando archivo");
                string nombreAttachment = string.Format("attachment; filename=\"{0}\"", nombreArchivo);
                Response.Clear();
                Response.ContentType = "binary/octet-stream";
                Response.AddHeader("content-disposition", nombreAttachment);
                Response.BinaryWrite(archivo);
            }
            else
            {
                throw new ExcelException("El archivo de Excel que se desea generar no existe");
            }
        }
    }
}