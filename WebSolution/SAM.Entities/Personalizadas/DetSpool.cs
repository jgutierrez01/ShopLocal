using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SAM.Entities.Grid;
using SAM.Entities.Cache;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class DetSpool : GrdIngSpool
    {
        public DetSpool() { }

        public DetSpool(Spool sp, Dictionary<int, string> tiposJunta, Dictionary<int, string> tiposCorte, Dictionary<int, string> fabAreas, Dictionary<int, string> familiasAcero)
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
            FamiliaAcero1 = familiasAcero[sp.FamiliaAcero1ID];
            FamiliaAcero2 = sp.FamiliaAcero2 != null ? familiasAcero[sp.FamiliaAcero2ID.Value] : string.Empty;
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

            Juntas = (from junta in sp.JuntaSpool select new DetJuntaSpool(junta, tiposJunta, fabAreas, familiasAcero)).ToList();
            Materiales = (from material in sp.MaterialSpool select new DetMaterialSpool(material)).ToList();
            Cortes = (from corte in sp.CorteSpool select new DetCorteSpool(corte, tiposCorte)).ToList();
            Holds = (from hold in sp.SpoolHoldHistorial select new DetHoldSpool(hold)).ToList();
        }

        [DataMember]
        public List<DetJuntaSpool> Juntas { get; set; }
        [DataMember]
        public List<DetHoldSpool> Holds { get; set; }
        [DataMember]
        public List<DetMaterialSpool> Materiales { get; set; }
        [DataMember]
        public List<DetCorteSpool> Cortes { get; set; }
    }
}
