using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SAM.Web.DataModel
{
    public partial class Usuario
    {
        [MetadataTypeAttribute(typeof(Usuario.UsuarioMetadata))]
        internal sealed class UsuarioMetadata
        {
            private UsuarioMetadata()
            {
            }

            public Guid UserId { get; set; }

            public int PerfilID { get; set; }

            public string Nombre { get; set; }

            public string ApPaterno { get; set; }

            public string ApMaterno { get; set; }

            public string Idioma { get; set; }

            public bool BloqueadoPorAdministrador { get; set; }

            public bool EsAdministradorSistema { get; set; }

            [RoundtripOriginal]
            public Guid UsuarioModifica { get; set; }

            [RoundtripOriginal]
            public DateTime FechaModificacion { get; set; }

            public byte[] VersionRegistro { get; set; }

            [RoundtripOriginal]
            public string VistaDashboard { get; set; }
        }
    }
}
