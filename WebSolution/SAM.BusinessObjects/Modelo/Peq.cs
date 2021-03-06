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
    [KnownType(typeof(FamiliaAcero))]
    [KnownType(typeof(TipoJunta))]
    [KnownType(typeof(Proyecto))]
    [Serializable]
    public partial class Peq: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int PeqID
        {
            get { return _peqID; }
            set
            {
                if (_peqID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'PeqID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _peqID = value;
                    OnPropertyChanged("PeqID");
                }
            }
        }
        private int _peqID;
    
        [DataMember]
        public int FamiliaAceroID
        {
            get { return _familiaAceroID; }
            set
            {
                if (_familiaAceroID != value)
                {
                    ChangeTracker.RecordOriginalValue("FamiliaAceroID", _familiaAceroID);
                    if (!IsDeserializing)
                    {
                        if (FamiliaAcero != null && FamiliaAcero.FamiliaAceroID != value)
                        {
                            FamiliaAcero = null;
                        }
                    }
                    _familiaAceroID = value;
                    OnPropertyChanged("FamiliaAceroID");
                }
            }
        }
        private int _familiaAceroID;
    
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
        public int TipoJuntaID
        {
            get { return _tipoJuntaID; }
            set
            {
                if (_tipoJuntaID != value)
                {
                    ChangeTracker.RecordOriginalValue("TipoJuntaID", _tipoJuntaID);
                    if (!IsDeserializing)
                    {
                        if (TipoJunta != null && TipoJunta.TipoJuntaID != value)
                        {
                            TipoJunta = null;
                        }
                    }
                    _tipoJuntaID = value;
                    OnPropertyChanged("TipoJuntaID");
                }
            }
        }
        private int _tipoJuntaID;
    
        [DataMember]
        public decimal Equivalencia
        {
            get { return _equivalencia; }
            set
            {
                if (_equivalencia != value)
                {
                    _equivalencia = value;
                    OnPropertyChanged("Equivalencia");
                }
            }
        }
        private decimal _equivalencia;
    
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
        public int ProyectoID
        {
            get { return _proyectoID; }
            set
            {
                if (_proyectoID != value)
                {
                    ChangeTracker.RecordOriginalValue("ProyectoID", _proyectoID);
                    if (!IsDeserializing)
                    {
                        if (Proyecto != null && Proyecto.ProyectoID != value)
                        {
                            Proyecto = null;
                        }
                    }
                    _proyectoID = value;
                    OnPropertyChanged("ProyectoID");
                }
            }
        }
        private int _proyectoID;

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
    
        [DataMember]
        public FamiliaAcero FamiliaAcero
        {
            get { return _familiaAcero; }
            set
            {
                if (!ReferenceEquals(_familiaAcero, value))
                {
                    var previousValue = _familiaAcero;
                    _familiaAcero = value;
                    FixupFamiliaAcero(previousValue);
                    OnNavigationPropertyChanged("FamiliaAcero");
                }
            }
        }
        private FamiliaAcero _familiaAcero;
    
        [DataMember]
        public TipoJunta TipoJunta
        {
            get { return _tipoJunta; }
            set
            {
                if (!ReferenceEquals(_tipoJunta, value))
                {
                    var previousValue = _tipoJunta;
                    _tipoJunta = value;
                    FixupTipoJunta(previousValue);
                    OnNavigationPropertyChanged("TipoJunta");
                }
            }
        }
        private TipoJunta _tipoJunta;
    
        [DataMember]
        public Proyecto Proyecto
        {
            get { return _proyecto; }
            set
            {
                if (!ReferenceEquals(_proyecto, value))
                {
                    var previousValue = _proyecto;
                    _proyecto = value;
                    FixupProyecto(previousValue);
                    OnNavigationPropertyChanged("Proyecto");
                }
            }
        }
        private Proyecto _proyecto;

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
            FamiliaAcero = null;
            TipoJunta = null;
            Proyecto = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupCedula(Cedula previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Peq.Contains(this))
            {
                previousValue.Peq.Remove(this);
            }
    
            if (Cedula != null)
            {
                if (!Cedula.Peq.Contains(this))
                {
                    Cedula.Peq.Add(this);
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
    
            if (previousValue != null && previousValue.Peq.Contains(this))
            {
                previousValue.Peq.Remove(this);
            }
    
            if (Diametro != null)
            {
                if (!Diametro.Peq.Contains(this))
                {
                    Diametro.Peq.Add(this);
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
    
        private void FixupFamiliaAcero(FamiliaAcero previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Peq.Contains(this))
            {
                previousValue.Peq.Remove(this);
            }
    
            if (FamiliaAcero != null)
            {
                if (!FamiliaAcero.Peq.Contains(this))
                {
                    FamiliaAcero.Peq.Add(this);
                }
    
                FamiliaAceroID = FamiliaAcero.FamiliaAceroID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("FamiliaAcero")
                    && (ChangeTracker.OriginalValues["FamiliaAcero"] == FamiliaAcero))
                {
                    ChangeTracker.OriginalValues.Remove("FamiliaAcero");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("FamiliaAcero", previousValue);
                }
                if (FamiliaAcero != null && !FamiliaAcero.ChangeTracker.ChangeTrackingEnabled)
                {
                    FamiliaAcero.StartTracking();
                }
            }
        }
    
        private void FixupTipoJunta(TipoJunta previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Peq.Contains(this))
            {
                previousValue.Peq.Remove(this);
            }
    
            if (TipoJunta != null)
            {
                if (!TipoJunta.Peq.Contains(this))
                {
                    TipoJunta.Peq.Add(this);
                }
    
                TipoJuntaID = TipoJunta.TipoJuntaID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("TipoJunta")
                    && (ChangeTracker.OriginalValues["TipoJunta"] == TipoJunta))
                {
                    ChangeTracker.OriginalValues.Remove("TipoJunta");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("TipoJunta", previousValue);
                }
                if (TipoJunta != null && !TipoJunta.ChangeTracker.ChangeTrackingEnabled)
                {
                    TipoJunta.StartTracking();
                }
            }
        }
    
        private void FixupProyecto(Proyecto previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Peq.Contains(this))
            {
                previousValue.Peq.Remove(this);
            }
    
            if (Proyecto != null)
            {
                if (!Proyecto.Peq.Contains(this))
                {
                    Proyecto.Peq.Add(this);
                }
    
                ProyectoID = Proyecto.ProyectoID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Proyecto")
                    && (ChangeTracker.OriginalValues["Proyecto"] == Proyecto))
                {
                    ChangeTracker.OriginalValues.Remove("Proyecto");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Proyecto", previousValue);
                }
                if (Proyecto != null && !Proyecto.ChangeTracker.ChangeTrackingEnabled)
                {
                    Proyecto.StartTracking();
                }
            }
        }

        #endregion

    }
}
