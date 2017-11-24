using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Utilerias
{
    public static class EtiquetasMaterialUtil
    {

        // <summary>
        // Compara las etiquetas de un listado de materiales vs un listado de juntas        
        // </summary>
        // <param name="materiales">Listado de materiales</param>
        // <param name="juntas">Listado de juntas</param>
        // <returns>ID del primer material que se encontró dentro del listado de juntas, de lo contrario -1</returns>
        //public static int ComparaEtiquetas(List<MaterialSpool> materiales, List<JuntaSpool> juntas)
        //{
        //    bool contieneEtiqueta = false;

        //    int materialID = -1;

        //    Comparando etiquetaMaterial 1            
        //    foreach (MaterialSpool material in materiales)
        //    {
        //        if (juntas.Select(y => y.EtiquetaMaterial1).Contains(material.Etiqueta))
        //        {
        //            contieneEtiqueta = true;
        //            materialID = material.MaterialSpoolID;
        //            break;
        //        }
        //    }


        //    if (!contieneEtiqueta)
        //    {
        //        foreach (MaterialSpool material in materiales)
        //        {
        //            if (juntas.Select(y => y.EtiquetaMaterial2).Contains(material.Etiqueta))
        //            {
        //                contieneEtiqueta = true;
        //                materialID = material.MaterialSpoolID;
        //                break;
        //            }
        //        }
        //    }

        //    Comparando enteros  
        //    if (!contieneEtiqueta)
        //    {

        //        foreach (MaterialSpool material in materiales)
        //        {
        //            foreach (JuntaSpool junta in juntas)
        //            {
        //                try
        //                {
        //                    if (int.Parse(junta.EtiquetaMaterial1) == int.Parse(material.Etiqueta))
        //                    {
        //                        contieneEtiqueta = true;
        //                        materialID = material.MaterialSpoolID;
        //                        break;
        //                    }
        //                    else if ((int.Parse(junta.EtiquetaMaterial2) == int.Parse(material.Etiqueta)))
        //                    {
        //                        contieneEtiqueta = true;
        //                        materialID = material.MaterialSpoolID;
        //                        break;
        //                    }
        //                }
        //                catch
        //                {
        //                }
        //            }

        //            if (contieneEtiqueta)
        //            {
        //                break;
        //            }
        //        }
        //    }

        //    return materialID;
        //}

        // <summary>
        // Obtiene el ID del material que corresponde a la etiqueta material 1 de la junta  
        // </summary>
        // <param name="materiales">Listado de materiales</param>
        // <param name="juntas">Junta</param>
        // <returns>ID del material, de lo contrario -1</returns>
        //public static int ObtenMaterialSpoolDeEtiqueta1(List<MaterialSpool> materiales, JuntaSpool junta)
        //{
        //    bool contieneEtiqueta = false;

        //    int materialID = -1;

        //    Comparando etiquetaMaterial 1            
        //    foreach (MaterialSpool material in materiales)
        //    {
        //        if (junta.EtiquetaMaterial1 == material.Etiqueta)
        //        {
        //            contieneEtiqueta = true;
        //            materialID = material.MaterialSpoolID;
        //            break;
        //        }
        //    }

        //    Comparando enteros  
        //    if (!contieneEtiqueta)
        //    {
        //        foreach (MaterialSpool material in materiales)
        //        {
        //            try
        //            {
        //                if (int.Parse(junta.EtiquetaMaterial1) == int.Parse(material.Etiqueta))
        //                {
        //                    contieneEtiqueta = true;
        //                    materialID = material.MaterialSpoolID;
        //                }
        //            }
        //            catch
        //            {
        //            }
        //        }
        //    }

        //    return materialID;
        //}

        // <summary>
        // Obtiene el ID del material que corresponde a la etiqueta material 2 de la junta  
        // </summary>
        // <param name="materiales">Listado de materiales</param>
        // <param name="juntas">Junta</param>
        // <returns>ID del material, de lo contrario -1</returns>
        //public static int ObtenMaterialSpoolDeEtiqueta2(List<MaterialSpool> materiales, JuntaSpool junta)
        //{
        //    bool contieneEtiqueta = false;

        //    int materialID = -1;

        //    Comparando etiquetaMaterial 1            
        //    foreach (MaterialSpool material in materiales)
        //    {
        //        if (junta.EtiquetaMaterial2 == material.Etiqueta)
        //        {
        //            contieneEtiqueta = true;
        //            materialID = material.MaterialSpoolID;
        //            break;
        //        }
        //    }

        //    Comparando enteros  
        //    if (!contieneEtiqueta)
        //    {
        //        foreach (MaterialSpool material in materiales)
        //        {
        //            try
        //            {
        //                if (int.Parse(junta.EtiquetaMaterial2) == int.Parse(material.Etiqueta))
        //                {
        //                    contieneEtiqueta = true;
        //                    materialID = material.MaterialSpoolID;
        //                }
        //            }
        //            catch
        //            {
        //            }
        //        }
        //    }

        //    return materialID;
        //}


        /// <summary>
        /// Compara la etiqueta de un material vs un listado de juntas
        /// </summary>
        /// <param name="material">Material a comparar</param>
        /// <param name="juntas">Listado de Juntas</param>
        /// <returns>True - si el material se encontro en el listado / False - si no se encontro el material</returns>
        public static bool ComparaMaterialConJuntas(MaterialSpool material, List<JuntaSpool> juntas)
        {
            bool contieneEtiqueta = false;

            //Comparando etiquetaMaterial 1            
            if (juntas.Select(y => y.EtiquetaMaterial1).Contains(material.Etiqueta))
            {
                contieneEtiqueta = true;
            }

            if (!contieneEtiqueta)
            {
                if (juntas.Select(y => y.EtiquetaMaterial2).Contains(material.Etiqueta))
                {
                    contieneEtiqueta = true;
                }
            }

            //Comparando enteros  
            if (!contieneEtiqueta)
            {
                foreach (JuntaSpool junta in juntas)
                {
                    try
                    {
                        if (int.Parse(junta.EtiquetaMaterial1) == int.Parse(material.Etiqueta))
                        {
                            contieneEtiqueta = true;
                            break;
                        }
                        else if ((int.Parse(junta.EtiquetaMaterial2) == int.Parse(material.Etiqueta)))
                        {
                            contieneEtiqueta = true;
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
            }


            return contieneEtiqueta;
        }

        public static int ObtenMaterialSpoolDeEtiqueta(IEnumerable<Simple> materiales, string etiqueta)
        {
            bool contieneEtiqueta = false;

            int materialID = -1;

            //Comparando etiquetaMaterial 1            
            foreach (Simple material in materiales)
            {
                if (etiqueta == material.Valor)
                {
                    contieneEtiqueta = true;
                    materialID = material.ID;
                    break;
                }
            }

            //Comparando enteros  
            if (!contieneEtiqueta)
            {
                foreach (Simple material in materiales)
                {
                    try
                    {
                        if (int.Parse(etiqueta) == int.Parse(material.Valor))
                        {
                            contieneEtiqueta = true;
                            materialID = material.ID;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return materialID;
        }

       

    }
}
