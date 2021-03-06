//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace SAM.Entities
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(ReporteSpoolPnd))]
    [KnownType(typeof(SpoolRequisicion))]
    [KnownType(typeof(WorkstatusSpool))]
    [Serializable]
    public partial class SpoolReportePnd: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int SpoolReportePndID
        {
            get { return _spoolReportePndID; }
            set
            {
                if (_spoolReportePndID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'SpoolReportePndID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _spoolReportePndID = value;
                    OnPropertyChanged("SpoolReportePndID");
                }
            }
        }
        private int _spoolReportePndID;
    
        [DataMember]
        public int ReporteSpoolPndID
        {
            get { return _reporteSpoolPndID; }
            set
            {
                if (_reporteSpoolPndID != value)
                {
                    ChangeTracker.RecordOriginalValue("ReporteSpoolPndID", _reporteSpoolPndID);
                    if (!IsDeserializing)
                    {
                        if (ReporteSpoolPnd != null && ReporteSpoolPnd.ReporteSpoolPndID != value)
                        {
                            ReporteSpoolPnd = null;
                        }
                    }
                    _reporteSpoolPndID = value;
                    OnPropertyChanged("ReporteSpoolPndID");
                }
            }
        }
        private int _reporteSpoolPndID;
    
        [DataMember]
        public int WorkstatusSpoolID
        {
            get { return _workstatusSpoolID; }
            set
            {
                if (_workstatusSpoolID != value)
                {
                    ChangeTracker.RecordOriginalValue("WorkstatusSpoolID", _workstatusSpoolID);
                    if (!IsDeserializing)
                    {
                        if (WorkstatusSpool != null && WorkstatusSpool.WorkstatusSpoolID != value)
                        {
                            WorkstatusSpool = null;
                        }
                    }
                    _workstatusSpoolID = value;
                    OnPropertyChanged("WorkstatusSpoolID");
                }
            }
        }
        private int _workstatusSpoolID;
    
        [DataMember]
        public int SpoolRequisicionID
        {
            get { return _spoolRequisicionID; }
            set
            {
                if (_spoolRequisicionID != value)
                {
                    ChangeTracker.RecordOriginalValue("SpoolRequisicionID", _spoolRequisicionID);
                    if (!IsDeserializing)
                    {
                        if (SpoolRequisicion != null && SpoolRequisicion.SpoolRequisicionID != value)
                        {
                            SpoolRequisicion = null;
                        }
                    }
                    _spoolRequisicionID = value;
                    OnPropertyChanged("SpoolRequisicionID");
                }
            }
        }
        private int _spoolRequisicionID;
    
        [DataMember]
        public System.DateTime FechaPrueba
        {
            get { return _fechaPrueba; }
            set
            {
                if (_fechaPrueba != value)
                {
                    _fechaPrueba = value;
                    OnPropertyChanged("FechaPrueba");
                }
            }
        }
        private System.DateTime _fechaPrueba;
    
        [DataMember]
        public bool Aprobado
        {
            get { return _aprobado; }
            set
            {
                if (_aprobado != value)
                {
                    _aprobado = value;
                    OnPropertyChanged("Aprobado");
                }
            }
        }
        private bool _aprobado;
    
        [DataMember]
        public string Observaciones
        {
            get { return _observaciones; }
            set
            {
                if (_observaciones != value)
                {
                    _observaciones = value;
                    OnPropertyChanged("Observaciones");
                }
            }
        }
        private string _observaciones;
    
        [DataMember]
        public Nullable<System.Guid> UsuarioModifica
        {
            get { return _usuarioModifica; }
            set
            {
                if (_usuarioModifica != value)
                {
                    _usuarioModifica = value;
                    OnPropertyChanged("UsuarioModifica");
                }
            }
        }
        private Nullable<System.Guid> _usuarioModifica;
    
        [DataMember]
        public Nullable<System.DateTime> FechaModificacion
        {
            get { return _fechaModificacion; }
            set
            {
                if (_fechaModificacion != value)
                {
                    _fechaModificacion = value;
                    OnPropertyChanged("FechaModificacion");
                }
            }
        }
        private Nullable<System.DateTime> _fechaModificacion;
    
        [DataMember]
        public byte[] VersionRegistro
        {
            get { return _versionRegistro; }
            set
            {
                if (_versionRegistro != value)
                {
                    _versionRegistro = value;
                    OnPropertyChanged("VersionRegistro");
                }
            }
        }
        private byte[] _versionRegistro;
    
        [DataMember]
        public Nullable<int> Hoja
        {
            get { return _hoja; }
            set
            {
                if (_hoja != value)
                {
                    _hoja = value;
                    OnPropertyChanged("Hoja");
                }
            }
        }
        private Nullable<int> _hoja;

        #endregion

        #region Navigation Properties
    
        [DataMember]
        public ReporteSpoolPnd ReporteSpoolPnd
        {
            get { return _reporteSpoolPnd; }
            set
            {
                if (!ReferenceEquals(_reporteSpoolPnd, value))
                {
                    var previousValue = _reporteSpoolPnd;
                    _reporteSpoolPnd = value;
                    FixupReporteSpoolPnd(previousValue);
                    OnNavigationPropertyChanged("ReporteSpoolPnd");
                }
            }
        }
        private ReporteSpoolPnd _reporteSpoolPnd;
    
        [DataMember]
        public SpoolRequisicion SpoolRequisicion
        {
            get { return _spoolRequisicion; }
            set
            {
                if (!ReferenceEquals(_spoolRequisicion, value))
                {
                    var previousValue = _spoolRequisicion;
                    _spoolRequisicion = value;
                    FixupSpoolRequisicion(previousValue);
                    OnNavigationPropertyChanged("SpoolRequisicion");
                }
            }
        }
        private SpoolRequisicion _spoolRequisicion;
    
        [DataMember]
        public WorkstatusSpool WorkstatusSpool
        {
            get { return _workstatusSpool; }
            set
            {
                if (!ReferenceEquals(_workstatusSpool, value))
                {
                    var previousValue = _workstatusSpool;
                    _workstatusSpool = value;
                    FixupWorkstatusSpool(previousValue);
                    OnNavigationPropertyChanged("WorkstatusSpool");
                }
            }
        }
        private WorkstatusSpool _workstatusSpool;

        #endregion

        #region ChangeTracking
    
        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (ChangeTracker.State != ObjectState.Added && ChangeTracker.State != ObjectState.Deleted)
            {
                ChangeTracker.State = ObjectState.Modified;
            }
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        protected virtual void OnNavigationPropertyChanged(String propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged{ add { _propertyChanged += value; } remove { _propertyChanged -= value; } }
        private event PropertyChangedEventHandler _propertyChanged;
        private ObjectChangeTracker _changeTracker;
    
        [DataMember]
        public ObjectChangeTracker ChangeTracker
        {
            get
            {
                if (_changeTracker == null)
                {
                    _changeTracker = new ObjectChangeTracker();
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
                return _changeTracker;
            }
            set
            {
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging -= HandleObjectStateChanging;
                }
                _changeTracker = value;
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
            }
        }
    
        private void HandleObjectStateChanging(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                ClearNavigationProperties();
            }
        }
    
        protected bool IsDeserializing { get; private set; }
    
        [OnDeserializing]
        public void OnDeserializingMethod(StreamingContext context)
        {
            IsDeserializing = true;
        }
    
        [OnDeserialized]
    	public void OnDeserializedMethod(StreamingContext context)
    	{
    		IsDeserializing = false;
    		ChangeTracker.ChangeTrackingEnabled = true;
    		}
    
    
        protected virtual void ClearNavigationProperties()
        {
            ReporteSpoolPnd = null;
            SpoolRequisicion = null;
            WorkstatusSpool = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupReporteSpoolPnd(ReporteSpoolPnd previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.SpoolReportePnd.Contains(this))
            {
                previousValue.SpoolReportePnd.Remove(this);
            }
    
            if (ReporteSpoolPnd != null)
            {
                if (!ReporteSpoolPnd.SpoolReportePnd.Contains(this))
                {
                    ReporteSpoolPnd.SpoolReportePnd.Add(this);
                }
    
                ReporteSpoolPndID = ReporteSpoolPnd.ReporteSpoolPndID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("ReporteSpoolPnd")
                    && (ChangeTracker.OriginalValues["ReporteSpoolPnd"] == ReporteSpoolPnd))
                {
                    ChangeTracker.OriginalValues.Remove("ReporteSpoolPnd");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("ReporteSpoolPnd", previousValue);
                }
                if (ReporteSpoolPnd != null && !ReporteSpoolPnd.ChangeTracker.ChangeTrackingEnabled)
                {
                    ReporteSpoolPnd.StartTracking();
                }
            }
        }
    
        private void FixupSpoolRequisicion(SpoolRequisicion previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.SpoolReportePnd.Contains(this))
            {
                previousValue.SpoolReportePnd.Remove(this);
            }
    
            if (SpoolRequisicion != null)
            {
                if (!SpoolRequisicion.SpoolReportePnd.Contains(this))
                {
                    SpoolRequisicion.SpoolReportePnd.Add(this);
                }
    
                SpoolRequisicionID = SpoolRequisicion.SpoolRequisicionID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("SpoolRequisicion")
                    && (ChangeTracker.OriginalValues["SpoolRequisicion"] == SpoolRequisicion))
                {
                    ChangeTracker.OriginalValues.Remove("SpoolRequisicion");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("SpoolRequisicion", previousValue);
                }
                if (SpoolRequisicion != null && !SpoolRequisicion.ChangeTracker.ChangeTrackingEnabled)
                {
                    SpoolRequisicion.StartTracking();
                }
            }
        }
    
        private void FixupWorkstatusSpool(WorkstatusSpool previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.SpoolReportePnd.Contains(this))
            {
                previousValue.SpoolReportePnd.Remove(this);
            }
    
            if (WorkstatusSpool != null)
            {
                if (!WorkstatusSpool.SpoolReportePnd.Contains(this))
                {
                    WorkstatusSpool.SpoolReportePnd.Add(this);
                }
    
                WorkstatusSpoolID = WorkstatusSpool.WorkstatusSpoolID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("WorkstatusSpool")
                    && (ChangeTracker.OriginalValues["WorkstatusSpool"] == WorkstatusSpool))
                {
                    ChangeTracker.OriginalValues.Remove("WorkstatusSpool");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("WorkstatusSpool", previousValue);
                }
                if (WorkstatusSpool != null && !WorkstatusSpool.ChangeTracker.ChangeTrackingEnabled)
                {
                    WorkstatusSpool.StartTracking();
                }
            }
        }

        #endregion

    }
}
