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
    [KnownType(typeof(TransferenciaSpool1))]
    [KnownType(typeof(Usuario))]
    [Serializable]
    public partial class Transferencia1: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int TransferenciaID
        {
            get { return _transferenciaID; }
            set
            {
                if (_transferenciaID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'TransferenciaID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _transferenciaID = value;
                    OnPropertyChanged("TransferenciaID");
                }
            }
        }
        private int _transferenciaID;
    
        [DataMember]
        public int TransferenciaSpoolID
        {
            get { return _transferenciaSpoolID; }
            set
            {
                if (_transferenciaSpoolID != value)
                {
                    ChangeTracker.RecordOriginalValue("TransferenciaSpoolID", _transferenciaSpoolID);
                    if (!IsDeserializing)
                    {
                        if (TransferenciaSpool != null && TransferenciaSpool.TransferenciaSpoolID != value)
                        {
                            TransferenciaSpool = null;
                        }
                    }
                    _transferenciaSpoolID = value;
                    OnPropertyChanged("TransferenciaSpoolID");
                }
            }
        }
        private int _transferenciaSpoolID;
    
        [DataMember]
        public string NumeroTransferencia
        {
            get { return _numeroTransferencia; }
            set
            {
                if (_numeroTransferencia != value)
                {
                    _numeroTransferencia = value;
                    OnPropertyChanged("NumeroTransferencia");
                }
            }
        }
        private string _numeroTransferencia;
    
        [DataMember]
        public System.DateTime FechaTransferencia
        {
            get { return _fechaTransferencia; }
            set
            {
                if (_fechaTransferencia != value)
                {
                    _fechaTransferencia = value;
                    OnPropertyChanged("FechaTransferencia");
                }
            }
        }
        private System.DateTime _fechaTransferencia;
    
        [DataMember]
        public Nullable<int> DestinoID
        {
            get { return _destinoID; }
            set
            {
                if (_destinoID != value)
                {
                    _destinoID = value;
                    OnPropertyChanged("DestinoID");
                }
            }
        }
        private Nullable<int> _destinoID;
    
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
        public TransferenciaSpool1 TransferenciaSpool
        {
            get { return _transferenciaSpool; }
            set
            {
                if (!ReferenceEquals(_transferenciaSpool, value))
                {
                    var previousValue = _transferenciaSpool;
                    _transferenciaSpool = value;
                    FixupTransferenciaSpool(previousValue);
                    OnNavigationPropertyChanged("TransferenciaSpool");
                }
            }
        }
        private TransferenciaSpool1 _transferenciaSpool;
    
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
    		}
    
    
        protected virtual void ClearNavigationProperties()
        {
            TransferenciaSpool = null;
            Usuario = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupTransferenciaSpool(TransferenciaSpool1 previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Transferencia.Contains(this))
            {
                previousValue.Transferencia.Remove(this);
            }
    
            if (TransferenciaSpool != null)
            {
                if (!TransferenciaSpool.Transferencia.Contains(this))
                {
                    TransferenciaSpool.Transferencia.Add(this);
                }
    
                TransferenciaSpoolID = TransferenciaSpool.TransferenciaSpoolID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("TransferenciaSpool")
                    && (ChangeTracker.OriginalValues["TransferenciaSpool"] == TransferenciaSpool))
                {
                    ChangeTracker.OriginalValues.Remove("TransferenciaSpool");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("TransferenciaSpool", previousValue);
                }
                if (TransferenciaSpool != null && !TransferenciaSpool.ChangeTracker.ChangeTrackingEnabled)
                {
                    TransferenciaSpool.StartTracking();
                }
            }
        }
    
        private void FixupUsuario(Usuario previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Transferencia1.Contains(this))
            {
                previousValue.Transferencia1.Remove(this);
            }
    
            if (Usuario != null)
            {
                if (!Usuario.Transferencia1.Contains(this))
                {
                    Usuario.Transferencia1.Add(this);
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

        #endregion

    }
}
