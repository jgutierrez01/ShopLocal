using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Entities.Personalizadas
{
       [Serializable]
    public class DetSpoolHold: SAM.Entities.Grid.GrdIngSpool
    {
        public DetSpoolHold() { }

        public DetSpoolHold(Spool sp)
        {
            #region Propiedades del spool

            SpoolID = sp.SpoolID;
            Prioridad = sp.Prioridad ?? 99;
            Nombre = sp.Nombre;
            Dibujo = sp.Dibujo;
            Pdis = sp.Pdis ?? 0;
            Cedula = sp.Cedula;
            Peso = sp.Peso ?? 0;
            Area = sp.Area ?? 0;
            Especificacion = sp.Especificacion;
            TieneHoldCalidad = sp.SpoolHold != null ? sp.SpoolHold.TieneHoldCalidad : false;
            TieneHoldIngenieria = sp.SpoolHold != null ? sp.SpoolHold.TieneHoldIngenieria : false;
            Confinado = sp.SpoolHold != null ? sp.SpoolHold.Confinado : false;
            Segmento1 = sp.Segmento1;
            Segmento2 = sp.Segmento2;
            Segmento3 = sp.Segmento3;
            Segmento4 = sp.Segmento4;
            Segmento5 = sp.Segmento5;
            Segmento6 = sp.Segmento6;
            Segmento7 = sp.Segmento7;
            RevisionCliente = sp.RevisionCliente;
            RevisionSteelgo = sp.Revision;
            PendienteDocumental = sp.PendienteDocumental;
            PorcentajePnd = sp.PorcentajePnd ?? 0;
            PendienteDocumental = sp.PendienteDocumental;
            AprobadoParaCruce = sp.AprobadoParaCruce;
            RequierePwht = sp.RequierePwht;
            ProyectoID = sp.ProyectoID;
            DiametroPlano = sp.DiametroPlano ?? 0;
            DiametroMayor = sp.DiametroMayor ?? 0;

            #endregion
                       
            Holds = (from hold in sp.SpoolHoldHistorial select new DetHoldSpool(hold)).ToList();
        }

        
        [DataMember]
        public List<DetHoldSpool> Holds { get; set; }
    }
}