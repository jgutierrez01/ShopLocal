using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using SAM.Web.Classes;
using System.Globalization;
using SAM.Web.Common;

namespace SAM.Web.Controles.Spool
{
    /// <summary>
    /// Control que en modo sólo lectura despliega la información de holds de un spool
    /// </summary>
    public partial class Hold : UserControl, IMappable
    {

        private List<DetHoldSpool> Holds
        {
            get
            {
                return (List<DetHoldSpool>)ViewState["Holds"];
            }
            set
            {
                ViewState["Holds"] = value;
            }
        }

        public RequiredFieldValidator validarMotivo;
        
        /// <summary>
        /// Mappea a los controles de la página de manera recursiva (los checkboxes)
        /// y luego manualmente al repeater con el historial de holds.
        /// </summary>
        /// <param name="entity">Debe ser una instancia de DetSpool</param>
        public void Map(object entity)
        {
            Controls.IterateRecursivelyStopOnIMappableAndUserControl(c =>
            {
                if (c is IMappableField)
                {
                    ((IMappableField)c).Map(entity);
                }
            });

            //List<DetHoldSpool> holds = ((DetSpoolHold)entity).Holds;
            Holds = ((DetSpoolHold)entity).Holds;
            if (Holds != null && Holds.Count > 0)
            {
                repHolds.DataSource = Holds.OrderBy(x => x.FechaHold);
                repHolds.DataBind();
            }
        }

        public void Unmap(object entity)
        {
            bool guarda = Session["guardaHold"].SafeBoolParse();
            if (guarda)
            {
                Entities.Spool spools = (Entities.Spool)entity;
                Entities.SpoolHoldHistorial historial = new SpoolHoldHistorial();
                spools.StartTracking();
                if (spools.SpoolHold == null)
                {
                    spools.SpoolHold = new SpoolHold();
                }
                spools.SpoolHold.StartTracking();
                spools.SpoolHold.FechaModificacion = DateTime.Now.Date;
                spools.SpoolHold.TieneHoldIngenieria = chkHoldIngenieria.Checked;
                spools.SpoolHold.UsuarioModifica = SessionFacade.UserId;
                spools.SpoolHold.StopTracking();
                historial.FechaHold = DateTime.Now.Date;
                historial.FechaModificacion = DateTime.Now.Date;
                historial.Observaciones = txtMotivoH.Value;
                historial.UsuarioModifica = SessionFacade.UserId;
                historial.TipoHold = TipoHoldSpool.INGENIERIA;
                spools.SpoolHoldHistorial.Add(historial);
                spools.StopTracking();
            }

        }

        protected void chkHoldIngenieria_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHoldIngenieria.Checked != Session["hold"].SafeBoolParse())
            {
                txtMotivoH.Value = "Editado por: " + SessionFacade.NombreCompleto;
                Session["guardaHold"] = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if(IsPostBack)
            {
                    if (chkHoldIngenieria.Checked != Session["hold"].SafeBoolParse())
                    {
                        //if (txtMotivoH.Text == "")
                        //{
                        //    if (CultureInfo.CurrentCulture.Name == "en-US")
                        //    {
                        //        txtMotivoH.Text = "Edited by " + SessionFacade.NombreCompleto;
                        //    }
                        //    else
                        //    {
                        //        txtMotivoH.Text = "Editado por: " + SessionFacade.NombreCompleto;
                        //    }
                        //}
                        //lblAviso.Visible = false;
                        Session["guardaHold"] = true;
                    }
                    else
                    {
                        txtMotivoH.Value = "";
                        Session["guardaHold"] = false;
                    }
            }
            if (!IsPostBack)
            {
                
                Session["hold"] = chkHoldIngenieria.Checked;
            }
            
        }

    }
}