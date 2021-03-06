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
    [KnownType(typeof(InspeccionVisualPatio))]
    [Serializable]
    public partial class InspeccionVisualPatioDefecto: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int InspeccionVisualPatioDefecto1
        {
            get { return _inspeccionVisualPatioDefecto1; }
            set
            {
                if (_inspeccionVisualPatioDefecto1 != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'InspeccionVisualPatioDefecto1' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _inspeccionVisualPatioDefecto1 = value;
                    OnPropertyChanged("InspeccionVisualPatioDefecto1");
                }
            }
        }
        private int _inspeccionVisualPatioDefecto1;
    
        [DataMember]
        public int InspeccionVisualPatioID
        {
            get { return _inspeccionVisualPatioID; }
            set
            {
                if (_inspeccionVisualPatioID != value)
                {
                    ChangeTracker.RecordOriginalValue("InspeccionVisualPatioID", _inspeccionVisualPatioID);
                    if (!IsDeserializing)
                    {
                        if (InspeccionVisualPatio != null && InspeccionVisualPatio.InspeccionVisualPatioID != value)
                        {
                            InspeccionVisualPatio = null;
                        }
                    }
                    _inspeccionVisualPatioID = value;
                    OnPropertyChanged("InspeccionVisualPatioID");
                }
            }
        }
        private int _inspeccionVisualPatioID;
    
        [DataMember]
        public int DefectoID
        {
            get { return _defectoID; }
            set
            {
                if (_defectoID != value)
                {
                    _defectoID = value;
                    OnPropertyChanged("DefectoID");
                }
            }
        }
        private int _defectoID;
    
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
        public InspeccionVisualPatio InspeccionVisualPatio
        {
            get { return _inspeccionVisualPatio; }
            set
            {
                if (!ReferenceEquals(_inspeccionVisualPatio, value))
                {
                    var previousValue = _inspeccionVisualPatio;
                    _inspeccionVisualPatio = value;
                    FixupInspeccionVisualPatio(previousValue);
                    OnNavigationPropertyChanged("InspeccionVisualPatio");
                }
            }
        }
        private InspeccionVisualPatio _inspeccionVisualPatio;

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
            InspeccionVisualPatio = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupInspeccionVisualPatio(InspeccionVisualPatio previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.InspeccionVisualPatioDefecto.Contains(this))
            {
                previousValue.InspeccionVisualPatioDefecto.Remove(this);
            }
    
            if (InspeccionVisualPatio != null)
            {
                if (!InspeccionVisualPatio.InspeccionVisualPatioDefecto.Contains(this))
                {
                    InspeccionVisualPatio.InspeccionVisualPatioDefecto.Add(this);
                }
    
                InspeccionVisualPatioID = InspeccionVisualPatio.InspeccionVisualPatioID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("InspeccionVisualPatio")
                    && (ChangeTracker.OriginalValues["InspeccionVisualPatio"] == InspeccionVisualPatio))
                {
                    ChangeTracker.OriginalValues.Remove("InspeccionVisualPatio");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("InspeccionVisualPatio", previousValue);
                }
                if (InspeccionVisualPatio != null && !InspeccionVisualPatio.ChangeTracker.ChangeTrackingEnabled)
                {
                    InspeccionVisualPatio.StartTracking();
                }
            }
        }

        #endregion

    }
}
