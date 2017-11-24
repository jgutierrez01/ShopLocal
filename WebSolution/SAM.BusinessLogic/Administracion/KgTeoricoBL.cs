using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Excepciones;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessLogic.Administracion
{
    public class KgTeoricoBL
    {
        private static readonly object _mutex = new object();
        private static KgTeoricoBL _instance;
        private decimal MAXIMO_KGTEORICO = Convert.ToDecimal(99999999.9999);

        private KgTeoricoBL()
        {
        }

        public static KgTeoricoBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new KgTeoricoBL();
                    }
                }
                return _instance;
            }
        }


        /*
         *      PROCESOS Y VALIDACIONES EN ARCHIVO
         */
        public void ProcesaArchivoKgTeoricos(Stream stream, Guid usuarioModifica)
        {
            string[] lineas = IOUtils.GetLinesFromTextStream(stream);

            if (lineas.Length == 0)
            {
                throw new ExcepcionKgTeorico(MensajesError.LineasX_Invalido);
            }

            List<string> errores = new List<string>();
            List<KgTeorico> kgTeoricos = new List<KgTeorico>();
            List<DiametroCache> diam = CacheCatalogos.Instance.ObtenerDiametros();
            List<CedulaCache> ced = CacheCatalogos.Instance.ObtenerCedulas();
            List<SimpleDecimal> lstKg = new List<SimpleDecimal>();

            using (SamContext ctx = new SamContext())
            {
                decimal valorKgTeorico;

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

                    if (diametro == null && cedula == null && numLinea == 0)
                    {
                    }
                    else
                    {

                        if (diametro == null)
                        {
                            errores.Add(string.Format(MensajesError.DiametroX_NoExiste, numLinea + 1, palabras[0]));
                        }
                        else
                        {
                            diametroID = diametro.ID;
                        }


                        if (cedula == null)
                        {
                            errores.Add(string.Format(MensajesError.CedulaX_NoExiste, numLinea + 1, palabras[1]));
                        }
                        else
                        {
                            cedulaID = cedula.ID;
                        }

                        if (!decimal.TryParse(palabras[2], out valorKgTeorico))
                        {
                            errores.Add(string.Format(MensajesError.KgTeoricoX_Invalido, numLinea + 1, palabras[2]));
                        }
                        else if (valorKgTeorico > MAXIMO_KGTEORICO || valorKgTeorico <= 0)
                        {
                            errores.Add(string.Format(MensajesError.KgTeoricoX_Rango, valorKgTeorico, numLinea + 1));
                            throw new ExcepcionKgTeorico(string.Format(MensajesError.KgTeoricoX_Rango, valorKgTeorico, numLinea + 1));
                        }

                        kgTeoricos = KgTeoricoBO.Instance.ObtenerTodos();
                        KgTeorico kgteo = kgTeoricos.Where(x => x.DiametroID == diametroID && x.CedulaID == cedulaID).SingleOrDefault();

                        if (kgteo != null)
                        {
                            //actualizar
                            kgteo.StartTracking();
                            kgteo.UsuarioModifica = usuarioModifica;
                            kgteo.FechaModificacion = DateTime.Now;
                            kgteo.Valor = valorKgTeorico;
                            kgteo.StopTracking();
                            ctx.KgTeorico.ApplyChanges(kgteo);
                        }
                        else
                        {
                            SimpleDecimal nuevo = new SimpleDecimal { ID = diametroID, Valor = cedulaID };

                            if (lstKg.Count > 0)
                            {
                                //Se verifica si el archivo contiene valores duplicados para la misma cedula y diametro
                                if (lstKg.Where(x => x.ID == nuevo.ID && x.Valor == nuevo.Valor).Any())
                                {
                                    errores.Add(string.Format(MensajesError.PeqX_Repetido, valorDiametro, cedulaStr));
                                }
                            }

                            //Se trata de una nueva
                            kgteo = new KgTeorico
                                        {
                                            CedulaID = cedulaID,
                                            Valor = valorKgTeorico,
                                            DiametroID = diametroID,
                                            FechaModificacion = DateTime.Now,
                                            UsuarioModifica = usuarioModifica
                                        };

                            lstKg.Add(nuevo);

                            //Agregar la nueva
                            ctx.KgTeorico.ApplyChanges(kgteo);
                        }
                    }
                }
                if (errores.Count == 0)
                {
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ExcepcionKgTeorico(errores);
                }
            }
        }
    }
}

