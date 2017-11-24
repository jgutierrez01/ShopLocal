using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Excepciones;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Proyectos;
using SAM.Web.Classes;
using SAM.Entities.Grid;
using SAM.Entities;
using SAM.Web.Common;
using Telerik.Web.UI;
using Resources;

namespace SAM.Web.Controles.ImportarProyecto
{
    public partial class WPS : System.Web.UI.UserControl
    {
        #region variables privadas

        public int ProyectoID
        {
            get { return hdnProyectoID.Value.SafeIntParse(); }
            set { hdnProyectoID.Value = value.ToString(); }
        }

        private int ProyImportaID
        {
            get
            {
                return ViewState["ProyImportaID"].SafeIntParse();
            }
            set
            {
                ViewState["ProyImportaID"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                cargaCombos();
            }
        }

        protected void grdWpsProyecto_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// Muestra los wps del proyecto seleccionado que sean candidatos a importar, es decir que no se encuentren ya dados de alta dentro del proyecto que se está configurando.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            ProyImportaID = ddlProyecto3.SelectedValue.SafeIntParse();
                
            if (ProyImportaID > 0)
            {
                grdWpsProyecto.Visible = true;
                EstablecerDataSource();
                grdWpsProyecto.DataBind();
            }
            else
            {
                grdWpsProyecto.Visible = false;
            }
        }

        /// <summary>
        /// Importa los wps seleccionados al proyecto que se está configurando
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkImportar_OnClick(object sender, EventArgs e)
        {
            List<int> ids =
                grdWpsProyecto.Items.Cast<GridDataItem>().Where(x => x.Selected).Select(
                    x => x.GetDataKeyValue("WpsID").SafeIntParse()).ToList();

            try
            {
                if (ids.Count > 0)
                {
                    ProyectoConfiguracionBO.Instance.GuardaWPSProyecto(ids, ProyectoID, SessionFacade.UserId);
                    EstablecerDataSource();
                    grdWpsProyecto.DataBind();
                }
                else
                {
                    throw new Exception(MensajesErrorWeb.Exception_Error);
                }
            }
            catch (BaseValidationException ex)
            {
                CustomValidator cv = new CustomValidator();

                cv.IsValid = false;
                cv.ErrorMessage = ex.Message;
                cv.ValidationGroup = "vgWps";
                cv.Display = ValidatorDisplay.None;
                Page.Form.Controls.Add(cv);
            }
        }

        /// <summary>
        /// Carga los proyectos que son propios del usuario
        /// </summary>
        private void cargaCombos()
        {
            ddlProyecto3.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
        }

        private void EstablecerDataSource()
        {
            List<Wps> wpsExistentes = WpsBO.Instance.ObtenerPorProyecto(ProyectoID).ToList();
            grdWpsProyecto.DataSource = WpsBO.Instance.ObtenerWpsPorProyecto(ProyImportaID).Where(x => !wpsExistentes.Select(y => y.WpsID).Contains(x.WpsID)).ToList();
        }
       
    }
}