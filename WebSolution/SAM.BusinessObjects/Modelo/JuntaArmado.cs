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
    [KnownType(typeof(JuntaWorkstatus))]
    [KnownType(typeof(NumeroUnico))]
    [KnownType(typeof(Taller))]
    [KnownType(typeof(Tubero))]
    [Serializable]
    public partial class JuntaArmado: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int JuntaArmadoID
        {
            get { return _juntaArmadoID; }
            set
            {
                if (_juntaArmadoID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'JuntaArmadoID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _juntaArmadoID = value;
                    OnPropertyChanged("JuntaArmadoID");
                }
            }
        }
        private int _juntaArmadoID;
    
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
        public Nullable<int> NumeroUnico1ID
        {
            get { return _numeroUnico1ID; }
            set
            {
                if (_numeroUnico1ID != value)
                {
                    ChangeTracker.RecordOriginalValue("NumeroUnico1ID", _numeroUnico1ID);
                    if (!IsDeserializing)
                    {
                        if (NumeroUnico != null && NumeroUnico.NumeroUnicoID != value)
                        {
                            NumeroUnico = null;
                        }
                        if (NumeroUnico2 != null && NumeroUnico2.NumeroUnicoID != value)
                        {
                            NumeroUnico2 = null;
                        }
                    }
                    _numeroUnico1ID = value;
                    OnPropertyChanged("NumeroUnico1ID");
                }
            }
        }
        private Nullable<int> _numeroUnico1ID;
    
        [DataMember]
        public Nullable<int> NumeroUnico2ID
        {
            get { return _numeroUnico2ID; }
            set
            {
                if (_numeroUnico2ID != value)
                {
                    ChangeTracker.RecordOriginalValue("NumeroUnico2ID", _numeroUnico2ID);
                    if (!IsDeserializing)
                    {
                        if (NumeroUnico1 != null && NumeroUnico1.NumeroUnicoID != value)
                        {
                            NumeroUnico1 = null;
                        }
                        if (NumeroUnico3 != null && NumeroUnico3.NumeroUnicoID != value)
                        {
                            NumeroUnico3 = null;
                        }
                    }
                    _numeroUnico2ID = value;
                    OnPropertyChanged("NumeroUnico2ID");
                }
            }
        }
        private Nullable<int> _numeroUnico2ID;
    
        [DataMember]
        public int TallerID
        {
            get { return _tallerID; }
            set
            {
                if (_tallerID != value)
                {
                    ChangeTracker.RecordOriginalValue("TallerID", _tallerID);
                    if (!IsDeserializing)
                    {
                        if (Taller != null && Taller.TallerID != value)
                        {
                            Taller = null;
                        }
                    }
                    _tallerID = value;
                    OnPropertyChanged("TallerID");
                }
            }
        }
        private int _tallerID;
    
        [DataMember]
        public int TuberoID
        {
            get { return _tuberoID; }
            set
            {
                if (_tuberoID != value)
                {
                    ChangeTracker.RecordOriginalValue("TuberoID", _tuberoID);
                    if (!IsDeserializing)
                    {
                        if (Tubero != null && Tubero.TuberoID != value)
                        {
                            Tubero = null;
                        }
                    }
                    _tuberoID = value;
                    OnPropertyChanged("TuberoID");
                }
            }
        }
        private int _tuberoID;
    
        [DataMember]
        public System.DateTime FechaArmado
        {
            get { return _fechaArmado; }
            set
            {
                if (_fechaArmado != value)
                {
                    _fechaArmado = value;
                    OnPropertyChanged("FechaArmado");
                }
            }
        }
        private System.DateTime _fechaArmado;
    
        [DataMember]
        public System.DateTime FechaReporte
        {
            get { return _fechaReporte; }
            set
            {
                if (_fechaReporte != value)
                {
                    _fechaReporte = value;
                    OnPropertyChanged("FechaReporte");
                }
            }
        }
        private System.DateTime _fechaReporte;
    
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
                    _versionRegistro = value;
                    OnPropertyChanged("VersionRegistro");
                }
            }
        }
        private byte[] _versionRegistro;

        #endregion

        #region Navigation Properties
    
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
        public NumeroUnico NumeroUnico
        {
            get { return _numeroUnico; }
            set
            {
                if (!ReferenceEquals(_numeroUnico, value))
                {
                    var previousValue = _numeroUnico;
                    _numeroUnico = value;
                    FixupNumeroUnico(previousValue);
                    OnNavigationPropertyChanged("NumeroUnico");
                }
            }
        }
        private NumeroUnico _numeroUnico;
    
        [DataMember]
        public NumeroUnico NumeroUnico1
        {
            get { return _numeroUnico1; }
            set
            {
                if (!ReferenceEquals(_numeroUnico1, value))
                {
                    var previousValue = _numeroUnico1;
                    _numeroUnico1 = value;
                    FixupNumeroUnico1(previousValue);
                    OnNavigationPropertyChanged("NumeroUnico1");
                }
            }
        }
        private NumeroUnico _numeroUnico1;
    
        [DataMember]
        public Taller Taller
        {
            get { return _taller; }
            set
            {
                if (!ReferenceEquals(_taller, value))
                {
                    var previousValue = _taller;
                    _taller = value;
                    FixupTaller(previousValue);
                    OnNavigationPropertyChanged("Taller");
                }
            }
        }
        private Taller _taller;
    
        [DataMember]
        public Tubero Tubero
        {
            get { return _tubero; }
            set
            {
                if (!ReferenceEquals(_tubero, value))
                {
                    var previousValue = _tubero;
                    _tubero = value;
                    FixupTubero(previousValue);
                    OnNavigationPropertyChanged("Tubero");
                }
            }
        }
        private Tubero _tubero;
    
        [DataMember]
        public NumeroUnico NumeroUnico2
        {
            get { return _numeroUnico2; }
            set
            {
                if (!ReferenceEquals(_numeroUnico2, value))
                {
                    var previousValue = _numeroUnico2;
                    _numeroUnico2 = value;
                    FixupNumeroUnico2(previousValue);
                    OnNavigationPropertyChanged("NumeroUnico2");
                }
            }
        }
        private NumeroUnico _numeroUnico2;
    
        [DataMember]
        public NumeroUnico NumeroUnico3
        {
            get { return _numeroUnico3; }
            set
            {
                if (!ReferenceEquals(_numeroUnico3, value))
                {
                    var previousValue = _numeroUnico3;
                    _numeroUnico3 = value;
                    FixupNumeroUnico3(previousValue);
                    OnNavigationPropertyChanged("NumeroUnico3");
                }
            }
        }
        private NumeroUnico _numeroUnico3;
    
        [DataMember]
        public TrackableCollection<JuntaWorkstatus> JuntaWorkstatus1
        {
            get
            {
                if (_juntaWorkstatus1 == null)
                {
                    _juntaWorkstatus1 = new TrackableCollection<JuntaWorkstatus>();
                    _juntaWorkstatus1.CollectionChanged += FixupJuntaWorkstatus1;
                }
                return _juntaWorkstatus1;
            }
            set
            {
                if (!ReferenceEquals(_juntaWorkstatus1, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_juntaWorkstatus1 != null)
                    {
                        _juntaWorkstatus1.CollectionChanged -= FixupJuntaWorkstatus1;
                    }
                    _juntaWorkstatus1 = value;
                    if (_juntaWorkstatus1 != null)
                    {
                        _juntaWorkstatus1.CollectionChanged += FixupJuntaWorkstatus1;
                    }
                    OnNavigationPropertyChanged("JuntaWorkstatus1");
                }
            }
        }
        private TrackableCollection<JuntaWorkstatus> _juntaWorkstatus1;

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
    			if (_juntaWorkstatus1 != null)
    		{
    			_juntaWorkstatus1.CollectionChanged -= FixupJuntaWorkstatus1;
    			_juntaWorkstatus1.CollectionChanged += FixupJuntaWorkstatus1;
    			}
    		}
    
    
        protected virtual void ClearNavigationProperties()
        {
            JuntaWorkstatus = null;
            NumeroUnico = null;
            NumeroUnico1 = null;
            Taller = null;
            Tubero = null;
            NumeroUnico2 = null;
            NumeroUnico3 = null;
            JuntaWorkstatus1.Clear();
        }

        #endregion

        #region Association Fixup
    
        private void FixupJuntaWorkstatus(JuntaWorkstatus previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.JuntaArmado.Contains(this))
            {
                previousValue.JuntaArmado.Remove(this);
            }
    
            if (JuntaWorkstatus != null)
            {
                if (!JuntaWorkstatus.JuntaArmado.Contains(this))
                {
                    JuntaWorkstatus.JuntaArmado.Add(this);
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
    
        private void FixupNumeroUnico(NumeroUnico previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.JuntaArmado.Contains(this))
            {
                previousValue.JuntaArmado.Remove(this);
            }
    
            if (NumeroUnico != null)
            {
                if (!NumeroUnico.JuntaArmado.Contains(this))
                {
                    NumeroUnico.JuntaArmado.Add(this);
                }
    
                NumeroUnico1ID = NumeroUnico.NumeroUnicoID;
            }
            else if (!skipKeys)
            {
                NumeroUnico1ID = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("NumeroUnico")
                    && (ChangeTracker.OriginalValues["NumeroUnico"] == NumeroUnico))
                {
                    ChangeTracker.OriginalValues.Remove("NumeroUnico");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("NumeroUnico", previousValue);
                }
                if (NumeroUnico != null && !NumeroUnico.ChangeTracker.ChangeTrackingEnabled)
                {
                    NumeroUnico.StartTracking();
                }
            }
        }
    
        private void FixupNumeroUnico1(NumeroUnico previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.JuntaArmado1.Contains(this))
            {
                previousValue.JuntaArmado1.Remove(this);
            }
    
            if (NumeroUnico1 != null)
            {
                if (!NumeroUnico1.JuntaArmado1.Contains(this))
                {
                    NumeroUnico1.JuntaArmado1.Add(this);
                }
    
                NumeroUnico2ID = NumeroUnico1.NumeroUnicoID;
            }
            else if (!skipKeys)
            {
                NumeroUnico2ID = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("NumeroUnico1")
                    && (ChangeTracker.OriginalValues["NumeroUnico1"] == NumeroUnico1))
                {
                    ChangeTracker.OriginalValues.Remove("NumeroUnico1");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("NumeroUnico1", previousValue);
                }
                if (NumeroUnico1 != null && !NumeroUnico1.ChangeTracker.ChangeTrackingEnabled)
                {
                    NumeroUnico1.StartTracking();
                }
            }
        }
    
        private void FixupTaller(Taller previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.JuntaArmado.Contains(this))
            {
                previousValue.JuntaArmado.Remove(this);
            }
    
            if (Taller != null)
            {
                if (!Taller.JuntaArmado.Contains(this))
                {
                    Taller.JuntaArmado.Add(this);
                }
    
                TallerID = Taller.TallerID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Taller")
                    && (ChangeTracker.OriginalValues["Taller"] == Taller))
                {
                    ChangeTracker.OriginalValues.Remove("Taller");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Taller", previousValue);
                }
                if (Taller != null && !Taller.ChangeTracker.ChangeTrackingEnabled)
                {
                    Taller.StartTracking();
                }
            }
        }
    
        private void FixupTubero(Tubero previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.JuntaArmado.Contains(this))
            {
                previousValue.JuntaArmado.Remove(this);
            }
    
            if (Tubero != null)
            {
                if (!Tubero.JuntaArmado.Contains(this))
                {
                    Tubero.JuntaArmado.Add(this);
                }
    
                TuberoID = Tubero.TuberoID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Tubero")
                    && (ChangeTracker.OriginalValues["Tubero"] == Tubero))
                {
                    ChangeTracker.OriginalValues.Remove("Tubero");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Tubero", previousValue);
                }
                if (Tubero != null && !Tubero.ChangeTracker.ChangeTrackingEnabled)
                {
                    Tubero.StartTracking();
                }
            }
        }
    
        private void FixupNumeroUnico2(NumeroUnico previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.JuntaArmado2.Contains(this))
            {
                previousValue.JuntaArmado2.Remove(this);
            }
    
            if (NumeroUnico2 != null)
            {
                if (!NumeroUnico2.JuntaArmado2.Contains(this))
                {
                    NumeroUnico2.JuntaArmado2.Add(this);
                }
    
                NumeroUnico1ID = NumeroUnico2.NumeroUnicoID;
            }
            else if (!skipKeys)
            {
                NumeroUnico1ID = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("NumeroUnico2")
                    && (ChangeTracker.OriginalValues["NumeroUnico2"] == NumeroUnico2))
                {
                    ChangeTracker.OriginalValues.Remove("NumeroUnico2");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("NumeroUnico2", previousValue);
                }
                if (NumeroUnico2 != null && !NumeroUnico2.ChangeTracker.ChangeTrackingEnabled)
                {
                    NumeroUnico2.StartTracking();
                }
            }
        }
    
        private void FixupNumeroUnico3(NumeroUnico previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.JuntaArmado3.Contains(this))
            {
                previousValue.JuntaArmado3.Remove(this);
            }
    
            if (NumeroUnico3 != null)
            {
                if (!NumeroUnico3.JuntaArmado3.Contains(this))
                {
                    NumeroUnico3.JuntaArmado3.Add(this);
                }
    
                NumeroUnico2ID = NumeroUnico3.NumeroUnicoID;
            }
            else if (!skipKeys)
            {
                NumeroUnico2ID = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("NumeroUnico3")
                    && (ChangeTracker.OriginalValues["NumeroUnico3"] == NumeroUnico3))
                {
                    ChangeTracker.OriginalValues.Remove("NumeroUnico3");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("NumeroUnico3", previousValue);
                }
                if (NumeroUnico3 != null && !NumeroUnico3.ChangeTracker.ChangeTrackingEnabled)
                {
                    NumeroUnico3.StartTracking();
                }
            }
        }
    
        private void FixupJuntaWorkstatus1(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (JuntaWorkstatus item in e.NewItems)
                {
                    item.JuntaArmado1 = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("JuntaWorkstatus1", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (JuntaWorkstatus item in e.OldItems)
                {
                    if (ReferenceEquals(item.JuntaArmado1, this))
                    {
                        item.JuntaArmado1 = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("JuntaWorkstatus1", item);
                    }
                }
            }
        }

        #endregion

    }
}
