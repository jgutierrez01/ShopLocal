using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Mimo.Framework.Common;

namespace SAM.Entities
{
    public partial class Usuario
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string UsernameOriginal { get; set; }
        
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string EmailOriginal { get; set; }
        
        [DataMember]
        public bool IsApproved { get; set; }
        
        [DataMember]
        public bool IsLockedOut { get; set; }

        [DataMember]
        public string Estatus
        {
            get
            {
                if (BloqueadoPorAdministrador)
                {
                    return Mensajes.Estatus_Usuario_Desactivado;
                }

                if (IsLockedOut)
                {
                    return Mensajes.Estatus_Usuario_BloquedaPasswordsIncorrectos;
                }

                if (!IsApproved)
                {
                    return Mensajes.Estatus_Usuario_SinActivacion;
                }

                return Mensajes.Estatus_Usuario_Activo;
            }
            set
            {
                //intencional para que el mapping jale
            }
        }

        [DataMember]
        public string IdiomaTexto
        {
            get
            {
                if (Idioma.Equals(LanguageHelper.ESPANOL))
                {
                    return Mensajes.Espanol;
                }

                return Mensajes.Ingles;
            }
        }
    }
}
