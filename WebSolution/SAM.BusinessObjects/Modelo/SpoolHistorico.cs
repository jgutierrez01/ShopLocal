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
    [KnownType(typeof(CorteSpoolHistorico))]
    [KnownType(typeof(JuntaSpoolHistorico))]
    [KnownType(typeof(MaterialSpoolHistorico))]
    [Serializable]
    public partial class SpoolHistorico: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int SpoolHistoricoID
        {
            get { return _spoolHistoricoID; }
            set
            {
                if (_spoolHistoricoID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'SpoolHistoricoID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _spoolHistoricoID = value;
                    OnPropertyChanged("SpoolHistoricoID");
                }
            }
        }
        private int _spoolHistoricoID;
    
        [DataMember]
        public int SpoolID
        {
            get { return _spoolID; }
            set
            {
                if (_spoolID != value)
                {
                    _spoolID = value;
                    OnPropertyChanged("SpoolID");
                }
            }
        }
        private int _spoolID;
    
        [DataMember]
        public int ProyectoID
        {
            get { return _proyectoID; }
            set
            {
                if (_proyectoID != value)
                {
                    _proyectoID = value;
                    OnPropertyChanged("ProyectoID");
                }
            }
        }
        private int _proyectoID;
    
        [DataMember]
        public int FamiliaAcero1ID
        {
            get { return _familiaAcero1ID; }
            set
            {
                if (_familiaAcero1ID != value)
                {
                    _familiaAcero1ID = value;
                    OnPropertyChanged("FamiliaAcero1ID");
                }
            }
        }
        private int _familiaAcero1ID;
    
        [DataMember]
        public Nullable<int> FamiliaAcero2ID
        {
            get { return _familiaAcero2ID; }
            set
            {
                if (_familiaAcero2ID != value)
                {
                    _familiaAcero2ID = value;
                    OnPropertyChanged("FamiliaAcero2ID");
                }
            }
        }
        private Nullable<int> _familiaAcero2ID;
    
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
        public string Dibujo
        {
            get { return _dibujo; }
            set
            {
                if (_dibujo != value)
                {
                    _dibujo = value;
                    OnPropertyChanged("Dibujo");
                }
            }
        }
        private string _dibujo;
    
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
        public string Cedula
        {
            get { return _cedula; }
            set
            {
                if (_cedula != value)
                {
                    _cedula = value;
                    OnPropertyChanged("Cedula");
                }
            }
        }
        private string _cedula;
    
        [DataMember]
        public Nullable<decimal> Pdis
        {
            get { return _pdis; }
            set
            {
                if (_pdis != value)
                {
                    _pdis = value;
                    OnPropertyChanged("Pdis");
                }
            }
        }
        private Nullable<decimal> _pdis;
    
        [DataMember]
        public Nullable<decimal> DiametroPlano
        {
            get { return _diametroPlano; }
            set
            {
                if (_diametroPlano != value)
                {
                    _diametroPlano = value;
                    OnPropertyChanged("DiametroPlano");
                }
            }
        }
        private Nullable<decimal> _diametroPlano;
    
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
        public Nullable<int> PorcentajePnd
        {
            get { return _porcentajePnd; }
            set
            {
                if (_porcentajePnd != value)
                {
                    _porcentajePnd = value;
                    OnPropertyChanged("PorcentajePnd");
                }
            }
        }
        private Nullable<int> _porcentajePnd;
    
        [DataMember]
        public bool RequierePwht
        {
            get { return _requierePwht; }
            set
            {
                if (_requierePwht != value)
                {
                    _requierePwht = value;
                    OnPropertyChanged("RequierePwht");
                }
            }
        }
        private bool _requierePwht;
    
        [DataMember]
        public bool PendienteDocumental
        {
            get { return _pendienteDocumental; }
            set
            {
                if (_pendienteDocumental != value)
                {
                    _pendienteDocumental = value;
                    OnPropertyChanged("PendienteDocumental");
                }
            }
        }
        private bool _pendienteDocumental;
    
        [DataMember]
        public bool AprobadoParaCruce
        {
            get { return _aprobadoParaCruce; }
            set
            {
                if (_aprobadoParaCruce != value)
                {
                    _aprobadoParaCruce = value;
                    OnPropertyChanged("AprobadoParaCruce");
                }
            }
        }
        private bool _aprobadoParaCruce;
    
        [DataMember]
        public Nullable<int> Prioridad
        {
            get { return _prioridad; }
            set
            {
                if (_prioridad != value)
                {
                    _prioridad = value;
                    OnPropertyChanged("Prioridad");
                }
            }
        }
        private Nullable<int> _prioridad;
    
        [DataMember]
        public string Revision
        {
            get { return _revision; }
            set
            {
                if (_revision != value)
                {
                    _revision = value;
                    OnPropertyChanged("Revision");
                }
            }
        }
        private string _revision;
    
        [DataMember]
        public string RevisionCliente
        {
            get { return _revisionCliente; }
            set
            {
                if (_revisionCliente != value)
                {
                    _revisionCliente = value;
                    OnPropertyChanged("RevisionCliente");
                }
            }
        }
        private string _revisionCliente;
    
        [DataMember]
        public string Segmento1
        {
            get { return _segmento1; }
            set
            {
                if (_segmento1 != value)
                {
                    _segmento1 = value;
                    OnPropertyChanged("Segmento1");
                }
            }
        }
        private string _segmento1;
    
        [DataMember]
        public string Segmento2
        {
            get { return _segmento2; }
            set
            {
                if (_segmento2 != value)
                {
                    _segmento2 = value;
                    OnPropertyChanged("Segmento2");
                }
            }
        }
        private string _segmento2;
    
        [DataMember]
        public string Segmento3
        {
            get { return _segmento3; }
            set
            {
                if (_segmento3 != value)
                {
                    _segmento3 = value;
                    OnPropertyChanged("Segmento3");
                }
            }
        }
        private string _segmento3;
    
        [DataMember]
        public string Segmento4
        {
            get { return _segmento4; }
            set
            {
                if (_segmento4 != value)
                {
                    _segmento4 = value;
                    OnPropertyChanged("Segmento4");
                }
            }
        }
        private string _segmento4;
    
        [DataMember]
        public string Segmento5
        {
            get { return _segmento5; }
            set
            {
                if (_segmento5 != value)
                {
                    _segmento5 = value;
                    OnPropertyChanged("Segmento5");
                }
            }
        }
        private string _segmento5;
    
        [DataMember]
        public string Segmento6
        {
            get { return _segmento6; }
            set
            {
                if (_segmento6 != value)
                {
                    _segmento6 = value;
                    OnPropertyChanged("Segmento6");
                }
            }
        }
        private string _segmento6;
    
        [DataMember]
        public string Segmento7
        {
            get { return _segmento7; }
            set
            {
                if (_segmento7 != value)
                {
                    _segmento7 = value;
                    OnPropertyChanged("Segmento7");
                }
            }
        }
        private string _segmento7;
    
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
        public string SistemaPintura
        {
            get { return _sistemaPintura; }
            set
            {
                if (_sistemaPintura != value)
                {
                    _sistemaPintura = value;
                    OnPropertyChanged("SistemaPintura");
                }
            }
        }
        private string _sistemaPintura;
    
        [DataMember]
        public string ColorPintura
        {
            get { return _colorPintura; }
            set
            {
                if (_colorPintura != value)
                {
                    _colorPintura = value;
                    OnPropertyChanged("ColorPintura");
                }
            }
        }
        private string _colorPintura;
    
        [DataMember]
        public string CodigoPintura
        {
            get { return _codigoPintura; }
            set
            {
                if (_codigoPintura != value)
                {
                    _codigoPintura = value;
                    OnPropertyChanged("CodigoPintura");
                }
            }
        }
        private string _codigoPintura;
    
        [DataMember]
        public string ProyectoNombre
        {
            get { return _proyectoNombre; }
            set
            {
                if (_proyectoNombre != value)
                {
                    _proyectoNombre = value;
                    OnPropertyChanged("ProyectoNombre");
                }
            }
        }
        private string _proyectoNombre;
    
        [DataMember]
        public string FamiliaAcero1Nombre
        {
            get { return _familiaAcero1Nombre; }
            set
            {
                if (_familiaAcero1Nombre != value)
                {
                    _familiaAcero1Nombre = value;
                    OnPropertyChanged("FamiliaAcero1Nombre");
                }
            }
        }
        private string _familiaAcero1Nombre;
    
        [DataMember]
        public string FamiliaAcero2Nombre
        {
            get { return _familiaAcero2Nombre; }
            set
            {
                if (_familiaAcero2Nombre != value)
                {
                    _familiaAcero2Nombre = value;
                    OnPropertyChanged("FamiliaAcero2Nombre");
                }
            }
        }
        private string _familiaAcero2Nombre;
    
        [DataMember]
        public Nullable<bool> TieneHoldIngenieria
        {
            get { return _tieneHoldIngenieria; }
            set
            {
                if (_tieneHoldIngenieria != value)
                {
                    _tieneHoldIngenieria = value;
                    OnPropertyChanged("TieneHoldIngenieria");
                }
            }
        }
        private Nullable<bool> _tieneHoldIngenieria;
    
        [DataMember]
        public Nullable<bool> TieneHoldCalidad
        {
            get { return _tieneHoldCalidad; }
            set
            {
                if (_tieneHoldCalidad != value)
                {
                    _tieneHoldCalidad = value;
                    OnPropertyChanged("TieneHoldCalidad");
                }
            }
        }
        private Nullable<bool> _tieneHoldCalidad;
    
        [DataMember]
        public Nullable<bool> Confinado
        {
            get { return _confinado; }
            set
            {
                if (_confinado != value)
                {
                    _confinado = value;
                    OnPropertyChanged("Confinado");
                }
            }
        }
        private Nullable<bool> _confinado;

        #endregion

        #region Navigation Properties
    
        [DataMember]
        public TrackableCollection<CorteSpoolHistorico> CorteSpoolHistorico
        {
            get
            {
                if (_corteSpoolHistorico == null)
                {
                    _corteSpoolHistorico = new TrackableCollection<CorteSpoolHistorico>();
                    _corteSpoolHistorico.CollectionChanged += FixupCorteSpoolHistorico;
                }
                return _corteSpoolHistorico;
            }
            set
            {
                if (!ReferenceEquals(_corteSpoolHistorico, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_corteSpoolHistorico != null)
                    {
                        _corteSpoolHistorico.CollectionChanged -= FixupCorteSpoolHistorico;
                    }
                    _corteSpoolHistorico = value;
                    if (_corteSpoolHistorico != null)
                    {
                        _corteSpoolHistorico.CollectionChanged += FixupCorteSpoolHistorico;
                    }
                    OnNavigationPropertyChanged("CorteSpoolHistorico");
                }
            }
        }
        private TrackableCollection<CorteSpoolHistorico> _corteSpoolHistorico;
    
        [DataMember]
        public TrackableCollection<JuntaSpoolHistorico> JuntaSpoolHistorico
        {
            get
            {
                if (_juntaSpoolHistorico == null)
                {
                    _juntaSpoolHistorico = new TrackableCollection<JuntaSpoolHistorico>();
                    _juntaSpoolHistorico.CollectionChanged += FixupJuntaSpoolHistorico;
                }
                return _juntaSpoolHistorico;
            }
            set
            {
                if (!ReferenceEquals(_juntaSpoolHistorico, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_juntaSpoolHistorico != null)
                    {
                        _juntaSpoolHistorico.CollectionChanged -= FixupJuntaSpoolHistorico;
                    }
                    _juntaSpoolHistorico = value;
                    if (_juntaSpoolHistorico != null)
                    {
                        _juntaSpoolHistorico.CollectionChanged += FixupJuntaSpoolHistorico;
                    }
                    OnNavigationPropertyChanged("JuntaSpoolHistorico");
                }
            }
        }
        private TrackableCollection<JuntaSpoolHistorico> _juntaSpoolHistorico;
    
        [DataMember]
        public TrackableCollection<MaterialSpoolHistorico> MaterialSpoolHistorico
        {
            get
            {
                if (_materialSpoolHistorico == null)
                {
                    _materialSpoolHistorico = new TrackableCollection<MaterialSpoolHistorico>();
                    _materialSpoolHistorico.CollectionChanged += FixupMaterialSpoolHistorico;
                }
                return _materialSpoolHistorico;
            }
            set
            {
                if (!ReferenceEquals(_materialSpoolHistorico, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_materialSpoolHistorico != null)
                    {
                        _materialSpoolHistorico.CollectionChanged -= FixupMaterialSpoolHistorico;
                    }
                    _materialSpoolHistorico = value;
                    if (_materialSpoolHistorico != null)
                    {
                        _materialSpoolHistorico.CollectionChanged += FixupMaterialSpoolHistorico;
                    }
                    OnNavigationPropertyChanged("MaterialSpoolHistorico");
                }
            }
        }
        private TrackableCollection<MaterialSpoolHistorico> _materialSpoolHistorico;

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
    			if (_corteSpoolHistorico != null)
    		{
    			_corteSpoolHistorico.CollectionChanged -= FixupCorteSpoolHistorico;
    			_corteSpoolHistorico.CollectionChanged += FixupCorteSpoolHistorico;
    			}
    			if (_juntaSpoolHistorico != null)
    		{
    			_juntaSpoolHistorico.CollectionChanged -= FixupJuntaSpoolHistorico;
    			_juntaSpoolHistorico.CollectionChanged += FixupJuntaSpoolHistorico;
    			}
    			if (_materialSpoolHistorico != null)
    		{
    			_materialSpoolHistorico.CollectionChanged -= FixupMaterialSpoolHistorico;
    			_materialSpoolHistorico.CollectionChanged += FixupMaterialSpoolHistorico;
    			}
    		}
    
    
        protected virtual void ClearNavigationProperties()
        {
            CorteSpoolHistorico.Clear();
            JuntaSpoolHistorico.Clear();
            MaterialSpoolHistorico.Clear();
        }

        #endregion

        #region Association Fixup
    
        private void FixupCorteSpoolHistorico(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (CorteSpoolHistorico item in e.NewItems)
                {
                    item.SpoolHistorico = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("CorteSpoolHistorico", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (CorteSpoolHistorico item in e.OldItems)
                {
                    if (ReferenceEquals(item.SpoolHistorico, this))
                    {
                        item.SpoolHistorico = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("CorteSpoolHistorico", item);
                    }
                }
            }
        }
    
        private void FixupJuntaSpoolHistorico(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (JuntaSpoolHistorico item in e.NewItems)
                {
                    item.SpoolHistorico = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("JuntaSpoolHistorico", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (JuntaSpoolHistorico item in e.OldItems)
                {
                    if (ReferenceEquals(item.SpoolHistorico, this))
                    {
                        item.SpoolHistorico = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("JuntaSpoolHistorico", item);
                    }
                }
            }
        }
    
        private void FixupMaterialSpoolHistorico(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (MaterialSpoolHistorico item in e.NewItems)
                {
                    item.SpoolHistorico = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("MaterialSpoolHistorico", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (MaterialSpoolHistorico item in e.OldItems)
                {
                    if (ReferenceEquals(item.SpoolHistorico, this))
                    {
                        item.SpoolHistorico = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("MaterialSpoolHistorico", item);
                    }
                }
            }
        }

        #endregion

    }
}
