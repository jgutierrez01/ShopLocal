using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Ingenieria;
using SAM.BusinessObjects.Ingenieria;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using SAM.Web.Classes;


namespace SAM.Web.Controles.Ingenieria
{
    /// <summary>
    /// Control que en modo sólo lectura despliega la información de los materiales de un spool
    /// </summary>
    public partial class MaterialRO : ControlHomologacion
    {
        private bool primerRegistroBD = true;
        private bool primerRegistroPendiente = true;


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            litError.Text = "";
            divError.Visible = false;

            primerRegistroBD = true;
            primerRegistroPendiente = true;
        }

        protected override void Map()
        {
            hidSpoolId.Value = SpoolBD.SpoolID.ToString();
            hidSpoolPendienteId.Value = SpoolEnArchivo.SpoolPendienteID.ToString();

            Rebind();

        }

        protected void repMaterialesBD_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsHeader())
            {
                Literal litRevisionBD = ((Literal)e.Item.FindControl("litRevisionBD"));
                litRevisionBD.Text = RevisionBD;
            }
            if (e.Item.IsItem())
            {
                
                MaterialSpool materialSpoolEnBD = (MaterialSpool)e.Item.DataItem;
                
                Literal lit = ((Literal)e.Item.FindControl("litEtiqueta"));
                lit.Text =
                    construyeTDEtiqueta(
                        materialSpoolEnBD.Etiqueta + "<input type='hidden' id='hidMaterialSpoolId' value='" +
                        materialSpoolEnBD.MaterialSpoolID + "'/>",
                        MaterialSpoolIng.TieneMaterialDespacho(materialSpoolEnBD), String.Empty);
                
                lit = ((Literal)e.Item.FindControl("litCodigoItemCode"));
                lit.Text = construyeTD(materialSpoolEnBD.ItemCode.Codigo);

                lit = ((Literal)e.Item.FindControl("litDescripcionItemCode"));
                lit.Text = construyeTD(materialSpoolEnBD.ItemCode.DescripcionEspanol);

                lit = ((Literal)e.Item.FindControl("litDiametro1"));
                lit.Text = construyeTD(materialSpoolEnBD.Diametro1.DiameterFormat());

                lit = ((Literal)e.Item.FindControl("litDiametro2"));
                lit.Text = construyeTD(materialSpoolEnBD.Diametro2.DiameterFormat());

                lit = ((Literal)e.Item.FindControl("litCantidad"));
                lit.Text = construyeTD(materialSpoolEnBD.Cantidad.ToString());

                lit = ((Literal)e.Item.FindControl("litCategoria"));
                lit.Text = construyeTD(materialSpoolEnBD.Grupo);

                lit = ((Literal)e.Item.FindControl("litEspecificacion"));
                lit.Text = construyeTD(materialSpoolEnBD.Especificacion);

                lit = ((Literal)e.Item.FindControl("litPeso"));
                lit.Text = construyeTD(materialSpoolEnBD.Peso.ToString());


                Literal trAbre = (Literal)e.Item.FindControl("litTrAbre");
                trAbre.Text = @"<tr class=""" + (e.Item.ItemType == ListItemType.Item ? "repFila " : "repFilaPar") + @" simplehighlight"">";

                if(primerRegistroBD)
                {
                    primerRegistroBD = false;
                    trAbre.Text = @"<tr class=""" + (e.Item.ItemType == ListItemType.Item ? "repFila " : "repFilaPar") + @" simplehighlight homologacionSelected"">";

                }else
                {
                    trAbre.Text = @"<tr class=""" + (e.Item.ItemType == ListItemType.Item ? "repFila " : "repFilaPar") + @" simplehighlight"">";
                    
                }

            }


        }

        protected void repMaterialesArchivo_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsHeader())
            {
                Literal litRevisionArchivo = ((Literal)e.Item.FindControl("litRevisionArchivo"));
                litRevisionArchivo.Text = RevisionArchivo;
            }
            if (e.Item.IsItem())
            {
                if (e.Item.IsItem())
                {
                    MaterialSpoolPendiente materialSpooPendiente = (MaterialSpoolPendiente)e.Item.DataItem;

                    LlenaMaterialSpoolPendiente(e, materialSpooPendiente);
                }
            }

        }

        private void LlenaMaterialSpoolPendiente(RepeaterItemEventArgs e, MaterialSpoolPendiente materialSpooPendiente)
        {
            Literal lit = ((Literal)e.Item.FindControl("litCodigoItemCode"));
            lit.Text = construyeTD(materialSpooPendiente.ItemCode.Codigo);

            lit = ((Literal)e.Item.FindControl("litDescripcionItemCode"));
            lit.Text = construyeTD(materialSpooPendiente.ItemCode.DescripcionEspanol);

            lit = ((Literal)e.Item.FindControl("litDiametro1"));
            lit.Text = construyeTD(materialSpooPendiente.Diametro1.DiameterFormat());

            lit = ((Literal)e.Item.FindControl("litDiametro2"));
            lit.Text = construyeTD(materialSpooPendiente.Diametro2.DiameterFormat());

            lit = ((Literal)e.Item.FindControl("litCantidad"));
            lit.Text = construyeTD(materialSpooPendiente.Cantidad.ToString());

            lit = ((Literal)e.Item.FindControl("litEtiqueta"));
            lit.Text = construyeTD(materialSpooPendiente.Etiqueta + "<input type='hidden' id='hidMaterialSpoolId' value='" + materialSpooPendiente.MaterialSpoolPendienteID + "'/>");

            lit = ((Literal)e.Item.FindControl("litCategoria"));
            lit.Text = construyeTD(materialSpooPendiente.Grupo);

            lit = ((Literal)e.Item.FindControl("litEspecificacion"));
            lit.Text = construyeTD(materialSpooPendiente.Especificacion);

            lit = ((Literal)e.Item.FindControl("litPeso"));
            lit.Text = construyeTD(materialSpooPendiente.Peso.ToString());


            Literal trAbre = (Literal)e.Item.FindControl("litTrAbre");

            if (primerRegistroPendiente)
            {
                primerRegistroPendiente = false;
                trAbre.Text = @"<tr class=""" + (e.Item.ItemType == ListItemType.Item ? "repFila " : "repFilaPar") +
                              @" simplehighlight homologacionSelected"">";
            }else
            {
                trAbre.Text = @"<tr class=""" + (e.Item.ItemType == ListItemType.Item ? "repFila " : "repFilaPar") +
                              @" simplehighlight"">";
            }


        }

        private int MaterialSpoolPendienteId
        {
            get
            {
                return hidMaterialSpoolPendienteId.Value.SafeIntParse();
            }
        }

        private int MaterialSpoolId
        {
            get
            {
                return hidMaterialSpoolId.Value.SafeIntParse();
            }
        }

        protected void btnIgual_OnClick(object sender, EventArgs e)
        {
            IngenieriaBL.Instance.AccionesHomologacion(MaterialSpoolPendienteId, MaterialSpoolId, AccionesHomologacion.Igual);
            MaterialPendientePorHomologar mph =
            MaterialPendienteHelper.MaterialesPendientesPorHomologar.Where(
                x => x.PasoValidacion && x.MaterialSpoolPendienteID == MaterialSpoolPendienteId).SingleOrDefault();

            if (mph == null)
            {
                mph = MaterialPendienteHelper.MaterialesPendientesPorHomologar.Where(
                x => !x.PasoValidacion && x.MaterialSpoolPendienteID == MaterialSpoolPendienteId).Last();
            }

            if (!mph.PasoValidacion)
            {
                divError.Visible = true;
                litError.Text = mph.MensajeValidacion;
            }
            Rebind();
        }

        protected void btnNuevo_OnClick(object sender, EventArgs e)
        {
            IngenieriaBL.Instance.AccionesHomologacion(MaterialSpoolPendienteId, MaterialSpoolId, AccionesHomologacion.Nuevo);
            MaterialPendientePorHomologar mph =
            MaterialPendienteHelper.MaterialesPendientesPorHomologar.Where(
                x => x.PasoValidacion && x.MaterialSpoolPendienteID == MaterialSpoolPendienteId).SingleOrDefault();

            if (mph == null)
            {
                mph = MaterialPendienteHelper.MaterialesPendientesPorHomologar.Where(
                x => !x.PasoValidacion && x.MaterialSpoolPendienteID == MaterialSpoolPendienteId).Last();
            }

            if (!mph.PasoValidacion)
            {
                divError.Visible = true;
                litError.Text = mph.MensajeValidacion;
            }
            Rebind();
        }

        protected void btnEliminar_OnClick(object sender, EventArgs e)
        {
            IngenieriaBL.Instance.AccionesHomologacion(MaterialSpoolPendienteId, MaterialSpoolId, AccionesHomologacion.Eliminar);
            MaterialPendientePorHomologar mph =
            MaterialPendienteHelper.MaterialesPendientesPorHomologar.Where(
                x => x.PasoValidacion && x.MaterialSpoolID == MaterialSpoolId).SingleOrDefault();
            if(mph == null)
            {
                mph  = MaterialPendienteHelper.MaterialesPendientesPorHomologar.Where(
                x => !x.PasoValidacion && x.MaterialSpoolID == MaterialSpoolId).Last();
            }

            if (!mph.PasoValidacion)
            {
                divError.Visible = true;
                litError.Text = mph.MensajeValidacion;
            }
            Rebind();
        }

        protected void btnSimilar_OnClick(object sender, EventArgs e)
        {
            IngenieriaBL.Instance.AccionesHomologacion(MaterialSpoolPendienteId, MaterialSpoolId, AccionesHomologacion.Similar);
            MaterialPendientePorHomologar mph =
            MaterialPendienteHelper.MaterialesPendientesPorHomologar.Where(
                x =>x.PasoValidacion && x.MaterialSpoolPendienteID == MaterialSpoolPendienteId).SingleOrDefault();

            if (mph == null)
            {
                mph = MaterialPendienteHelper.MaterialesPendientesPorHomologar.Where(
                x => !x.PasoValidacion && x.MaterialSpoolPendienteID == MaterialSpoolPendienteId).Last();
            }

            if (!mph.PasoValidacion)
            {
                divError.Visible = true;
                litError.Text = mph.MensajeValidacion;
            }
            Rebind();
        }

        protected void repMaterialesResultado_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                MaterialPendientePorHomologar mph = (MaterialPendientePorHomologar) e.Item.DataItem;

                if (!mph.PasoValidacion)
                {
                    return;
                }

                if (mph.Accion != AccionesHomologacion.Eliminar)
                {
                    LlenaMaterialSpoolPendiente(e,
                                                SpoolEnArchivo.MaterialSpoolPendiente.Single(
                                                    x => x.MaterialSpoolPendienteID == mph.MaterialSpoolPendienteID));
                }
            }
            


        }

        protected void Rebind()
        {

            SpoolBD = SpoolBO.Instance.ObtenerDetalleHomologacion(hidSpoolId.Value.SafeIntParse());
            SpoolEnArchivo = IngenieriaBL.Instance.ObtenerPendientePorHomologar(hidSpoolPendienteId.Value.SafeIntParse());

            List<MaterialSpoolPendiente> materialesArchivo = SpoolEnArchivo.MaterialSpoolPendiente.ToList();
            List<MaterialSpool> materialesBD = SpoolBD.MaterialSpool.ToList();
            List<MaterialPendientePorHomologar> mph = MaterialPendienteHelper.MaterialesPendientesPorHomologar;

            repMaterialesArchivo.DataSource =
                materialesArchivo.Where(
                    x =>
                    !mph.Where(y => y.PasoValidacion).Select(y => y.MaterialSpoolPendienteID).Contains(
                        x.MaterialSpoolPendienteID)).OrderBy(x => x.Etiqueta);

            repMaterialesResultado.DataSource = MaterialPendienteHelper.MaterialesPendientesPorHomologar;


            repMaterialesBD.DataSource = materialesBD.Where(
                    x =>
                    !mph.Where(y => y.PasoValidacion).Select(y => y.MaterialSpoolID).Contains(
                        x.MaterialSpoolID)).OrderBy(x => x.Etiqueta);

            repMaterialesBD.DataBind();
            repMaterialesArchivo.DataBind();
            repMaterialesResultado.DataBind();

            
        }
        
    }
}