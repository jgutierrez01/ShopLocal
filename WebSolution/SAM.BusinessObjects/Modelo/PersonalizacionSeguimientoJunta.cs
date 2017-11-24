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
    [KnownType(typeof(DetallePersonalizacionSeguimientoJunta))]
    [KnownType(typeof(Usuario))]
    [Serializable]
    public partial class PersonalizacionSeguimientoJunta: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int PersonalizacionSeguimientoJuntaID
        {
            get { return _personalizacionSeguimientoJuntaID; }
            set
            {
                if (_personalizacionSeguimientoJuntaID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'PersonalizacionSeguimientoJuntaID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _personalizacionSeguimientoJuntaID = value;
                    OnPropertyChanged("PersonalizacionSeguimientoJuntaID");
                }
            }
        }
        private int _personalizacionSeguimientoJuntaID;
    
        [DataMember]
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (_nombre != value)
                {
                    _nombre = value;
                    OnPropertyChanged("Nombre");
                }
            }
        }
        private string _nombre;
    
        [DataMember]
        public System.Guid UserId
        {
            get { return _userId; }
            set
            {
                if (_userId != value)
                {
                    ChangeTracker.RecordOriginalValue("UserId", _userId);
                    if (!IsDeserializing)
                    {
                        if (Usuario != null && Usuario.UserId != value)
                        {
                            Usuario = null;
                        }
                    }
                    _userId = value;
                    OnPropertyChanged("UserId");
                }
            }
        }
        private System.Guid _userId;
    
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
        public TrackableCollection<DetallePersonalizacionSeguimientoJunta> DetallePersonalizacionSeguimientoJunta
        {
            get
            {
                if (_detallePersonalizacionSeguimientoJunta == null)
                {
                    _detallePersonalizacionSeguimientoJunta = new TrackableCollection<DetallePersonalizacionSeguimientoJunta>();
                    _detallePersonalizacionSeguimientoJunta.CollectionChanged += FixupDetallePersonalizacionSeguimientoJunta;
                }
                return _detallePersonalizacionSeguimientoJunta;
            }
            set
            {
                if (!ReferenceEquals(_detallePersonalizacionSeguimientoJunta, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_detallePersonalizacionSeguimientoJunta != null)
                    {
                        _detallePersonalizacionSeguimientoJunta.CollectionChanged -= FixupDetallePersonalizacionSeguimientoJunta;
                    }
                    _detallePersonalizacionSeguimientoJunta = value;
                    if (_detallePersonalizacionSeguimientoJunta != null)
                    {
                        _detallePersonalizacionSeguimientoJunta.CollectionChanged += FixupDetallePersonalizacionSeguimientoJunta;
                    }
                    OnNavigationPropertyChanged("DetallePersonalizacionSeguimientoJunta");
                }
            }
        }
        private TrackableCollection<DetallePersonalizacionSeguimientoJunta> _detallePersonalizacionSeguimientoJunta;
    
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
    			if (_detallePersonalizacionSeguimientoJunta != null)
    		{
    			_detallePersonalizacionSeguimientoJunta.CollectionChanged -= FixupDetallePersonalizacionSeguimientoJunta;
    			_detallePersonalizacionSeguimientoJunta.CollectionChanged += FixupDetallePersonalizacionSeguimientoJunta;
    			}
    		}
    
    
        protected virtual void ClearNavigationProperties()
        {
            DetallePersonalizacionSeguimientoJunta.Clear();
            Usuario = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupUsuario(Usuario previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.PersonalizacionSeguimientoJunta.Contains(this))
            {
                previousValue.PersonalizacionSeguimientoJunta.Remove(this);
            }
    
            if (Usuario != null)
            {
                if (!Usuario.PersonalizacionSeguimientoJunta.Contains(this))
                {
                    Usuario.PersonalizacionSeguimientoJunta.Add(this);
                }
    
                UserId = Usuario.UserId;
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
    
        private void FixupDetallePersonalizacionSeguimientoJunta(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (DetallePersonalizacionSeguimientoJunta item in e.NewItems)
                {
                    item.PersonalizacionSeguimientoJunta = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("DetallePersonalizacionSeguimientoJunta", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (DetallePersonalizacionSeguimientoJunta item in e.OldItems)
                {
                    if (ReferenceEquals(item.PersonalizacionSeguimientoJunta, this))
                    {
                        item.PersonalizacionSeguimientoJunta = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("DetallePersonalizacionSeguimientoJunta", item);
                    }
                }
            }
        }

        #endregion

    }
}
