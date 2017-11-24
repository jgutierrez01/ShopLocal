using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAM.Mobile.Clases
{
    public class WebConstants
    {
        #region Urls publicos

        public struct PublicUrl
        {
            public const string LOGIN = "~/Login.aspx";
            public const string LOGOUT = "/Logout.aspx";
        }

        #endregion

        #region Urls Moviles

        public struct MobileUrl
        {
            public const string DASHBOARD = "~/Dashboard.aspx";
            public const string CAMBIOPATIO = "~/Paginas/CambioPatio.aspx";
            public const string ARMADO = "~/Paginas/Armado.aspx";
            public const string DETALLEARMADO = "~/Paginas/DetalleArmado.aspx";
            public const string SOLDADURA = "~/Paginas/Soldadura.aspx";
            public const string DETALLESOLDADURA = "~/Paginas/DetalleSoldadura.aspx";
            public const string INSPECCIONVISUAL = "~/Paginas/InspeccionVisual.aspx";
            public const string DETALLEINSPECCIONVISUAL = "~/Paginas/DetalleInspeccionVisual.aspx";
            public const string INSPECCIONDIMENSIONAL = "~/Paginas/InspeccionDimensional.aspx";
            public const string DETALLEINSPECCIONDIMENSIONAL = "~/Paginas/DetalleInspeccionDimensional.aspx";
            public const string SEGUIMIENTOSPOOL = "~/Paginas/SeguimientoSpool.aspx";
            public const string DETALLESEGUIMIENTOSPOOL = "~/Paginas/DetalleSeguimientoSpool.aspx";
            public const string CAMBIOPROYECTO = "~/Paginas/SeleccionarProyecto.aspx";
        }

        #endregion
    }
}