using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessLogic.Excepciones;

namespace SAM.BusinessLogic.Proyectos
{
    public class ItemCodeEquivalenteBL
    {
        private static readonly object _mutex = new object();
        private static ItemCodeEquivalenteBL _instance;

        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private ItemCodeEquivalenteBL()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase ItemCodeEquivalenteBL
        /// </summary>
        public static ItemCodeEquivalenteBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ItemCodeEquivalenteBL();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Equivalentes"></param>
        /// <param name="userID"></param>
        public void GuardaEquivalencias(int ? itemCodeEquivalenteID, List<GrdItemCodeEquivalente> Equivalentes, Guid userID, bool unidireccional)
        {
            #region Validar unicidad

            int equivalentesUnicos =
                Equivalentes.Select
                (
                    x => new
                    {
                        ItemCodeID = x.ItemCodeID,
                        D1 = x.D1,
                        D2 = x.D2,
                        ItemEquivalenteID = x.ItemEquivalenteID,
                        D1Eq = x.D1Eq,
                        D2Eq = x.D2Eq
                    }
                ).Distinct().Count();

            if (Equivalentes.Count != equivalentesUnicos)
            {
                throw new ExcepcionItemCodeEquivalente(MensajesError.ItemCodeEquivalenteDuplicado);
            }

            #endregion
            
            try
            {
                //Este método hace varias consultas "uno a uno", no debe haber mayor problema de performance ya que
                //en general las equivalencias son muy chicas
                using (SamContext ctx = new SamContext())
                {
                    GrdItemCodeEquivalente elemento = Equivalentes[0];

                    List<ItemCodeEquivalente> lstBD =
                        ctx.ItemCodeEquivalente.Where(x => x.ItemCodeID == elemento.ItemCodeID
                                                           && x.Diametro1 == elemento.D1
                                                           && x.Diametro2 == elemento.D2)
                                               .ToList();

                    if (!itemCodeEquivalenteID.HasValue && lstBD.Count > 0)
                    {
                        //Están tratando de sobre-escribir una equivalencia que ya existe, muy
                        //seguramente sin darse cuenta
                        throw new ExcepcionItemCodeEquivalente(string.Format(MensajesError.ItemCodeEquivalenteYaExiste, elemento.CodigoIC, elemento.D1, elemento.D2));
                    }

                    ItemCodeEquivalente bd;

                    foreach (GrdItemCodeEquivalente mem in Equivalentes)
                    {
                        bd = lstBD.Where(x => x.ItemEquivalenteID == mem.ItemEquivalenteID
                                              && x.DiametroEquivalente1 == mem.D1Eq
                                              && x.DiametroEquivalente2 == mem.D2Eq)
                                  .SingleOrDefault();

                        if (bd == null)
                        {
                            //es uno nuevo
                            agregaEquivalente(ctx, mem, userID, unidireccional);
                        }
                    }

                    GrdItemCodeEquivalente memoria;

                    for (int i = lstBD.Count - 1; i >= 0; i--)
                    {
                        bd = lstBD[i];
                        memoria = Equivalentes.Where(x => x.ItemEquivalenteID == bd.ItemEquivalenteID
                                                           && x.D1Eq == bd.DiametroEquivalente1
                                                           && x.D2Eq == bd.DiametroEquivalente2)
                                              .SingleOrDefault();

                        if (memoria == null)
                        {
                            //Lo quieren eliminar de la BD
                            elminaEquivalente(ctx, bd, userID);
                        }
                    }

                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_Concurrencia);
            }
        }

        private void elminaEquivalente(SamContext ctx, ItemCodeEquivalente bd, Guid userID)
        {           

            ctx.DeleteObject(bd);
        }

        private void agregaEquivalente(SamContext ctx, GrdItemCodeEquivalente mem, Guid userID, bool unidireccional)
        {
            ItemCodeEquivalente icEq = new ItemCodeEquivalente
            {
                ItemCodeID = mem.ItemCodeID,
                Diametro1 = mem.D1,
                Diametro2 = mem.D2,
                ItemEquivalenteID = mem.ItemEquivalenteID,
                DiametroEquivalente1 = mem.D1Eq,
                DiametroEquivalente2 = mem.D2Eq,
                UsuarioModifica = userID,
                FechaModificacion = DateTime.Now
            };

            if (!unidireccional)
            {
                //Ahora vamos a revisar si la pareja ya existe o si la necesitamos crear
                bool parejaExiste =
                    ctx.ItemCodeEquivalente.Any
                    (
                        x => x.ItemCodeID == mem.ItemEquivalenteID
                             && x.Diametro1 == mem.D1Eq
                             && x.Diametro2 == mem.D2Eq
                             && x.ItemEquivalenteID == mem.ItemCodeID
                             && x.DiametroEquivalente1 == mem.D1
                             && x.DiametroEquivalente2 == mem.D2
                    );

                if (!parejaExiste)
                {
                    ItemCodeEquivalente icEq2 = new ItemCodeEquivalente
                    {
                        ItemCodeID = mem.ItemEquivalenteID,
                        Diametro1 = mem.D1Eq,
                        Diametro2 = mem.D2Eq,
                        ItemEquivalenteID = mem.ItemCodeID,
                        DiametroEquivalente1 = mem.D1,
                        DiametroEquivalente2 = mem.D2,
                        UsuarioModifica = userID,
                        FechaModificacion = DateTime.Now
                    };

                    ctx.ItemCodeEquivalente.ApplyChanges(icEq2);
                }
            }

            ctx.ItemCodeEquivalente.ApplyChanges(icEq);

        }


        public void BorraGrupoDeEquivalencias(int itemCodeEquivalenteID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                ItemCodeEquivalente icEq = ctx.ItemCodeEquivalente
                                              .Where(x => x.ItemCodeEquivalenteID == itemCodeEquivalenteID)
                                              .Single();

                List<ItemCodeEquivalente> lst = ctx.ItemCodeEquivalente
                                                   .Where(x => x.ItemCodeID == icEq.ItemCodeID
                                                               && x.Diametro1 == icEq.Diametro1
                                                               && x.Diametro2 == icEq.Diametro2)
                                                   .ToList();

                lst.ForEach(x => elminaEquivalente(ctx, x, userID));

                ctx.SaveChanges();
            }
        }
    }
}
