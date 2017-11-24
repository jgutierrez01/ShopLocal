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
    public class CamposMaterialSpoolBL
    {
        private static readonly object _mutex = new object();
        private static CamposMaterialSpoolBL _instance;

        private CamposMaterialSpoolBL()
        {
        }

        public static CamposMaterialSpoolBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CamposMaterialSpoolBL();
                    }
                }
                return _instance;
            }
        }

        public void ProcesaCamposAdicionales(Stream stream, Guid usuarioModifica, int proyectoID)
        {
            string[] lineas = IOUtils.GetLinesFromTextStream(stream);

            if (lineas.Length == 0)
            {
                throw new ExcepcionPeq(MensajesError.LineasX_Invalido);
            }

            List<string> errores = new List<string>();
            List<MaterialSpool> materialesSpools = MaterialSpoolBO.Instance.ObtenerListaMaterialesPorProyecto(proyectoID);

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
                        throw new ExcepcionPeq(MensajesError.MaterialSpoolX_CamposInvalidos);
                    }

                    string nombreSpool = palabras[0];
                    string etiquetaMaterial = palabras[1];
                    string campo1 = palabras[2];
                    string campo2 = palabras[3];
                    string campo3 = palabras[4];
                    string campo4 = palabras[5];
                    string campo5 = palabras[6];

                    

                    //Verificar si el primer renglon son titulos
                    if (!nombreSpool.ToLower().StartsWith("spool"))
                    {
                        //JuntaSpool js = juntaSpools.Where(x => x.Spool.Nombre == nombreSpool && x.Etiqueta == etiquetaJunta).SingleOrDefault();
                        MaterialSpool material = materialesSpools.Where(x => x.Spool.Nombre == nombreSpool && x.Etiqueta == etiquetaMaterial).SingleOrDefault();

                        if (material != null)
                        {
                            //actualizar
                            material.StartTracking();
                            material.Campo1 = !String.IsNullOrEmpty(campo1.Trim()) ? campo1 : string.Empty;
                            material.Campo2 = !String.IsNullOrEmpty(campo2.Trim()) ? campo2 : string.Empty;
                            material.Campo3 = !String.IsNullOrEmpty(campo3.Trim()) ? campo3 : string.Empty;
                            material.Campo4 = !String.IsNullOrEmpty(campo4.Trim()) ? campo4 : string.Empty;
                            material.Campo5 = !String.IsNullOrEmpty(campo5.Trim()) ? campo5 : string.Empty;
                            material.UsuarioModifica = usuarioModifica;
                            material.FechaModificacion = DateTime.Now;

                            ctx.MaterialSpool.ApplyChanges(material);
                        }
                        else
                        {
                            errores.Add(string.Format(MensajesError.MaterialSpoolX_Invalido, numLinea + 1, etiquetaMaterial, nombreSpool));
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


    } // Fin Clase
}
