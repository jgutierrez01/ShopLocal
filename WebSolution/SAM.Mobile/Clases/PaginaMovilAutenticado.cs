using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mimo.Framework.Common;
using Mimo.Framework.WebControls;
using Mimo.Framework.Extensions;

namespace SAM.Mobile.Clases
{
    public class PaginaMovilAutenticado : PaginaMovil
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

        protected override void OnLoad(EventArgs e)
        {
            if (RevisarSeguridad && !SeguridadWeb.UsuarioTieneAcceso(this))
            {
                //Generar error 401 (Unauthorized access)
                throw new HttpException(0x191, "El usuario no tiene acceso al recurso solicitado");
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
            }
        }
    }
}