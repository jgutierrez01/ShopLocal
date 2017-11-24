using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mimo.Framework.Common;
using SAM.BusinessLogic.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Ingenieria;

namespace SAM.BusinessLogic.Administracion
{
    public class CamposJuntaSpoolBL
    {
        private static readonly object _mutex = new object();
        private static CamposJuntaSpoolBL _instance;

        private CamposJuntaSpoolBL()
        {
        }

        public static CamposJuntaSpoolBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CamposJuntaSpoolBL();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="usuarioModifica"></param>
        /// <param name="proyectoID"></param>
        public void ProcesaCamposAdicionales(Stream stream, Guid usuarioModifica, int proyectoID)
        {
            string[] lineas = IOUtils.GetLinesFromTextStream(stream);

            if (lineas.Length == 0)
            {
                throw new ExcepcionPeq(MensajesError.LineasX_Invalido);
            }

            List<string> errores = new List<string>();
            List<JuntaSpool> juntaSpools = JuntaSpoolBO.Instance.ObtenerConSpoolsPorProyecto(proyectoID);

            using (SamContext ctx = new SamContext())
            {
                #region Iterar para cada renglon del archivo
                for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
                {
                    string linea = lineas[numLinea];
                    string[] palabras = linea.Split(',');
                    palabras = palabras.ToList().Select(p => p.Trim()).ToArray();

                    if (palabras.Length != 7)
                    {
                        throw new ExcepcionPeq(MensajesError.JuntaSpoolX_CamposInvalidos);
                    }

                    string nombreSpool = palabras[0];
                    string etiquetaJunta = palabras[1];
                    string fabClas = palabras[2];
                    string campo2 = palabras[3];
                    string campo3 = palabras[4];
                    string campo4 = palabras[5];
                    string campo5 = palabras[6];

                    

                    //Verificar si el primer renglon son titulos
                    if (!nombreSpool.ToLower().StartsWith("spool"))
                    {
                        JuntaSpool js = juntaSpools.Where(x => x.Spool.Nombre == nombreSpool && x.Etiqueta == etiquetaJunta).SingleOrDefault();

                        if (js != null)
                        {
                            //actualizar
                            js.StartTracking();
                            js.FabClas = !String.IsNullOrEmpty(fabClas.Trim()) ? fabClas : null;
                            js.Campo2 = !String.IsNullOrEmpty(campo2.Trim()) ? campo2 : null;
                            js.Campo3 = !String.IsNullOrEmpty(campo3.Trim()) ? campo3 : null;
                            js.Campo4 = !String.IsNullOrEmpty(campo4.Trim()) ? campo4 : null;
                            js.Campo5 = !String.IsNullOrEmpty(campo5.Trim()) ? campo5 : null;
                            js.UsuarioModifica = usuarioModifica;
                            js.FechaModificacion = DateTime.Now;

                            ctx.JuntaSpool.ApplyChanges(js);
                        }
                        else
                        {
                            errores.Add(string.Format(MensajesError.JuntaSpoolX_Invalido, numLinea + 1, etiquetaJunta, nombreSpool));
                        }
                    }
                }
                #endregion

                if (errores.Count == 0)
                {
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ExcepcionPeq(errores);
                }
            }
        }
    }
}
