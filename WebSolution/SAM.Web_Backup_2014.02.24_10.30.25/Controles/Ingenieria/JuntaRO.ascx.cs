using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Ingenieria;
using SAM.Entities;


namespace SAM.Web.Controles.Ingenieria
{
    /// <summary>
    /// Control que en modo sólo lectura despliega la información de las juntas de un spool
    /// </summary>
    public partial class JuntaRO : ControlHomologacion
    {

        protected void repJuntasArchivo_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsHeader())
            {
                Literal litRevisionArchivo = ((Literal)e.Item.FindControl("litRevisionArchivo"));
                litRevisionArchivo.Text = RevisionArchivo;
            }
            if (e.Item.IsItem())
            {
               
                JuntaSpoolPendiente juntaSpoolEnArchivo = (JuntaSpoolPendiente)e.Item.DataItem;

                Literal lit = ((Literal)e.Item.FindControl("litEtiqueta"));
                lit.Text = construyeTDEtiqueta(juntaSpoolEnArchivo.Etiqueta, false, MensajesToolTip.JuntaPreviamenteArmada);

                lit = ((Literal)e.Item.FindControl("litDiametro"));
                lit.Text = construyeTD(juntaSpoolEnArchivo.Diametro.DiameterFormat());

                lit = ((Literal)e.Item.FindControl("litTipoJunta"));
                lit.Text = construyeTD(juntaSpoolEnArchivo.TipoJunta.Nombre);

                lit = ((Literal)e.Item.FindControl("litCedula"));
                lit.Text = construyeTD(juntaSpoolEnArchivo.Cedula);

                lit = ((Literal)e.Item.FindControl("litLocalizacion"));
                lit.Text = construyeTD(juntaSpoolEnArchivo.EtiquetaMaterial1 + "-" + juntaSpoolEnArchivo.EtiquetaMaterial2);

                lit = ((Literal)e.Item.FindControl("litFamiliasAcero"));
                //lit.Text = construyeTD(juntaSpoolEnArchivo.FamiliaAcero.Nombre);
                lit.Text =  construyeTD(juntaSpoolEnArchivo.FamiliaAcero.Nombre +
                                    (juntaSpoolEnArchivo.FamiliaAcero1 != null
                                        ? "/" + juntaSpoolEnArchivo.FamiliaAcero1.Nombre
                                        : string.Empty));

                lit = ((Literal)e.Item.FindControl("litFabArea"));
                lit.Text = construyeTD(juntaSpoolEnArchivo.FabArea.Codigo);
                
                Literal trAbre = (Literal)e.Item.FindControl("litTrAbre");
                trAbre.Text = @"<tr class=""" + (e.Item.ItemType == ListItemType.Item ? "repFila " : "repFilaPar") + @""">";


            }


        }

        protected void repJuntasBD_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.IsHeader())
            {
                Literal litRevisionBD = ((Literal)e.Item.FindControl("litRevisionBD"));
                litRevisionBD.Text = RevisionBD;
            }
            if (e.Item.IsItem())
            {
                
                Entities.JuntaSpool juntaSpoolEnBD = (Entities.JuntaSpool)e.Item.DataItem;
                Literal lit = ((Literal)e.Item.FindControl("litEtiqueta"));
                lit.Text = construyeTDEtiqueta(juntaSpoolEnBD.Etiqueta,
                                               JuntaSpoolIng.TieneArmadoOSoldado(juntaSpoolEnBD), string.Empty);

                lit = ((Literal)e.Item.FindControl("litDiametro"));
                lit.Text = construyeTD(juntaSpoolEnBD.Diametro.DiameterFormat());

                lit = ((Literal)e.Item.FindControl("litTipoJunta"));
                lit.Text = construyeTD(juntaSpoolEnBD.TipoJunta.Nombre);

                lit = ((Literal)e.Item.FindControl("litCedula"));
                lit.Text = construyeTD(juntaSpoolEnBD.Cedula);

                lit = ((Literal)e.Item.FindControl("litLocalizacion"));
                lit.Text = construyeTD(juntaSpoolEnBD.EtiquetaMaterial1 + "-" + juntaSpoolEnBD.EtiquetaMaterial2);

                lit = ((Literal)e.Item.FindControl("litFamiliasAcero"));
                lit.Text =
                    construyeTD(juntaSpoolEnBD.FamiliaAcero.Nombre +
                                        (juntaSpoolEnBD.FamiliaAcero1 != null
                                            ? "/" + juntaSpoolEnBD.FamiliaAcero1.Nombre
                                            : string.Empty));

                lit = ((Literal)e.Item.FindControl("litFabArea"));
                lit.Text = construyeTD(juntaSpoolEnBD.FabArea.Codigo);

                Literal trAbre = (Literal)e.Item.FindControl("litTrAbre");
                trAbre.Text = @"<tr class=""" + (e.Item.ItemType == ListItemType.Item ? "repFila " : "repFilaPar") + @""">";


            }


        }

        protected override void Map()
        {
           
            List<JuntaSpoolPendiente> junatssArchivo = SpoolEnArchivo.JuntaSpoolPendiente.ToList();
            
            //agregamos a los cortes en bd los registros nuevos
            List<JuntaSpool> juntasBD = SpoolBD.JuntaSpool.ToList();
            
            repJuntasArchivo.DataSource = junatssArchivo.OrderBy(x => x.Etiqueta);
            repJuntasBD.DataSource = juntasBD.OrderBy(x => x.Etiqueta);
            repJuntasBD.DataBind();
            repJuntasArchivo.DataBind();
        }

        
    }
}