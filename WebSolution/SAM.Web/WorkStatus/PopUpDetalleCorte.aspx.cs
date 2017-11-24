using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Materiales;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Workstatus;
using Mimo.Framework.Exceptions;
using SAM.Web.Common;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpDetalleCorte : SamPaginaPopup
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoACorte(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando ver un corte {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                cargaDatos();
            }

        }

        private void cargaDatos()
        {
            Corte corte = CorteBO.Instance.ObtenerCorteConDetalles(EntityID.Value);
            NumeroUnico numUnico = NumeroUnicoBO.Instance.ObtenerConIC(corte.NumeroUnicoCorte.NumeroUnicoID);

            lblDescripcion.Text = numUnico.ItemCode.DescripcionEspanol;
            lblDiametro1.Text = numUnico.Diametro1.ToString();
            lblDiametro2.Text = numUnico.Diametro2.ToString();
            lblEstatus.Text = TraductorEnumeraciones.TextoCanceladoAprobado(!corte.Cancelado);
            lblItemCode.Text = numUnico.ItemCode.Codigo;
            lblMerma.Text = corte.Merma.ToString();
            lblNumeroUnico.Text = numUnico.Codigo + "-" + corte.NumeroUnicoCorte.Segmento;
            lblSobrante.Text = corte.Sobrante.ToString();

            List<GrdDetalleCorte> lista = new List<GrdDetalleCorte>();

            foreach (CorteDetalle detalle in corte.CorteDetalle)
            {
                GrdDetalleCorte grd = new GrdDetalleCorte
                {
                    CorteDetalleID = detalle.CorteDetalleID,
                    Fecha = detalle.FechaCorte.Value,
                    Maquina = detalle.MaquinaID.HasValue ? detalle.Maquina.Nombre : string.Empty,
                    NumeroControl = detalle.OrdenTrabajoSpool.NumeroControl,
                    Etiqueta = detalle.MaterialSpool.Etiqueta,
                    CantidadReal = detalle.Cantidad,
                    CantidadRequerida = detalle.MaterialSpool.Cantidad,
                    Cancelado= detalle.Cancelado,
                    Estatus = TraductorEnumeraciones.TextoCanceladoActivo(!detalle.Cancelado)
                };

                lista.Add(grd);
            }


            repCortesDetalle.DataSource = lista;
            repCortesDetalle.DataBind();

        }

        protected void repCortesDetalle_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string command = e.CommandName;
            int corteDetalleID = e.CommandArgument.SafeIntParse();

            try
            {
                if (command == "cancelar")
                {
                    CorteBL.Instance.CancelarCorteDetalle(corteDetalleID, SessionFacade.UserId);
                }
                //Actualizar el grid
                cargaDatos();
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        protected void repCortesDetalle_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                GrdDetalleCorte item = e.Item.DataItem as GrdDetalleCorte;

                if (item.Cancelado)
                {
                    ImageButton imgCancelar = e.Item.FindControl("imgCancelar") as ImageButton;
                    imgCancelar.Visible = false;
                }
            }
        }
    }
}