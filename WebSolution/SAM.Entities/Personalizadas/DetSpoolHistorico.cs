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
    public class DetSpoolHistorico : GrdIngSpool
    {
        public DetSpoolHistorico() { }

        public DetSpoolHistorico(SpoolHistorico sp, Dictionary<int, string> tiposJunta, Dictionary<int, string> tiposCorte, Dictionary<int, string> fabAreas, Dictionary<int, string> familiasAcero)
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
            FamiliaAcero2 = sp.FamiliaAcero2ID != null ? familiasAcero[sp.FamiliaAcero2ID.Value] : string.Empty;
            TieneHoldCalidad = sp.TieneHoldCalidad != null ? sp.TieneHoldCalidad.Value : false;
            TieneHoldIngenieria = sp.TieneHoldIngenieria != null ? sp.TieneHoldIngenieria.Value : false;
            Confinado = sp.Confinado != null ? sp.Confinado.Value : false;
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
            AprobadoParaCruce = sp.AprobadoParaCruce;
            RequierePwht = sp.RequierePwht;
            ProyectoID = sp.ProyectoID;
            DiametroPlano = sp.DiametroPlano ?? 0;

            #endregion

            Juntas = (from junta in sp.JuntaSpoolHistorico select new DetJuntaSpoolHistorico(junta, tiposJunta, fabAreas, familiasAcero)).ToList();
            Materiales = (from material in sp.MaterialSpoolHistorico select new DetMaterialSpoolHistorico(material)).ToList();
            Cortes = (from corte in sp.CorteSpoolHistorico select new DetCorteSpoolHistorico(corte, tiposCorte)).ToList();
        }

        [DataMember]
        public List<DetJuntaSpoolHistorico> Juntas { get; set; }
        [DataMember]
        public List<DetHoldSpool> Holds { get; set; }
        [DataMember]
        public List<DetMaterialSpoolHistorico> Materiales { get; set; }
        [DataMember]
        public List<DetCorteSpoolHistorico> Cortes { get; set; }
    }
}
