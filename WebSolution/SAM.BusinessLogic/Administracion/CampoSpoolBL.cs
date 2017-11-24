using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Excepciones;
using SAM.Entities;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessLogic.Administracion
{
    public class CampoSpoolBL
    {
        private static readonly object _mutex = new object();
        private static CampoSpoolBL _instance;

        private CampoSpoolBL()
        {
        }

        public static CampoSpoolBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CampoSpoolBL();
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
        /// <param name="tipoJuntaID"></param>
        /// <param name="familiaAceroID"></param>
        /*
         *      PROCESOS Y VALIDACIONES EN ARCHIVO
         */
        public void ProcesaPeso(Stream stream, Guid usuarioModifica, int proyectoID) 
        {
            string[] lineas = IOUtils.GetLinesFromTextStream(stream);

            if (lineas.Length == 0)
            {
                throw new ExcepcionPeq(MensajesError.LineasX_Invalido);
            }

            List<string> errores = new List<string>();
            List<Spool> sList = SpoolBO.Instance.ObtenerPorProyecto(proyectoID);

            using (SamContext ctx = new SamContext())
            {

                #region Iterar para cada renglon del archivo

                for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
                {
                    string linea = lineas[numLinea];
                    string[] palabras = linea.Split(',');
                    int columnas = palabras.Count();
                    palabras = palabras.ToList().Select(p => p.Trim()).ToArray();

                    if (palabras.Length != 6)
                    {
                        throw new ExcepcionPeq(MensajesError.SpoolX_CamposInvalidos);
                    }

                    string nombre = palabras[0];
                    string campo1 = columnas > 1 ? palabras[1] : String.Empty;
                    string campo2 = columnas > 2 ? palabras[2] : String.Empty;
                    string campo3 = columnas > 3 ? palabras[3] : String.Empty;
                    string campo4 = columnas > 4 ? palabras[4] : String.Empty;
                    string campo5 = columnas > 5 ? palabras[5] : String.Empty;

                    

                    //Verificar si el primer renglon son titulos
                    if (!nombre.ToLower().StartsWith("spool"))
                    {
                        Spool s = sList.Where(x => x.Nombre == nombre).SingleOrDefault();

                        if (s != null)
                        {
                            //actualizar
                            s.StartTracking();
                            s.UsuarioModifica = usuarioModifica;
                            s.FechaModificacion = DateTime.Now;
                            s.Campo1 = !String.IsNullOrEmpty(campo1.Trim()) ? campo1 : s.Campo1;
                            s.Campo2 = !String.IsNullOrEmpty(campo2.Trim()) ? campo2 : s.Campo2;
                            s.Campo3 = !String.IsNullOrEmpty(campo3.Trim()) ? campo3 : s.Campo3;
                            s.Campo4 = !String.IsNullOrEmpty(campo4.Trim()) ? campo4 : s.Campo4;
                            s.Campo5 = !String.IsNullOrEmpty(campo5.Trim()) ? campo5 : s.Campo5;

                            ctx.Spool.ApplyChanges(s);
                        }
                        else
                        {
                            errores.Add(string.Format(MensajesError.SpoolX_Invalido, numLinea + 1, nombre));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="usuarioModifica"></param>
        /// <param name="proyectoID"></param>
        public void ProcesaPrioridades(Stream stream, Guid usuarioModifica, int proyectoID)
        {
            string[] lineas = IOUtils.GetLinesFromTextStream(stream);

            if (lineas.Length == 0)
            {
                throw new ExcepcionPeq(MensajesError.LineasX_Invalido);
            }

            List<string> errores = new List<string>();
            List<Spool> sList = SpoolBO.Instance.ObtenerPorProyecto(proyectoID);

            using (SamContext ctx = new SamContext())
            {
                #region Iterar para cada renglon del archivo
                for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
                {
                    string linea = lineas[numLinea];
                    string[] palabras = linea.Split(',');
                    palabras = palabras.ToList().Select(p => p.Trim()).ToArray();

                    string nombre = palabras[0];
                    string prioridad = palabras[1];

                    //Verificar si el primer renglon son titulos
                    if (!nombre.ToLower().StartsWith("spool"))
                    {
                        Spool s = sList.Where(x => x.Nombre == nombre).SingleOrDefault();
                        int p = prioridad.SafeIntParse();


                        if (s != null && p != -1)
                        {
                            //actualizar
                            s.StartTracking();
                            s.UsuarioModifica = usuarioModifica;
                            s.FechaModificacion = DateTime.Now;
                            s.Prioridad = p;

                            ctx.Spool.ApplyChanges(s);
                        }
                        else if (s == null && p == -1)
                        {
                        }
                        else if (s == null)
                        {
                            errores.Add(string.Format(MensajesError.SpoolX_Invalido, numLinea + 1, nombre));
                        }
                        else
                        {
                            errores.Add(string.Format(MensajesError.PrioridadX_Invalido, numLinea + 1, prioridad));
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
