using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.BusinessObjects.Proyectos;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Proyectos
{
    public partial class Programa : SamPaginaPrincipal
    {
        #region Variable privadas

        /// <summary>
        /// Numero de registros de periodo
        /// </summary>
        private int _periodos = 0;

        #endregion

        #region Eventos de Página

        /// <summary>
        /// Carga los valores iniciales en los controles de la página, los cuales se obtiene de la BD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar a la programación de un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_Programa, EntityID.Value);
                cargaInformacion(EntityID.Value);
                titulo.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_PROYECTO, EntityID.Value);
            }
        }

        #endregion

        #region Event Handlers de los eventos disparados por los controles de la página

        /// <summary>
        /// Agrega una nueva fila al repeater de periodos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAgregar_Click(object sender, EventArgs e)
        {
            agregaPeriodo();
        }

        /// <summary>
        /// Agrega una nueva fila al repeater de periodos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgAgregar_Click(object sender, EventArgs e)
        {
            agregaPeriodo();
        }

        /// <summary>
        /// Elimina el último elemento de la lista de periodos y vuelve a hacer el binding al repeater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBorrar_Click(object sender, EventArgs e)
        {
            List<PeriodoPrograma> lst = unbindRepeater();
            lst.RemoveAt(lst.Count - 1);
            bindPeriodos(lst);
        }

        /// <summary>
        /// Hace el unbinding de los controles y manda llamar a middle-tier para persistir
        /// la información en base de datos.
        /// 
        /// La capa de negocios se encarga de la lógica necesaria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            //Las validaciones de estos grupos NO SE disparan automáticos ya que el botón pertenece a un validation groups distinto
            //Validar server-side los periodos
            Validate("vgPeriodos");
            //Validar server-side los controles globales
            Validate("vgPrincipal");
            
            if (Page.IsValid)
            {
                ProyectoPrograma programa = new ProyectoPrograma();

                programa.Rango = ddlRango.SelectedValue;
                programa.Unidades = ddlUnidades.SelectedValue;
                programa.IsosReprogramados = txtIsosReprogramaciones.Text.SafeIntParse(0);
                programa.IsosPlaneados = txtIsosPlaneados.Text.SafeIntParse(0);
                programa.SpoolsPlaneados = txtSpoolsPlaneados.Text.SafeIntParse(0);
                programa.SpoolsReprogramados = txtSpoolsReprogramaciones.Text.SafeIntParse(0);

                List<PeriodoPrograma> lst = unbindRepeater();

                try
                {
                    ProgramaBO.Instance.Guarda(programa, lst, SessionFacade.UserId, EntityID.Value, dtpFechaInicial.SelectedDate.Value);

                    Response.Redirect(string.Format(WebConstants.ProyectoUrl.DET_PROYECTO, EntityID.Value));
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex, "valPrincipal");
                }
            }
        }

        /// <summary>
        /// Se dispara cada que se hace binding de una fila del repeater.
        /// En este método se establecen los valores de los controles del repeater.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void repPeriodos_ItemDataBound(object sender, RepeaterItemEventArgs args)
        {
            if (args.Item.IsItem())
            {
                RepeaterItem item = args.Item;
                PeriodoPrograma periodo = (PeriodoPrograma)item.DataItem;

                Label lblNumero = (Label)item.FindControl("lblNumero");
                Label lblFechaInicio = (Label)item.FindControl("lblFechaInicio");
                Label lblFechaFin = (Label)item.FindControl("lblFechaFin");
                Label lblUnidades = (Label)item.FindControl("lblUnidades");
                Label lblUnidades2 = (Label)item.FindControl("lblUnidades2");
                TextBox txtContrato = (TextBox)item.FindControl("txtContrato");
                TextBox txtReprogramaciones = (TextBox)item.FindControl("txtReprogramaciones");
                ImageButton imgBorrar = (ImageButton)item.FindControl("imgBorrar");

                lblNumero.Text = periodo.Numero.ToString();
                lblFechaInicio.Text = periodo.FechaInicio.ToShortDateString();
                lblFechaFin.Text = periodo.FechaFin.ToShortDateString();
                txtContrato.Text = periodo.PorContrato.ToString("#,##0.00");
                txtReprogramaciones.Text = periodo.Reprogramaciones.ToString("#,##0.00");

                string unidades = ddlUnidades.SelectedItem.Text;

                if (unidades == "m2")
                {
                    unidades = "m<sup>2</sup>";
                }

                lblUnidades.Text = unidades;
                lblUnidades2.Text = unidades;

                imgBorrar.Visible = item.ItemIndex == _periodos - 1;
            }
        }

        /// <summary>
        /// Se asegura que los valores numéricos introducidos para los periodos estén entre 0 y 99999999.99
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void cusRango_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            decimal valor = -1;

            if (decimal.TryParse(args.Value, out valor))
            {
                if (valor >= 0 && valor <= 99999999.99m)
                {
                    args.IsValid = true;
                }
            }
        }

        #endregion

        #region Binding y unbinding de información

        /// <summary>
        /// En base a un proyecto carga la información de programa y periodos
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        public void cargaInformacion(int proyectoID)
        {
            headerProyecto.BindInfo(proyectoID);
            ProyectoPrograma programa = ProgramaBO.Instance.ObtenerConPeriodosPorProyectoID(proyectoID);
            Proyecto proyecto = ProyectoBO.Instance.Obtener(proyectoID);
            
            txtIsosPlaneados.Text = programa.IsosPlaneados.ToString();
            txtIsosReprogramaciones.Text = programa.IsosReprogramados.ToString();
            txtSpoolsPlaneados.Text = programa.SpoolsPlaneados.ToString();
            txtSpoolsReprogramaciones.Text = programa.SpoolsReprogramados.ToString();

            ddlRango.SelectedValue = programa.Rango;
            ddlUnidades.SelectedValue = programa.Unidades;

            dtpFechaInicial.SelectedDate = proyecto.FechaInicio;

            bindPeriodos(programa.PeriodoPrograma.OrderBy(x => x.Numero).ToList());
        }

        /// <summary>
        /// Toma una lista de PeriodoPrograma y se la establece al repeater de periodos
        /// </summary>
        /// <param name="periodos">Lista de periodos de BD</param>
        private void bindPeriodos(List<PeriodoPrograma> periodos)
        {
            _periodos = periodos.Count;
            repPeriodos.DataSource = periodos;
            repPeriodos.DataBind();
        }

        /// <summary>
        /// Itera los elementos del repeater y toma sus valores para convertirlos en una lista de
        /// pbjetos de tipo periodo lista para binding al mismo repeater o para guardar a base de datos.
        /// </summary>
        /// <returns>Lista de objetos tipo PeriodoPrograma con la información cargada en el repeater</returns>
        private List<PeriodoPrograma> unbindRepeater()
        {
            List<PeriodoPrograma> lst = new List<PeriodoPrograma>((repPeriodos.Items.Count + 1) * 2);

            foreach (RepeaterItem item in repPeriodos.Items)
            {
                if (item.IsItem())
                {
                    Label lblNumero = (Label)item.FindControl("lblNumero");
                    Label lblFechaInicio = (Label)item.FindControl("lblFechaInicio");
                    Label lblFechaFin = (Label)item.FindControl("lblFechaFin");
                    TextBox txtContrato = (TextBox)item.FindControl("txtContrato");
                    TextBox txtReprogramaciones = (TextBox)item.FindControl("txtReprogramaciones");

                    lst.Add(new PeriodoPrograma()
                    {
                        Numero = lblNumero.Text.SafeIntParse(),
                        FechaInicio = Convert.ToDateTime(lblFechaInicio.Text),
                        FechaFin = Convert.ToDateTime(lblFechaFin.Text),
                        PorContrato = txtContrato.Text.SafeDecimalParse(),
                        Reprogramaciones = txtReprogramaciones.Text.SafeDecimalParse()
                    });
                }
            }

            return lst;
        }

        #endregion

        #region Auxiliares

        /// <summary>
        /// Agrega una nueva fila de periodo al repeater en la última posición
        /// </summary>
        private void agregaPeriodo()
        {
            List<PeriodoPrograma> lst = unbindRepeater();
            PeriodoPrograma ultimoPeriodo = lst.LastOrDefault();
            PeriodoPrograma nuevoPeriodo = new PeriodoPrograma();

            nuevoPeriodo.Numero = ultimoPeriodo != null ? ultimoPeriodo.Numero + 1 : 0;
            nuevoPeriodo.FechaInicio = calculaFechaInicio(ultimoPeriodo);
            nuevoPeriodo.FechaFin = calculaFechaFin(nuevoPeriodo.FechaInicio);

            lst.Add(nuevoPeriodo);
            bindPeriodos(lst);
        }

        /// <summary>
        /// Calcula la fecha de inicio del siguiente periodo en base a la fecha de fin del periodo anterior
        /// </summary>
        /// <param name="ultimoPeriodo">Periodo anterior o nulo si no existe</param>
        /// <returns>Fecha de inicio del siguiente periodo</returns>
        private DateTime calculaFechaInicio(PeriodoPrograma ultimoPeriodo)
        {
            if (ultimoPeriodo == null)
            {
                return dtpFechaInicial.SelectedDate.Value;
            }
            else
            {
                return ultimoPeriodo.FechaFin.AddDays(1);
            }
        }


        /// <summary>
        /// Calcula la fecha de fin de un periodo en base a su fecha inicial y el rango seleccionado
        /// </summary>
        /// <param name="inicio">Fecha de inicio del periodo</param>
        /// <returns>Fecha de fin calculada</returns>
        private DateTime calculaFechaFin(DateTime inicio)
        {
            switch (ddlRango.SelectedValue.ToLowerInvariant())
            {
                case "s": //semana
                    return inicio.AddDays(6);
                case "m": //mes
                    return inicio.AddMonths(1).AddDays(-1);
            }

            return inicio;
        }

        #endregion
    }
}