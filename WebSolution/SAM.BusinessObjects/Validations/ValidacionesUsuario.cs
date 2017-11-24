using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using SAM.Common.Membership;
using System.Text.RegularExpressions;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesUsuario
    {

        /// <summary>
        /// Regresa true si es válido, false de lo contrario
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userActual"></param>
        /// <param name="errores"></param>
        /// <returns></returns>
        public static  bool ValidaUsernameDuplicado(string username, Guid userActual, List<string> errores)
        {
            MembershipUser mUsu = Membership.GetUser(username);

            if (mUsu != null)
            {
                if (userActual != Guid.Empty)
                {
                    if ((Guid)mUsu.ProviderUserKey == userActual)
                    {
                        //Edición, es válido que sea el mismo username, de hecho es esperado
                        return true;
                    }
                }

                if (errores != null)
                {
                    errores.Add(AuxiliarMembershipProvider.ErrorAmigable(MembershipCreateStatus.DuplicateUserName));
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Regresa true si es válido, false de lo contrario
        /// </summary>
        /// <param name="correo"></param>
        /// <param name="userActual"></param>
        /// <param name="errores"></param>
        /// <returns></returns>
        public static bool ValidaCorreoDuplicadoUsuarioNuevo(string correo, Guid userActual, List<string> errores)
        {
            MembershipUserCollection usuarios = Membership.FindUsersByEmail(correo);

            if (usuarios.Count > 0)
            {

                if ( userActual != Guid.Empty )
                {
                    foreach (MembershipUser usuario in usuarios)
                    {
                        if ((Guid)usuario.ProviderUserKey == userActual)
                        {
                            //Edicion de usuario, es válido que el correo sea igual
                            return true;
                        }
                    }
                }

                if (errores != null)
                {
                    errores.Add(AuxiliarMembershipProvider.ErrorAmigable(MembershipCreateStatus.DuplicateEmail));
                }
                return false;
            }

            return true;
        }


        /// <summary>
        /// Regresa true si el password cumple con las características de seguridad, false
        /// de lo contrario.
        /// </summary>
        /// <param name="password">password a evaluar</param>
        /// <returns>true si el password cumple</returns>
        public static bool PasswordCumpleRequisitos(string password)
        {
            bool valido = false;

            //Revisar en base a la configuración del provider
            int minLength = Membership.MinRequiredPasswordLength;
            int minNonAlpha = Membership.MinRequiredNonAlphanumericCharacters;
            string regEx = Membership.PasswordStrengthRegularExpression;

            int nonAlpha = 0;
            foreach (char c in password)
                if (!char.IsLetterOrDigit(c))
                    nonAlpha++;

            if (password.Length < minLength)
                return false;

            if (nonAlpha < minNonAlpha)
                return false;

            //we need to check for regular expression strength
            if (!string.IsNullOrEmpty(regEx))
            {
                Regex regex = null;
                try
                {
                    regex = new Regex(regEx);

                    if (regex.IsMatch(password))
                        valido = true;
                }
                catch
                {
                    //La expresión regular es inválida
                    valido = false;
                }
            }

            return valido;
        }
    }
}
