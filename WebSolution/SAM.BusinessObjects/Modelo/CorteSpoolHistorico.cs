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
    [KnownType(typeof(SpoolHistorico))]
    [Serializable]
    public partial class CorteSpoolHistorico: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int CorteSpoolHistoricoID
        {
            get { return _corteSpoolHistoricoID; }
            set
            {
                if (_corteSpoolHistoricoID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'CorteSpoolHistoricoID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _corteSpoolHistoricoID = value;
                    OnPropertyChanged("CorteSpoolHistoricoID");
                }
            }
        }
        private int _corteSpoolHistoricoID;
    
        [DataMember]
        public int SpoolHistoricoID
        {
            get { return _spoolHistoricoID; }
            set
            {
                if (_spoolHistoricoID != value)
                {
                    ChangeTracker.RecordOriginalValue("SpoolHistoricoID", _spoolHistoricoID);
                    if (!IsDeserializing)
                    {
                        if (SpoolHistorico != null && SpoolHistorico.SpoolHistoricoID != value)
                        {
                            SpoolHistorico = null;
                        }
                    }
                    _spoolHistoricoID = value;
                    OnPropertyChanged("SpoolHistoricoID");
                }
            }
        }
        private int _spoolHistoricoID;
    
        [DataMember]
        public int CorteSpoolID
        {
            get { return _corteSpoolID; }
            set
            {
                if (_corteSpoolID != value)
                {
                    _corteSpoolID = value;
                    OnPropertyChanged("CorteSpoolID");
                }
            }
        }
        private int _corteSpoolID;
    
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
        public int ItemCodeID
        {
            get { return _itemCodeID; }
            set
            {
                if (_itemCodeID != value)
                {
                    _itemCodeID = value;
                    OnPropertyChanged("ItemCodeID");
                }
            }
        }
        private int _itemCodeID;
    
        [DataMember]
        public int TipoCorte1ID
        {
            get { return _tipoCorte1ID; }
            set
            {
                if (_tipoCorte1ID != value)
                {
                    _tipoCorte1ID = value;
                    OnPropertyChanged("TipoCorte1ID");
                }
            }
        }
        private int _tipoCorte1ID;
    
        [DataMember]
        public int TipoCorte2ID
        {
            get { return _tipoCorte2ID; }
            set
            {
                if (_tipoCorte2ID != value)
                {
                    _tipoCorte2ID = value;
                    OnPropertyChanged("TipoCorte2ID");
                }
            }
        }
        private int _tipoCorte2ID;
    
        [DataMember]
        public string EtiquetaMaterial
        {
            get { return _etiquetaMaterial; }
            set
            {
                if (_etiquetaMaterial != value)
                {
                    _etiquetaMaterial = value;
                    OnPropertyChanged("EtiquetaMaterial");
                }
            }
        }
        private string _etiquetaMaterial;
    
        [DataMember]
        public string EtiquetaSeccion
        {
            get { return _etiquetaSeccion; }
            set
            {
                if (_etiquetaSeccion != value)
                {
                    _etiquetaSeccion = value;
                    OnPropertyChanged("EtiquetaSeccion");
                }
            }
        }
        private string _etiquetaSeccion;
    
        [DataMember]
        public decimal Diametro
        {
            get { return _diametro; }
            set
            {
                if (_diametro != value)
                {
                    _diametro = value;
                    OnPropertyChanged("Diametro");
                }
            }
        }
        private decimal _diametro;
    
        [DataMember]
        public string InicioFin
        {
            get { return _inicioFin; }
            set
            {
                if (_inicioFin != value)
                {
                    _inicioFin = value;
                    OnPropertyChanged("InicioFin");
                }
            }
        }
        private string _inicioFin;
    
        [DataMember]
        public Nullable<int> Cantidad
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
        private Nullable<int> _cantidad;
    
        [DataMember]
        public string Observaciones
        {
            get { return _observaciones; }
            set
            {
                if (_observaciones != value)
                {
                    _observaciones = value;
                    OnPropertyChanged("Observaciones");
                }
            }
        }
        private string _observaciones;
    
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
        public string SpoolNombre
        {
            get { return _spoolNombre; }
            set
            {
                if (_spoolNombre != value)
                {
                    _spoolNombre = value;
                    OnPropertyChanged("SpoolNombre");
                }
            }
        }
        private string _spoolNombre;
    
        [DataMember]
        public string ItemCodeCodigo
        {
            get { return _itemCodeCodigo; }
            set
            {
                if (_itemCodeCodigo != value)
                {
                    _itemCodeCodigo = value;
                    OnPropertyChanged("ItemCodeCodigo");
                }
            }
        }
        private string _itemCodeCodigo;
    
        [DataMember]
        public string ItemCodeDescripcionEspanol
        {
            get { return _itemCodeDescripcionEspanol; }
            set
            {
                if (_itemCodeDescripcionEspanol != value)
                {
                    _itemCodeDescripcionEspanol = value;
                    OnPropertyChanged("ItemCodeDescripcionEspanol");
                }
            }
        }
        private string _itemCodeDescripcionEspanol;
    
        [DataMember]
        public string ItemCodeDescripcionIngles
        {
            get { return _itemCodeDescripcionIngles; }
            set
            {
                if (_itemCodeDescripcionIngles != value)
                {
                    _itemCodeDescripcionIngles = value;
                    OnPropertyChanged("ItemCodeDescripcionIngles");
                }
            }
        }
        private string _itemCodeDescripcionIngles;
    
        [DataMember]
        public string TipoCorte1Codigo
        {
            get { return _tipoCorte1Codigo; }
            set
            {
                if (_tipoCorte1Codigo != value)
                {
                    _tipoCorte1Codigo = value;
                    OnPropertyChanged("TipoCorte1Codigo");
                }
            }
        }
        private string _tipoCorte1Codigo;
    
        [DataMember]
        public string TipoCorte2Codigo
        {
            get { return _tipoCorte2Codigo; }
            set
            {
                if (_tipoCorte2Codigo != value)
                {
                    _tipoCorte2Codigo = value;
                    OnPropertyChanged("TipoCorte2Codigo");
                }
            }
        }
        private string _tipoCorte2Codigo;

        #endregion

        #region Navigation Properties
    
        [DataMember]
        public SpoolHistorico SpoolHistorico
        {
            get { return _spoolHistorico; }
            set
            {
                if (!ReferenceEquals(_spoolHistorico, value))
                {
                    var previousValue = _spoolHistorico;
                    _spoolHistorico = value;
                    FixupSpoolHistorico(previousValue);
                    OnNavigationPropertyChanged("SpoolHistorico");
                }
            }
        }
        private SpoolHistorico _spoolHistorico;

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
            SpoolHistorico = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupSpoolHistorico(SpoolHistorico previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.CorteSpoolHistorico.Contains(this))
            {
                previousValue.CorteSpoolHistorico.Remove(this);
            }
    
            if (SpoolHistorico != null)
            {
                if (!SpoolHistorico.CorteSpoolHistorico.Contains(this))
                {
                    SpoolHistorico.CorteSpoolHistorico.Add(this);
                }
    
                SpoolHistoricoID = SpoolHistorico.SpoolHistoricoID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("SpoolHistorico")
                    && (ChangeTracker.OriginalValues["SpoolHistorico"] == SpoolHistorico))
                {
                    ChangeTracker.OriginalValues.Remove("SpoolHistorico");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("SpoolHistorico", previousValue);
                }
                if (SpoolHistorico != null && !SpoolHistorico.ChangeTracker.ChangeTrackingEnabled)
                {
                    SpoolHistorico.StartTracking();
                }
            }
        }

        #endregion

    }
}
