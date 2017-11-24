﻿using System;
using SAM.Entities.Grid;

namespace SAM.Web.Controles.Calidad.SegJunta
{
    public partial class PruebaPTPostTT : ControlSegJunta
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected internal override void Map(GrdSeguimientoJunta detalle)
        {
            repsPnd.DatasourceCuad = detalle.PruebaPTPostTTPndCuad;
            repsPnd.DatasourceSect = detalle.PruebaPTPostTTPndSector;
        }
    }
}