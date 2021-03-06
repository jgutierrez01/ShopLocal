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
    [KnownType(typeof(Usuario))]
    [KnownType(typeof(BastonSpool))]
    [KnownType(typeof(JuntaSpool))]
    [Serializable]
    public partial class BastonSpoolJunta: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int BastonSpoolJuntaID
        {
            get { return _bastonSpoolJuntaID; }
            set
            {
                if (_bastonSpoolJuntaID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'BastonSpoolJuntaID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _bastonSpoolJuntaID = value;
                    OnPropertyChanged("BastonSpoolJuntaID");
                }
            }
        }
        private int _bastonSpoolJuntaID;
    
        [DataMember]
        public int BastonSpoolID
        {
            get { return _bastonSpoolID; }
            set
            {
                if (_bastonSpoolID != value)
                {
                    ChangeTracker.RecordOriginalValue("BastonSpoolID", _bastonSpoolID);
                    if (!IsDeserializing)
                    {
                        if (BastonSpool != null && BastonSpool.BastonSpoolID != value)
                        {
                            BastonSpool = null;
                        }
                    }
                    _bastonSpoolID = value;
                    OnPropertyChanged("BastonSpoolID");
                }
            }
        }
        private int _bastonSpoolID;
    
        [DataMember]
        public int JuntaSpoolID
        {
            get { return _juntaSpoolID; }
            set
            {
                if (_juntaSpoolID != value)
                {
                    ChangeTracker.RecordOriginalValue("JuntaSpoolID", _juntaSpoolID);
                    if (!IsDeserializing)
                    {
                        if (JuntaSpool != null && JuntaSpool.JuntaSpoolID != value)
                        {
                            JuntaSpool = null;
                        }
                    }
                    _juntaSpoolID = value;
                    OnPropertyChanged("JuntaSpoolID");
                }
            }
        }
        private int _juntaSpoolID;
    
        [DataMember]
        public Nullable<System.Guid> UsuarioModifica
        {
            get { return _usuarioModifica; }
            set
            {
                if (_usuarioModifica != value)
                {
                    ChangeTracker.RecordOriginalValue("UsuarioModifica", _usuarioModifica);
                    if (!IsDeserializing)
                    {
                        if (Usuario != null && Usuario.UserId != value)
                        {
                            Usuario = null;
                        }
                    }
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

        #endregion

        #region Navigation Properties
    
        [DataMember]
        public Usuario Usuario
        {
            get { return _usuario; }
            set
            {
                if (!ReferenceEquals(_usuario, value))
                {
                    var previousValue = _usuario;
                    _usuario = value;
                    FixupUsuario(previousValue);
                    OnNavigationPropertyChanged("Usuario");
                }
            }
        }
        private Usuario _usuario;
    
        [DataMember]
        public BastonSpool BastonSpool
        {
            get { return _bastonSpool; }
            set
            {
                if (!ReferenceEquals(_bastonSpool, value))
                {
                    var previousValue = _bastonSpool;
                    _bastonSpool = value;
                    FixupBastonSpool(previousValue);
                    OnNavigationPropertyChanged("BastonSpool");
                }
            }
        }
        private BastonSpool _bastonSpool;
    
        [DataMember]
        public JuntaSpool JuntaSpool
        {
            get { return _juntaSpool; }
            set
            {
                if (!ReferenceEquals(_juntaSpool, value))
                {
                    var previousValue = _juntaSpool;
                    _juntaSpool = value;
                    FixupJuntaSpool(previousValue);
                    OnNavigationPropertyChanged("JuntaSpool");
                }
            }
        }
        private JuntaSpool _juntaSpool;

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
            Usuario = null;
            BastonSpool = null;
            JuntaSpool = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupUsuario(Usuario previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.BastonSpoolJunta.Contains(this))
            {
                previousValue.BastonSpoolJunta.Remove(this);
            }
    
            if (Usuario != null)
            {
                if (!Usuario.BastonSpoolJunta.Contains(this))
                {
                    Usuario.BastonSpoolJunta.Add(this);
                }
    
                UsuarioModifica = Usuario.UserId;
            }
            else if (!skipKeys)
            {
                UsuarioModifica = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Usuario")
                    && (ChangeTracker.OriginalValues["Usuario"] == Usuario))
                {
                    ChangeTracker.OriginalValues.Remove("Usuario");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Usuario", previousValue);
                }
                if (Usuario != null && !Usuario.ChangeTracker.ChangeTrackingEnabled)
                {
                    Usuario.StartTracking();
                }
            }
        }
    
        private void FixupBastonSpool(BastonSpool previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.BastonSpoolJunta.Contains(this))
            {
                previousValue.BastonSpoolJunta.Remove(this);
            }
    
            if (BastonSpool != null)
            {
                if (!BastonSpool.BastonSpoolJunta.Contains(this))
                {
                    BastonSpool.BastonSpoolJunta.Add(this);
                }
    
                BastonSpoolID = BastonSpool.BastonSpoolID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("BastonSpool")
                    && (ChangeTracker.OriginalValues["BastonSpool"] == BastonSpool))
                {
                    ChangeTracker.OriginalValues.Remove("BastonSpool");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("BastonSpool", previousValue);
                }
                if (BastonSpool != null && !BastonSpool.ChangeTracker.ChangeTrackingEnabled)
                {
                    BastonSpool.StartTracking();
                }
            }
        }
    
        private void FixupJuntaSpool(JuntaSpool previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.BastonSpoolJunta.Contains(this))
            {
                previousValue.BastonSpoolJunta.Remove(this);
            }
    
            if (JuntaSpool != null)
            {
                if (!JuntaSpool.BastonSpoolJunta.Contains(this))
                {
                    JuntaSpool.BastonSpoolJunta.Add(this);
                }
    
                JuntaSpoolID = JuntaSpool.JuntaSpoolID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("JuntaSpool")
                    && (ChangeTracker.OriginalValues["JuntaSpool"] == JuntaSpool))
                {
                    ChangeTracker.OriginalValues.Remove("JuntaSpool");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("JuntaSpool", previousValue);
                }
                if (JuntaSpool != null && !JuntaSpool.ChangeTracker.ChangeTrackingEnabled)
                {
                    JuntaSpool.StartTracking();
                }
            }
        }

        #endregion

    }
}
