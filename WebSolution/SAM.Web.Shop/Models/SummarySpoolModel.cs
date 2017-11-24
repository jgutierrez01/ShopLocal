﻿using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Web; 
   
namespace SAM.Web.Shop.Models 
{ 
    public class SummarySpoolModel 
    {
        public string ReporteLiberacionDimensional { get; set; }
        public DateTime? FechaLiberacionDimensional { get; set; }
        public string SistemaPintura { get; set; }
        public DateTime? FechaPrimario { get; set; }
        public DateTime? FechaAcabado { get; set; }
        public string Campo54 { get; set; }
        public string Cuadrante { get; set; }
        public string Spool{ get; set; }
        public int NumeroControlId { get; set; }
        public decimal M2 { get; set; }
        public decimal Kg { get; set; }
        public string KGSGRUPO { get; set; }
        public string InspectorLiberacionDimensional { get; set; }
        
        public List<DetailSummaryJointModel> DetailJoints { get; set; } 
        public List<DetailMaterialSummaryModel> DetailMaterials{ get; set; }
       
        public string Yard { get; set; }
        public string NumberControl {get; set;}
        public int NumberControlId { get; set; }
        public string Project { get; set; }
        public string NumeroEmbarque { get; set; }
        public int PorcentajePND { get; set; }
        public string RequierePWHT { get; set; }
        public string Hold { get; set; }
        public DateTime? FechaOkPnd { get; set; }
        public decimal DiametroMayor { get; set; }
        public string PrimarioFecha
        {
            get
            {
                return FechaPrimario.HasValue ? FechaPrimario.Value.ToShortDateString() : string.Empty;
            }
        }

        public string AcabadoFecha
        {
            get
            {
                return FechaAcabado.HasValue ? FechaAcabado.Value.ToShortDateString() : string.Empty;
            }
        }
        public string LibDimFecha
        {
            get
            {
                return FechaLiberacionDimensional.HasValue ? FechaLiberacionDimensional.Value.ToShortDateString() : string.Empty;
            }
        }
        public SummarySpoolModel() 
        { 
        }

        public string DateOkPnd
        {
            get { return FechaOkPnd.HasValue ? FechaOkPnd.Value.ToShortDateString() : string.Empty; }
        }
    } 
}
