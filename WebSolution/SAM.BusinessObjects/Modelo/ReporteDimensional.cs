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
    [KnownType(typeof(Proyecto))]
    [KnownType(typeof(TipoReporteDimensional))]
    [KnownType(typeof(ReporteDimensionalDetalle))]
    [Serializable]
    public partial class ReporteDimensional: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int ReporteDimensionalID
        {
            get { return _reporteDimensionalID; }
            set
            {
                if (_reporteDimensionalID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'ReporteDimensionalID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _reporteDimensionalID = value;
                    OnPropertyChanged("ReporteDimensionalID");
                }
            }
        }
        private int _reporteDimensionalID;
    
        [DataMember]
        public int TipoReporteDimensionalID
        {
            get { return _tipoReporteDimensionalID; }
            set
            {
                if (_tipoReporteDimensionalID != value)
                {
                    ChangeTracker.RecordOriginalValue("TipoReporteDimensionalID", _tipoReporteDimensionalID);
                    if (!IsDeserializing)
                    {
                        if (TipoReporteDimensional != null && TipoReporteDimensional.TipoReporteDimensionalID != value)
                        {
                            TipoReporteDimensional = null;
                        }
                    }
                    _tipoReporteDimensionalID = value;
                    OnPropertyChanged("TipoReporteDimensionalID");
                }
            }
        }
        private int _tipoReporteDimensionalID;
    
        [DataMember]
        public int ProyectoID
        {
            get { return _proyectoID; }
            set
            {
                if (_proyectoID != value)
                {
                    ChangeTracker.RecordOriginalValue("ProyectoID", _proyectoID);
                    if (!IsDeserializing)
                    {
                        if (Proyecto != null && Proyecto.ProyectoID != value)
                        {
                            Proyecto = null;
                        }
                    }
                    _proyectoID = value;
                    OnPropertyChanged("ProyectoID");
                }
            }
        }
        private int _proyectoID;
    
        [DataMember]
        public System.DateTime FechaReporte
        {
            get { return _fechaReporte; }
            set
            {
                if (_fechaReporte != value)
                {
                    _fechaReporte = value;
                    OnPropertyChanged("FechaReporte");
                }
            }
        }
        private System.DateTime _fechaReporte;
    
        [DataMember]
        public string NumeroReporte
        {
            get { return _numeroReporte; }
            set
            {
                if (_numeroReporte != value)
                {
                    _numeroReporte = value;
                    OnPropertyChanged("NumeroReporte");
                }
            }
        }
        private string _numeroReporte;
    
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
                    ChangeTracker.RecordOriginalValue("VersionRegistro", _versionRegistro);
                    _versionRegistro = value;
                    OnPropertyChanged("VersionRegistro");
                }
            }
        }
        private byte[] _versionRegistro;

        #endregion

        #region Navigation Properties
    
        [DataMember]
        public Proyecto Proyecto
        {
            get { return _proyecto; }
            set
            {
                if (!ReferenceEquals(_proyecto, value))
                {
                    var previousValue = _proyecto;
                    _proyecto = value;
                    FixupProyecto(previousValue);
                    OnNavigationPropertyChanged("Proyecto");
                }
            }
        }
        private Proyecto _proyecto;
    
        [DataMember]
        public TipoReporteDimensional TipoReporteDimensional
        {
            get { return _tipoReporteDimensional; }
            set
            {
                if (!ReferenceEquals(_tipoReporteDimensional, value))
                {
                    var previousValue = _tipoReporteDimensional;
                    _tipoReporteDimensional = value;
                    FixupTipoReporteDimensional(previousValue);
                    OnNavigationPropertyChanged("TipoReporteDimensional");
                }
            }
        }
        private TipoReporteDimensional _tipoReporteDimensional;
    
        [DataMember]
        public TrackableCollection<ReporteDimensionalDetalle> ReporteDimensionalDetalle
        {
            get
            {
                if (_reporteDimensionalDetalle == null)
                {
                    _reporteDimensionalDetalle = new TrackableCollection<ReporteDimensionalDetalle>();
                    _reporteDimensionalDetalle.CollectionChanged += FixupReporteDimensionalDetalle;
                }
                return _reporteDimensionalDetalle;
            }
            set
            {
                if (!ReferenceEquals(_reporteDimensionalDetalle, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_reporteDimensionalDetalle != null)
                    {
                        _reporteDimensionalDetalle.CollectionChanged -= FixupReporteDimensionalDetalle;
                    }
                    _reporteDimensionalDetalle = value;
                    if (_reporteDimensionalDetalle != null)
                    {
                        _reporteDimensionalDetalle.CollectionChanged += FixupReporteDimensionalDetalle;
                    }
                    OnNavigationPropertyChanged("ReporteDimensionalDetalle");
                }
            }
        }
        private TrackableCollection<ReporteDimensionalDetalle> _reporteDimensionalDetalle;

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
    			if (_reporteDimensionalDetalle != null)
    		{
    			_reporteDimensionalDetalle.CollectionChanged -= FixupReporteDimensionalDetalle;
    			_reporteDimensionalDetalle.CollectionChanged += FixupReporteDimensionalDetalle;
    			}
    		}
    
    
        protected virtual void ClearNavigationProperties()
        {
            Proyecto = null;
            TipoReporteDimensional = null;
            ReporteDimensionalDetalle.Clear();
        }

        #endregion

        #region Association Fixup
    
        private void FixupProyecto(Proyecto previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.ReporteDimensional.Contains(this))
            {
                previousValue.ReporteDimensional.Remove(this);
            }
    
            if (Proyecto != null)
            {
                if (!Proyecto.ReporteDimensional.Contains(this))
                {
                    Proyecto.ReporteDimensional.Add(this);
                }
    
                ProyectoID = Proyecto.ProyectoID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Proyecto")
                    && (ChangeTracker.OriginalValues["Proyecto"] == Proyecto))
                {
                    ChangeTracker.OriginalValues.Remove("Proyecto");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Proyecto", previousValue);
                }
                if (Proyecto != null && !Proyecto.ChangeTracker.ChangeTrackingEnabled)
                {
                    Proyecto.StartTracking();
                }
            }
        }
    
        private void FixupTipoReporteDimensional(TipoReporteDimensional previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.ReporteDimensional.Contains(this))
            {
                previousValue.ReporteDimensional.Remove(this);
            }
    
            if (TipoReporteDimensional != null)
            {
                if (!TipoReporteDimensional.ReporteDimensional.Contains(this))
                {
                    TipoReporteDimensional.ReporteDimensional.Add(this);
                }
    
                TipoReporteDimensionalID = TipoReporteDimensional.TipoReporteDimensionalID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("TipoReporteDimensional")
                    && (ChangeTracker.OriginalValues["TipoReporteDimensional"] == TipoReporteDimensional))
                {
                    ChangeTracker.OriginalValues.Remove("TipoReporteDimensional");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("TipoReporteDimensional", previousValue);
                }
                if (TipoReporteDimensional != null && !TipoReporteDimensional.ChangeTracker.ChangeTrackingEnabled)
                {
                    TipoReporteDimensional.StartTracking();
                }
            }
        }
    
        private void FixupReporteDimensionalDetalle(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (ReporteDimensionalDetalle item in e.NewItems)
                {
                    item.ReporteDimensional = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("ReporteDimensionalDetalle", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (ReporteDimensionalDetalle item in e.OldItems)
                {
                    if (ReferenceEquals(item.ReporteDimensional, this))
                    {
                        item.ReporteDimensional = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("ReporteDimensionalDetalle", item);
                    }
                }
            }
        }

        #endregion

    }
}
