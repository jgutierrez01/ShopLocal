using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Entities.Personalizadas.Shop
{  
    public class SummarySpool
    {        
        public string ReporteLiberacionDimensional { get; set; }
        public DateTime? FechaLiberacionDimensional { get; set; } 
        public string ResultadoLiberacionDimensional { get; set; }
        public string SistemaPintura { get; set; }        
        public DateTime? FechaPrimario { get; set; }        
        public DateTime? FechaAcabado { get; set; }
        public string Campo54 { get; set; }
        public string Cuadrante { get; set; }        
        public List<DetailSummaryJoint> DetailJoints { get; set; }        
        public List<DetailMaterialSummary> DetailMaterials { get; set; }        
        public string Yard { get; set; }        
        public string NumberControl { get; set; }
        public string NumberControId { get; set; }
        public string Project { get; set; }
        public string Spool { get; set; }
        public string NumeroEmbarque { get; set; }
        public int PorcentajePND { get; set; }
        public string RequierePWHT { get; set; }
        public string Hold { get; set; }
        public decimal M2 { get; set; }
        public decimal Kg { get; set; }
        public string KGSGRUPO { get; set; }
        public string InspectorLiberacionDimensional { get; set; }
        public DateTime? FechaOkPnd { get; set; }
        public decimal DiametroMayor { get; set; }
        public string GrupoAcero { get; set; }
        public int Prioridad { get; set; }
        public SummarySpool() 
        {
            this.DetailJoints = new List<DetailSummaryJoint>(); 
            this.DetailMaterials = new List<DetailMaterialSummary>(); 
        }

        public SummarySpool(string ReporteLiberacionDimensional, DateTime? FechaLiberacionDimensional, string ResultadoDimensional, string SistemaPintura, DateTime? FechaPrimario, DateTime? FechaAcabado,
                    string Cuadrante, string NumeroEmbarque, int PorcentajePND, string RequierePWHT, List<DetailSummaryJoint> detJoints, List<DetailMaterialSummary> detMaterials, decimal M2, decimal Kg,string InspectorLiberacionDimensional, DateTime? FechaOkPnd,decimal DiametroMayor)
        {
            this.DetailJoints.AddRange(detJoints);
            this.DetailMaterials.AddRange(detMaterials);
            this.ReporteLiberacionDimensional = ReporteLiberacionDimensional;
            this.FechaLiberacionDimensional = FechaLiberacionDimensional;
            this.ResultadoLiberacionDimensional = ResultadoDimensional;
            this.SistemaPintura = SistemaPintura;
            this.FechaAcabado = FechaAcabado;
            this.FechaPrimario = FechaPrimario;
            this.Cuadrante = Cuadrante;
            this.RequierePWHT = RequierePWHT;
            this.NumeroEmbarque = NumeroEmbarque;
            this.PorcentajePND = PorcentajePND;
            this.Kg = Kg;
            this.M2 = M2;
            this.InspectorLiberacionDimensional = InspectorLiberacionDimensional;
            this.FechaOkPnd=FechaOkPnd;
            this.DiametroMayor = DiametroMayor;
        }
    }
}