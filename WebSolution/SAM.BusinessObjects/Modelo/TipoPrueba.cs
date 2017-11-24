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
    [KnownType(typeof(Defecto))]
    [KnownType(typeof(ReportePnd))]
    [KnownType(typeof(ReporteTt))]
    [KnownType(typeof(Requisicion))]
    [KnownType(typeof(ReporteCampoPND))]
    [KnownType(typeof(ReporteCampoTT))]
    [KnownType(typeof(RequisicionCampo))]
    [Serializable]
    public partial class TipoPrueba: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int TipoPruebaID
        {
            get { return _tipoPruebaID; }
            set
            {
                if (_tipoPruebaID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'TipoPruebaID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _tipoPruebaID = value;
                    OnPropertyChanged("TipoPruebaID");
                }
            }
        }
        private int _tipoPruebaID;
    
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
    
        [DataMember]
        public string Categoria
        {
            get { return _categoria; }
            set
            {
                if (_categoria != value)
                {
                    _categoria = value;
                    OnPropertyChanged("Categoria");
                }
            }
        }
        private string _categoria;

        #endregion

        #region Navigation Properties
    
        [DataMember]
        public TrackableCollection<Defecto> Defecto
        {
            get
            {
                if (_defecto == null)
                {
                    _defecto = new TrackableCollection<Defecto>();
                    _defecto.CollectionChanged += FixupDefecto;
                }
                return _defecto;
            }
            set
            {
                if (!ReferenceEquals(_defecto, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_defecto != null)
                    {
                        _defecto.CollectionChanged -= FixupDefecto;
                    }
                    _defecto = value;
                    if (_defecto != null)
                    {
                        _defecto.CollectionChanged += FixupDefecto;
                    }
                    OnNavigationPropertyChanged("Defecto");
                }
            }
        }
        private TrackableCollection<Defecto> _defecto;
    
        [DataMember]
        public TrackableCollection<ReportePnd> ReportePnd
        {
            get
            {
                if (_reportePnd == null)
                {
                    _reportePnd = new TrackableCollection<ReportePnd>();
                    _reportePnd.CollectionChanged += FixupReportePnd;
                }
                return _reportePnd;
            }
            set
            {
                if (!ReferenceEquals(_reportePnd, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_reportePnd != null)
                    {
                        _reportePnd.CollectionChanged -= FixupReportePnd;
                    }
                    _reportePnd = value;
                    if (_reportePnd != null)
                    {
                        _reportePnd.CollectionChanged += FixupReportePnd;
                    }
                    OnNavigationPropertyChanged("ReportePnd");
                }
            }
        }
        private TrackableCollection<ReportePnd> _reportePnd;
    
        [DataMember]
        public TrackableCollection<ReporteTt> ReporteTt
        {
            get
            {
                if (_reporteTt == null)
                {
                    _reporteTt = new TrackableCollection<ReporteTt>();
                    _reporteTt.CollectionChanged += FixupReporteTt;
                }
                return _reporteTt;
            }
            set
            {
                if (!ReferenceEquals(_reporteTt, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_reporteTt != null)
                    {
                        _reporteTt.CollectionChanged -= FixupReporteTt;
                    }
                    _reporteTt = value;
                    if (_reporteTt != null)
                    {
                        _reporteTt.CollectionChanged += FixupReporteTt;
                    }
                    OnNavigationPropertyChanged("ReporteTt");
                }
            }
        }
        private TrackableCollection<ReporteTt> _reporteTt;
    
        [DataMember]
        public TrackableCollection<Requisicion> Requisicion
        {
            get
            {
                if (_requisicion == null)
                {
                    _requisicion = new TrackableCollection<Requisicion>();
                    _requisicion.CollectionChanged += FixupRequisicion;
                }
                return _requisicion;
            }
            set
            {
                if (!ReferenceEquals(_requisicion, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_requisicion != null)
                    {
                        _requisicion.CollectionChanged -= FixupRequisicion;
                    }
                    _requisicion = value;
                    if (_requisicion != null)
                    {
                        _requisicion.CollectionChanged += FixupRequisicion;
                    }
                    OnNavigationPropertyChanged("Requisicion");
                }
            }
        }
        private TrackableCollection<Requisicion> _requisicion;
    
        [DataMember]
        public TrackableCollection<ReporteCampoPND> ReporteCampoPND
        {
            get
            {
                if (_reporteCampoPND == null)
                {
                    _reporteCampoPND = new TrackableCollection<ReporteCampoPND>();
                    _reporteCampoPND.CollectionChanged += FixupReporteCampoPND;
                }
                return _reporteCampoPND;
            }
            set
            {
                if (!ReferenceEquals(_reporteCampoPND, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_reporteCampoPND != null)
                    {
                        _reporteCampoPND.CollectionChanged -= FixupReporteCampoPND;
                    }
                    _reporteCampoPND = value;
                    if (_reporteCampoPND != null)
                    {
                        _reporteCampoPND.CollectionChanged += FixupReporteCampoPND;
                    }
                    OnNavigationPropertyChanged("ReporteCampoPND");
                }
            }
        }
        private TrackableCollection<ReporteCampoPND> _reporteCampoPND;
    
        [DataMember]
        public TrackableCollection<ReporteCampoTT> ReporteCampoTT
        {
            get
            {
                if (_reporteCampoTT == null)
                {
                    _reporteCampoTT = new TrackableCollection<ReporteCampoTT>();
                    _reporteCampoTT.CollectionChanged += FixupReporteCampoTT;
                }
                return _reporteCampoTT;
            }
            set
            {
                if (!ReferenceEquals(_reporteCampoTT, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_reporteCampoTT != null)
                    {
                        _reporteCampoTT.CollectionChanged -= FixupReporteCampoTT;
                    }
                    _reporteCampoTT = value;
                    if (_reporteCampoTT != null)
                    {
                        _reporteCampoTT.CollectionChanged += FixupReporteCampoTT;
                    }
                    OnNavigationPropertyChanged("ReporteCampoTT");
                }
            }
        }
        private TrackableCollection<ReporteCampoTT> _reporteCampoTT;
    
        [DataMember]
        public TrackableCollection<RequisicionCampo> RequisicionCampo
        {
            get
            {
                if (_requisicionCampo == null)
                {
                    _requisicionCampo = new TrackableCollection<RequisicionCampo>();
                    _requisicionCampo.CollectionChanged += FixupRequisicionCampo;
                }
                return _requisicionCampo;
            }
            set
            {
                if (!ReferenceEquals(_requisicionCampo, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_requisicionCampo != null)
                    {
                        _requisicionCampo.CollectionChanged -= FixupRequisicionCampo;
                    }
                    _requisicionCampo = value;
                    if (_requisicionCampo != null)
                    {
                        _requisicionCampo.CollectionChanged += FixupRequisicionCampo;
                    }
                    OnNavigationPropertyChanged("RequisicionCampo");
                }
            }
        }
        private TrackableCollection<RequisicionCampo> _requisicionCampo;

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
    			if (_defecto != null)
    		{
    			_defecto.CollectionChanged -= FixupDefecto;
    			_defecto.CollectionChanged += FixupDefecto;
    			}
    			if (_reportePnd != null)
    		{
    			_reportePnd.CollectionChanged -= FixupReportePnd;
    			_reportePnd.CollectionChanged += FixupReportePnd;
    			}
    			if (_reporteTt != null)
    		{
    			_reporteTt.CollectionChanged -= FixupReporteTt;
    			_reporteTt.CollectionChanged += FixupReporteTt;
    			}
    			if (_requisicion != null)
    		{
    			_requisicion.CollectionChanged -= FixupRequisicion;
    			_requisicion.CollectionChanged += FixupRequisicion;
    			}
    			if (_reporteCampoPND != null)
    		{
    			_reporteCampoPND.CollectionChanged -= FixupReporteCampoPND;
    			_reporteCampoPND.CollectionChanged += FixupReporteCampoPND;
    			}
    			if (_reporteCampoTT != null)
    		{
    			_reporteCampoTT.CollectionChanged -= FixupReporteCampoTT;
    			_reporteCampoTT.CollectionChanged += FixupReporteCampoTT;
    			}
    			if (_requisicionCampo != null)
    		{
    			_requisicionCampo.CollectionChanged -= FixupRequisicionCampo;
    			_requisicionCampo.CollectionChanged += FixupRequisicionCampo;
    			}
    		}
    
    
        protected virtual void ClearNavigationProperties()
        {
            Defecto.Clear();
            ReportePnd.Clear();
            ReporteTt.Clear();
            Requisicion.Clear();
            ReporteCampoPND.Clear();
            ReporteCampoTT.Clear();
            RequisicionCampo.Clear();
        }

        #endregion

        #region Association Fixup
    
        private void FixupDefecto(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (Defecto item in e.NewItems)
                {
                    item.TipoPrueba = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("Defecto", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Defecto item in e.OldItems)
                {
                    if (ReferenceEquals(item.TipoPrueba, this))
                    {
                        item.TipoPrueba = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("Defecto", item);
                    }
                }
            }
        }
    
        private void FixupReportePnd(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (ReportePnd item in e.NewItems)
                {
                    item.TipoPrueba = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("ReportePnd", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (ReportePnd item in e.OldItems)
                {
                    if (ReferenceEquals(item.TipoPrueba, this))
                    {
                        item.TipoPrueba = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("ReportePnd", item);
                    }
                }
            }
        }
    
        private void FixupReporteTt(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (ReporteTt item in e.NewItems)
                {
                    item.TipoPrueba = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("ReporteTt", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (ReporteTt item in e.OldItems)
                {
                    if (ReferenceEquals(item.TipoPrueba, this))
                    {
                        item.TipoPrueba = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("ReporteTt", item);
                    }
                }
            }
        }
    
        private void FixupRequisicion(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (Requisicion item in e.NewItems)
                {
                    item.TipoPrueba = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("Requisicion", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Requisicion item in e.OldItems)
                {
                    if (ReferenceEquals(item.TipoPrueba, this))
                    {
                        item.TipoPrueba = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("Requisicion", item);
                    }
                }
            }
        }
    
        private void FixupReporteCampoPND(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (ReporteCampoPND item in e.NewItems)
                {
                    item.TipoPrueba = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("ReporteCampoPND", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (ReporteCampoPND item in e.OldItems)
                {
                    if (ReferenceEquals(item.TipoPrueba, this))
                    {
                        item.TipoPrueba = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("ReporteCampoPND", item);
                    }
                }
            }
        }
    
        private void FixupReporteCampoTT(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (ReporteCampoTT item in e.NewItems)
                {
                    item.TipoPrueba = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("ReporteCampoTT", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (ReporteCampoTT item in e.OldItems)
                {
                    if (ReferenceEquals(item.TipoPrueba, this))
                    {
                        item.TipoPrueba = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("ReporteCampoTT", item);
                    }
                }
            }
        }
    
        private void FixupRequisicionCampo(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (RequisicionCampo item in e.NewItems)
                {
                    item.TipoPrueba = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("RequisicionCampo", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (RequisicionCampo item in e.OldItems)
                {
                    if (ReferenceEquals(item.TipoPrueba, this))
                    {
                        item.TipoPrueba = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("RequisicionCampo", item);
                    }
                }
            }
        }

        #endregion

    }
}
