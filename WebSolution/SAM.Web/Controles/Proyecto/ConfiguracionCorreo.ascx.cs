using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Proyectos;
using SAM.Web.Classes;
using Mimo.Framework.WebControls;
using SAM.Entities;
using SAM.Web.Common;

namespace SAM.Web.Controles.Proyecto
{
    public partial class ConfiguracionCorreo : System.Web.UI.UserControl, IMappable
    {

        private int? ProyectoID
        {
            get
            {
                if (ViewState["ProyectoID"] != null)
                {
                    return (int)ViewState["ProyectoID"];
                }
                return null;
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        private byte[] VersionRegistro
        {
            get
            {
                if (ViewState["VersionRegistro"] != null)
                {
                    return (byte[])ViewState["VersionRegistro"];
                }
                return null;
            }
            set
            {
                ViewState["VersionRegistro"] = value;
            }
        }

        #region IMappable Members
        public void Map(object entity)
        {
            Entities.Proyecto proyectoCargado = (Entities.Proyecto)entity;

            proyectoCargado.StartTracking();
            proyectoCargado.ProyectoConfiguracion.StartTracking();

            ProyectoID = proyectoCargado.ProyectoID;
            VersionRegistro = proyectoCargado.VersionRegistro;

            if (proyectoCargado.ProyectoConfiguracion != null)
            {
                txtNombreCorreo.Text = proyectoCargado.ProyectoConfiguracion.CorreoPeqKgEsp;
            }
            else
            {
                txtNombreCorreo.Text = string.Empty;
            }

        }

        public void Unmap(object entity)
        {
            Entities.Proyecto proyectoCargado = (Entities.Proyecto)entity;

            proyectoCargado.StartTracking();
            proyectoCargado.ProyectoConfiguracion.StartTracking();

            if (ProyectoID != null)
            {
                proyectoCargado.ProyectoID = ProyectoID.Value;
                proyectoCargado.VersionRegistro = VersionRegistro;

                proyectoCargado.ProyectoConfiguracion.CorreoPeqKgEsp = txtNombreCorreo.Text;
                proyectoCargado.ProyectoConfiguracion.UsuarioModifica = SessionFacade.UserId;
                proyectoCargado.ProyectoConfiguracion.FechaModificacion = DateTime.Now;
            }
        }

        #endregion

    }
}