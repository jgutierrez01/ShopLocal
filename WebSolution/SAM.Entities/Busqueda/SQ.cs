﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Entities.Busqueda
{
    [Serializable]
    public class CuadranteNumeroControlSQ
    {
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public int CuadranteID { get; set; }
        [DataMember]
        public string Cuadrante { get; set; }
        [DataMember]
        public int Accion { get; set; }
        [DataMember]
        public int OrdenTrabajoSpoolID { get; set; }
        [DataMember]
        public string SqCliente { get; set; }
        [DataMember]
        public string SQ { get; set; }
        [DataMember]
        public bool TieneHoldIngenieria { get; set; }
        public bool OkPnd { get; set; }
        public int Incidencias { get; set; }
        public bool Granel { get; set; }
    }

    [Serializable]
    public class CuadranteSQ
    {
        [DataMember]
        public int CuadranteID { get; set; }
        [DataMember]
        public string Cuadrante { get; set; }
    }

    [Serializable]
    public class OrdenTrabajoSpoolSQ
    {
        [DataMember]
        public int OrdenTrabajoSpoolID { get; set; }
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string SqCliente { get; set; }
        [DataMember]
        public string sqinterno { get; set; }
        [DataMember]
        public bool TieneHoldIngenieria { get; set; }
        [DataMember]
        public bool OkPnd { get; set; }        
        [DataMember]
        public int Incidencias { get; set; }
        [DataMember]
        public bool Granel { get; set; }
    }
    [Serializable]
    public class AutorizarSI
    {
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public int ProyectoID { get; set; }
        [DataMember]
        public int OrdenTrabajoSpoolID { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public int CuadranteID { get; set; }
        [DataMember]
        public string Cuadrante { get; set; }
        [DataMember]
        public string SI { get; set; }
        [DataMember]
        public string SqCliente { get; set; }
        [DataMember]
        public bool Hold { get; set; }
        [DataMember]
        public bool OkPnd { get; set; }
        [DataMember]
        public bool Autorizado { get; set; }
        [DataMember]
        public bool NoAutorizado { get; set; }
        [DataMember]
        public int Accion { get; set; }          
        [DataMember]
        public int Incidencias { get; set; }      
        [DataMember]
        public bool Granel { get; set; }        
    }   
    public class TipoIncidencia
    {
        public int TipoIncidenciaID { get; set; }
        public string Incidencia { get; set; }        
        public TipoIncidencia()
        {
            TipoIncidenciaID = 0;
            Incidencia = "";            
        }
    }
    public class IncidenciaDetalle
    {
        public int ID { get; set; }
        public string Etiqueta { get; set; }
        public IncidenciaDetalle()
        {
            ID = 0;
            Etiqueta = "";
        }
    }
    public class ListaErrores
    {
        public int ErrorID { get; set; }
        public string Error { get; set; }
        public ListaErrores()
        {
            ErrorID = 0;
            Error = "";
        }
    }
    public class IncidenciaC
    {
        public int Accion { get; set; }
        public int IncidenciaID { get; set; }
        public int SpoolID { get; set; }
        public string NumeroControl { get; set; }
        public string Incidencia { get; set; }
        public string MaterialJunta { get; set; }        
        public int ErrorID { get; set; }
        public string Error { get; set; }
        public string Observaciones { get; set; }
        public string SI { get; set; }
        public string Usuario { get; set; }            
        public string FechaIncidencia { get; set; }
    }
    public class DetalleGuardarAutorizacion
    {
        public int SpoolID { get; set; }
        public bool Autorizacion { get; set; }               
    }
    public class GuardarAutorizacion
    {
        public List<DetalleGuardarAutorizacion> Detalle { get; set; }
    }
    public class ListaIncidencia
    {        
        public int SpoolID { get; set; }
        public int ProyectoID { get; set; }
        public int CuadranteID { get; set; }
        public string Cuadrante { get; set; }
        public string NumeroControl { get; set; }                        
        public bool Hold { get; set; }                        
        public int Incidencias { get; set; }
        public bool Granel { get; set; }
    }
    public class AgregarSI
    {
        public int SpoolID { get; set; }
        public int ProyectoID { get; set; }
        public int CuadranteID { get; set; }
        public string Cuadrante { get; set; }
        public string NumeroControl { get; set; }
        public bool Hold { get; set; }
        public string SqCliente { get; set; }
        public string SI { get; set; }
        public bool Granel { get; set; }
    }
    public class Datos
    {
        public List<DetalleGenerarSI> Detalle { get; set; }
    }
    public class DetalleGenerarSI
    {
        public int SpoolID { get; set; }
        public int ProyectoID { get; set; }
        public int CuadranteID { get; set; }
    }

    public class ObjectoSpool {
        public int SpoolID { get; set; }
        public string NumeroControl { get; set; }
    }
}
