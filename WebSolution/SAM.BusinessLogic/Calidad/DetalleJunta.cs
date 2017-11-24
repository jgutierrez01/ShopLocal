using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities.Grid;
using SAM.Entities;

namespace SAM.BusinessLogic.Calidad
{
    public abstract class DetalleJunta
    {
        public int ? JuntaID { get; set; }
        public int EsJuntaCampo { get; set; }
        public int JuntaSpoolID {get;set;}
        public int ProyectoID {get;set;}

        public static DetalleJunta ObtenerInstaciaConcreta(int proyectoID, int juntaSpoolID, int ? juntaID, bool esJuntaCampo)
        {
            DetalleJunta junta = null;

            if (esJuntaCampo)
            {
                junta = new DetalleJuntaCampo(proyectoID, juntaSpoolID, juntaID);
            }
            else
            {
                junta = new DetalleJuntaWorkstatus(proyectoID, juntaSpoolID, juntaID);
            }

            return junta;
        }

        public abstract void ComplementaInformacion(SamContext ctx, GrdSeguimientoJunta junta);
    }
}
