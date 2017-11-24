using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessLogic.Produccion
{
    public class OrdenTrabajoEspecialSpoolBL
    {
        //private static readonly object _mutex = new object();
        //private static OrdenTrabajoEspecialSpoolBL _instance;

        ///// <summary>
        ///// obtiene la instancia de la clase OrdenTrabajoBL
        ///// </summary>
        //public static OrdenTrabajoEspecialSpoolBL Instance
        //{
        //    get
        //    {
        //        lock (_mutex)
        //        {
        //            if (_instance == null)
        //            {
        //                _instance = new OrdenTrabajoEspecialSpoolBL();
        //            }
        //        }
        //        return _instance;
        //    }
        //}

        //public void GenerarOrdenTrabajoEspecialSpool(int ordenTrabajoEspecialID, Guid userID, List<Spool> spools, string numeroOrden)
        //{
        //    using (SamContext ctx = new SamContext())
        //    {
        //        int consecutivoOrden = 0;
        //        int partida = 0;
        //        string formato = "000";

        //        OrdenTrabajoEspecialSpool odtesSpool;
        //        foreach (Spool spool in spools)
        //        {
        //            consecutivoOrden++;
        //            partida++;
        //            odtesSpool = new OrdenTrabajoEspecialSpool();

        //            Spool actualizarSpool = (from spoolBD in ctx.Spool
        //                                     where spoolBD.SpoolID == spool.SpoolID
        //                                     select spoolBD).Single();
                    
        //            actualizarSpool.StartTracking();
        //            actualizarSpool.UltimaOrdenTrabajoEspecial = numeroOrden + "-" + consecutivoOrden.ToString(formato);
        //            actualizarSpool.FechaModificacion = DateTime.Now;
        //            actualizarSpool.UsuarioModifica = userID;
        //            actualizarSpool.StopTracking();
        //            ctx.Spool.ApplyChanges(actualizarSpool);
        //            //ctx.SaveChanges();

                    
        //            //Creamos la orden de trabajo
        //            odtesSpool.NumeroControl = numeroOrden + "-" + consecutivoOrden.ToString(formato);
        //            odtesSpool.Partida = partida;
        //            odtesSpool.OrdenTrabajoEspecialID = ordenTrabajoEspecialID;
        //            odtesSpool.UsuarioModifica = userID;
        //            odtesSpool.SpoolID = spool.SpoolID;
        //            odtesSpool.FechaModificacion = DateTime.Now;
        //            odtesSpool.EsAsignado = false;
        //            ctx.OrdenTrabajoEspecialSpool.ApplyChanges(odtesSpool);
        //            //ctx.SaveChanges();
        //        }

        //        ctx.SaveChanges();
        //    }
        //}

    }//Fin Clase
}
