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
    [KnownType(typeof(JuntaSpool))]
    [Serializable]
    public partial class AgrupadoresPorJunta: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int AgrupadorJuntaID
        {
            get { return _agrupadorJuntaID; }
            set
            {
                if (_agrupadorJuntaID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'AgrupadorJuntaID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _agrupadorJuntaID = value;
                    OnPropertyChanged("AgrupadorJuntaID");
                }
            }
        }
        private int _agrupadorJuntaID;
    
        [DataMember]
        public int JuntaSpoolID
        {
            get { return _juntaSpoolID; }
            set
            {
                if (_juntaSpoolID != value)
                {
                    ChangeTracker.RecordOriginalValue("JuntaSpoolID", _juntaSpoolID);
                    if (!IsDeserializing)
                    {
                        if (JuntaSpool != null && JuntaSpool.JuntaSpoolID != value)
                        {
                            JuntaSpool = null;
                        }
                    }
                    _juntaSpoolID = value;
                    OnPropertyChanged("JuntaSpoolID");
                }
            }
        }
        private int _juntaSpoolID;
    
        [DataMember]
        public string ClasificacionPND
        {
            get { return _clasificacionPND; }
            set
            {
                if (_clasificacionPND != value)
                {
                    _clasificacionPND = value;
                    OnPropertyChanged("ClasificacionPND");
                }
            }
        }
        private string _clasificacionPND;
    
        [DataMember]
        public string ClasificacionReparacion
        {
            get { return _clasificacionReparacion; }
            set
            {
                if (_clasificacionReparacion != value)
                {
                    _clasificacionReparacion = value;
                    OnPropertyChanged("ClasificacionReparacion");
                }
            }
        }
        private string _clasificacionReparacion;
    
        [DataMember]
        public string ClasificaiconSoporte
        {
            get { return _clasificaiconSoporte; }
            set
            {
                if (_clasificaiconSoporte != value)
                {
                    _clasificaiconSoporte = value;
                    OnPropertyChanged("ClasificaiconSoporte");
                }
            }
        }
        private string _clasificaiconSoporte;
    
        [DataMember]
        public string Grupo1
        {
            get { return _grupo1; }
            set
            {
                if (_grupo1 != value)
                {
                    _grupo1 = value;
                    OnPropertyChanged("Grupo1");
                }
            }
        }
        private string _grupo1;
    
        [DataMember]
        public string Grupo2
        {
            get { return _grupo2; }
            set
            {
                if (_grupo2 != value)
                {
                    _grupo2 = value;
                    OnPropertyChanged("Grupo2");
                }
            }
        }
        private string _grupo2;
    
        [DataMember]
        public string Grupo3
        {
            get { return _grupo3; }
            set
            {
                if (_grupo3 != value)
                {
                    _grupo3 = value;
                    OnPropertyChanged("Grupo3");
                }
            }
        }
        private string _grupo3;
    
        [DataMember]
        public string Grupo4
        {
            get { return _grupo4; }
            set
            {
                if (_grupo4 != value)
                {
                    _grupo4 = value;
                    OnPropertyChanged("Grupo4");
                }
            }
        }
        private string _grupo4;
    
        [DataMember]
        public string Grupo5
        {
            get { return _grupo5; }
            set
            {
                if (_grupo5 != value)
                {
                    _grupo5 = value;
                    OnPropertyChanged("Grupo5");
                }
            }
        }
        private string _grupo5;
    
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
        public JuntaSpool JuntaSpool
        {
            get { return _juntaSpool; }
            set
            {
                if (!ReferenceEquals(_juntaSpool, value))
                {
                    var previousValue = _juntaSpool;
                    _juntaSpool = value;
                    FixupJuntaSpool(previousValue);
                    OnNavigationPropertyChanged("JuntaSpool");
                }
            }
        }
        private JuntaSpool _juntaSpool;

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
            JuntaSpool = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupJuntaSpool(JuntaSpool previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.AgrupadoresPorJunta.Contains(this))
            {
                previousValue.AgrupadoresPorJunta.Remove(this);
            }
    
            if (JuntaSpool != null)
            {
                if (!JuntaSpool.AgrupadoresPorJunta.Contains(this))
                {
                    JuntaSpool.AgrupadoresPorJunta.Add(this);
                }
    
                JuntaSpoolID = JuntaSpool.JuntaSpoolID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("JuntaSpool")
                    && (ChangeTracker.OriginalValues["JuntaSpool"] == JuntaSpool))
                {
                    ChangeTracker.OriginalValues.Remove("JuntaSpool");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("JuntaSpool", previousValue);
                }
                if (JuntaSpool != null && !JuntaSpool.ChangeTracker.ChangeTrackingEnabled)
                {
                    JuntaSpool.StartTracking();
                }
            }
        }

        #endregion

    }
}
