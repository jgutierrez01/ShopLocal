using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Entities;
using SAMEntities = SAM.Entities;
using SAM.BusinessObjects.Materiales;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;
using Mimo.Framework.WebControls;
using SAM.BusinessObjects.Proyectos;

namespace SAM.Web.Controles.Materiales
{
    public partial class AltaNumeroUnicoAdicional : System.Web.UI.UserControl
    {
        private int ProyectoID
        {
            get
            {
                return (int)ViewState["ProyectoID"];
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        #region Propiedades publicas

        public bool MarcadoGolpe
        {
            get
            {
                return chkMacadoGolpe.Checked;
            }
        }
        public bool MarcadoAsme
        {
            get
            {
                return chkMarcadoAsme.Checked;
            }
        }
        public bool MarcadoPintura
        {
            get
            {
                return chkMarcadoPintura.Checked;
            }
        }
        public string PruebasHidrostaticas
        {
            get
            {
                return txtPruebas.Text;
            }
        }
        public List<SimpleString> Segmentos
        {
            get
            {
                if (phTubo.Visible == true)
                {
                    List<SimpleString> lista = new List<SimpleString>();

                    foreach (RepeaterItem item in repRacks.Items)
                    {
                        if (item.IsItem())
                        {
                            Label lblSegmento = (Label)item.FindControl("lblSegmento");
                            TextBox txtIntRack = (TextBox)item.FindControl("txtRack");

                            lista.Add(new SimpleString { ID = txtIntRack.Text, Valor = lblSegmento.Text });
                        }
                    }

                    return lista;
                }

                return null;
            }
        }
        public string Rack
        {
            get
            {
                return txtRack.Text;
            }
        }

        public int? RecepcionID { get; set; }
        public NumeroUnico NumUnico { get; set; }
        public LabeledTextBox[] camposRecepcion;
        public LabeledTextBox[] camposNumeroUnico;

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProyectoID = Request.QueryString["PID"].ToString().SafeIntParse();
            }

            camposRecepcion = new LabeledTextBox[] { txtCampoLibreRecepcion1, txtCampoLibreRecepcion2, txtCampoLibreRecepcion3, txtCampoLibreRecepcion4, txtCampoLibreRecepcion5 };
            camposNumeroUnico = new LabeledTextBox[] { txtCampoLibre1, txtCampoLibre2, txtCampoLibre3, txtCampoLibre4, txtCampoLibre5 };
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                estableceVisibilidadCampos(camposRecepcion);
                estableceVisibilidadCampos(camposNumeroUnico);

                if (RecepcionID != null)
                {
                    cargarRegistrosConRecepcion();
                }
                else
                {
                    cargarRegistrosCamposNumeroUnico();
                }
            }
        }

        /// <summary>
        /// Carga la información que se logre obtener en los campos del control
        /// </summary>
        public void CargaInformacion(NumeroUnico numUnico, NumeroUnico numAnterior)
        {
            if (numAnterior != null)
            {
                txtPruebas.Text = numAnterior.PruebasHidrostaticas;
                chkMacadoGolpe.Checked = numAnterior.MarcadoGolpe;
                chkMarcadoAsme.Checked = numAnterior.MarcadoAsme;
                chkMarcadoPintura.Checked = numAnterior.MarcadoPintura;
                txtRack.Text = numAnterior.Rack == null ? "" : numAnterior.Rack;
            }
            else
            {
                txtPruebas.Text = numUnico.PruebasHidrostaticas;
                chkMacadoGolpe.Checked = numUnico.MarcadoGolpe;
                chkMarcadoAsme.Checked = numUnico.MarcadoAsme;
                chkMarcadoPintura.Checked = numUnico.MarcadoPintura;

                if (numUnico.NumeroUnicoSegmento != null && numUnico.NumeroUnicoSegmento.Count > 0)
                {
                    phAccesorio.Visible = false;
                    phTubo.Visible = true;

                    repRacks.DataSource = numUnico.NumeroUnicoSegmento.OrderBy(x => x.Segmento);
                    repRacks.DataBind();
                }
                else
                {
                    txtRack.Text = numUnico.Rack == null ? "" : numUnico.Rack;
                }
            }
        }

        /// <summary>
        /// Limpia los campos del control
        /// </summary>
        public void LimpiaDatos()
        {
            chkMacadoGolpe.Checked = false;
            chkMarcadoAsme.Checked = false;
            chkMarcadoPintura.Checked = false;
            txtRack.Text = string.Empty;
        }

        /// <summary>
        /// Carga la información de los campos de recepción y número único
        /// </summary>
        private void cargarRegistrosConRecepcion()
        {
            SAMEntities.Proyecto proyecto = ProyectoBO.Instance.ObtenerConCamposRecepcion(ProyectoID);
            SAMEntities.Recepcion recepcion = RecepcionBO.Instance.Obtener((int)RecepcionID);

            #region Cargar campos recepción

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion1))
            {
                txtCampoLibreRecepcion1.Visible = true;
                txtCampoLibreRecepcion1.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion1);
                txtCampoLibreRecepcion1.Text = recepcion.CampoLibre1;
            }

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion2))
            {
                txtCampoLibreRecepcion2.Visible = true;
                txtCampoLibreRecepcion2.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion2);
                txtCampoLibreRecepcion2.Text = recepcion.CampoLibre2;
            }

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion3))
            {
                txtCampoLibreRecepcion3.Visible = true;
                txtCampoLibreRecepcion3.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion3);
                txtCampoLibreRecepcion3.Text = recepcion.CampoLibre3;
            }

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion4))
            {
                txtCampoLibreRecepcion4.Visible = true;
                txtCampoLibreRecepcion4.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion4);
                txtCampoLibreRecepcion4.Text = recepcion.CampoLibre4;
            }

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion5))
            {
                txtCampoLibreRecepcion5.Visible = true;
                txtCampoLibreRecepcion5.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion5);
                txtCampoLibreRecepcion5.Text = recepcion.CampoLibre5;
            }

            #endregion
            #region Cargar campos número único

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoNumeroUnico1))
            {
                txtCampoLibre1.Visible = true;
                txtCampoLibre1.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoNumeroUnico1);
            }

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoNumeroUnico2))
            {
                txtCampoLibre2.Visible = true;
                txtCampoLibre2.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoNumeroUnico2);
            }

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoNumeroUnico3))
            {
                txtCampoLibre3.Visible = true;
                txtCampoLibre3.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoNumeroUnico3);
            }

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoNumeroUnico4))
            {
                txtCampoLibre4.Visible = true;
                txtCampoLibre4.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoNumeroUnico4);
            }

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoNumeroUnico5))
            {
                txtCampoLibre5.Visible = true;
                txtCampoLibre5.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoNumeroUnico5);
            }

            #endregion
        }

        /// <summary>
        /// Carga los registros de los campos adicionales del número único para su edición
        /// </summary>
        /// <param name="numUnico"></param>
        private void cargarRegistrosCamposNumeroUnico()
        {
            SAMEntities.Proyecto proyecto = ProyectoBO.Instance.ObtenerConCamposRecepcion(ProyectoID);

            #region Cargar campos número único

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion1))
            {
                txtCampoLibreRecepcion1.Visible = true;
                txtCampoLibreRecepcion1.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion1);
                txtCampoLibreRecepcion1.Text = NumUnico.CampoLibreRecepcion1;
            }
            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion2))
            {
                txtCampoLibreRecepcion2.Visible = true;
                txtCampoLibreRecepcion2.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion2);
                txtCampoLibreRecepcion2.Text = NumUnico.CampoLibreRecepcion2;
            }
            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion3))
            {
                txtCampoLibreRecepcion3.Visible = true;
                txtCampoLibreRecepcion3.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion3);
                txtCampoLibreRecepcion3.Text = NumUnico.CampoLibreRecepcion3;
            }
            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion4))
            {
                txtCampoLibreRecepcion4.Visible = true;
                txtCampoLibreRecepcion4.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion4);
                txtCampoLibreRecepcion4.Text = NumUnico.CampoLibreRecepcion4;
            }
            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoRecepcion5))
            {
                txtCampoLibreRecepcion5.Visible = true;
                txtCampoLibreRecepcion5.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoRecepcion5);
                txtCampoLibreRecepcion5.Text = NumUnico.CampoLibreRecepcion5;
            }

            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoNumeroUnico1))
            {
                txtCampoLibre1.Visible = true;
                txtCampoLibre1.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoNumeroUnico1);
                txtCampoLibre1.Text = NumUnico.CampoLibre1;
            }
            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoNumeroUnico2))
            {
                txtCampoLibre2.Visible = true;
                txtCampoLibre2.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoNumeroUnico2);
                txtCampoLibre2.Text = NumUnico.CampoLibre2;
            }
            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoNumeroUnico3))
            {
                txtCampoLibre3.Visible = true;
                txtCampoLibre3.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoNumeroUnico3);
                txtCampoLibre3.Text = NumUnico.CampoLibre3;
            }
            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoNumeroUnico4))
            {
                txtCampoLibre4.Visible = true;
                txtCampoLibre4.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoNumeroUnico4);
                txtCampoLibre4.Text = NumUnico.CampoLibre4;
            }
            if (!string.IsNullOrEmpty(proyecto.ProyectoCamposRecepcion.CampoNumeroUnico5))
            {
                txtCampoLibre5.Visible = true;
                txtCampoLibre5.Label = string.Format("{0}:", proyecto.ProyectoCamposRecepcion.CampoNumeroUnico5);
                txtCampoLibre5.Text = NumUnico.CampoLibre5;
            }

            #endregion
        }

        /// <summary>
        /// Oculta los campos que no serán utilizados
        /// </summary>
        /// <param name="campos"></param>
        private void estableceVisibilidadCampos(LabeledTextBox[] campos)
        {
            for (int i = 0; i < campos.Count(); i++)
            {
                campos[i].Visible = false;
            }
        }
    }
}