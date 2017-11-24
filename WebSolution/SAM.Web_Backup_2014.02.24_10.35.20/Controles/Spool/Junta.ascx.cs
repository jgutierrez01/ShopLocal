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
using Telerik.Web.UI;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Validations;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Administracion;
using System.Threading;
using Mimo.Framework.Common;

namespace SAM.Web.Controles.Spool
{
    public partial class Junta : System.Web.UI.UserControl, IMappable
    {

        public int ProyectoID
        {
            get
            {
                return ViewState["ProyectoID"].SafeIntParse();
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        public List<int> lstJuntaIds
        {
            get
            {
                if (ViewState["JuntaIds"] == null)
                {
                    ViewState["JuntaIds"] = new List<int>();
                }
                return (List<int>)ViewState["JuntaIds"];
            }
            set
            {
                ViewState["JuntaIds"] = value;
            }

        }

        public List<int> lstJuntasEliminadasIds
        {
            get
            {
                if (ViewState["JuntasEliminadasIds"] == null)
                {
                    ViewState["JuntasEliminadasIds"] = new List<int>();
                }
                return (List<int>)ViewState["JuntasEliminadasIds"];
            }
            set
            {
                ViewState["JuntasEliminadasIds"] = value;
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

        private List<Entities.JuntaSpool> Juntas
        {
            get
            {
                if (ViewState["Juntas"] == null)
                {
                    ViewState["Juntas"] = new List<Entities.JuntaSpool>();
                }

                return (List<Entities.JuntaSpool>)ViewState["Juntas"];
            }
            set
            {
                ViewState["Juntas"] = value;
            }
        }

        private string Permiso = Thread.CurrentThread.CurrentUICulture.Name == LanguageHelper.INGLES ? "Engineer Edition" : "Edición de Ingeniería";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarCombos();
            }
        }

        private void cargarCombos()
        {
            ddlTipoJunta.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposJunta());
            ddlFabArea.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerFabAreas());
            ddlFamAcero1.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerFamiliasAcero());
            ddlFamAcero2.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerFamiliasAcero());
        }

        protected void grdJuntas_OnNeedDataSource(object sender, EventArgs e)
        {
            grdJuntas.DataSource = Juntas;
        }

        protected void grdJuntas_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            int juntaID = e.CommandArgument.SafeIntParse();
            
            //Verificar si tiene armado o soldadura, sino permitimos la Edición
            bool soldado, armado;
            
            soldado = ValidacionesJuntaSpool.EstaSoldado(juntaID);
            armado = ValidacionesJuntaSpool.EstaArmado(juntaID);

            try
            {
                Entities.JuntaSpool junta = Juntas.Where(x => x.JuntaSpoolID == juntaID).SingleOrDefault();

                bool tienePermiso = SeguridadWeb.UsuarioPuedeEditar(Permiso);                

                if (junta != null)
                {
                    if (soldado || armado)
                    {
                        if (tienePermiso)
                        {
                            MostrarEdicion(junta, true, e);
                        }
                        else
                        {
                            throw new Exception(MensajesErrorUI.Excepcion_NoPermiteEdicionTieneArmadoOSoldadura);
                        }
                    }
                    else
                    {
                        MostrarEdicion(junta, false, e);
                    }
                }
            }

            catch (Exception ex)
            {
                CustomValidator cv = new CustomValidator
                {
                    ErrorMessage = ex.Message,
                    IsValid = false,
                    Display = ValidatorDisplay.None,
                    ValidationGroup = "vgEncabezadoJunta",
                };

                Page.Form.Controls.Add(cv);
            }
        }

        protected void MostrarEdicion(Entities.JuntaSpool junta, bool mostrarAdvertencia, Telerik.Web.UI.GridCommandEventArgs e)
        {
            lblAdvertencia.Visible = mostrarAdvertencia;

            //si es edicion
            if (e.CommandName == "Editar")
            {
                panelJunta.Visible = true;
                btnAgregar.Visible = false;
                btnEditar.Visible = true;
                hdnJuntaID.Value = junta.JuntaSpoolID.ToString();

                //Asignar valores a los controles
                txtEtiqueta.Text = junta.Etiqueta;
                txtDiametro.Text = junta.Diametro.SafeStringParse();
                txtEspesor.Text = junta.Espesor.SafeStringParse();
                txtCedula.Text = junta.Cedula;
                txtLocalizacion.Text = junta.EtiquetaMaterial1 + "-" + junta.EtiquetaMaterial2;
                txtKgTeoricos.Text = junta.KgTeoricos.SafeStringParse();
                ddlTipoJunta.SelectedValue = junta.TipoJuntaID.SafeStringParse();
                ddlFabArea.SelectedValue = junta.FabAreaID.SafeStringParse();
                txtPulgadas.Text = junta.Peqs.SafeStringParse();
                ddlFamAcero1.SelectedValue = junta.FamiliaAceroMaterial1ID.SafeStringParse();
                ddlFamAcero2.SelectedValue = junta.FamiliaAceroMaterial2ID != null ? junta.FamiliaAceroMaterial2ID.SafeStringParse() : "";
            }
            else if (e.CommandName == "Borrar")
            {
                Juntas.Remove(junta);
                grdJuntas.DataSource = Juntas;
                grdJuntas.DataBind();
            }
        }

        protected void grdJuntas_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            int idJuntaSpoolID, idTipoJunta, idFamAcero1, idFabArea;
            int? idFamAcero2;
            string etiquetaMaterial1, etiquetaMaterial2;

            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                JuntaSpool juntas = (JuntaSpool)e.Item.DataItem;

                idJuntaSpoolID = juntas.JuntaSpoolID;
                idTipoJunta = juntas.TipoJuntaID;
                idFamAcero1 = juntas.FamiliaAceroMaterial1ID;
                idFamAcero2 = juntas.FamiliaAceroMaterial2ID;
                idFabArea = juntas.FabAreaID;
                etiquetaMaterial1 = juntas.EtiquetaMaterial1;
                etiquetaMaterial2 = juntas.EtiquetaMaterial2;
       
                if (etiquetaMaterial2 != null)
                {
                    dataItem["Localizacion"].Text = etiquetaMaterial1 + "-" + etiquetaMaterial2;
                }
                else
                {
                    dataItem["Localizacion"].Text = etiquetaMaterial1;
                }

                dataItem["TipoJunta"].Text = CacheCatalogos.Instance.ObtenerTiposJunta().Where(x => x.ID == idTipoJunta).Select(y => y.Nombre).SingleOrDefault();
                dataItem["FamiliaAceroMaterial1"].Text = CacheCatalogos.Instance.ObtenerFamiliasAcero().Where(x => x.ID == idFamAcero1).Select(y => y.Nombre).SingleOrDefault();
                dataItem["FamiliaAceroMaterial2"].Text = CacheCatalogos.Instance.ObtenerFamiliasAcero().Where(x => x.ID == idFamAcero2).Select(y => y.Nombre).SingleOrDefault();
                dataItem["FabArea"].Text = CacheCatalogos.Instance.ObtenerFabAreas().Where(x => x.ID == idFabArea).Select(y => y.Nombre).SingleOrDefault();
                
            }


        }

        protected void lnkAgregar_OnClick(object sender, EventArgs e)
        {
            limpiarDatosDeCaptura();
            panelJunta.Visible = true;
            btnAgregar.Visible = true;
            btnEditar.Visible = false;
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Entities.JuntaSpool junta;

                Page.Validate("vgJunta");
                if (!Page.IsValid) return;

                panelJunta.Visible = false;
                btnAgregar.Visible = true;
                btnEditar.Visible = false;

                if (string.IsNullOrEmpty(hdnJuntaID.Value))
                {
                    junta = new Entities.JuntaSpool();
                    junta.JuntaSpoolID = NextID--;
                    Juntas.Add(junta);
                }
                else
                {
                    junta = Juntas.Where(x => x.JuntaSpoolID == hdnJuntaID.Value.SafeIntParse()).Single();
                }

                //asignar a junta los datos de los controles
                junta.Etiqueta = txtEtiqueta.Text;
                junta.Diametro = txtDiametro.Text.SafeDecimalParse();
                junta.Cedula = txtCedula.Text;

                if (txtLocalizacion.Text != string.Empty)
                {
                    string[] etiqueta = txtLocalizacion.Text.Split('-');

                    junta.EtiquetaMaterial1 = etiqueta[0].Trim();
                    junta.EtiquetaMaterial2 = etiqueta[1].Trim();
                    //if (JuntaSpoolBO.Instance.ExisteJuntaConLocalizacion(junta))
                    //{
                    //    throw new Exception(MensajesErrorUI.Excepcion_LocalizacionDuplicada);
                    //}
                }

                int idDiametro = 0;
                int idCedula = 0;
                //Obtener el Id de Espesor y Cedula
                if (txtDiametro.Text != string.Empty)
                {
                    idDiametro = CacheCatalogos.Instance.ObtenerDiametros().Where(x => x.Valor == txtDiametro.Text.SafeDecimalParse()).First().ID;
                }
                if (txtCedula.Text != string.Empty)
                {
                    idCedula = CacheCatalogos.Instance.ObtenerCedulas().Where(x => x.Nombre == txtCedula.Text).First().ID;
                }

                //Decimal espesor, kg, peqs;
                KgTeorico kgTeorico = KgTeoricoBO.Instance.ObtenerPorDiametroCedula(idDiametro, idCedula);
                Espesor espesor = EspesorBO.Instance.ObtenerPorDiametroCedula(idDiametro, idCedula);
                Peq peq = PeqBO.Instance.Obtener(idDiametro,
                                                  idCedula,
                                                  ddlTipoJunta.SelectedValue.SafeIntParse(),
                                                  ddlFamAcero1.SelectedValue.SafeIntParse(),
                                                  ProyectoID);

                junta.Espesor = espesor != null ? espesor.Valor : 0;
                junta.KgTeoricos = kgTeorico != null ? kgTeorico.Valor : 0;
                junta.Peqs = peq != null ? peq.Equivalencia : 0;
                junta.TipoJuntaID = ddlTipoJunta.SelectedValue.SafeIntParse();
                junta.FabAreaID = ddlFabArea.SelectedValue.SafeIntParse();
                junta.FamiliaAceroMaterial1ID = ddlFamAcero1.SelectedValue.SafeIntParse();
                junta.FamiliaAceroMaterial2ID = ddlFamAcero2.SelectedValue.SafeIntParse() > 0 ? ddlFamAcero2.SelectedValue.SafeIntParse() : (int?)null;

                //limpia datos de captura
                limpiarDatosDeCaptura();
                grdJuntas.DataSource = Juntas;
                grdJuntas.DataBind();
            }
            catch (Exception ex)
            {
                CustomValidator cv = new CustomValidator
                {
                    ErrorMessage = ex.Message,
                    IsValid = false,
                    Display = ValidatorDisplay.None,
                    ValidationGroup = "vgEncabezadoJunta",
                };

                Page.Form.Controls.Add(cv);
            }
        }

        private void limpiarDatosDeCaptura()
        {
            txtEtiqueta.Text = string.Empty;
            txtDiametro.Text = string.Empty;
            txtEspesor.Text = string.Empty;
            txtCedula.Text = string.Empty;
            txtLocalizacion.Text = string.Empty;
            txtKgTeoricos.Text = string.Empty;
            ddlTipoJunta.SelectedValue = string.Empty;
            ddlFabArea.SelectedValue = string.Empty;
            txtPulgadas.Text = string.Empty;
            ddlFamAcero1.SelectedValue = string.Empty;
            ddlFamAcero2.SelectedValue = string.Empty;
            hdnJuntaID.Value = string.Empty;
        }

        protected void cvDiametro_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = ValidacionesIngenieria.DiametroExiste(txtDiametro.Text.SafeDecimalParse());
        }

        protected void cvCedula_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = ValidacionesIngenieria.CedulaExiste(txtCedula.Text);
        }

        protected void cvEtiqueta_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (Juntas.Any(x => x.Etiqueta == txtEtiqueta.Text && x.JuntaSpoolID != hdnJuntaID.Value.SafeIntParse()))
            {
                e.IsValid = false;
            }
        }

        protected void cvLocalizacion_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = true;
            if (txtLocalizacion.Text != null)
            {
                string[] etiqueta = txtLocalizacion.Text.Split('-');

                if (etiqueta.Count() != 2)
                {
                    e.IsValid = false;
                }
            }
        }

        #region IMappable Members

        public void Map(object entity)
        {
            List<Entities.JuntaSpool> lstJuntas = (List<Entities.JuntaSpool>)((TrackableCollection<Entities.JuntaSpool>)entity).ToList();
            Juntas = lstJuntas;            
            grdJuntas.DataSource = Juntas;
            grdJuntas.DataBind();
        }

        public void Unmap(object entity)
        {
            TrackableCollection<Entities.JuntaSpool> lstJuntas = (TrackableCollection<Entities.JuntaSpool>)entity;

            //Buscar en BD para saber si lo borraron
            for (int i = lstJuntas.Count - 1; i >= 0; i--)
            {
                Entities.JuntaSpool junta = lstJuntas[i];

                //buscar en memoria
                Entities.JuntaSpool mem = Juntas.Where(x => x.JuntaSpoolID == junta.JuntaSpoolID).SingleOrDefault();

                if (mem == null)
                {
                    //lo borraron
                    junta.StartTracking();
                    junta.MarkAsDeleted();
                    junta.StopTracking();

                    //agregamos los Ids de las juntas eliminadas
                    lstJuntasEliminadasIds.Add(junta.JuntaSpoolID);
                }
            }

            //Barriendo la coleccion de memoria primero
            foreach (Entities.JuntaSpool junta in Juntas)
            {
                //buscarlo en BD
                Entities.JuntaSpool bd = lstJuntas.Where(x => x.JuntaSpoolID == junta.JuntaSpoolID).SingleOrDefault();

                //si ya esta en BD es edicion, sino es alta
                if (bd == null)
                {
                    //Es un alta
                    bd = new Entities.JuntaSpool();
                    lstJuntas.Add(bd);
                }

                //Verificar si las propiedades son iguales para indicar si hubo alguna edición
                // bd vs junta
                bool esEdicion = false;
                bool cambioFabArea = false;

                if (bd.Etiqueta != junta.Etiqueta)
                {
                    esEdicion = true;
                }
                if (bd.Diametro != junta.Diametro)
                {
                    esEdicion = true;
                }
                if (bd.Cedula != junta.Cedula)
                {
                    esEdicion = true;
                }
                if (bd.EtiquetaMaterial1 != junta.EtiquetaMaterial1)
                {
                    esEdicion = true;
                }
                if (bd.EtiquetaMaterial2 != junta.EtiquetaMaterial2)
                {
                    esEdicion = true;
                }
                if (bd.Espesor != junta.Espesor)
                {
                    esEdicion = true;
                }
                if (bd.KgTeoricos != junta.KgTeoricos)
                {
                    esEdicion = true;
                }
                if (bd.Peqs != junta.Peqs)
                {
                    esEdicion = true;
                }
                if (bd.TipoJuntaID != junta.TipoJuntaID)
                {
                    esEdicion = true;
                }
                if (bd.FabAreaID != junta.FabAreaID)
                {
                    esEdicion = true;
                    cambioFabArea = true;
                }
                if (bd.FamiliaAceroMaterial1ID != junta.FamiliaAceroMaterial1ID)
                {
                    esEdicion = true;
                }
                if (bd.FamiliaAceroMaterial2ID != junta.FamiliaAceroMaterial2ID)
                {
                    esEdicion = true;
                }

                if (esEdicion)
                {
                    lstJuntaIds.Add(junta.JuntaSpoolID);
                }
                //Si cambia el fab area se agrega a jutnas eliminadas para que se elimine la ODT ya que se requiere reingenieria.
                if (cambioFabArea)
                {
                    lstJuntasEliminadasIds.Add(junta.JuntaSpoolID);
                }

                //copiar propiedades
                bd.StartTracking();

                bd.Etiqueta = junta.Etiqueta;
                bd.Diametro = junta.Diametro;
                bd.Cedula = junta.Cedula;
                bd.EtiquetaMaterial1 = junta.EtiquetaMaterial1;
                bd.EtiquetaMaterial2 = junta.EtiquetaMaterial2;
                bd.Espesor = junta.Espesor;
                bd.KgTeoricos = junta.KgTeoricos;
                bd.Peqs = junta.Peqs;
                bd.TipoJuntaID = junta.TipoJuntaID;
                bd.FabAreaID = junta.FabAreaID;
                bd.FamiliaAceroMaterial1ID = junta.FamiliaAceroMaterial1ID;
                bd.FamiliaAceroMaterial2ID = junta.FamiliaAceroMaterial2ID > 0 ? junta.FamiliaAceroMaterial2ID : (int?)null;

                bd.VersionRegistro = junta.VersionRegistro;
                bd.UsuarioModifica = SessionFacade.UserId;
                bd.FechaModificacion = DateTime.Now;
                bd.StopTracking();
            }
        }

        #endregion
    }
}