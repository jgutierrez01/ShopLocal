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
    [KnownType(typeof(Cliente))]
    [Serializable]
    public partial class ContactoCliente: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int ContactoClienteID
        {
            get { return _contactoClienteID; }
            set
            {
                if (_contactoClienteID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'ContactoClienteID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _contactoClienteID = value;
                    OnPropertyChanged("ContactoClienteID");
                }
            }
        }
        private int _contactoClienteID;
    
        [DataMember]
        public int ClienteID
        {
            get { return _clienteID; }
            set
            {
                if (_clienteID != value)
                {
                    ChangeTracker.RecordOriginalValue("ClienteID", _clienteID);
                    if (!IsDeserializing)
                    {
                        if (Cliente != null && Cliente.ClienteID != value)
                        {
                            Cliente = null;
                        }
                    }
                    _clienteID = value;
                    OnPropertyChanged("ClienteID");
                }
            }
        }
        private int _clienteID;
    
        [DataMember]
        public string Puesto
        {
            get { return _puesto; }
            set
            {
                if (_puesto != value)
                {
                    _puesto = value;
                    OnPropertyChanged("Puesto");
                }
            }
        }
        private string _puesto;
    
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
        public string ApPaterno
        {
            get { return _apPaterno; }
            set
            {
                if (_apPaterno != value)
                {
                    _apPaterno = value;
                    OnPropertyChanged("ApPaterno");
                }
            }
        }
        private string _apPaterno;
    
        [DataMember]
        public string ApMaterno
        {
            get { return _apMaterno; }
            set
            {
                if (_apMaterno != value)
                {
                    _apMaterno = value;
                    OnPropertyChanged("ApMaterno");
                }
            }
        }
        private string _apMaterno;
    
        [DataMember]
        public string CorreoElectronico
        {
            get { return _correoElectronico; }
            set
            {
                if (_correoElectronico != value)
                {
                    _correoElectronico = value;
                    OnPropertyChanged("CorreoElectronico");
                }
            }
        }
        private string _correoElectronico;
    
        [DataMember]
        public string TelefonoOficina
        {
            get { return _telefonoOficina; }
            set
            {
                if (_telefonoOficina != value)
                {
                    _telefonoOficina = value;
                    OnPropertyChanged("TelefonoOficina");
                }
            }
        }
        private string _telefonoOficina;
    
        [DataMember]
        public string TelefonoParticular
        {
            get { return _telefonoParticular; }
            set
            {
                if (_telefonoParticular != value)
                {
                    _telefonoParticular = value;
                    OnPropertyChanged("TelefonoParticular");
                }
            }
        }
        private string _telefonoParticular;
    
        [DataMember]
        public string TelefonoCelular
        {
            get { return _telefonoCelular; }
            set
            {
                if (_telefonoCelular != value)
                {
                    _telefonoCelular = value;
                    OnPropertyChanged("TelefonoCelular");
                }
            }
        }
        private string _telefonoCelular;
    
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
        public Cliente Cliente
        {
            get { return _cliente; }
            set
            {
                if (!ReferenceEquals(_cliente, value))
                {
                    var previousValue = _cliente;
                    _cliente = value;
                    FixupCliente(previousValue);
                    OnNavigationPropertyChanged("Cliente");
                }
            }
        }
        private Cliente _cliente;

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
            Cliente = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupCliente(Cliente previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.ContactoCliente.Contains(this))
            {
                previousValue.ContactoCliente.Remove(this);
            }
    
            if (Cliente != null)
            {
                if (!Cliente.ContactoCliente.Contains(this))
                {
                    Cliente.ContactoCliente.Add(this);
                }
    
                ClienteID = Cliente.ClienteID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Cliente")
                    && (ChangeTracker.OriginalValues["Cliente"] == Cliente))
                {
                    ChangeTracker.OriginalValues.Remove("Cliente");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Cliente", previousValue);
                }
                if (Cliente != null && !Cliente.ChangeTracker.ChangeTrackingEnabled)
                {
                    Cliente.StartTracking();
                }
            }
        }

        #endregion

    }
}
