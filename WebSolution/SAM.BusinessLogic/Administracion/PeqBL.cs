using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessLogic.Administracion
{
    public class PeqBL
    {
        private static readonly object _mutex = new object();
        private static PeqBL _instance;
        private decimal MAXIMO_PEQ = Convert.ToDecimal(99999999.9999);

        private PeqBL()
        {
        }

        public static PeqBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PeqBL();
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
        public void ProcesaPeq(Stream stream, Guid usuarioModifica, int tipoJuntaID, int familiaAceroID, int proyectoID)
        {
            string[] lineas = IOUtils.GetLinesFromTextStream(stream);
            
            if (lineas.Length == 0)
            {
                throw new ExcepcionPeq(MensajesError.LineasX_Invalido);
            }

            List<string> errores = new List<string>();
            List<DiametroCache> diam = CacheCatalogos.Instance.ObtenerDiametros();
            List<CedulaCache> ced = CacheCatalogos.Instance.ObtenerCedulas();
            List<SimpleDecimal> lstPeq = new List<SimpleDecimal>();

            using(SamContext ctx = new SamContext())
            {
                //Solo las peqs que nos interesan
                List<Peq> peqs = ctx.Peq
                                    .Where(x => x.FamiliaAceroID == familiaAceroID && x.TipoJuntaID == tipoJuntaID && x.ProyectoID == proyectoID)
                                    .ToList();

                decimal valorPeq;

                #region Iterar para cada renglon del archivo

                for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
                {
                    string linea = lineas[numLinea];
                    string[] palabras = linea.Split(',');
                    palabras = palabras.ToList().Select(p => p.Trim()).ToArray();
                    int diametroID = 0;
                    int cedulaID = 0;

                    decimal valorDiametro = palabras[0].SafeDecimalParse();
                    DiametroCache diametro = diam.SingleOrDefault(x => x.Valor == valorDiametro);
                    string cedulaStr = palabras[1];
                    CedulaCache cedula = ced.SingleOrDefault(x => x.Nombre.EqualsIgnoreCase(cedulaStr));

                    if(diametro == null && cedula == null && numLinea == 0)
                    {
                    }
                    else
                    {
                        if (diametro == null)
                        {
                            //Internacionalizar y especificar qué diámetro
                            errores.Add(string.Format(MensajesError.DiametroX_NoExiste, numLinea + 1, palabras[0]));
                        }
                        else
                        {
                            diametroID = diametro.ID;
                        }

                        if (cedula == null)
                        {
                            //Internacionalizar y especificar qué cédula
                            errores.Add(string.Format(MensajesError.CedulaX_NoExiste, numLinea + 1, palabras[1]));
                        }
                        else
                        {
                            cedulaID = cedula.ID;
                        }

                        if (!decimal.TryParse(palabras[2], out valorPeq))
                        {
                            errores.Add(string.Format(MensajesError.PeqX_Invalido, numLinea + 1, palabras[2]));
                        }
                        else if (valorPeq > MAXIMO_PEQ || valorPeq <= 0)
                        {
                            //Especificar para que línea del archivo
                            errores.Add(string.Format(MensajesError.PeqX_Rango, valorPeq, numLinea + 1));
                            throw new ExcepcionPeq(string.Format(MensajesError.PeqX_Rango, valorPeq, numLinea + 1));
                        }

                        
                        Peq peq = peqs.Where(x => x.DiametroID == diametroID && x.CedulaID == cedulaID && x.ProyectoID == proyectoID).SingleOrDefault();

                        if (peq != null)
                        {
                            //actualizar
                            peq.StartTracking();
                            peq.UsuarioModifica = usuarioModifica;
                            peq.FechaModificacion = DateTime.Now;
                            peq.Equivalencia = valorPeq;
                            peq.StopTracking();
                            ctx.Peq.ApplyChanges(peq);
                        }
                        else
                        { 
                            SimpleDecimal nuevo = new SimpleDecimal { ID = diametroID, Valor = cedulaID };
                            
                            if (lstPeq.Count > 0)
                            {                               
                                //Se verifica si el archivo contiene valores duplicados para la misma cedula y diametro
                                if (lstPeq.Where(x => x.ID == nuevo.ID && x.Valor == nuevo.Valor).Any())
                                {
                                    errores.Add(string.Format(MensajesError.PeqX_Repetido, valorDiametro, cedulaStr));
                                }
                            }

                            //Se trata de una nueva
                            peq = new Peq
                            {
                                ProyectoID = proyectoID,
                                CedulaID = cedulaID,
                                Equivalencia = valorPeq,
                                TipoJuntaID = tipoJuntaID,
                                FamiliaAceroID = familiaAceroID,
                                DiametroID = diametroID,
                                FechaModificacion = DateTime.Now,
                                UsuarioModifica = usuarioModifica
                            };

                            lstPeq.Add(nuevo);

                            //Agregar la nueva
                            ctx.Peq.ApplyChanges(peq);
                        }
                    }
                }

                #endregion

                if(errores.Count == 0)
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
