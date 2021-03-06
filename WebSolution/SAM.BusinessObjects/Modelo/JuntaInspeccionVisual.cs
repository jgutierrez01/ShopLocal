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
    [KnownType(typeof(InspeccionVisual))]
    [KnownType(typeof(JuntaWorkstatus))]
    [KnownType(typeof(JuntaInspeccionVisualDefecto))]
    [KnownType(typeof(Taller))]
    [Serializable]
    public partial class JuntaInspeccionVisual: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int JuntaInspeccionVisualID
        {
            get { return _juntaInspeccionVisualID; }
            set
            {
                if (_juntaInspeccionVisualID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'JuntaInspeccionVisualID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _juntaInspeccionVisualID = value;
                    OnPropertyChanged("JuntaInspeccionVisualID");
                }
            }
        }
        private int _juntaInspeccionVisualID;
    
        [DataMember]
        public int InspeccionVisualID
        {
            get { return _inspeccionVisualID; }
            set
            {
                if (_inspeccionVisualID != value)
                {
                    ChangeTracker.RecordOriginalValue("InspeccionVisualID", _inspeccionVisualID);
                    if (!IsDeserializing)
                    {
                        if (InspeccionVisual != null && InspeccionVisual.InspeccionVisualID != value)
                        {
                            InspeccionVisual = null;
                        }
                    }
                    _inspeccionVisualID = value;
                    OnPropertyChanged("InspeccionVisualID");
                }
            }
        }
        private int _inspeccionVisualID;
    
        [DataMember]
        public int JuntaWorkstatusID
        {
            get { return _juntaWorkstatusID; }
            set
            {
                if (_juntaWorkstatusID != value)
                {
                    ChangeTracker.RecordOriginalValue("JuntaWorkstatusID", _juntaWorkstatusID);
                    if (!IsDeserializing)
                    {
                        if (JuntaWorkstatus != null && JuntaWorkstatus.JuntaWorkstatusID != value)
                        {
                            JuntaWorkstatus = null;
                        }
                    }
                    _juntaWorkstatusID = value;
                    OnPropertyChanged("JuntaWorkstatusID");
                }
            }
        }
        private int _juntaWorkstatusID;
    
        [DataMember]
        public Nullable<System.DateTime> FechaInspeccion
        {
            get { return _fechaInspeccion; }
            set
            {
                if (_fechaInspeccion != value)
                {
                    _fechaInspeccion = value;
                    OnPropertyChanged("FechaInspeccion");
                }
            }
        }
        private Nullable<System.DateTime> _fechaInspeccion;
    
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
                    ChangeTracker.RecordOriginalValue("VersionRegistro", _versionRegistro);
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
    
        [DataMember]
        public Nullable<int> TallerID
        {
            get { return _tallerID; }
            set
            {
                if (_tallerID != value)
                {
                    ChangeTracker.RecordOriginalValue("TallerID", _tallerID);
                    if (!IsDeserializing)
                    {
                        if (Taller != null && Taller.TallerID != value)
                        {
                            Taller = null;
                        }
                    }
                    _tallerID = value;
                    OnPropertyChanged("TallerID");
                }
            }
        }
        private Nullable<int> _tallerID;
    
        [DataMember]
        public Nullable<int> InspectorID
        {
            get { return _inspectorID; }
            set
            {
                if (_inspectorID != value)
                {
                    _inspectorID = value;
                    OnPropertyChanged("InspectorID");
                }
            }
        }
        private Nullable<int> _inspectorID;

        #endregion

        #region Navigation Properties
    
        [DataMember]
        public InspeccionVisual InspeccionVisual
        {
            get { return _inspeccionVisual; }
            set
            {
                if (!ReferenceEquals(_inspeccionVisual, value))
                {
                    var previousValue = _inspeccionVisual;
                    _inspeccionVisual = value;
                    FixupInspeccionVisual(previousValue);
                    OnNavigationPropertyChanged("InspeccionVisual");
                }
            }
        }
        private InspeccionVisual _inspeccionVisual;
    
        [DataMember]
        public JuntaWorkstatus JuntaWorkstatus
        {
            get { return _juntaWorkstatus; }
            set
            {
                if (!ReferenceEquals(_juntaWorkstatus, value))
                {
                    var previousValue = _juntaWorkstatus;
                    _juntaWorkstatus = value;
                    FixupJuntaWorkstatus(previousValue);
                    OnNavigationPropertyChanged("JuntaWorkstatus");
                }
            }
        }
        private JuntaWorkstatus _juntaWorkstatus;
    
        [DataMember]
        public TrackableCollection<JuntaWorkstatus> JuntasWorkstatus
        {
            get
            {
                if (_juntasWorkstatus == null)
                {
                    _juntasWorkstatus = new TrackableCollection<JuntaWorkstatus>();
                    _juntasWorkstatus.CollectionChanged += FixupJuntasWorkstatus;
                }
                return _juntasWorkstatus;
            }
            set
            {
                if (!ReferenceEquals(_juntasWorkstatus, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_juntasWorkstatus != null)
                    {
                        _juntasWorkstatus.CollectionChanged -= FixupJuntasWorkstatus;
                    }
                    _juntasWorkstatus = value;
                    if (_juntasWorkstatus != null)
                    {
                        _juntasWorkstatus.CollectionChanged += FixupJuntasWorkstatus;
                    }
                    OnNavigationPropertyChanged("JuntasWorkstatus");
                }
            }
        }
        private TrackableCollection<JuntaWorkstatus> _juntasWorkstatus;
    
        [DataMember]
        public TrackableCollection<JuntaInspeccionVisualDefecto> JuntaInspeccionVisualDefecto
        {
            get
            {
                if (_juntaInspeccionVisualDefecto == null)
                {
                    _juntaInspeccionVisualDefecto = new TrackableCollection<JuntaInspeccionVisualDefecto>();
                    _juntaInspeccionVisualDefecto.CollectionChanged += FixupJuntaInspeccionVisualDefecto;
                }
                return _juntaInspeccionVisualDefecto;
            }
            set
            {
                if (!ReferenceEquals(_juntaInspeccionVisualDefecto, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_juntaInspeccionVisualDefecto != null)
                    {
                        _juntaInspeccionVisualDefecto.CollectionChanged -= FixupJuntaInspeccionVisualDefecto;
                    }
                    _juntaInspeccionVisualDefecto = value;
                    if (_juntaInspeccionVisualDefecto != null)
                    {
                        _juntaInspeccionVisualDefecto.CollectionChanged += FixupJuntaInspeccionVisualDefecto;
                    }
                    OnNavigationPropertyChanged("JuntaInspeccionVisualDefecto");
                }
            }
        }
        private TrackableCollection<JuntaInspeccionVisualDefecto> _juntaInspeccionVisualDefecto;
    
        [DataMember]
        public Taller Taller
        {
            get { return _taller; }
            set
            {
                if (!ReferenceEquals(_taller, value))
                {
                    var previousValue = _taller;
                    _taller = value;
                    FixupTaller(previousValue);
                    OnNavigationPropertyChanged("Taller");
                }
            }
        }
        private Taller _taller;

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
    			if (_juntasWorkstatus != null)
    		{
    			_juntasWorkstatus.CollectionChanged -= FixupJuntasWorkstatus;
    			_juntasWorkstatus.CollectionChanged += FixupJuntasWorkstatus;
    			}
    			if (_juntaInspeccionVisualDefecto != null)
    		{
    			_juntaInspeccionVisualDefecto.CollectionChanged -= FixupJuntaInspeccionVisualDefecto;
    			_juntaInspeccionVisualDefecto.CollectionChanged += FixupJuntaInspeccionVisualDefecto;
    			}
    		}
    
    
        protected virtual void ClearNavigationProperties()
        {
            InspeccionVisual = null;
            JuntaWorkstatus = null;
            JuntasWorkstatus.Clear();
            JuntaInspeccionVisualDefecto.Clear();
            Taller = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupInspeccionVisual(InspeccionVisual previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.JuntaInspeccionVisual.Contains(this))
            {
                previousValue.JuntaInspeccionVisual.Remove(this);
            }
    
            if (InspeccionVisual != null)
            {
                if (!InspeccionVisual.JuntaInspeccionVisual.Contains(this))
                {
                    InspeccionVisual.JuntaInspeccionVisual.Add(this);
                }
    
                InspeccionVisualID = InspeccionVisual.InspeccionVisualID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("InspeccionVisual")
                    && (ChangeTracker.OriginalValues["InspeccionVisual"] == InspeccionVisual))
                {
                    ChangeTracker.OriginalValues.Remove("InspeccionVisual");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("InspeccionVisual", previousValue);
                }
                if (InspeccionVisual != null && !InspeccionVisual.ChangeTracker.ChangeTrackingEnabled)
                {
                    InspeccionVisual.StartTracking();
                }
            }
        }
    
        private void FixupJuntaWorkstatus(JuntaWorkstatus previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.JuntasInspeccionadas.Contains(this))
            {
                previousValue.JuntasInspeccionadas.Remove(this);
            }
    
            if (JuntaWorkstatus != null)
            {
                if (!JuntaWorkstatus.JuntasInspeccionadas.Contains(this))
                {
                    JuntaWorkstatus.JuntasInspeccionadas.Add(this);
                }
    
                JuntaWorkstatusID = JuntaWorkstatus.JuntaWorkstatusID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("JuntaWorkstatus")
                    && (ChangeTracker.OriginalValues["JuntaWorkstatus"] == JuntaWorkstatus))
                {
                    ChangeTracker.OriginalValues.Remove("JuntaWorkstatus");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("JuntaWorkstatus", previousValue);
                }
                if (JuntaWorkstatus != null && !JuntaWorkstatus.ChangeTracker.ChangeTrackingEnabled)
                {
                    JuntaWorkstatus.StartTracking();
                }
            }
        }
    
        private void FixupTaller(Taller previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.JuntaInspeccionVisual.Contains(this))
            {
                previousValue.JuntaInspeccionVisual.Remove(this);
            }
    
            if (Taller != null)
            {
                if (!Taller.JuntaInspeccionVisual.Contains(this))
                {
                    Taller.JuntaInspeccionVisual.Add(this);
                }
    
                TallerID = Taller.TallerID;
            }
            else if (!skipKeys)
            {
                TallerID = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Taller")
                    && (ChangeTracker.OriginalValues["Taller"] == Taller))
                {
                    ChangeTracker.OriginalValues.Remove("Taller");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Taller", previousValue);
                }
                if (Taller != null && !Taller.ChangeTracker.ChangeTrackingEnabled)
                {
                    Taller.StartTracking();
                }
            }
        }
    
        private void FixupJuntasWorkstatus(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (JuntaWorkstatus item in e.NewItems)
                {
                    item.InspeccionVisual = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("JuntasWorkstatus", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (JuntaWorkstatus item in e.OldItems)
                {
                    if (ReferenceEquals(item.InspeccionVisual, this))
                    {
                        item.InspeccionVisual = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("JuntasWorkstatus", item);
                    }
                }
            }
        }
    
        private void FixupJuntaInspeccionVisualDefecto(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (JuntaInspeccionVisualDefecto item in e.NewItems)
                {
                    item.JuntaInspeccionVisual = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("JuntaInspeccionVisualDefecto", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (JuntaInspeccionVisualDefecto item in e.OldItems)
                {
                    if (ReferenceEquals(item.JuntaInspeccionVisual, this))
                    {
                        item.JuntaInspeccionVisual = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("JuntaInspeccionVisualDefecto", item);
                    }
                }
            }
        }

        #endregion

    }
}
