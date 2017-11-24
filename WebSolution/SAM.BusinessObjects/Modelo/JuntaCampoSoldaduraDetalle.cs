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
    [KnownType(typeof(Consumible))]
    [KnownType(typeof(JuntaCampoSoldadura))]
    [KnownType(typeof(Soldador))]
    [KnownType(typeof(TecnicaSoldador))]
    [Serializable]
    public partial class JuntaCampoSoldaduraDetalle: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int JuntaCampoSoldaduraDetalleID
        {
            get { return _juntaCampoSoldaduraDetalleID; }
            set
            {
                if (_juntaCampoSoldaduraDetalleID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'JuntaCampoSoldaduraDetalleID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _juntaCampoSoldaduraDetalleID = value;
                    OnPropertyChanged("JuntaCampoSoldaduraDetalleID");
                }
            }
        }
        private int _juntaCampoSoldaduraDetalleID;
    
        [DataMember]
        public int JuntaCampoSoldaduraID
        {
            get { return _juntaCampoSoldaduraID; }
            set
            {
                if (_juntaCampoSoldaduraID != value)
                {
                    ChangeTracker.RecordOriginalValue("JuntaCampoSoldaduraID", _juntaCampoSoldaduraID);
                    if (!IsDeserializing)
                    {
                        if (JuntaCampoSoldadura != null && JuntaCampoSoldadura.JuntaCampoSoldaduraID != value)
                        {
                            JuntaCampoSoldadura = null;
                        }
                    }
                    _juntaCampoSoldaduraID = value;
                    OnPropertyChanged("JuntaCampoSoldaduraID");
                }
            }
        }
        private int _juntaCampoSoldaduraID;
    
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
        public int ConsumibleID
        {
            get { return _consumibleID; }
            set
            {
                if (_consumibleID != value)
                {
                    ChangeTracker.RecordOriginalValue("ConsumibleID", _consumibleID);
                    if (!IsDeserializing)
                    {
                        if (Consumible != null && Consumible.ConsumibleID != value)
                        {
                            Consumible = null;
                        }
                    }
                    _consumibleID = value;
                    OnPropertyChanged("ConsumibleID");
                }
            }
        }
        private int _consumibleID;
    
        [DataMember]
        public Nullable<int> TecnicaSoldadorID
        {
            get { return _tecnicaSoldadorID; }
            set
            {
                if (_tecnicaSoldadorID != value)
                {
                    ChangeTracker.RecordOriginalValue("TecnicaSoldadorID", _tecnicaSoldadorID);
                    if (!IsDeserializing)
                    {
                        if (TecnicaSoldador != null && TecnicaSoldador.TecnicaSoldadorID != value)
                        {
                            TecnicaSoldador = null;
                        }
                    }
                    _tecnicaSoldadorID = value;
                    OnPropertyChanged("TecnicaSoldadorID");
                }
            }
        }
        private Nullable<int> _tecnicaSoldadorID;
    
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
        public Consumible Consumible
        {
            get { return _consumible; }
            set
            {
                if (!ReferenceEquals(_consumible, value))
                {
                    var previousValue = _consumible;
                    _consumible = value;
                    FixupConsumible(previousValue);
                    OnNavigationPropertyChanged("Consumible");
                }
            }
        }
        private Consumible _consumible;
    
        [DataMember]
        public JuntaCampoSoldadura JuntaCampoSoldadura
        {
            get { return _juntaCampoSoldadura; }
            set
            {
                if (!ReferenceEquals(_juntaCampoSoldadura, value))
                {
                    var previousValue = _juntaCampoSoldadura;
                    _juntaCampoSoldadura = value;
                    FixupJuntaCampoSoldadura(previousValue);
                    OnNavigationPropertyChanged("JuntaCampoSoldadura");
                }
            }
        }
        private JuntaCampoSoldadura _juntaCampoSoldadura;
    
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
        public TecnicaSoldador TecnicaSoldador
        {
            get { return _tecnicaSoldador; }
            set
            {
                if (!ReferenceEquals(_tecnicaSoldador, value))
                {
                    var previousValue = _tecnicaSoldador;
                    _tecnicaSoldador = value;
                    FixupTecnicaSoldador(previousValue);
                    OnNavigationPropertyChanged("TecnicaSoldador");
                }
            }
        }
        private TecnicaSoldador _tecnicaSoldador;

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
            Consumible = null;
            JuntaCampoSoldadura = null;
            Soldador = null;
            TecnicaSoldador = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupConsumible(Consumible previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.JuntaCampoSoldaduraDetalle.Contains(this))
            {
                previousValue.JuntaCampoSoldaduraDetalle.Remove(this);
            }
    
            if (Consumible != null)
            {
                if (!Consumible.JuntaCampoSoldaduraDetalle.Contains(this))
                {
                    Consumible.JuntaCampoSoldaduraDetalle.Add(this);
                }
    
                ConsumibleID = Consumible.ConsumibleID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Consumible")
                    && (ChangeTracker.OriginalValues["Consumible"] == Consumible))
                {
                    ChangeTracker.OriginalValues.Remove("Consumible");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Consumible", previousValue);
                }
                if (Consumible != null && !Consumible.ChangeTracker.ChangeTrackingEnabled)
                {
                    Consumible.StartTracking();
                }
            }
        }
    
        private void FixupJuntaCampoSoldadura(JuntaCampoSoldadura previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.JuntaCampoSoldaduraDetalle.Contains(this))
            {
                previousValue.JuntaCampoSoldaduraDetalle.Remove(this);
            }
    
            if (JuntaCampoSoldadura != null)
            {
                if (!JuntaCampoSoldadura.JuntaCampoSoldaduraDetalle.Contains(this))
                {
                    JuntaCampoSoldadura.JuntaCampoSoldaduraDetalle.Add(this);
                }
    
                JuntaCampoSoldaduraID = JuntaCampoSoldadura.JuntaCampoSoldaduraID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("JuntaCampoSoldadura")
                    && (ChangeTracker.OriginalValues["JuntaCampoSoldadura"] == JuntaCampoSoldadura))
                {
                    ChangeTracker.OriginalValues.Remove("JuntaCampoSoldadura");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("JuntaCampoSoldadura", previousValue);
                }
                if (JuntaCampoSoldadura != null && !JuntaCampoSoldadura.ChangeTracker.ChangeTrackingEnabled)
                {
                    JuntaCampoSoldadura.StartTracking();
                }
            }
        }
    
        private void FixupSoldador(Soldador previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.JuntaCampoSoldaduraDetalle.Contains(this))
            {
                previousValue.JuntaCampoSoldaduraDetalle.Remove(this);
            }
    
            if (Soldador != null)
            {
                if (!Soldador.JuntaCampoSoldaduraDetalle.Contains(this))
                {
                    Soldador.JuntaCampoSoldaduraDetalle.Add(this);
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
    
        private void FixupTecnicaSoldador(TecnicaSoldador previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.JuntaCampoSoldaduraDetalle.Contains(this))
            {
                previousValue.JuntaCampoSoldaduraDetalle.Remove(this);
            }
    
            if (TecnicaSoldador != null)
            {
                if (!TecnicaSoldador.JuntaCampoSoldaduraDetalle.Contains(this))
                {
                    TecnicaSoldador.JuntaCampoSoldaduraDetalle.Add(this);
                }
    
                TecnicaSoldadorID = TecnicaSoldador.TecnicaSoldadorID;
            }
            else if (!skipKeys)
            {
                TecnicaSoldadorID = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("TecnicaSoldador")
                    && (ChangeTracker.OriginalValues["TecnicaSoldador"] == TecnicaSoldador))
                {
                    ChangeTracker.OriginalValues.Remove("TecnicaSoldador");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("TecnicaSoldador", previousValue);
                }
                if (TecnicaSoldador != null && !TecnicaSoldador.ChangeTracker.ChangeTrackingEnabled)
                {
                    TecnicaSoldador.StartTracking();
                }
            }
        }

        #endregion

    }
}
