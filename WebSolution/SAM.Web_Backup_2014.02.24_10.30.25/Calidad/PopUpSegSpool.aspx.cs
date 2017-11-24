using System;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Ingenieria;
using SAM.Entities.Personalizadas;
using SAM.Web.Classes;
using SAM.Web.Controles.Calidad.SegSpool;
using System.Web;

namespace SAM.Web.Calidad
{   
    public partial class PopUpSegSpool : SamPaginaPopup
    {
        protected void Page_Init(object sender, EventArgs e)
        {
           
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (EntityID != null)
                {
                    if (!SeguridadQs.TieneAccesoASpool(EntityID.Value))
                    {
                        //Generar error 401 (Unauthorized access)
                        string mensaje = string.Format("El usuario {0} está intentando accesar el spool {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                        UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                    }

                    //Aqui obtenemos la entidad con la que vamos a llenar los controles del popUp y se la pasamos a Cada control para que tome 
                    //los datos que necesita
                    DetGrdSeguimientoSpool det = SpoolBO.Instance.ObtenerDetalleSeguimiento(EntityID.Value);
                    Controls.IterateRecursively(c =>
                    {
                        if (c is ControlSegSpool)
                        {
                            ((ControlSegSpool)c).MapGeneric(det);
                        }
                    });
                }
            }
        }
    }
}