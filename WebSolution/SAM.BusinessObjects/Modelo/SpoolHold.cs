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
    [KnownType(typeof(Spool))]
    [Serializable]
    public partial class SpoolHold: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int SpoolID
        {
            get { return _spoolID; }
            set
            {
                if (_spoolID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'SpoolID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    if (!IsDeserializing)
                    {
                        if (Spool != null && Spool.SpoolID != value)
                        {
                            Spool = null;
                        }
                    }
                    _spoolID = value;
                    OnPropertyChanged("SpoolID");
                }
            }
        }
        private int _spoolID;
    
        [DataMember]
        public bool TieneHoldIngenieria
        {
            get { return _tieneHoldIngenieria; }
            set
            {
                if (_tieneHoldIngenieria != value)
                {
                    _tieneHoldIngenieria = value;
                    OnPropertyChanged("TieneHoldIngenieria");
                }
            }
        }
        private bool _tieneHoldIngenieria;
    
        [DataMember]
        public bool TieneHoldCalidad
        {
            get { return _tieneHoldCalidad; }
            set
            {
                if (_tieneHoldCalidad != value)
                {
                    _tieneHoldCalidad = value;
                    OnPropertyChanged("TieneHoldCalidad");
                }
            }
        }
        private bool _tieneHoldCalidad;
    
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
        public bool Confinado
        {
            get { return _confinado; }
            set
            {
                if (_confinado != value)
                {
                    _confinado = value;
                    OnPropertyChanged("Confinado");
                }
            }
        }
        private bool _confinado;

        #endregion

        #region Navigation Properties
    
        [DataMember]
        public Spool Spool
        {
            get { return _spool; }
            set
            {
                if (!ReferenceEquals(_spool, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added && value != null)
                    {
                        // This the dependent end of an identifying relationship, so the principal end cannot be changed if it is already set,
                        // otherwise it can only be set to an entity with a primary key that is the same value as the dependent's foreign key.
                        if (SpoolID != value.SpoolID)
                        {
                            throw new InvalidOperationException("The principal end of an identifying relationship can only be changed when the dependent end is in the Added state.");
                        }
                    }
                    var previousValue = _spool;
                    _spool = value;
                    FixupSpool(previousValue);
                    OnNavigationPropertyChanged("Spool");
                }
            }
        }
        private Spool _spool;

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
    
        // This entity type is the dependent end in at least one association that performs cascade deletes.
        // This event handler will process notifications that occur when the principal end is deleted.
        internal void HandleCascadeDelete(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                this.MarkAsDeleted();
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
            Spool = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupSpool(Spool previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && ReferenceEquals(previousValue.SpoolHold, this))
            {
                previousValue.SpoolHold = null;
            }
    
            if (Spool != null)
            {
                Spool.SpoolHold = this;
                SpoolID = Spool.SpoolID;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Spool")
                    && (ChangeTracker.OriginalValues["Spool"] == Spool))
                {
                    ChangeTracker.OriginalValues.Remove("Spool");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Spool", previousValue);
                }
                if (Spool != null && !Spool.ChangeTracker.ChangeTrackingEnabled)
                {
                    Spool.StartTracking();
                }
            }
        }

        #endregion

    }
}
