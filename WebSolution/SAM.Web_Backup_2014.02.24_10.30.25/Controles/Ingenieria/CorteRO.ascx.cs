using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Ingenieria;
using SAM.Entities;
using SAM.Entities.Personalizadas;

namespace SAM.Web.Controles.Ingenieria
{
    /// <summary>
    /// Versión solo lectura de los cortes de un spool en particular
    /// </summary>
    public partial class CorteRO : ControlHomologacion
    {
       
        protected override void Map()
        {
            
            //agregamos a los cortes en el archivo los registros de los eliminados
            List<CorteSpoolPendiente> cortesArchivo = SpoolEnArchivo.CorteSpoolPendiente.ToList();
            List<CorteSpool> cortesBD = SpoolBD.CorteSpool.ToList();

            repCortesArchivo.DataSource = cortesArchivo.OrderBy(x=> x.EtiquetaMaterial);
            repCortesBD.DataSource = cortesBD.OrderBy(x => x.EtiquetaMaterial);
            repCortesBD.DataBind();
            repCortesArchivo.DataBind();
        }

        protected void repCortesArchivo_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsHeader())
            {
                Literal litRevisionArchivo = ((Literal)e.Item.FindControl("litRevisionArchivo"));
                litRevisionArchivo.Text = RevisionArchivo;
            }
            if (e.Item.IsItem())
            {
                string trClass = "";
                CorteSpoolPendiente corteSpoolEnArchivo = (CorteSpoolPendiente)e.Item.DataItem;
                
                
                Literal lit = ((Literal)e.Item.FindControl("litEtiquetaMaterial"));
                lit.Text = construyeTD( corteSpoolEnArchivo.EtiquetaMaterial);

                lit = ((Literal)e.Item.FindControl("litCodigoItemCode"));
                lit.Text = construyeTD(corteSpoolEnArchivo.ItemCode.Codigo);

                lit = ((Literal)e.Item.FindControl("litDescripcionItemCode"));
                lit.Text = construyeTD(corteSpoolEnArchivo.ItemCode.DescripcionEspanol);

                lit = ((Literal)e.Item.FindControl("litDiametro"));
                lit.Text = construyeTD(corteSpoolEnArchivo.Diametro.DiameterFormat());

                lit = ((Literal)e.Item.FindControl("litCantidad"));
                lit.Text = construyeTD(string.Empty + corteSpoolEnArchivo.Cantidad);

                lit = ((Literal)e.Item.FindControl("litEtiquetaSeccion"));
                lit.Text = construyeTD(corteSpoolEnArchivo.EtiquetaSeccion);

                lit = ((Literal)e.Item.FindControl("litInicioFin"));
                lit.Text = construyeTD(corteSpoolEnArchivo.InicioFin);

                lit = ((Literal)e.Item.FindControl("litTipoCorte1"));
                lit.Text = construyeTD(corteSpoolEnArchivo.TipoCorte.Nombre);

                lit = ((Literal)e.Item.FindControl("litTipoCorte2"));
                lit.Text = construyeTD(corteSpoolEnArchivo.TipoCorte1.Nombre);

                
                Literal trAbre = (Literal)e.Item.FindControl("litTrAbre");
                trAbre.Text = @"<tr class=""" + (e.Item.ItemType == ListItemType.Item ? "repFila " : "repFilaPar") + trClass + @""">";


            }


        }

        protected void repCortesBD_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsHeader())
            {
                Literal litRevisionBD = ((Literal)e.Item.FindControl("litRevisionBD"));
                litRevisionBD.Text = RevisionBD;
            }
            if (e.Item.IsItem())
            {
               
                CorteSpool corteSpoolEnBD = (CorteSpool) e.Item.DataItem;
               
                Literal lit = ((Literal) e.Item.FindControl("litEtiquetaMaterial"));
                lit.Text = construyeTD(corteSpoolEnBD.EtiquetaMaterial);

                lit = ((Literal) e.Item.FindControl("litCodigoItemCode"));
                lit.Text = construyeTD(corteSpoolEnBD.ItemCode.Codigo);

                lit = ((Literal) e.Item.FindControl("litDescripcionItemCode"));
                lit.Text = construyeTD(corteSpoolEnBD.ItemCode.DescripcionEspanol);

                lit = ((Literal) e.Item.FindControl("litDiametro"));
                lit.Text = construyeTD(corteSpoolEnBD.Diametro.DiameterFormat());

                lit = ((Literal) e.Item.FindControl("litCantidad"));
                lit.Text = construyeTD(corteSpoolEnBD.Cantidad);

                lit = ((Literal) e.Item.FindControl("litEtiquetaSeccion"));
                lit.Text = construyeTD(corteSpoolEnBD.EtiquetaSeccion);

                lit = ((Literal) e.Item.FindControl("litInicioFin"));
                lit.Text = construyeTD(corteSpoolEnBD.InicioFin);

                lit = ((Literal) e.Item.FindControl("litTipoCorte1"));
                lit.Text = construyeTD(corteSpoolEnBD.TipoCorte.Nombre);

                lit = ((Literal) e.Item.FindControl("litTipoCorte2"));
                lit.Text = construyeTD(corteSpoolEnBD.TipoCorte1.Nombre);

                
                Literal trAbre = (Literal)e.Item.FindControl("litTrAbre");
                trAbre.Text = @"<tr class=""" + (e.Item.ItemType == ListItemType.Item ? "repFila " : "repFilaPar") + @""">";
                

            }


        }

       
    }
}