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
    [KnownType(typeof(OrdenTrabajoEspecial))]
    [KnownType(typeof(Spool))]
    [Serializable]
    public partial class OrdenTrabajoEspecialSpool: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int OrdenTrabajoEspecialSpoolID
        {
            get { return _ordenTrabajoEspecialSpoolID; }
            set
            {
                if (_ordenTrabajoEspecialSpoolID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'OrdenTrabajoEspecialSpoolID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _ordenTrabajoEspecialSpoolID = value;
                    OnPropertyChanged("OrdenTrabajoEspecialSpoolID");
                }
            }
        }
        private int _ordenTrabajoEspecialSpoolID;
    
        [DataMember]
        public int OrdenTrabajoEspecialID
        {
            get { return _ordenTrabajoEspecialID; }
            set
            {
                if (_ordenTrabajoEspecialID != value)
                {
                    ChangeTracker.RecordOriginalValue("OrdenTrabajoEspecialID", _ordenTrabajoEspecialID);
                    if (!IsDeserializing)
                    {
                        if (OrdenTrabajoEspecial != null && OrdenTrabajoEspecial.OrdenTrabajoEspecialID != value)
                        {
                            OrdenTrabajoEspecial = null;
                        }
                    }
                    _ordenTrabajoEspecialID = value;
                    OnPropertyChanged("OrdenTrabajoEspecialID");
                }
            }
        }
        private int _ordenTrabajoEspecialID;
    
        [DataMember]
        public int SpoolID
        {
            get { return _spoolID; }
            set
            {
                if (_spoolID != value)
                {
                    ChangeTracker.RecordOriginalValue("SpoolID", _spoolID);
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
        public int Partida
        {
            get { return _partida; }
            set
            {
                if (_partida != value)
                {
                    _partida = value;
                    OnPropertyChanged("Partida");
                }
            }
        }
        private int _partida;
    
        [DataMember]
        public string NumeroControl
        {
            get { return _numeroControl; }
            set
            {
                if (_numeroControl != value)
                {
                    _numeroControl = value;
                    OnPropertyChanged("NumeroControl");
                }
            }
        }
        private string _numeroControl;
    
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
        public bool EsAsignado
        {
            get { return _esAsignado; }
            set
            {
                if (_esAsignado != value)
                {
                    _esAsignado = value;
                    OnPropertyChanged("EsAsignado");
                }
            }
        }
        private bool _esAsignado;

        #endregion

        #region Navigation Properties
    
        [DataMember]
        public OrdenTrabajoEspecial OrdenTrabajoEspecial
        {
            get { return _ordenTrabajoEspecial; }
            set
            {
                if (!ReferenceEquals(_ordenTrabajoEspecial, value))
                {
                    var previousValue = _ordenTrabajoEspecial;
                    _ordenTrabajoEspecial = value;
                    FixupOrdenTrabajoEspecial(previousValue);
                    OnNavigationPropertyChanged("OrdenTrabajoEspecial");
                }
            }
        }
        private OrdenTrabajoEspecial _ordenTrabajoEspecial;
    
        [DataMember]
        public Spool Spool
        {
            get { return _spool; }
            set
            {
                if (!ReferenceEquals(_spool, value))
                {
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
            OrdenTrabajoEspecial = null;
            Spool = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupOrdenTrabajoEspecial(OrdenTrabajoEspecial previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.OrdenTrabajoEspecialSpool.Contains(this))
            {
                previousValue.OrdenTrabajoEspecialSpool.Remove(this);
            }
    
            if (OrdenTrabajoEspecial != null)
            {
                if (!OrdenTrabajoEspecial.OrdenTrabajoEspecialSpool.Contains(this))
                {
                    OrdenTrabajoEspecial.OrdenTrabajoEspecialSpool.Add(this);
                }
    
                OrdenTrabajoEspecialID = OrdenTrabajoEspecial.OrdenTrabajoEspecialID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("OrdenTrabajoEspecial")
                    && (ChangeTracker.OriginalValues["OrdenTrabajoEspecial"] == OrdenTrabajoEspecial))
                {
                    ChangeTracker.OriginalValues.Remove("OrdenTrabajoEspecial");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("OrdenTrabajoEspecial", previousValue);
                }
                if (OrdenTrabajoEspecial != null && !OrdenTrabajoEspecial.ChangeTracker.ChangeTrackingEnabled)
                {
                    OrdenTrabajoEspecial.StartTracking();
                }
            }
        }
    
        private void FixupSpool(Spool previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.OrdenTrabajoEspecialSpool.Contains(this))
            {
                previousValue.OrdenTrabajoEspecialSpool.Remove(this);
            }
    
            if (Spool != null)
            {
                if (!Spool.OrdenTrabajoEspecialSpool.Contains(this))
                {
                    Spool.OrdenTrabajoEspecialSpool.Add(this);
                }
    
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
