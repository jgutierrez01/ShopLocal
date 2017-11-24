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
    [KnownType(typeof(DestajoSoldador))]
    [KnownType(typeof(JuntaWorkstatus))]
    [KnownType(typeof(Proyecto))]
    [Serializable]
    public partial class DestajoSoldadorDetalle: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int DestajoSoldadorDetalleID
        {
            get { return _destajoSoldadorDetalleID; }
            set
            {
                if (_destajoSoldadorDetalleID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'DestajoSoldadorDetalleID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _destajoSoldadorDetalleID = value;
                    OnPropertyChanged("DestajoSoldadorDetalleID");
                }
            }
        }
        private int _destajoSoldadorDetalleID;
    
        [DataMember]
        public int DestajoSoldadorID
        {
            get { return _destajoSoldadorID; }
            set
            {
                if (_destajoSoldadorID != value)
                {
                    ChangeTracker.RecordOriginalValue("DestajoSoldadorID", _destajoSoldadorID);
                    if (!IsDeserializing)
                    {
                        if (DestajoSoldador != null && DestajoSoldador.DestajoSoldadorID != value)
                        {
                            DestajoSoldador = null;
                        }
                    }
                    _destajoSoldadorID = value;
                    OnPropertyChanged("DestajoSoldadorID");
                }
            }
        }
        private int _destajoSoldadorID;
    
        [DataMember]
        public int JuntaWorkstatusID
        {
            get { return _juntaWorkstatusID; }
            set
            {
                if (_juntaWorkstatusID != value)
                {
                    ChangeTracker.RecordOriginalValue("JuntaWorkstatusID", _juntaWorkstatusID);
                    if (!IsDeserializing)
                    {
                        if (JuntaWorkstatus != null && JuntaWorkstatus.JuntaWorkstatusID != value)
                        {
                            JuntaWorkstatus = null;
                        }
                    }
                    _juntaWorkstatusID = value;
                    OnPropertyChanged("JuntaWorkstatusID");
                }
            }
        }
        private int _juntaWorkstatusID;
    
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
        public decimal PDIs
        {
            get { return _pDIs; }
            set
            {
                if (_pDIs != value)
                {
                    _pDIs = value;
                    OnPropertyChanged("PDIs");
                }
            }
        }
        private decimal _pDIs;
    
        [DataMember]
        public decimal CostoUnitarioRaiz
        {
            get { return _costoUnitarioRaiz; }
            set
            {
                if (_costoUnitarioRaiz != value)
                {
                    _costoUnitarioRaiz = value;
                    OnPropertyChanged("CostoUnitarioRaiz");
                }
            }
        }
        private decimal _costoUnitarioRaiz;
    
        [DataMember]
        public decimal CostoUnitarioRelleno
        {
            get { return _costoUnitarioRelleno; }
            set
            {
                if (_costoUnitarioRelleno != value)
                {
                    _costoUnitarioRelleno = value;
                    OnPropertyChanged("CostoUnitarioRelleno");
                }
            }
        }
        private decimal _costoUnitarioRelleno;
    
        [DataMember]
        public bool RaizDividida
        {
            get { return _raizDividida; }
            set
            {
                if (_raizDividida != value)
                {
                    _raizDividida = value;
                    OnPropertyChanged("RaizDividida");
                }
            }
        }
        private bool _raizDividida;
    
        [DataMember]
        public bool RellenoDividido
        {
            get { return _rellenoDividido; }
            set
            {
                if (_rellenoDividido != value)
                {
                    _rellenoDividido = value;
                    OnPropertyChanged("RellenoDividido");
                }
            }
        }
        private bool _rellenoDividido;
    
        [DataMember]
        public byte NumeroFondeadores
        {
            get { return _numeroFondeadores; }
            set
            {
                if (_numeroFondeadores != value)
                {
                    _numeroFondeadores = value;
                    OnPropertyChanged("NumeroFondeadores");
                }
            }
        }
        private byte _numeroFondeadores;
    
        [DataMember]
        public byte NumeroRellenadores
        {
            get { return _numeroRellenadores; }
            set
            {
                if (_numeroRellenadores != value)
                {
                    _numeroRellenadores = value;
                    OnPropertyChanged("NumeroRellenadores");
                }
            }
        }
        private byte _numeroRellenadores;
    
        [DataMember]
        public decimal DestajoRaiz
        {
            get { return _destajoRaiz; }
            set
            {
                if (_destajoRaiz != value)
                {
                    _destajoRaiz = value;
                    OnPropertyChanged("DestajoRaiz");
                }
            }
        }
        private decimal _destajoRaiz;
    
        [DataMember]
        public decimal DestajoRelleno
        {
            get { return _destajoRelleno; }
            set
            {
                if (_destajoRelleno != value)
                {
                    _destajoRelleno = value;
                    OnPropertyChanged("DestajoRelleno");
                }
            }
        }
        private decimal _destajoRelleno;
    
        [DataMember]
        public decimal ProrrateoCuadro
        {
            get { return _prorrateoCuadro; }
            set
            {
                if (_prorrateoCuadro != value)
                {
                    _prorrateoCuadro = value;
                    OnPropertyChanged("ProrrateoCuadro");
                }
            }
        }
        private decimal _prorrateoCuadro;
    
        [DataMember]
        public decimal ProrrateoDiasFestivos
        {
            get { return _prorrateoDiasFestivos; }
            set
            {
                if (_prorrateoDiasFestivos != value)
                {
                    _prorrateoDiasFestivos = value;
                    OnPropertyChanged("ProrrateoDiasFestivos");
                }
            }
        }
        private decimal _prorrateoDiasFestivos;
    
        [DataMember]
        public decimal ProrrateoOtros
        {
            get { return _prorrateoOtros; }
            set
            {
                if (_prorrateoOtros != value)
                {
                    _prorrateoOtros = value;
                    OnPropertyChanged("ProrrateoOtros");
                }
            }
        }
        private decimal _prorrateoOtros;
    
        [DataMember]
        public decimal Ajuste
        {
            get { return _ajuste; }
            set
            {
                if (_ajuste != value)
                {
                    _ajuste = value;
                    OnPropertyChanged("Ajuste");
                }
            }
        }
        private decimal _ajuste;
    
        [DataMember]
        public decimal Total
        {
            get { return _total; }
            set
            {
                if (_total != value)
                {
                    _total = value;
                    OnPropertyChanged("Total");
                }
            }
        }
        private decimal _total;
    
        [DataMember]
        public string Comentarios
        {
            get { return _comentarios; }
            set
            {
                if (_comentarios != value)
                {
                    _comentarios = value;
                    OnPropertyChanged("Comentarios");
                }
            }
        }
        private string _comentarios;
    
        [DataMember]
        public bool EsDePeriodoAnterior
        {
            get { return _esDePeriodoAnterior; }
            set
            {
                if (_esDePeriodoAnterior != value)
                {
                    _esDePeriodoAnterior = value;
                    OnPropertyChanged("EsDePeriodoAnterior");
                }
            }
        }
        private bool _esDePeriodoAnterior;
    
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
        public bool CostoRaizVacio
        {
            get { return _costoRaizVacio; }
            set
            {
                if (_costoRaizVacio != value)
                {
                    _costoRaizVacio = value;
                    OnPropertyChanged("CostoRaizVacio");
                }
            }
        }
        private bool _costoRaizVacio;
    
        [DataMember]
        public bool CostoRellenoVacio
        {
            get { return _costoRellenoVacio; }
            set
            {
                if (_costoRellenoVacio != value)
                {
                    _costoRellenoVacio = value;
                    OnPropertyChanged("CostoRellenoVacio");
                }
            }
        }
        private bool _costoRellenoVacio;

        #endregion

        #region Navigation Properties
    
        [DataMember]
        public DestajoSoldador DestajoSoldador
        {
            get { return _destajoSoldador; }
            set
            {
                if (!ReferenceEquals(_destajoSoldador, value))
                {
                    var previousValue = _destajoSoldador;
                    _destajoSoldador = value;
                    FixupDestajoSoldador(previousValue);
                    OnNavigationPropertyChanged("DestajoSoldador");
                }
            }
        }
        private DestajoSoldador _destajoSoldador;
    
        [DataMember]
        public JuntaWorkstatus JuntaWorkstatus
        {
            get { return _juntaWorkstatus; }
            set
            {
                if (!ReferenceEquals(_juntaWorkstatus, value))
                {
                    var previousValue = _juntaWorkstatus;
                    _juntaWorkstatus = value;
                    FixupJuntaWorkstatus(previousValue);
                    OnNavigationPropertyChanged("JuntaWorkstatus");
                }
            }
        }
        private JuntaWorkstatus _juntaWorkstatus;
    
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
            DestajoSoldador = null;
            JuntaWorkstatus = null;
            Proyecto = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupDestajoSoldador(DestajoSoldador previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.DestajoSoldadorDetalle.Contains(this))
            {
                previousValue.DestajoSoldadorDetalle.Remove(this);
            }
    
            if (DestajoSoldador != null)
            {
                if (!DestajoSoldador.DestajoSoldadorDetalle.Contains(this))
                {
                    DestajoSoldador.DestajoSoldadorDetalle.Add(this);
                }
    
                DestajoSoldadorID = DestajoSoldador.DestajoSoldadorID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("DestajoSoldador")
                    && (ChangeTracker.OriginalValues["DestajoSoldador"] == DestajoSoldador))
                {
                    ChangeTracker.OriginalValues.Remove("DestajoSoldador");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("DestajoSoldador", previousValue);
                }
                if (DestajoSoldador != null && !DestajoSoldador.ChangeTracker.ChangeTrackingEnabled)
                {
                    DestajoSoldador.StartTracking();
                }
            }
        }
    
        private void FixupJuntaWorkstatus(JuntaWorkstatus previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.DestajoSoldadorDetalle.Contains(this))
            {
                previousValue.DestajoSoldadorDetalle.Remove(this);
            }
    
            if (JuntaWorkstatus != null)
            {
                if (!JuntaWorkstatus.DestajoSoldadorDetalle.Contains(this))
                {
                    JuntaWorkstatus.DestajoSoldadorDetalle.Add(this);
                }
    
                JuntaWorkstatusID = JuntaWorkstatus.JuntaWorkstatusID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("JuntaWorkstatus")
                    && (ChangeTracker.OriginalValues["JuntaWorkstatus"] == JuntaWorkstatus))
                {
                    ChangeTracker.OriginalValues.Remove("JuntaWorkstatus");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("JuntaWorkstatus", previousValue);
                }
                if (JuntaWorkstatus != null && !JuntaWorkstatus.ChangeTracker.ChangeTrackingEnabled)
                {
                    JuntaWorkstatus.StartTracking();
                }
            }
        }
    
        private void FixupProyecto(Proyecto previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.DestajoSoldadorDetalle.Contains(this))
            {
                previousValue.DestajoSoldadorDetalle.Remove(this);
            }
    
            if (Proyecto != null)
            {
                if (!Proyecto.DestajoSoldadorDetalle.Contains(this))
                {
                    Proyecto.DestajoSoldadorDetalle.Add(this);
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
