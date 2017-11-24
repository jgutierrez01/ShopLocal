using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace SAM.Common.Membership
{
    public static class AuxiliarMembershipProvider
    {
        public const string PREGUNTA_SECRETA_DEFAULT = "Pregunta";
        public const string RESPUESTA_SECRETA_DEFAULT = "Respuesta";

        /// <summary>
        /// Falta implementación e internacionalizar
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ErrorAmigable(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.DuplicateEmail:
                    return Mensajes.Membership_CorreoDuplicado;

                case MembershipCreateStatus.DuplicateProviderUserKey:
                    return Mensajes.Membership_LlaveDuplicada;

                case MembershipCreateStatus.DuplicateUserName:
                    return Mensajes.Membership_UsuarioDuplicado;

                case MembershipCreateStatus.InvalidAnswer:
                    return Mensajes.Membership_RespuestaInvalida;

                case MembershipCreateStatus.InvalidEmail:
                    return Mensajes.Membership_CorreoIvalido;

                case MembershipCreateStatus.InvalidPassword:
                    return Mensajes.Membership_PasswordInvalido;

                case MembershipCreateStatus.InvalidProviderUserKey:
                    return Mensajes.Membership_LlaveInvalida;

                case MembershipCreateStatus.InvalidQuestion:
                    return Mensajes.Membership_PreguntaInvalida;

                case MembershipCreateStatus.InvalidUserName:
                    return Mensajes.Membership_NombreUsuarioInvalido;

                case MembershipCreateStatus.ProviderError:
                    return Mensajes.Membership_ErrorProvider;

                case MembershipCreateStatus.UserRejected:
                    return Mensajes.Membership_UserRejected;
            }

            return string.Empty;
        }

    }
}
