using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mimo.Framework.Common;
using SAM.BusinessLogic.Excepciones;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using Mimo.Framework.Extensions;

namespace SAM.BusinessLogic.Administracion
{
    public class PesoItemCodeBL
    {
        private static readonly object _mutex = new object();
        private static PesoItemCodeBL _instance;
        private decimal MAXIMO_PESO = Convert.ToDecimal(9999999.99);

        private PesoItemCodeBL()
        {
        }

        public static PesoItemCodeBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PesoItemCodeBL();
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
            List<ItemCode> icList = ItemCodeBO.Instance.ObtenerListaPorProyectoID(proyectoID);

            using (SamContext ctx = new SamContext())
            {

                #region Iterar para cada renglon del archivo

                for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
                {
                    string linea = lineas[numLinea];
                    string[] palabras = linea.Split(',');
                    palabras = palabras.ToList().Select(p => p.Trim()).ToArray();

                    string codigo = palabras[0];
                    decimal peso = -1;
                    string desc = string.Empty;
                    decimal? diametro1 = null;
                    decimal? diametro2 = null;
                    int? familiaAceroID = null;

                    if (palabras.Length > 2)
                    {
                        desc = palabras[2];
                    }

                    //Verificar si el primer renglon son titulos
                    if (!codigo.ToLower().StartsWith("item"))
                    {
                        if (palabras.Length > 3)
                        {
                            if (!string.IsNullOrEmpty(palabras[3].Trim()))
                            {
                                diametro1 = palabras[3].SafeDecimalNullableParse();
                                if (diametro1 == null)
                                {
                                    errores.Add(string.Format(MensajesError.Diametro_FormatoInvalido, numLinea + 1, palabras[3]));
                                }
                            }
                        }
                        if (palabras.Length > 4)
                        {
                            if (!string.IsNullOrEmpty(palabras[4].Trim()))
                            {
                                diametro2 = palabras[4].SafeDecimalNullableParse();
                                if (diametro2 == null)
                                {
                                    errores.Add(string.Format(MensajesError.Diametro_FormatoInvalido, numLinea + 1, palabras[4]));
                                }
                            }
                        }
                        if (palabras.Length > 5)
                        {
                            // procedemos a buscar la familia acero para obtener su id
                            if (!string.IsNullOrEmpty(palabras[5].Trim()))
                            {
                                FamiliaAcero famAcero = FamiliaAceroBO.Instance.ObtenerTodas().FirstOrDefault(x => x.Nombre == palabras[5]);

                                if (famAcero != null)
                                {
                                    familiaAceroID = famAcero.FamiliaAceroID;
                                }
                                else
                                {
                                    errores.Add(string.Format(MensajesError.FamAcero_Invalido, numLinea + 1, palabras[5]));
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(palabras[1].Trim()))
                        {
                            if (!decimal.TryParse(palabras[1], out peso))
                            {
                                errores.Add(string.Format(MensajesError.PesoX_Invalido, numLinea + 1, palabras[1]));
                            }
                            else if (peso > MAXIMO_PESO || peso < 0)
                            {
                                //Especificar para que línea del archivo
                                errores.Add(string.Format(MensajesError.PesoX_Rango, peso, numLinea + 1));
                            }
                        }


                        ItemCode ic = icList.Where(x => x.Codigo == codigo && x.ProyectoID == proyectoID).SingleOrDefault();

                        if (ic != null)
                        {
                            //actualizar
                            ic.StartTracking();
                            ic.UsuarioModifica = usuarioModifica;
                            ic.FechaModificacion = DateTime.Now;
                            ic.Peso = peso != -1 ? peso : ic.Peso;
                            ic.DescripcionInterna = desc;
                            ic.Diametro1 = diametro1 != null ? diametro1 : ic.Diametro1;
                            ic.Diametro2 = diametro2 != null ? diametro2 : ic.Diametro2;
                            ic.FamiliaAceroID = familiaAceroID != null ? familiaAceroID : ic.FamiliaAceroID;
                            ctx.ItemCode.ApplyChanges(ic);
                        }
                        else
                        {
                            errores.Add(string.Format(MensajesError.ICX_Invalido, numLinea + 1, ic));
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
