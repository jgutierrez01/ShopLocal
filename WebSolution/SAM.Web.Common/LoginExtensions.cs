using System;
using System.Web.Security;
using SAM.BusinessObjects.Administracion;
using SAM.Entities;
using SAM.Web.Common.Resources;

namespace SAM.Web.Common
{
    public static class LoginExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static LoginResponse TryLogin(string username, string password)
        {
            LoginResponse response = new LoginResponse
            {
                Success = false
            };

            //Obtener el usuario
            MembershipUser mUser = Membership.GetUser(username);

            if (mUser == null)
            {
                response.ErrorMessage = MensajesLogin.Usuario_Password_Invalido;
                return response;
            }

            //Validar password vs usuario para ver si lo vamos a dejar entrar
            bool passwordValido =  Membership.ValidateUser(username, password);

            //Revisar si el usuario está bloqueado por muchos intentos de password
            if (mUser.IsLockedOut)
            {
                response.ErrorMessage = MensajesLogin.Password_Reintentos_Excedidos;
                return response;
            }

            //Revisar password
            if (!passwordValido)
            {
                response.ErrorMessage = MensajesLogin.Usuario_Password_Invalido;
                return response;
            }

            Usuario user =  UsuarioBO.Instance.Obtener((Guid)mUser.ProviderUserKey);

            //Revisar si la cuenta ya fue activada
            if (!mUser.IsApproved)
            {
                if (user.BloqueadoPorAdministrador)
                {
                    response.ErrorMessage = MensajesLogin.Cuenta_Bloqueada;
                    return response;
                }

                response.ErrorMessage = MensajesLogin.Cuenta_Falta_Activacion;
                return response;
            }

            response.User = user;
            response.Success = true;
            return response;
        }
    }
}
