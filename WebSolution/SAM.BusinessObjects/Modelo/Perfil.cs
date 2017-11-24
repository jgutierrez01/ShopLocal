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
    [KnownType(typeof(PerfilPermiso))]
    [KnownType(typeof(Usuario))]
    [Serializable]
    public partial class Perfil: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int PerfilID
        {
            get { return _perfilID; }
            set
            {
                if (_perfilID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'PerfilID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _perfilID = value;
                    OnPropertyChanged("PerfilID");
                }
            }
        }
        private int _perfilID;
    
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
        public string Descripcion
        {
            get { return _descripcion; }
            set
            {
                if (_descripcion != value)
                {
                    _descripcion = value;
                    OnPropertyChanged("Descripcion");
                }
            }
        }
        private string _descripcion;
    
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
        public string NombreIngles
        {
            get { return _nombreIngles; }
            set
            {
                if (_nombreIngles != value)
                {
                    _nombreIngles = value;
                    OnPropertyChanged("NombreIngles");
                }
            }
        }
        private string _nombreIngles;

        #endregion

        #region Navigation Properties
    
        [DataMember]
        public TrackableCollection<PerfilPermiso> PerfilPermiso
        {
            get
            {
                if (_perfilPermiso == null)
                {
                    _perfilPermiso = new TrackableCollection<PerfilPermiso>();
                    _perfilPermiso.CollectionChanged += FixupPerfilPermiso;
                }
                return _perfilPermiso;
            }
            set
            {
                if (!ReferenceEquals(_perfilPermiso, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_perfilPermiso != null)
                    {
                        _perfilPermiso.CollectionChanged -= FixupPerfilPermiso;
                    }
                    _perfilPermiso = value;
                    if (_perfilPermiso != null)
                    {
                        _perfilPermiso.CollectionChanged += FixupPerfilPermiso;
                    }
                    OnNavigationPropertyChanged("PerfilPermiso");
                }
            }
        }
        private TrackableCollection<PerfilPermiso> _perfilPermiso;
    
        [DataMember]
        public TrackableCollection<Usuario> Usuario
        {
            get
            {
                if (_usuario == null)
                {
                    _usuario = new TrackableCollection<Usuario>();
                    _usuario.CollectionChanged += FixupUsuario;
                }
                return _usuario;
            }
            set
            {
                if (!ReferenceEquals(_usuario, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_usuario != null)
                    {
                        _usuario.CollectionChanged -= FixupUsuario;
                    }
                    _usuario = value;
                    if (_usuario != null)
                    {
                        _usuario.CollectionChanged += FixupUsuario;
                    }
                    OnNavigationPropertyChanged("Usuario");
                }
            }
        }
        private TrackableCollection<Usuario> _usuario;

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
    			if (_perfilPermiso != null)
    		{
    			_perfilPermiso.CollectionChanged -= FixupPerfilPermiso;
    			_perfilPermiso.CollectionChanged += FixupPerfilPermiso;
    			}
    			if (_usuario != null)
    		{
    			_usuario.CollectionChanged -= FixupUsuario;
    			_usuario.CollectionChanged += FixupUsuario;
    			}
    		}
    
    
        protected virtual void ClearNavigationProperties()
        {
            PerfilPermiso.Clear();
            Usuario.Clear();
        }

        #endregion

        #region Association Fixup
    
        private void FixupPerfilPermiso(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (PerfilPermiso item in e.NewItems)
                {
                    item.Perfil = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("PerfilPermiso", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (PerfilPermiso item in e.OldItems)
                {
                    if (ReferenceEquals(item.Perfil, this))
                    {
                        item.Perfil = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("PerfilPermiso", item);
                    }
                }
            }
        }
    
        private void FixupUsuario(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (Usuario item in e.NewItems)
                {
                    item.Perfil = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("Usuario", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Usuario item in e.OldItems)
                {
                    if (ReferenceEquals(item.Perfil, this))
                    {
                        item.Perfil = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("Usuario", item);
                    }
                }
            }
        }

        #endregion

    }
}
