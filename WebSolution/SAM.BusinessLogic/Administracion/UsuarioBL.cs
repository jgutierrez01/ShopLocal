using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Administracion;
using SAM.BusinessLogic.Utilerias;
using System.Web.Security;
using Mimo.Framework.Common;

namespace SAM.BusinessLogic.Administracion
{
    public class UsuarioBL
    {
        private static readonly object _mutex = new object();
        private static UsuarioBL _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private UsuarioBL()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase UsuarioBL
        /// </summary>
        /// <returns></returns>
        public static UsuarioBL Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new UsuarioBL();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usu"></param>
        public void Guarda(Usuario usu)
        {
            bool nuevo = usu.ChangeTracker.State == ObjectState.Added;

            UsuarioBO.Instance.Guarda(usu);

            if (nuevo)
            {
                EnvioCorreos.Instance.EnviaCorreoActivacion(usu);
            }
        }

        /// <summary>
        /// Genera un nuevo password aleatorio y lo envía por correo al usuario en cuestión.
        /// </summary>
        /// <param name="userID"></param>
        public void ReiniciaPassword(Guid userID)
        {
            MembershipProvider provider = Membership.Providers["AdminMembershipProvider"];
            string password = UsuarioBO.GeneraPassword();

            MembershipUser usr = provider.GetUser(userID, false);
            usr.ChangePassword(usr.GetPassword(), password);

            Usuario usu = UsuarioBO.Instance.Obtener(userID);

            EnvioCorreos.Instance.EnviaCorreoReinicioPassword(usu, password);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        public void EnviaCorreoActivacion(Guid userID)
        {
            Usuario usu = UsuarioBO.Instance.Obtener(userID);
            EnvioCorreos.Instance.EnviaCorreoActivacion(usu);
        }
    }
}
