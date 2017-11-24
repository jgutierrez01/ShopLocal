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
    [KnownType(typeof(Cedula))]
    [KnownType(typeof(Diametro))]
    [Serializable]
    public partial class KgTeorico: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int DiametroID
        {
            get { return _diametroID; }
            set
            {
                if (_diametroID != value)
                {
                    ChangeTracker.RecordOriginalValue("DiametroID", _diametroID);
                    if (!IsDeserializing)
                    {
                        if (Diametro != null && Diametro.DiametroID != value)
                        {
                            Diametro = null;
                        }
                    }
                    _diametroID = value;
                    OnPropertyChanged("DiametroID");
                }
            }
        }
        private int _diametroID;
    
        [DataMember]
        public int CedulaID
        {
            get { return _cedulaID; }
            set
            {
                if (_cedulaID != value)
                {
                    ChangeTracker.RecordOriginalValue("CedulaID", _cedulaID);
                    if (!IsDeserializing)
                    {
                        if (Cedula != null && Cedula.CedulaID != value)
                        {
                            Cedula = null;
                        }
                    }
                    _cedulaID = value;
                    OnPropertyChanged("CedulaID");
                }
            }
        }
        private int _cedulaID;
    
        [DataMember]
        public decimal Valor
        {
            get { return _valor; }
            set
            {
                if (_valor != value)
                {
                    _valor = value;
                    OnPropertyChanged("Valor");
                }
            }
        }
        private decimal _valor;
    
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
        public int KgTeoricoID
        {
            get { return _kgTeoricoID; }
            set
            {
                if (_kgTeoricoID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'KgTeoricoID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _kgTeoricoID = value;
                    OnPropertyChanged("KgTeoricoID");
                }
            }
        }
        private int _kgTeoricoID;

        #endregion

        #region Navigation Properties
    
        [DataMember]
        public Cedula Cedula
        {
            get { return _cedula; }
            set
            {
                if (!ReferenceEquals(_cedula, value))
                {
                    var previousValue = _cedula;
                    _cedula = value;
                    FixupCedula(previousValue);
                    OnNavigationPropertyChanged("Cedula");
                }
            }
        }
        private Cedula _cedula;
    
        [DataMember]
        public Diametro Diametro
        {
            get { return _diametro; }
            set
            {
                if (!ReferenceEquals(_diametro, value))
                {
                    var previousValue = _diametro;
                    _diametro = value;
                    FixupDiametro(previousValue);
                    OnNavigationPropertyChanged("Diametro");
                }
            }
        }
        private Diametro _diametro;

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
            Cedula = null;
            Diametro = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupCedula(Cedula previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.KgTeorico.Contains(this))
            {
                previousValue.KgTeorico.Remove(this);
            }
    
            if (Cedula != null)
            {
                if (!Cedula.KgTeorico.Contains(this))
                {
                    Cedula.KgTeorico.Add(this);
                }
    
                CedulaID = Cedula.CedulaID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Cedula")
                    && (ChangeTracker.OriginalValues["Cedula"] == Cedula))
                {
                    ChangeTracker.OriginalValues.Remove("Cedula");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Cedula", previousValue);
                }
                if (Cedula != null && !Cedula.ChangeTracker.ChangeTrackingEnabled)
                {
                    Cedula.StartTracking();
                }
            }
        }
    
        private void FixupDiametro(Diametro previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.KgTeorico.Contains(this))
            {
                previousValue.KgTeorico.Remove(this);
            }
    
            if (Diametro != null)
            {
                if (!Diametro.KgTeorico.Contains(this))
                {
                    Diametro.KgTeorico.Add(this);
                }
    
                DiametroID = Diametro.DiametroID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Diametro")
                    && (ChangeTracker.OriginalValues["Diametro"] == Diametro))
                {
                    ChangeTracker.OriginalValues.Remove("Diametro");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Diametro", previousValue);
                }
                if (Diametro != null && !Diametro.ChangeTracker.ChangeTrackingEnabled)
                {
                    Diametro.StartTracking();
                }
            }
        }

        #endregion

    }
}
