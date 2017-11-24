using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Excepciones;
using SAM.BusinessObjects.Administracion;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessLogic.Administracion
{
    public class EspesorBL
    {
        private static readonly object _mutex = new object();
        private static EspesorBL _instance;
        private decimal MAXIMO_ESPESOR = Convert.ToDecimal(999999.9999);

        private EspesorBL()
        {
        }

        public static EspesorBL Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EspesorBL();
                    }
                }
                return _instance;
            }
        }



        /***************************************************
         *      PROCESOS Y VALIDACIONES EN ARCHIVO         *
         ***************************************************/

        public void ProcesaArchivoEspesores(Stream stream, Guid usuarioModifica)
        {
            string[] lineas = IOUtils.GetLinesFromTextStream(stream);

            if (lineas.Length == 0)
            {
                throw new ExcepcionEspesor(MensajesError.LineasX_Invalido);
            }

            List<string> errores = new List<string>();
            List<Espesor> espesores = new List<Espesor>();
            List<DiametroCache> diam = CacheCatalogos.Instance.ObtenerDiametros();
            List<CedulaCache> ced = CacheCatalogos.Instance.ObtenerCedulas();
            List<SimpleDecimal> lstEsp = new List<SimpleDecimal>();

            using (SamContext ctx = new SamContext())
            {
                decimal valorEspesor;

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

                        if (!decimal.TryParse(palabras[2], out valorEspesor))
                        {
                            errores.Add(string.Format(MensajesError.EspesorX_Invalido, numLinea + 1, palabras[2]));
                        }
                        else if (valorEspesor > MAXIMO_ESPESOR || valorEspesor <= 0)
                        {
                            errores.Add(string.Format(MensajesError.EspesorX_Rango, valorEspesor, numLinea + 1));
                            throw new ExcepcionEspesor(string.Format(MensajesError.EspesorX_Rango, valorEspesor, numLinea + 1));
                        }


                        espesores = EspesorBO.Instance.ObtenerTodos();
                        Espesor esp = espesores.Where(x => x.DiametroID == diametroID && x.CedulaID == cedulaID).SingleOrDefault();

                        if (esp != null)
                        {
                            //actualizar
                            esp.StartTracking();
                            esp.UsuarioModifica = usuarioModifica;
                            esp.FechaModificacion = DateTime.Now;
                            esp.Valor = valorEspesor;
                            esp.StopTracking();
                            ctx.Espesor.ApplyChanges(esp);

                        }
                        else
                        {
                            SimpleDecimal nuevo = new SimpleDecimal { ID = diametroID, Valor = cedulaID };

                            if (lstEsp.Count > 0)
                            {
                                //Se verifica si el archivo contiene valores duplicados para la misma cedula y diametro
                                if (lstEsp.Where(x => x.ID == nuevo.ID && x.Valor == nuevo.Valor).Any())
                                {
                                    errores.Add(string.Format(MensajesError.PeqX_Repetido, valorDiametro, cedulaStr));
                                }
                            }

                            //Se trata de una nueva
                            esp = new Espesor
                            {
                                CedulaID = cedulaID,
                                Valor = valorEspesor,
                                DiametroID = diametroID,
                                FechaModificacion = DateTime.Now,
                                UsuarioModifica = usuarioModifica
                            };

                            lstEsp.Add(nuevo);

                            //Agregar la nueva
                            ctx.Espesor.ApplyChanges(esp);
                        }
                    }
                }

                if(errores.Count == 0)
                {
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ExcepcionEspesor(errores);
                }
            }
        }
    }
}
