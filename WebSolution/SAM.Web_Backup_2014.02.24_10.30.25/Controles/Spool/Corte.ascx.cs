using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.WebControls;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.Web.Classes;
using Telerik.Web.UI;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Validations;

namespace SAM.Web.Controles.Spool
{
    public partial class Corte : UserControl, IMappable
    {
        
        private int NextID
        {
            get
            {
                if (ViewState["NextID"] == null)
                {
                    ViewState["NextID"] = -1;
                }
                return (int)ViewState["NextID"];
            }
            set
            {
                ViewState["NextID"] = value;
            }
        }

        private List<Entities.CorteSpool> Cortes
        {
            get
            {
                if (ViewState["Cortes"] == null)
                {
                    ViewState["Cortes"] = new List<Entities.CorteSpool>();
                }

                return (List<Entities.CorteSpool>)ViewState["Cortes"];
            }
            set
            {
                ViewState["Cortes"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarCombos();
            }
        }

        private void cargarCombos()
        {
            if (!IsPostBack)
            {
                ddlProfile1.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposCorte());
                ddlProfile2.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposCorte());
            }
        }

        protected void grdCortes_OnNeedDataSource(object sender, EventArgs e)
        {
            grdCortes.DataSource = Cortes;
        }

        protected void grdCortes_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            int corteID = e.CommandArgument.SafeIntParse();

            //si es edicion
            if (e.CommandName == "Editar")
            {
                panelCorte.Visible = true;
                btnAgregar.Visible = false;
                btnEditar.Visible = true;

                Entities.CorteSpool corte = Cortes.Where(x => x.CorteSpoolID == corteID).SingleOrDefault();
                hdnCorteID.Value = corte.CorteSpoolID.ToString();
                string descripcionItemCode = ItemCodeBO.Instance.Obtener(corte.ItemCodeID).Codigo;
                ddlItemCode.Items.Add(new RadComboBoxItem(descripcionItemCode, corte.ItemCodeID.SafeStringParse()));
                ddlItemCode.SelectedValue = corte.ItemCodeID.SafeStringParse();
                txtEtiquetaSeccion.Text = corte.EtiquetaSeccion;
                txtDiametro1.Text = corte.Diametro.SafeStringParse();
                txtCantidad.Text = corte.Cantidad.SafeStringParse();
                txtInicioFin.Text = corte.InicioFin;
                txtEtiquetaMaterial.Text = corte.EtiquetaMaterial;
                ddlProfile1.SelectedValue = corte.TipoCorte1ID.SafeStringParse();
                ddlProfile2.SelectedValue = corte.TipoCorte2ID.SafeStringParse();
                txtObservaciones.Text = corte.Observaciones;
            }
            else if (e.CommandName == "Borrar")
            {
                Entities.CorteSpool corte = Cortes.Where(x => x.CorteSpoolID == corteID).SingleOrDefault();
                Cortes.Remove(corte);
                grdCortes.DataSource = Cortes;
                grdCortes.DataBind();
            }
        }

        protected void grdCortes_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

            int idItemCode,idTipoCorte1,idTipoCorte2;

            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.CorteSpool corteS = (SAM.Entities.CorteSpool)e.Item.DataItem;
                GridItem item = (GridItem)e.Item;
                ItemCode ic;

                idItemCode = corteS.ItemCodeID;//dataItem["ItemCodeID"].Text.SafeIntParse();
                ic = ItemCodeBO.Instance.Obtener(idItemCode);

                dataItem["ItemCode"].Text = ic.Codigo;
                dataItem["ItemCodeDesc"].Text = LanguageHelper.CustomCulture == LanguageHelper.ESPANOL
                                                    ? ic.DescripcionEspanol
                                                    : ic.DescripcionIngles;

                idTipoCorte1 = corteS.TipoCorte1ID;//dataItem["TipoCorte1ID"].Text.SafeIntParse();
                dataItem["Profile1"].Text = CacheCatalogos.Instance.ObtenerTiposCorte().Where(x => x.ID == idTipoCorte1).Select(y => y.Nombre).SingleOrDefault();

                idTipoCorte2 = corteS.TipoCorte2ID;//dataItem["TipoCorte2ID"].Text.SafeIntParse();
                dataItem["Profile2"].Text = CacheCatalogos.Instance.ObtenerTiposCorte().Where(x => x.ID == idTipoCorte2).Select(y => y.Nombre).SingleOrDefault();
             
            }

        }

        protected void lnkAgregar_OnClick(object sender, EventArgs e)
        {
            limpiarDatosDeCaptura();
            panelCorte.Visible = true;
            btnAgregar.Visible = true;
            btnEditar.Visible = false;
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Page.Validate("vgCorte");
            if (!Page.IsValid) return;
            
            Entities.CorteSpool corte;
           
            btnEditar.Visible = false;
            btnAgregar.Visible = true;
            panelCorte.Visible = false;

            if (string.IsNullOrEmpty(hdnCorteID.Value))
            {
                corte = new Entities.CorteSpool();
                corte.CorteSpoolID = NextID--;
                Cortes.Add(corte);
            }
            else
            {
                corte = Cortes.Where(x => x.CorteSpoolID == hdnCorteID.Value.SafeIntParse()).Single();
            }

            //asignar datos de los controles a la instancia junta
            corte.ItemCodeID = ddlItemCode.SelectedValue.SafeIntParse();
            corte.EtiquetaMaterial = txtEtiquetaMaterial.Text;
            corte.Diametro = txtDiametro1.Text.SafeDecimalParse();
            corte.Cantidad = txtCantidad.Text.SafeIntParse();
            corte.InicioFin = txtInicioFin.Text;
            corte.EtiquetaSeccion = txtEtiquetaSeccion.Text;
            corte.TipoCorte1ID = ddlProfile1.SelectedValue.SafeIntParse();
            corte.TipoCorte2ID = ddlProfile2.SelectedValue.SafeIntParse();
            corte.Observaciones = txtObservaciones.Text;

            

       
            //limpia datos de captura
            limpiarDatosDeCaptura();
            grdCortes.DataSource = Cortes;
            grdCortes.DataBind();

        }

        private void limpiarDatosDeCaptura()
        {
            ddlItemCode.Text = string.Empty;
            ddlItemCode.Items.Clear();
            txtEtiquetaSeccion.Text = string.Empty;
            txtDiametro1.Text = string.Empty;
            txtCantidad.Text = string.Empty;
            txtInicioFin.Text = string.Empty;
            txtEtiquetaMaterial.Text = string.Empty;
            ddlProfile1.SelectedValue = string.Empty;
            ddlProfile2.SelectedValue = string.Empty;
            hdnCorteID.Value = string.Empty;
            txtObservaciones.Text = string.Empty;
        }

        protected void cvDiametro_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = ValidacionesIngenieria.DiametroExiste(txtDiametro1.Text.SafeDecimalParse());
        }

        protected void cusItemCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlItemCode.SelectedValue.SafeIntParse() > 0;
        }

        #region IMappable Members

        public void Map(object entity)
        {
            //No se utiliza
        }

        public void Map(object entity, int proyectoID)
        {
            List<Entities.CorteSpool> lstCortes = (List<Entities.CorteSpool>)((TrackableCollection<Entities.CorteSpool>)entity).ToList();
            Cortes = lstCortes;
            hdnProyectoID.Value = proyectoID.ToString();
            grdCortes.DataSource = Cortes;
            grdCortes.DataBind();
        }

        public void Unmap(object entity)
        {
            TrackableCollection<Entities.CorteSpool> lstCortes = (TrackableCollection<Entities.CorteSpool>)entity;

            //Buscar en BD para saber si lo borraron
            for (int i = lstCortes.Count - 1; i >= 0; i--)
            {
                Entities.CorteSpool corte = lstCortes[i];

                //buscar en memoria
                Entities.CorteSpool mem = Cortes.Where(x => x.CorteSpoolID == corte.CorteSpoolID).SingleOrDefault();

                if (mem == null)
                {
                    //lo borraron
                    corte.StartTracking();
                    corte.MarkAsDeleted();
                    corte.StopTracking();
                }
            }

            //Barriendo la coleccion de memoria primero
            foreach (Entities.CorteSpool corte in Cortes)
            {
                //buscarlo en BD
                Entities.CorteSpool bd = lstCortes.Where(x => x.CorteSpoolID == corte.CorteSpoolID).SingleOrDefault();

                //si ya esta en BD es edicion, sino es alta
                if (bd == null)
                {
                    //Es un alta
                    bd = new Entities.CorteSpool();
                    lstCortes.Add(bd);
                }

                //copiar propiedades
                bd.StartTracking();
                bd.ItemCodeID = corte.ItemCodeID;
                bd.EtiquetaMaterial = corte.EtiquetaMaterial;
                bd.Diametro = corte.Diametro;
                bd.Cantidad = corte.Cantidad;
                bd.InicioFin = corte.InicioFin;
                bd.EtiquetaSeccion = corte.EtiquetaSeccion;
                bd.TipoCorte1ID = corte.TipoCorte1ID;
                bd.TipoCorte2ID = corte.TipoCorte2ID;
                bd.Observaciones = corte.Observaciones;
                bd.VersionRegistro = corte.VersionRegistro;
                bd.UsuarioModifica = SessionFacade.UserId;
                bd.FechaModificacion = DateTime.Now;
                bd.StopTracking();
            }
        }

        #endregion

    }
}