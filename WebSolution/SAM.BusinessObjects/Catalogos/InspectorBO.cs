using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using System.Data.Objects;
using SAM.BusinessObjects.Validations;
using SAM.Entities.RadCombo;
using Mimo.Framework.Extensions;


namespace SAM.BusinessObjects.Catalogos
{
    public class InspectorBO
    {
        public event TableChangedHandler InspectorCambio;
        private static readonly object _mutex = new object();
        private static InspectorBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private InspectorBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase InspectorBO
        /// </summary>
        /// <returns></returns>
        public static InspectorBO Instance
        {
            get 
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new InspectorBO();
                    }
                }
                return _instance;
            }
        }

        public Inspector Obtener(int inspectorID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Inspector.Where(x => x.InspectorID == inspectorID).SingleOrDefault();
            }
        }


        public List<Inspector> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Inspector.ToList();
            }
        }

        public List<Inspector> ObtenerTodosConPatio()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Inspector.Include("Patio").OrderBy(x => x.PatioID).ToList();
            }
        }

        public List<Inspector> ObtenerTodosConPatioYTaller()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Inspector.Include("Patio").Include("Taller").OrderBy(x => x.PatioID).ToList();
            }
        }

        public void Guarda(Inspector inspector)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                   // if (ValidacionesInspector.CodigoDuplicado(ctx, inspector.Codigo, inspector.SoldadorID, inspector.PatioID))
                   // {
                   //     throw new ExcepcionDuplicados(new List<string>() { MensajesError.Excepcion_CodigoDuplicado });
                   // }

                    ctx.Inspector.ApplyChanges(inspector);

                    ctx.SaveChanges();

                    //ActulizaFechaVigencia(inspector.InspectorID);
                }

                if (InspectorCambio != null)
                {
                    InspectorCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }

        public void Borra(int inspectorID)
        {
            using (SamContext ctx = new SamContext())
            {
                //if (ValidacionesTransportista.TieneDestajoSoldador(ctx, inspectorID))
                //{
                //    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionDestajoSoldador);
                //}

                //if (ValidacionesTransportista.TieneJuntaSoldaduraDetalle(ctx, inspectorID))
                //{
                //    throw new ExcepcionRelaciones(MensajesError.Excepcion_SoldaduraDetallle);
                //}

                //if (ValidacionesTransportista.TieneWpq(ctx, inspectorID))
                //{
                //    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionWpq);
                //}

                Inspector inspector = ctx.Inspector.Where(x => x.InspectorID == inspectorID).Single();

                ctx.DeleteObject(inspector);
                ctx.SaveChanges();

                if (InspectorCambio != null)
                {
                    InspectorCambio();
                }

            }
        }
    
        /// <summary>
        /// Obtiene una lista de soldadores relacionados con un proyecto en especifico
        /// a travez del patio en el que se encuentran dados de alta
        /// </summary>
        /// <param name="tallerID">ID del proyecto</param>
        /// <returns>Lista de Soldadores</returns>
        public List<RadInspector> ObtenerPorTallerParaCombo(int tallerID, string contextText, int skip, int take)
        {
            List<RadInspector> result = new List<RadInspector>(take * 2);
            contextText = contextText.ToLower();

            using (SamContext ctx = new SamContext())
            {
                result =
                        (from x in ctx.Inspector.Where(y => y.Taller.Inspector.Select(z => z.TallerID).Contains(tallerID))
                         select new RadInspector
                         {
                             InspectorID = x.InspectorID,
                             TallerID = x.TallerID,
                             Nombre = x.Nombre,
                             ApPaterno = x.ApPaterno,
                             ApMaterno = x.ApMaterno,
                             NumeroEmpleado = x.NumeroEmpleado,
                             NombreCompleto = x.Nombre + " " + x.ApPaterno + " " + x.ApMaterno
                         }).ToList();

                return result.Where(x => x.NombreCompleto.ContainsIgnoreCase(contextText) || x.NumeroEmpleado.ContainsIgnoreCase(contextText))
                             .OrderBy(x => x.TallerID)
                             .Skip(skip)
                             .Take(take)
                             .ToList();
            }
        }
    }
}
