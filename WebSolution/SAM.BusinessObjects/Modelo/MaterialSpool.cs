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
    [KnownType(typeof(Spool))]
    [KnownType(typeof(CorteDetalle))]
    [KnownType(typeof(Despacho))]
    [KnownType(typeof(OrdenTrabajoMaterial))]
    [KnownType(typeof(CongeladoParcial))]
    [KnownType(typeof(ItemCode))]
    [Serializable]
    public partial class MaterialSpool: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
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
        public int ItemCodeID
        {
            get { return _itemCodeID; }
            set
            {
                if (_itemCodeID != value)
                {
                    ChangeTracker.RecordOriginalValue("ItemCodeID", _itemCodeID);
                    if (!IsDeserializing)
                    {
                        if (ItemCode != null && ItemCode.ItemCodeID != value)
                        {
                            ItemCode = null;
                        }
                    }
                    _itemCodeID = value;
                    OnPropertyChanged("ItemCodeID");
                }
            }
        }
        private int _itemCodeID;
    
        [DataMember]
        public decimal Diametro1
        {
            get { return _diametro1; }
            set
            {
                if (_diametro1 != value)
                {
                    _diametro1 = value;
                    OnPropertyChanged("Diametro1");
                }
            }
        }
        private decimal _diametro1;
    
        [DataMember]
        public decimal Diametro2
        {
            get { return _diametro2; }
            set
            {
                if (_diametro2 != value)
                {
                    _diametro2 = value;
                    OnPropertyChanged("Diametro2");
                }
            }
        }
        private decimal _diametro2;
    
        [DataMember]
        public string Etiqueta
        {
            get { return _etiqueta; }
            set
            {
                if (_etiqueta != value)
                {
                    _etiqueta = value;
                    OnPropertyChanged("Etiqueta");
                }
            }
        }
        private string _etiqueta;
    
        [DataMember]
        public int Cantidad
        {
            get { return _cantidad; }
            set
            {
                if (_cantidad != value)
                {
                    _cantidad = value;
                    OnPropertyChanged("Cantidad");
                }
            }
        }
        private int _cantidad;
    
        [DataMember]
        public string Especificacion
        {
            get { return _especificacion; }
            set
            {
                if (_especificacion != value)
                {
                    _especificacion = value;
                    OnPropertyChanged("Especificacion");
                }
            }
        }
        private string _especificacion;
    
        [DataMember]
        public string Grupo
        {
            get { return _grupo; }
            set
            {
                if (_grupo != value)
                {
                    _grupo = value;
                    OnPropertyChanged("Grupo");
                }
            }
        }
        private string _grupo;
    
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
        public int MaterialSpoolID
        {
            get { return _materialSpoolID; }
            set
            {
                if (_materialSpoolID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'MaterialSpoolID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _materialSpoolID = value;
                    OnPropertyChanged("MaterialSpoolID");
                }
            }
        }
        private int _materialSpoolID;
    
        [DataMember]
        public Nullable<decimal> Peso
        {
            get { return _peso; }
            set
            {
                if (_peso != value)
                {
                    _peso = value;
                    OnPropertyChanged("Peso");
                }
            }
        }
        private Nullable<decimal> _peso;
    
        [DataMember]
        public Nullable<decimal> Area
        {
            get { return _area; }
            set
            {
                if (_area != value)
                {
                    _area = value;
                    OnPropertyChanged("Area");
                }
            }
        }
        private Nullable<decimal> _area;
    
        [DataMember]
        public string DescripcionMaterial
        {
            get { return _descripcionMaterial; }
            set
            {
                if (_descripcionMaterial != value)
                {
                    _descripcionMaterial = value;
                    OnPropertyChanged("DescripcionMaterial");
                }
            }
        }
        private string _descripcionMaterial;
    
        [DataMember]
        public string Campo1
        {
            get { return _campo1; }
            set
            {
                if (_campo1 != value)
                {
                    _campo1 = value;
                    OnPropertyChanged("Campo1");
                }
            }
        }
        private string _campo1;
    
        [DataMember]
        public string Campo2
        {
            get { return _campo2; }
            set
            {
                if (_campo2 != value)
                {
                    _campo2 = value;
                    OnPropertyChanged("Campo2");
                }
            }
        }
        private string _campo2;
    
        [DataMember]
        public string Campo3
        {
            get { return _campo3; }
            set
            {
                if (_campo3 != value)
                {
                    _campo3 = value;
                    OnPropertyChanged("Campo3");
                }
            }
        }
        private string _campo3;
    
        [DataMember]
        public string Campo4
        {
            get { return _campo4; }
            set
            {
                if (_campo4 != value)
                {
                    _campo4 = value;
                    OnPropertyChanged("Campo4");
                }
            }
        }
        private string _campo4;
    
        [DataMember]
        public string Campo5
        {
            get { return _campo5; }
            set
            {
                if (_campo5 != value)
                {
                    _campo5 = value;
                    OnPropertyChanged("Campo5");
                }
            }
        }
        private string _campo5;

        #endregion

        #region Navigation Properties
    
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
        public TrackableCollection<Despacho> Despacho
        {
            get
            {
                if (_despacho == null)
                {
                    _despacho = new TrackableCollection<Despacho>();
                    _despacho.CollectionChanged += FixupDespacho;
                }
                return _despacho;
            }
            set
            {
                if (!ReferenceEquals(_despacho, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_despacho != null)
                    {
                        _despacho.CollectionChanged -= FixupDespacho;
                    }
                    _despacho = value;
                    if (_despacho != null)
                    {
                        _despacho.CollectionChanged += FixupDespacho;
                    }
                    OnNavigationPropertyChanged("Despacho");
                }
            }
        }
        private TrackableCollection<Despacho> _despacho;
    
        [DataMember]
        public TrackableCollection<OrdenTrabajoMaterial> OrdenTrabajoMaterial
        {
            get
            {
                if (_ordenTrabajoMaterial == null)
                {
                    _ordenTrabajoMaterial = new TrackableCollection<OrdenTrabajoMaterial>();
                    _ordenTrabajoMaterial.CollectionChanged += FixupOrdenTrabajoMaterial;
                }
                return _ordenTrabajoMaterial;
            }
            set
            {
                if (!ReferenceEquals(_ordenTrabajoMaterial, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_ordenTrabajoMaterial != null)
                    {
                        _ordenTrabajoMaterial.CollectionChanged -= FixupOrdenTrabajoMaterial;
                    }
                    _ordenTrabajoMaterial = value;
                    if (_ordenTrabajoMaterial != null)
                    {
                        _ordenTrabajoMaterial.CollectionChanged += FixupOrdenTrabajoMaterial;
                    }
                    OnNavigationPropertyChanged("OrdenTrabajoMaterial");
                }
            }
        }
        private TrackableCollection<OrdenTrabajoMaterial> _ordenTrabajoMaterial;
    
        [DataMember]
        public TrackableCollection<CongeladoParcial> CongeladoParcial
        {
            get
            {
                if (_congeladoParcial == null)
                {
                    _congeladoParcial = new TrackableCollection<CongeladoParcial>();
                    _congeladoParcial.CollectionChanged += FixupCongeladoParcial;
                }
                return _congeladoParcial;
            }
            set
            {
                if (!ReferenceEquals(_congeladoParcial, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_congeladoParcial != null)
                    {
                        _congeladoParcial.CollectionChanged -= FixupCongeladoParcial;
                    }
                    _congeladoParcial = value;
                    if (_congeladoParcial != null)
                    {
                        _congeladoParcial.CollectionChanged += FixupCongeladoParcial;
                    }
                    OnNavigationPropertyChanged("CongeladoParcial");
                }
            }
        }
        private TrackableCollection<CongeladoParcial> _congeladoParcial;
    
        [DataMember]
        public ItemCode ItemCode
        {
            get { return _itemCode; }
            set
            {
                if (!ReferenceEquals(_itemCode, value))
                {
                    var previousValue = _itemCode;
                    _itemCode = value;
                    FixupItemCode(previousValue);
                    OnNavigationPropertyChanged("ItemCode");
                }
            }
        }
        private ItemCode _itemCode;

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
    			if (_despacho != null)
    		{
    			_despacho.CollectionChanged -= FixupDespacho;
    			_despacho.CollectionChanged += FixupDespacho;
    			}
    			if (_ordenTrabajoMaterial != null)
    		{
    			_ordenTrabajoMaterial.CollectionChanged -= FixupOrdenTrabajoMaterial;
    			_ordenTrabajoMaterial.CollectionChanged += FixupOrdenTrabajoMaterial;
    			}
    			if (_congeladoParcial != null)
    		{
    			_congeladoParcial.CollectionChanged -= FixupCongeladoParcial;
    			_congeladoParcial.CollectionChanged += FixupCongeladoParcial;
    			}
    		}
    
    
        protected virtual void ClearNavigationProperties()
        {
            Spool = null;
            CorteDetalle.Clear();
            Despacho.Clear();
            OrdenTrabajoMaterial.Clear();
            CongeladoParcial.Clear();
            ItemCode = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupSpool(Spool previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.MaterialSpool.Contains(this))
            {
                previousValue.MaterialSpool.Remove(this);
            }
    
            if (Spool != null)
            {
                if (!Spool.MaterialSpool.Contains(this))
                {
                    Spool.MaterialSpool.Add(this);
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
    
        private void FixupItemCode(ItemCode previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.MaterialSpool.Contains(this))
            {
                previousValue.MaterialSpool.Remove(this);
            }
    
            if (ItemCode != null)
            {
                if (!ItemCode.MaterialSpool.Contains(this))
                {
                    ItemCode.MaterialSpool.Add(this);
                }
    
                ItemCodeID = ItemCode.ItemCodeID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("ItemCode")
                    && (ChangeTracker.OriginalValues["ItemCode"] == ItemCode))
                {
                    ChangeTracker.OriginalValues.Remove("ItemCode");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("ItemCode", previousValue);
                }
                if (ItemCode != null && !ItemCode.ChangeTracker.ChangeTrackingEnabled)
                {
                    ItemCode.StartTracking();
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
                    item.MaterialSpool = this;
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
                    if (ReferenceEquals(item.MaterialSpool, this))
                    {
                        item.MaterialSpool = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("CorteDetalle", item);
                    }
                }
            }
        }
    
        private void FixupDespacho(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (Despacho item in e.NewItems)
                {
                    item.MaterialSpool = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("Despacho", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Despacho item in e.OldItems)
                {
                    if (ReferenceEquals(item.MaterialSpool, this))
                    {
                        item.MaterialSpool = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("Despacho", item);
                    }
                }
            }
        }
    
        private void FixupOrdenTrabajoMaterial(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (OrdenTrabajoMaterial item in e.NewItems)
                {
                    item.MaterialSpool = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("OrdenTrabajoMaterial", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (OrdenTrabajoMaterial item in e.OldItems)
                {
                    if (ReferenceEquals(item.MaterialSpool, this))
                    {
                        item.MaterialSpool = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("OrdenTrabajoMaterial", item);
                    }
                }
            }
        }
    
        private void FixupCongeladoParcial(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (CongeladoParcial item in e.NewItems)
                {
                    item.MaterialSpool = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("CongeladoParcial", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (CongeladoParcial item in e.OldItems)
                {
                    if (ReferenceEquals(item.MaterialSpool, this))
                    {
                        item.MaterialSpool = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("CongeladoParcial", item);
                    }
                }
            }
        }

        #endregion

    }
}
