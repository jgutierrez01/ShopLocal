using System.Linq;
using SAM.BusinessLogic.Ingenieria;
using Mimo.Framework.Extensions;
using System.Web.UI;
using SAM.Entities;

namespace SAM.Web.Controles.Ingenieria
{
    public abstract class ControlHomologacion: UserControl
    {
        private const string REVISION = "REV ST:{0} CTE:{1}";

        /// <summary>
        /// Metodo que sirve para cargar la informacion al control que herede de esta clase abstracta
        /// </summary>
        /// <param name="spoolBD"></param>
        /// <param name="spoolArchivo"></param>
        public void MapGeneric(Entities.Spool spoolBD, Entities.SpoolPendiente spoolArchivo)
        {
            RevisionBD = string.Format(REVISION, spoolBD.Revision, spoolBD.RevisionCliente);
            RevisionArchivo = string.Format(REVISION, spoolArchivo.Revision, spoolArchivo.RevisionCliente);
            SpoolBD = spoolBD;
            SpoolEnArchivo = spoolArchivo;
            Map();
        }

        /// <summary>
        /// Metodo para cargar la informacion particular del control
        /// </summary>
        protected abstract void Map();
       
        /// <summary>
        /// clases para identificar cambios y que el UI los muestre
        /// </summary>
        protected struct ClasesHtml
        {
            
            public const string ELIMINADO = "homologacionEliminado";
            public const string NUEVO = "homologacionNuevo";
            public const string DIFERENTE = "homologacionDiferente";
            public const string CONFLICTO = "/Imagenes/Iconos/ico_admiracion.png";  

        } 

        /// <summary>
        /// Construye un TD poniendo en la celda el primer paramtero y ponendo la clase HOMOLOGACIONDIFERENTE en caso q el segundo parametro sea diferente del primero
        /// </summary>
        /// <param name="objetoParaCelda"></param>
        /// <param name="objetoComprar"></param>
        /// <returns></returns>
        protected static string construyeTD(object objetoParaCelda, object objetoComprar)
        {
            string valorParaCelda = objetoParaCelda == null ? string.Empty : objetoParaCelda.ToString();
            string valorComprar = objetoComprar == null ? string.Empty : objetoComprar.ToString();
            const string td = @"<td class=""{0}"">{1}</td>";
            string tdClass = valorParaCelda.Trim().EqualsIgnoreCase(valorComprar.Trim())
                                 ? string.Empty
                                 : ClasesHtml.DIFERENTE;
            return string.Format(td, tdClass, valorParaCelda);
        }

        /// <summary>
        /// Construye un TD poniendo en la celda el paramtero enviado
        /// </summary>
        /// <param name="objetoParaCelda"></param>
        /// <returns></returns>
        protected static string construyeTD(object objetoParaCelda)
        {
            string valorParaCelda = objetoParaCelda == null ? string.Empty : objetoParaCelda.ToString();
            const string td = @"<td>{0}</td>";
            return string.Format(td, valorParaCelda);
        }

        /// <summary>
        /// Construye un TD poniendo en la celda el paramtero enviado, en caso de que el segundo parametro sea verdadero agregara una imagen dentro del td  y le pondra como title el 
        /// tercer parametro
        /// </summary>
        /// <param name="objetoParaCelda"></param>
        /// <param name="mostrarImagen"></param>
        /// <param name="tooltip"></param>
        /// <returns></returns>
        protected static string construyeTDEtiqueta(object objetoParaCelda, bool mostrarImagen, string tooltip)
        {
            string valorParaCelda = objetoParaCelda == null ? string.Empty : objetoParaCelda.ToString();
            const string img = @"<img src=""{0}"" title=""{1}"" />";
            const string td = @"<td>{0}{1}</td>";
            if(mostrarImagen)
            {
                string rdFinal =string.Format(img, ClasesHtml.CONFLICTO, tooltip);
                return string.Format(td, rdFinal, valorParaCelda);
            }
            return string.Format(td, string.Empty, valorParaCelda); 
        }

        /// <summary>
        /// Construye un TD poniendo en la celda el paramtero enviado, en caso de que el tercer parametro sea verdadero agregara una imagen dentro del td  y le pondra como title el 
        /// cuarto parametro, si el primer parametro y el segundo son diferentes marca el td con la clase HOMOLOGACIONDIFERENTE
        /// </summary>
        /// <param name="objetoParaCelda"></param>
        /// <param name="objetoComprar"></param>
        /// <param name="mostrarImagen"></param>
        /// <param name="tooltip"></param>
        /// <returns></returns>
        protected static string construyeTDEtiqueta(object objetoParaCelda,object objetoComprar, bool mostrarImagen, string tooltip)
        {
            string valorParaCelda = objetoParaCelda == null ? string.Empty : objetoParaCelda.ToString();
            const string img = @"<img src=""{0}"" style=""margin-right:5px"" title=""{1}"" />";
            string valorComprar = objetoComprar == null ? string.Empty : objetoComprar.ToString();
            const string td = @"<td class=""{0}"">{1}{2}</td>";
            string tdClass = valorParaCelda.Trim().EqualsIgnoreCase(valorComprar.Trim())
                                 ? string.Empty
                                 : ClasesHtml.DIFERENTE;
            if (mostrarImagen)
            {
                string rdFinal = string.Format(img, ClasesHtml.CONFLICTO, tooltip);
                return string.Format(td, tdClass, rdFinal, objetoParaCelda);
            }
            return string.Format(td, tdClass, string.Empty, objetoParaCelda);
        }

        public string RevisionBD { get; set; }
        public string RevisionArchivo { get; set; }
        protected Entities.Spool SpoolBD { get; set; }
        protected SpoolPendiente SpoolEnArchivo { get; set; }
        
    }
}
