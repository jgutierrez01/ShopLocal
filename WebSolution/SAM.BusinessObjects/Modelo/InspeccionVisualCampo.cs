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
    [KnownType(typeof(JuntaCampoInspeccionVisual))]
    [Serializable]
    public partial class InspeccionVisualCampo: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int InspeccionVisualCampoID
        {
            get { return _inspeccionVisualCampoID; }
            set
            {
                if (_inspeccionVisualCampoID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'InspeccionVisualCampoID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _inspeccionVisualCampoID = value;
                    OnPropertyChanged("InspeccionVisualCampoID");
                }
            }
        }
        private int _inspeccionVisualCampoID;
    
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
        public TrackableCollection<JuntaCampoInspeccionVisual> JuntaCampoInspeccionVisual
        {
            get
            {
                if (_juntaCampoInspeccionVisual == null)
                {
                    _juntaCampoInspeccionVisual = new TrackableCollection<JuntaCampoInspeccionVisual>();
                    _juntaCampoInspeccionVisual.CollectionChanged += FixupJuntaCampoInspeccionVisual;
                }
                return _juntaCampoInspeccionVisual;
            }
            set
            {
                if (!ReferenceEquals(_juntaCampoInspeccionVisual, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_juntaCampoInspeccionVisual != null)
                    {
                        _juntaCampoInspeccionVisual.CollectionChanged -= FixupJuntaCampoInspeccionVisual;
                    }
                    _juntaCampoInspeccionVisual = value;
                    if (_juntaCampoInspeccionVisual != null)
                    {
                        _juntaCampoInspeccionVisual.CollectionChanged += FixupJuntaCampoInspeccionVisual;
                    }
                    OnNavigationPropertyChanged("JuntaCampoInspeccionVisual");
                }
            }
        }
        private TrackableCollection<JuntaCampoInspeccionVisual> _juntaCampoInspeccionVisual;

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
    			if (_juntaCampoInspeccionVisual != null)
    		{
    			_juntaCampoInspeccionVisual.CollectionChanged -= FixupJuntaCampoInspeccionVisual;
    			_juntaCampoInspeccionVisual.CollectionChanged += FixupJuntaCampoInspeccionVisual;
    			}
    		}
    
    
        protected virtual void ClearNavigationProperties()
        {
            Proyecto = null;
            JuntaCampoInspeccionVisual.Clear();
        }

        #endregion

        #region Association Fixup
    
        private void FixupProyecto(Proyecto previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.InspeccionVisualCampo.Contains(this))
            {
                previousValue.InspeccionVisualCampo.Remove(this);
            }
    
            if (Proyecto != null)
            {
                if (!Proyecto.InspeccionVisualCampo.Contains(this))
                {
                    Proyecto.InspeccionVisualCampo.Add(this);
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
    
        private void FixupJuntaCampoInspeccionVisual(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (JuntaCampoInspeccionVisual item in e.NewItems)
                {
                    item.InspeccionVisualCampo = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("JuntaCampoInspeccionVisual", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (JuntaCampoInspeccionVisual item in e.OldItems)
                {
                    if (ReferenceEquals(item.InspeccionVisualCampo, this))
                    {
                        item.InspeccionVisualCampo = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("JuntaCampoInspeccionVisual", item);
                    }
                }
            }
        }

        #endregion

    }
}
