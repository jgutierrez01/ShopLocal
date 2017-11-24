using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using System.Data.SqlClient;
using SAM.BusinessObjects.Validations;

namespace SAM.BusinessObjects.Catalogos
{
    public class FamiliaMaterialBO
    {
        public event TableChangedHandler FamiliaMaterialCambio;
        private static readonly object _mutex = new object();
        private static FamiliaMaterialBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private FamiliaMaterialBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase FamiliaMaterialBO
        /// </summary>
        /// <returns></returns>
        public static FamiliaMaterialBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new FamiliaMaterialBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene las familias de material con paginacion, filtrado y ordenado en el servidor.
        /// </summary>
        /// <param name="tamanoPagina">Cantidad de registros por pagina</param>
        /// <param name="numeroPagina">Numero de la pagina a regresar (indice 0)</param>
        /// <param name="filtros">Filtros a aplicar</param>
        /// <param name="orden">Ordenamiento necesario</param>
        /// <param name="totalFilas">Cantidad de registros totales tomando en cuenta los filtros</param>
        /// <returns>Listado de Familias de material con únicamente los registros necesarios</returns>
        public List<FamiliaMaterial> ObtenerPaginado(int tamanoPagina,
                                                        int numeroPagina,
                                                        List<ObjectSetFilter> filtros,
                                                        List<ObjectSetOrder> orden,
                                                        ref int totalFilas)
        {
            List<FamiliaMaterial> lst = null;

            using (SamContext ctx = new SamContext())
            {
                lst =
                ctx.FamiliaMaterial.Page<FamiliaMaterial>(tamanoPagina,
                                                            numeroPagina,
                                                            filtros,
                                                            orden,
                                                            ref totalFilas).ToList();
            }

            return lst;
        }

        /// <summary>
        /// Obtiene una familia de material en particular
        /// </summary>
        /// <param name="familiaMaterialID"></param>
        /// <returns></returns>
        public FamiliaMaterial Obtener(int familiaMaterialID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.FamiliaMaterial.Where(x => x.FamiliaMaterialID == familiaMaterialID).FirstOrDefault();
            }
        }

        /// <summary>
        /// Guarda un registro de familia de material
        /// </summary>
        /// <param name="familia"></param>
        public void Guarda(FamiliaMaterial familia)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesFamiliaMaterial.NombreDuplicado(ctx, familia.Nombre, familia.FamiliaMaterialID))
                    {
                        throw new ExcepcionDuplicados(new List<string>() { MensajesError.Excepcion_NombreDuplicado });
                    }

                    ctx.FamiliaMaterial.ApplyChanges(familia);

                    ctx.SaveChanges();
                }

                if (FamiliaMaterialCambio != null)
                { 
                    FamiliaMaterialCambio(); 
                }

            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }

        }

        public List<FamiliaMaterial> ObtenerTodas()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.FamiliaMaterial.ToList();
            }
        }

        public void Borra(int familiaMaterialID)
        {
            using (SamContext ctx = new SamContext())
            {
                bool tieneFamiliaAcero = Validations.ValidacionesFamiliaMaterial.TieneFamiliaAcero(ctx, familiaMaterialID);

                if (!tieneFamiliaAcero)
                {
                    FamiliaMaterial famMaterial = ctx.FamiliaMaterial.Where(x => x.FamiliaMaterialID == familiaMaterialID).SingleOrDefault();
                    ctx.DeleteObject(famMaterial);
                    ctx.SaveChanges();

                    if (FamiliaMaterialCambio != null)
                    {
                        FamiliaMaterialCambio();
                    }
                }
                else
                {
                    throw new ExcepcionRelaciones(new List<string>() { MensajesError.Excepcion_RelacionFamMaterial });
                }
            }
        }
    }
}
