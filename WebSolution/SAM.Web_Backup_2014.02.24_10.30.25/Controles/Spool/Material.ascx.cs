using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.Web.Classes;
using SAM.BusinessLogic;
using Telerik.Web.UI;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Validations;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using Mimo.Framework.Common;
using System.Threading;

namespace SAM.Web.Controles.Spool
{
    public partial class Material : System.Web.UI.UserControl, IMappable
    {

        /// <summary>
        /// js que abre el popup de armado
        /// </summary>
        private const string JS_POPUP_JUNTA_ADICIONAL = "javascript:Sam.Ingenieria.AbrePopupJuntaAdicional('{0}','false');";

        public int ProyectoID
        {
            get
            {
                return hdnProyectoID.Value.SafeIntParse();
            }
            set
            {
                hdnProyectoID.Value = value.ToString();
            }
        }

        public List<int> lstMatIds
        {
            get
            {
                if (ViewState["MatIds"] == null)
                {
                    ViewState["MatIds"] = new List<int>();
                }
                return (List<int>)ViewState["MatIds"];
            }
            set
            {
                ViewState["MatIds"] = value;
            }

        }

        public List<int> lstMaterialesEliminadasIds
        {
            get
            {
                if (ViewState["MaterialesEliminadosIds"] == null)
                {
                    ViewState["MaterialesEliminadosIds"] = new List<int>();
                }
                return (List<int>)ViewState["MaterialesEliminadosIds"];
            }
            set
            {
                ViewState["MaterialesEliminadosIds"] = value;
            }

        }

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

        private List<Entities.MaterialSpool> MaterialesSpool
        {
            get
            {
                if (ViewState["MaterialesSpool"] == null)
                {
                    ViewState["MaterialesSpool"] = new List<Entities.MaterialSpool>();
                }

                return (List<Entities.MaterialSpool>)ViewState["MaterialesSpool"];
            }
            set
            {
                ViewState["MaterialesSpool"] = value;
            }
        }

        private string Permiso = Thread.CurrentThread.CurrentUICulture.Name == LanguageHelper.INGLES ? "Engineer Edition" : "Edición de Ingeniería";

        protected void grdMaterial_OnNeedDataSource(object sender, EventArgs e)
        {
            grdMaterial.DataSource = MaterialesSpool;
        }

        protected void grdMaterial_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            int materialID = e.CommandArgument.SafeIntParse();

            //Verificar si se encuentra despachado o cortado
            bool despachado, cortado;

            despachado = ValidacionesMaterialSpool.EstaDespachado(materialID);
            cortado = ValidacionesMaterialSpool.EstaCortado(materialID);

            try
            {

                bool tienePermiso = SeguridadWeb.UsuarioPuedeEditar(Permiso);

                if ((cortado || despachado))
                {
                    if (tienePermiso)
                    {
                        MostrarEdicion(materialID, true, e);
                    }
                    else
                    {
                        throw new Exception(MensajesErrorUI.Excepcion_TieneCorteODespacho);
                    }
                }   
                else
                {
                    MostrarEdicion(materialID, false, e);
                }
            }
            catch (Exception ex)
            {
                CustomValidator cv = new CustomValidator
                {
                    ErrorMessage = ex.Message,
                    IsValid = false,
                    Display = ValidatorDisplay.None,
                    ValidationGroup = "vgEncabezadoMaterial",
                };

                Page.Form.Controls.Add(cv);
            }
        }

        protected void MostrarEdicion(int materialID, bool mostrarAdvertencia, Telerik.Web.UI.GridCommandEventArgs e)
        {
            lblAdvertencia.Visible = mostrarAdvertencia;

            MaterialSpool material = MaterialesSpool.Where(x => x.MaterialSpoolID == materialID).SingleOrDefault();

            if (material != null)
            {
                //si es edicion
                if (e.CommandName == "Editar")
                {
                    panelMaterial.Visible = true;
                    btnAgregar.Visible = false;
                    btnEditar.Visible = true;

                    hdnMaterialID.Value = material.MaterialSpoolID.ToString();
                    ItemCode ic = ItemCodeBO.Instance.Obtener(material.ItemCodeID);
                    //Asignar valores a los controles
                    ddlItemCode.Items.Add(new RadComboBoxItem(ic.Codigo, material.ItemCodeID.SafeStringParse()));
                    ddlItemCode.SelectedValue = material.ItemCodeID.SafeStringParse();
                    txtDescripcionMaterial.Text = material.DescripcionMaterial;
                    txtDiametro1.Text = material.Diametro1.SafeStringParse();
                    txtDiametro2.Text = material.Diametro2.SafeStringParse();
                    txtCantidad.Text = material.Cantidad.SafeStringParse();
                    txtEtiqueta.Text = material.Etiqueta;
                    txtEspecificacion.Text = material.Especificacion;
                    txtGrupo.Text = material.Grupo;

                }
                else if (e.CommandName == "Borrar")
                {
                    MaterialesSpool.Remove(material);
                    grdMaterial.DataSource = MaterialesSpool;
                    grdMaterial.DataBind();
                }
            }
        }

        protected void grdMaterial_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            int idItemCode;

            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                MaterialSpool materialSpool = (MaterialSpool)e.Item.DataItem; ;
                
                idItemCode = materialSpool.ItemCodeID;//dataItem["ItemCodeID"].Text.SafeIntParse();
                dataItem["ItemCode"].Text = ItemCodeBO.Instance.Obtener(idItemCode).Codigo;

                HyperLink hlArmar = (HyperLink)dataItem["Armar_h"].FindControl("hlArmar");

                ItemCode itemCode = materialSpool.ItemCode;
                if (itemCode == null)
                {
                    itemCode = ItemCodeBO.Instance.Obtener(materialSpool.ItemCodeID);
                }
                if (itemCode.TipoMaterialID == (int)TipoMaterialEnum.Tubo)
                {
                    hlArmar.Visible = true;
                    hlArmar.NavigateUrl = string.Format(JS_POPUP_JUNTA_ADICIONAL, materialSpool.MaterialSpoolID);
                }
            }
        }

        protected void lnkAgregar_OnClick(object sender, EventArgs e)
        {
            limpiarDatosDeCaptura();
            panelMaterial.Visible = true;
            btnAgregar.Visible = true;
            btnEditar.Visible = false;
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Entities.MaterialSpool material;

            Page.Validate("vgMaterial");
            if (!Page.IsValid) return;

            panelMaterial.Visible = false;
            btnEditar.Visible = false;
            btnAgregar.Visible = true;

            if (string.IsNullOrEmpty(hdnMaterialID.Value))
            {
                material = new Entities.MaterialSpool();
                material.MaterialSpoolID = NextID--;
                MaterialesSpool.Add(material);
            }
            else
            {
                material = MaterialesSpool.Where(x => x.MaterialSpoolID == hdnMaterialID.Value.SafeIntParse()).Single();
            }
            
            //asignar datos de los controles a la instancia junta
            material.ItemCodeID = ddlItemCode.SelectedValue.SafeIntParse();
            material.DescripcionMaterial = txtDescripcionMaterial.Text;
            material.Diametro1 = txtDiametro1.Text.SafeDecimalParse();
            material.Diametro2 = txtDiametro2.Text.SafeDecimalParse();
            material.Cantidad = txtCantidad.Text.SafeIntParse();
            material.Etiqueta = txtEtiqueta.Text;
            material.Especificacion = txtEspecificacion.Text;
            material.Grupo = txtGrupo.Text;

            //limpia datos de captura
            limpiarDatosDeCaptura();
            grdMaterial.DataSource = MaterialesSpool;
            grdMaterial.DataBind();

        }



        private void limpiarDatosDeCaptura()
        {
            ddlItemCode.Text = string.Empty;
            ddlItemCode.SelectedValue = "";
            ddlItemCode.ClearSelection();
            ddlItemCode.Items.Clear();
            txtDescripcionMaterial.Text = string.Empty;
            txtDiametro1.Text = string.Empty;
            txtDiametro2.Text = string.Empty;
            txtCantidad.Text = string.Empty;
            txtEtiqueta.Text = string.Empty;
            txtEspecificacion.Text = string.Empty;
            txtGrupo.Text = string.Empty;
            hdnMaterialID.Value = string.Empty;
        }

        protected void cvDiametro1_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = ValidacionesIngenieria.DiametroExiste(txtDiametro1.Text.SafeDecimalParse());
        }

        protected void cvDiametro2_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (txtDiametro2.Text.SafeDecimalParse() > 0)
            {
                e.IsValid = ValidacionesIngenieria.DiametroExiste(txtDiametro2.Text.SafeDecimalParse());
            }
        }

        #region IMappable Members

        public void Map(object entity)
        {
            List<MaterialSpool> lstMateriales = (List<MaterialSpool>)((TrackableCollection<MaterialSpool>)entity).ToList();
            MaterialesSpool = lstMateriales;

            grdMaterial.DataSource = MaterialesSpool;
            grdMaterial.DataBind();

        }

        public void Unmap(object entity)
        {
            TrackableCollection<MaterialSpool> lstMateriales = (TrackableCollection<MaterialSpool>)entity;

            //Buscar en BD para saber si lo borraron
            for (int i = lstMateriales.Count - 1; i >= 0; i--)
            {
                MaterialSpool material = lstMateriales[i];

                //buscar en memoria
                MaterialSpool mem = MaterialesSpool.Where(x => x.MaterialSpoolID == material.MaterialSpoolID).SingleOrDefault();

                if (mem == null)
                {
                    //lo borraron
                    material.StartTracking();
                    material.MarkAsDeleted();
                    material.StopTracking();

                    //agregamos los Ids de las juntas eliminadas
                    lstMaterialesEliminadasIds.Add(material.MaterialSpoolID);
                }
            }

            //Barriendo la coleccion de memoria primero
            foreach (MaterialSpool material in MaterialesSpool)
            {
                //buscarlo en BD
                MaterialSpool bd = lstMateriales.Where(x => x.MaterialSpoolID == material.MaterialSpoolID).SingleOrDefault();

                //si ya esta en BD es edicion, sino es alta
                if (bd == null)
                {
                    //Es un alta
                    bd = new MaterialSpool();
                    lstMateriales.Add(bd);
                }
                else
                {
                    //Verificar si las propiedades son iguales para indicar si hubo alguna edición
                    // bd vs material
                    bool esEdicion = false;
                    // Si solo es edicion de descripción no se elimina la ODT, por lo tanto no ocupa reingeniería
                    bool esEdicionDescripcionMaterial = true;

                    if (bd.ItemCodeID != material.ItemCodeID)
                    {
                        esEdicion = true;
                        esEdicionDescripcionMaterial = false;
                    }
                    if (bd.DescripcionMaterial != material.DescripcionMaterial)
                    {
                        esEdicion = true;
                    }
                    if (bd.Diametro1 != material.Diametro1)
                    {
                        esEdicion = true;
                        esEdicionDescripcionMaterial = false;
                    }
                    if (bd.Diametro2 != material.Diametro2)
                    {
                        esEdicion = true;
                        esEdicionDescripcionMaterial = false;
                    }
                    if (bd.Cantidad != material.Cantidad)
                    {
                        esEdicion = true;
                        esEdicionDescripcionMaterial = false;
                    }
                    if (bd.Etiqueta != material.Etiqueta)
                    {
                        esEdicion = true;
                        esEdicionDescripcionMaterial = false;
                    }
                    if (bd.Especificacion != material.Especificacion)
                    {
                        esEdicion = true;
                        esEdicionDescripcionMaterial = false;
                    }
                    if (bd.Grupo != material.Grupo)
                    {
                        esEdicion = true;
                        esEdicionDescripcionMaterial = false;
                    }

                    //Si se editó, agregamos los Ids del material a una lista
                    if (esEdicion && !esEdicionDescripcionMaterial)
                    {
                        lstMatIds.Add(material.MaterialSpoolID);
                    }

                }



                //copiar propiedades<
                bd.StartTracking();
                bd.ItemCodeID = material.ItemCodeID;
                bd.DescripcionMaterial = material.DescripcionMaterial;
                bd.Diametro1 = material.Diametro1;
                bd.Diametro2 = material.Diametro2;
                bd.Cantidad = material.Cantidad;
                bd.Etiqueta = material.Etiqueta;
                bd.Especificacion = material.Especificacion;
                bd.Grupo = material.Grupo;
                bd.VersionRegistro = material.VersionRegistro;
                bd.UsuarioModifica = SessionFacade.UserId;
                bd.FechaModificacion = DateTime.Now;
                bd.StopTracking();
            }
        }

        #endregion

        protected void cusItemCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlItemCode.SelectedValue.SafeIntParse() > 0;
        }
    }
}