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
    [KnownType(typeof(Proyecto))]
    [KnownType(typeof(CorteDetalle))]
    [KnownType(typeof(NumeroUnicoMovimiento))]
    [KnownType(typeof(NumeroUnicoCorte))]
    [Serializable]
    public partial class Corte: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int CorteID
        {
            get { return _corteID; }
            set
            {
                if (_corteID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'CorteID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _corteID = value;
                    OnPropertyChanged("CorteID");
                }
            }
        }
        private int _corteID;
    
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
    
        [DataMember]
        public Nullable<int> Sobrante
        {
            get { return _sobrante; }
            set
            {
                if (_sobrante != value)
                {
                    _sobrante = value;
                    OnPropertyChanged("Sobrante");
                }
            }
        }
        private Nullable<int> _sobrante;
    
        [DataMember]
        public Nullable<int> Merma
        {
            get { return _merma; }
            set
            {
                if (_merma != value)
                {
                    _merma = value;
                    OnPropertyChanged("Merma");
                }
            }
        }
        private Nullable<int> _merma;
    
        [DataMember]
        public bool Cancelado
        {
            get { return _cancelado; }
            set
            {
                if (_cancelado != value)
                {
                    _cancelado = value;
                    OnPropertyChanged("Cancelado");
                }
            }
        }
        private bool _cancelado;
    
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
        public Nullable<int> MermaMovimientoID
        {
            get { return _mermaMovimientoID; }
            set
            {
                if (_mermaMovimientoID != value)
                {
                    ChangeTracker.RecordOriginalValue("MermaMovimientoID", _mermaMovimientoID);
                    if (!IsDeserializing)
                    {
                        if (MovimientoMerma != null && MovimientoMerma.NumeroUnicoMovimientoID != value)
                        {
                            MovimientoMerma = null;
                        }
                    }
                    _mermaMovimientoID = value;
                    OnPropertyChanged("MermaMovimientoID");
                }
            }
        }
        private Nullable<int> _mermaMovimientoID;
    
        [DataMember]
        public int NumeroUnicoCorteID
        {
            get { return _numeroUnicoCorteID; }
            set
            {
                if (_numeroUnicoCorteID != value)
                {
                    ChangeTracker.RecordOriginalValue("NumeroUnicoCorteID", _numeroUnicoCorteID);
                    if (!IsDeserializing)
                    {
                        if (NumeroUnicoCorte != null && NumeroUnicoCorte.NumeroUnicoCorteID != value)
                        {
                            NumeroUnicoCorte = null;
                        }
                    }
                    _numeroUnicoCorteID = value;
                    OnPropertyChanged("NumeroUnicoCorteID");
                }
            }
        }
        private int _numeroUnicoCorteID;
    
        [DataMember]
        public int Rack
        {
            get { return _rack; }
            set
            {
                if (_rack != value)
                {
                    _rack = value;
                    OnPropertyChanged("Rack");
                }
            }
        }
        private int _rack;
    
        [DataMember]
        public Nullable<int> PreparacionCorteMovimientoID
        {
            get { return _preparacionCorteMovimientoID; }
            set
            {
                if (_preparacionCorteMovimientoID != value)
                {
                    ChangeTracker.RecordOriginalValue("PreparacionCorteMovimientoID", _preparacionCorteMovimientoID);
                    if (!IsDeserializing)
                    {
                        if (PreparacionCorte != null && PreparacionCorte.NumeroUnicoMovimientoID != value)
                        {
                            PreparacionCorte = null;
                        }
                    }
                    _preparacionCorteMovimientoID = value;
                    OnPropertyChanged("PreparacionCorteMovimientoID");
                }
            }
        }
        private Nullable<int> _preparacionCorteMovimientoID;
    
        [DataMember]
        public Nullable<int> CortadorID
        {
            get { return _cortadorID; }
            set
            {
                if (_cortadorID != value)
                {
                    _cortadorID = value;
                    OnPropertyChanged("CortadorID");
                }
            }
        }
        private Nullable<int> _cortadorID;

        #endregion

        #region Navigation Properties
    
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
    
        [DataMember]
        public TrackableCollection<CorteDetalle> CorteDetalle
        {
            get
            {
                if (_corteDetalle == null)
                {
                    _corteDetalle = new TrackableCollection<CorteDetalle>();
                    _corteDetalle.CollectionChanged += FixupCorteDetalle;
                }
                return _corteDetalle;
            }
            set
            {
                if (!ReferenceEquals(_corteDetalle, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_corteDetalle != null)
                    {
                        _corteDetalle.CollectionChanged -= FixupCorteDetalle;
                    }
                    _corteDetalle = value;
                    if (_corteDetalle != null)
                    {
                        _corteDetalle.CollectionChanged += FixupCorteDetalle;
                    }
                    OnNavigationPropertyChanged("CorteDetalle");
                }
            }
        }
        private TrackableCollection<CorteDetalle> _corteDetalle;
    
        [DataMember]
        public NumeroUnicoMovimiento MovimientoMerma
        {
            get { return _movimientoMerma; }
            set
            {
                if (!ReferenceEquals(_movimientoMerma, value))
                {
                    var previousValue = _movimientoMerma;
                    _movimientoMerma = value;
                    FixupMovimientoMerma(previousValue);
                    OnNavigationPropertyChanged("MovimientoMerma");
                }
            }
        }
        private NumeroUnicoMovimiento _movimientoMerma;
    
        [DataMember]
        public NumeroUnicoCorte NumeroUnicoCorte
        {
            get { return _numeroUnicoCorte; }
            set
            {
                if (!ReferenceEquals(_numeroUnicoCorte, value))
                {
                    var previousValue = _numeroUnicoCorte;
                    _numeroUnicoCorte = value;
                    FixupNumeroUnicoCorte(previousValue);
                    OnNavigationPropertyChanged("NumeroUnicoCorte");
                }
            }
        }
        private NumeroUnicoCorte _numeroUnicoCorte;
    
        [DataMember]
        public NumeroUnicoMovimiento PreparacionCorte
        {
            get { return _preparacionCorte; }
            set
            {
                if (!ReferenceEquals(_preparacionCorte, value))
                {
                    var previousValue = _preparacionCorte;
                    _preparacionCorte = value;
                    FixupPreparacionCorte(previousValue);
                    OnNavigationPropertyChanged("PreparacionCorte");
                }
            }
        }
        private NumeroUnicoMovimiento _preparacionCorte;

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
    			if (_corteDetalle != null)
    		{
    			_corteDetalle.CollectionChanged -= FixupCorteDetalle;
    			_corteDetalle.CollectionChanged += FixupCorteDetalle;
    			}
    		}
    
    
        protected virtual void ClearNavigationProperties()
        {
            Proyecto = null;
            CorteDetalle.Clear();
            MovimientoMerma = null;
            NumeroUnicoCorte = null;
            PreparacionCorte = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupProyecto(Proyecto previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Corte.Contains(this))
            {
                previousValue.Corte.Remove(this);
            }
    
            if (Proyecto != null)
            {
                if (!Proyecto.Corte.Contains(this))
                {
                    Proyecto.Corte.Add(this);
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
    
        private void FixupMovimientoMerma(NumeroUnicoMovimiento previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.MermasCorte.Contains(this))
            {
                previousValue.MermasCorte.Remove(this);
            }
    
            if (MovimientoMerma != null)
            {
                if (!MovimientoMerma.MermasCorte.Contains(this))
                {
                    MovimientoMerma.MermasCorte.Add(this);
                }
    
                MermaMovimientoID = MovimientoMerma.NumeroUnicoMovimientoID;
            }
            else if (!skipKeys)
            {
                MermaMovimientoID = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("MovimientoMerma")
                    && (ChangeTracker.OriginalValues["MovimientoMerma"] == MovimientoMerma))
                {
                    ChangeTracker.OriginalValues.Remove("MovimientoMerma");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("MovimientoMerma", previousValue);
                }
                if (MovimientoMerma != null && !MovimientoMerma.ChangeTracker.ChangeTrackingEnabled)
                {
                    MovimientoMerma.StartTracking();
                }
            }
        }
    
        private void FixupNumeroUnicoCorte(NumeroUnicoCorte previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Corte.Contains(this))
            {
                previousValue.Corte.Remove(this);
            }
    
            if (NumeroUnicoCorte != null)
            {
                if (!NumeroUnicoCorte.Corte.Contains(this))
                {
                    NumeroUnicoCorte.Corte.Add(this);
                }
    
                NumeroUnicoCorteID = NumeroUnicoCorte.NumeroUnicoCorteID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("NumeroUnicoCorte")
                    && (ChangeTracker.OriginalValues["NumeroUnicoCorte"] == NumeroUnicoCorte))
                {
                    ChangeTracker.OriginalValues.Remove("NumeroUnicoCorte");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("NumeroUnicoCorte", previousValue);
                }
                if (NumeroUnicoCorte != null && !NumeroUnicoCorte.ChangeTracker.ChangeTrackingEnabled)
                {
                    NumeroUnicoCorte.StartTracking();
                }
            }
        }
    
        private void FixupPreparacionCorte(NumeroUnicoMovimiento previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Corte1.Contains(this))
            {
                previousValue.Corte1.Remove(this);
            }
    
            if (PreparacionCorte != null)
            {
                if (!PreparacionCorte.Corte1.Contains(this))
                {
                    PreparacionCorte.Corte1.Add(this);
                }
    
                PreparacionCorteMovimientoID = PreparacionCorte.NumeroUnicoMovimientoID;
            }
            else if (!skipKeys)
            {
                PreparacionCorteMovimientoID = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("PreparacionCorte")
                    && (ChangeTracker.OriginalValues["PreparacionCorte"] == PreparacionCorte))
                {
                    ChangeTracker.OriginalValues.Remove("PreparacionCorte");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("PreparacionCorte", previousValue);
                }
                if (PreparacionCorte != null && !PreparacionCorte.ChangeTracker.ChangeTrackingEnabled)
                {
                    PreparacionCorte.StartTracking();
                }
            }
        }
    
        private void FixupCorteDetalle(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (CorteDetalle item in e.NewItems)
                {
                    item.Corte = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("CorteDetalle", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (CorteDetalle item in e.OldItems)
                {
                    if (ReferenceEquals(item.Corte, this))
                    {
                        item.Corte = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("CorteDetalle", item);
                    }
                }
            }
        }

        #endregion

    }
}
