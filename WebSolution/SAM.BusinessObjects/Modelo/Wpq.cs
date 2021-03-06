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
    [KnownType(typeof(Soldador))]
    [KnownType(typeof(Wps))]
    [Serializable]
    public partial class Wpq: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int WpqID
        {
            get { return _wpqID; }
            set
            {
                if (_wpqID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'WpqID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _wpqID = value;
                    OnPropertyChanged("WpqID");
                }
            }
        }
        private int _wpqID;
    
        [DataMember]
        public int WpsID
        {
            get { return _wpsID; }
            set
            {
                if (_wpsID != value)
                {
                    ChangeTracker.RecordOriginalValue("WpsID", _wpsID);
                    if (!IsDeserializing)
                    {
                        if (Wps != null && Wps.WpsID != value)
                        {
                            Wps = null;
                        }
                    }
                    _wpsID = value;
                    OnPropertyChanged("WpsID");
                }
            }
        }
        private int _wpsID;
    
        [DataMember]
        public int SoldadorID
        {
            get { return _soldadorID; }
            set
            {
                if (_soldadorID != value)
                {
                    ChangeTracker.RecordOriginalValue("SoldadorID", _soldadorID);
                    if (!IsDeserializing)
                    {
                        if (Soldador != null && Soldador.SoldadorID != value)
                        {
                            Soldador = null;
                        }
                    }
                    _soldadorID = value;
                    OnPropertyChanged("SoldadorID");
                }
            }
        }
        private int _soldadorID;
    
        [DataMember]
        public System.DateTime FechaInicio
        {
            get { return _fechaInicio; }
            set
            {
                if (_fechaInicio != value)
                {
                    _fechaInicio = value;
                    OnPropertyChanged("FechaInicio");
                }
            }
        }
        private System.DateTime _fechaInicio;
    
        [DataMember]
        public System.DateTime FechaVigencia
        {
            get { return _fechaVigencia; }
            set
            {
                if (_fechaVigencia != value)
                {
                    _fechaVigencia = value;
                    OnPropertyChanged("FechaVigencia");
                }
            }
        }
        private System.DateTime _fechaVigencia;
    
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
        public Soldador Soldador
        {
            get { return _soldador; }
            set
            {
                if (!ReferenceEquals(_soldador, value))
                {
                    var previousValue = _soldador;
                    _soldador = value;
                    FixupSoldador(previousValue);
                    OnNavigationPropertyChanged("Soldador");
                }
            }
        }
        private Soldador _soldador;
    
        [DataMember]
        public Wps Wps
        {
            get { return _wps; }
            set
            {
                if (!ReferenceEquals(_wps, value))
                {
                    var previousValue = _wps;
                    _wps = value;
                    FixupWps(previousValue);
                    OnNavigationPropertyChanged("Wps");
                }
            }
        }
        private Wps _wps;

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
            Soldador = null;
            Wps = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupSoldador(Soldador previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Wpq.Contains(this))
            {
                previousValue.Wpq.Remove(this);
            }
    
            if (Soldador != null)
            {
                if (!Soldador.Wpq.Contains(this))
                {
                    Soldador.Wpq.Add(this);
                }
    
                SoldadorID = Soldador.SoldadorID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Soldador")
                    && (ChangeTracker.OriginalValues["Soldador"] == Soldador))
                {
                    ChangeTracker.OriginalValues.Remove("Soldador");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Soldador", previousValue);
                }
                if (Soldador != null && !Soldador.ChangeTracker.ChangeTrackingEnabled)
                {
                    Soldador.StartTracking();
                }
            }
        }
    
        private void FixupWps(Wps previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Wpq.Contains(this))
            {
                previousValue.Wpq.Remove(this);
            }
    
            if (Wps != null)
            {
                if (!Wps.Wpq.Contains(this))
                {
                    Wps.Wpq.Add(this);
                }
    
                WpsID = Wps.WpsID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Wps")
                    && (ChangeTracker.OriginalValues["Wps"] == Wps))
                {
                    ChangeTracker.OriginalValues.Remove("Wps");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Wps", previousValue);
                }
                if (Wps != null && !Wps.ChangeTracker.ChangeTrackingEnabled)
                {
                    Wps.StartTracking();
                }
            }
        }

        #endregion

    }
}
