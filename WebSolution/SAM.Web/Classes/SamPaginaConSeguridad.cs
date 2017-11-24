using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mimo.Framework.Extensions;
using SAM.Web.Common;

namespace SAM.Web.Classes
{
    public class SamPaginaConSeguridad : SamPaginaBase
    {
        private bool _revisarSeguridad = true;

        protected bool RevisarSeguridad
        {
            get
            {
                return _revisarSeguridad;
            }
            set
            {
                _revisarSeguridad = value;
            }
        }

        protected Guid? EntityUID
        {
            get
            {
                if (ViewState["EntityUID"] != null)
                {
                    return (Guid)ViewState["EntityUID"];
                }

                return null;
            }
            set
            {
                ViewState["EntityUID"] = value;
            }
        }

        protected int? EntityID
        {
            get
            {
                if (ViewState["EntityID"] != null)
                {
                    return (int)ViewState["EntityID"];
                }

                return null;
            }
            set
            {
                ViewState["EntityID"] = value;
            }
        }

        protected byte[] VersionRegistro
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

        protected int? otsID
        {
            get
            {
                if (ViewState["otsID"] != null)
                {
                    return (int)ViewState["otsID"];
                }
                return null;
            }
 
            set
            {
                ViewState["otsID"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (Session.IsNewSession || !SessionFacade.EstaLoggeado)
            {
                SeguridadWeb.LogoutImmediately();
            }

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (RevisarSeguridad && !SeguridadWeb.UsuarioTieneAcceso(this))
            {
                //Generar error 401 (Unauthorized access)
                UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado("El usuario no tiene acceso al recurso solicitado");
            }

            tomarValoresDefaultQs();
            base.OnLoad(e);
        }

        private void tomarValoresDefaultQs()
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    EntityID = Request.QueryString["ID"].SafeIntParse();
                }

                if (!string.IsNullOrEmpty(Request.QueryString["UID"]))
                {
                    EntityUID = new Guid(Request.QueryString["UID"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["otsID"])) 
                { 
                    otsID = Request.QueryString["otsID"].SafeIntParse(); 
                }
            }
        }
    }
}