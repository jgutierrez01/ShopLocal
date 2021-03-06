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
    [KnownType(typeof(EstimacionJunta))]
    [KnownType(typeof(EstimacionSpool))]
    [Serializable]
    public partial class Estimacion: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int EstimacionID
        {
            get { return _estimacionID; }
            set
            {
                if (_estimacionID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'EstimacionID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _estimacionID = value;
                    OnPropertyChanged("EstimacionID");
                }
            }
        }
        private int _estimacionID;
    
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
        public string NumeroEstimacion
        {
            get { return _numeroEstimacion; }
            set
            {
                if (_numeroEstimacion != value)
                {
                    _numeroEstimacion = value;
                    OnPropertyChanged("NumeroEstimacion");
                }
            }
        }
        private string _numeroEstimacion;
    
        [DataMember]
        public System.DateTime FechaEstimacion
        {
            get { return _fechaEstimacion; }
            set
            {
                if (_fechaEstimacion != value)
                {
                    _fechaEstimacion = value;
                    OnPropertyChanged("FechaEstimacion");
                }
            }
        }
        private System.DateTime _fechaEstimacion;
    
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
        public TrackableCollection<EstimacionJunta> EstimacionJunta
        {
            get
            {
                if (_estimacionJunta == null)
                {
                    _estimacionJunta = new TrackableCollection<EstimacionJunta>();
                    _estimacionJunta.CollectionChanged += FixupEstimacionJunta;
                }
                return _estimacionJunta;
            }
            set
            {
                if (!ReferenceEquals(_estimacionJunta, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_estimacionJunta != null)
                    {
                        _estimacionJunta.CollectionChanged -= FixupEstimacionJunta;
                    }
                    _estimacionJunta = value;
                    if (_estimacionJunta != null)
                    {
                        _estimacionJunta.CollectionChanged += FixupEstimacionJunta;
                    }
                    OnNavigationPropertyChanged("EstimacionJunta");
                }
            }
        }
        private TrackableCollection<EstimacionJunta> _estimacionJunta;
    
        [DataMember]
        public TrackableCollection<EstimacionSpool> EstimacionSpool
        {
            get
            {
                if (_estimacionSpool == null)
                {
                    _estimacionSpool = new TrackableCollection<EstimacionSpool>();
                    _estimacionSpool.CollectionChanged += FixupEstimacionSpool;
                }
                return _estimacionSpool;
            }
            set
            {
                if (!ReferenceEquals(_estimacionSpool, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_estimacionSpool != null)
                    {
                        _estimacionSpool.CollectionChanged -= FixupEstimacionSpool;
                    }
                    _estimacionSpool = value;
                    if (_estimacionSpool != null)
                    {
                        _estimacionSpool.CollectionChanged += FixupEstimacionSpool;
                    }
                    OnNavigationPropertyChanged("EstimacionSpool");
                }
            }
        }
        private TrackableCollection<EstimacionSpool> _estimacionSpool;

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
    			if (_estimacionJunta != null)
    		{
    			_estimacionJunta.CollectionChanged -= FixupEstimacionJunta;
    			_estimacionJunta.CollectionChanged += FixupEstimacionJunta;
    			}
    			if (_estimacionSpool != null)
    		{
    			_estimacionSpool.CollectionChanged -= FixupEstimacionSpool;
    			_estimacionSpool.CollectionChanged += FixupEstimacionSpool;
    			}
    		}
    
    
        protected virtual void ClearNavigationProperties()
        {
            Proyecto = null;
            EstimacionJunta.Clear();
            EstimacionSpool.Clear();
        }

        #endregion

        #region Association Fixup
    
        private void FixupProyecto(Proyecto previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Estimacion.Contains(this))
            {
                previousValue.Estimacion.Remove(this);
            }
    
            if (Proyecto != null)
            {
                if (!Proyecto.Estimacion.Contains(this))
                {
                    Proyecto.Estimacion.Add(this);
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
    
        private void FixupEstimacionJunta(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (EstimacionJunta item in e.NewItems)
                {
                    item.Estimacion = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("EstimacionJunta", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (EstimacionJunta item in e.OldItems)
                {
                    if (ReferenceEquals(item.Estimacion, this))
                    {
                        item.Estimacion = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("EstimacionJunta", item);
                    }
                }
            }
        }
    
        private void FixupEstimacionSpool(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (EstimacionSpool item in e.NewItems)
                {
                    item.Estimacion = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("EstimacionSpool", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (EstimacionSpool item in e.OldItems)
                {
                    if (ReferenceEquals(item.Estimacion, this))
                    {
                        item.Estimacion = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("EstimacionSpool", item);
                    }
                }
            }
        }

        #endregion

    }
}
