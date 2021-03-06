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
    [KnownType(typeof(JuntaReportePnd))]
    [KnownType(typeof(JuntaCampoReportePND))]
    [Serializable]
    public partial class TipoRechazo: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int TipoRechazoID
        {
            get { return _tipoRechazoID; }
            set
            {
                if (_tipoRechazoID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'TipoRechazoID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _tipoRechazoID = value;
                    OnPropertyChanged("TipoRechazoID");
                }
            }
        }
        private int _tipoRechazoID;
    
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
        public TrackableCollection<JuntaReportePnd> JuntaReportePnd
        {
            get
            {
                if (_juntaReportePnd == null)
                {
                    _juntaReportePnd = new TrackableCollection<JuntaReportePnd>();
                    _juntaReportePnd.CollectionChanged += FixupJuntaReportePnd;
                }
                return _juntaReportePnd;
            }
            set
            {
                if (!ReferenceEquals(_juntaReportePnd, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_juntaReportePnd != null)
                    {
                        _juntaReportePnd.CollectionChanged -= FixupJuntaReportePnd;
                    }
                    _juntaReportePnd = value;
                    if (_juntaReportePnd != null)
                    {
                        _juntaReportePnd.CollectionChanged += FixupJuntaReportePnd;
                    }
                    OnNavigationPropertyChanged("JuntaReportePnd");
                }
            }
        }
        private TrackableCollection<JuntaReportePnd> _juntaReportePnd;
    
        [DataMember]
        public TrackableCollection<JuntaCampoReportePND> JuntaCampoReportePND
        {
            get
            {
                if (_juntaCampoReportePND == null)
                {
                    _juntaCampoReportePND = new TrackableCollection<JuntaCampoReportePND>();
                    _juntaCampoReportePND.CollectionChanged += FixupJuntaCampoReportePND;
                }
                return _juntaCampoReportePND;
            }
            set
            {
                if (!ReferenceEquals(_juntaCampoReportePND, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_juntaCampoReportePND != null)
                    {
                        _juntaCampoReportePND.CollectionChanged -= FixupJuntaCampoReportePND;
                    }
                    _juntaCampoReportePND = value;
                    if (_juntaCampoReportePND != null)
                    {
                        _juntaCampoReportePND.CollectionChanged += FixupJuntaCampoReportePND;
                    }
                    OnNavigationPropertyChanged("JuntaCampoReportePND");
                }
            }
        }
        private TrackableCollection<JuntaCampoReportePND> _juntaCampoReportePND;

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
    			if (_juntaReportePnd != null)
    		{
    			_juntaReportePnd.CollectionChanged -= FixupJuntaReportePnd;
    			_juntaReportePnd.CollectionChanged += FixupJuntaReportePnd;
    			}
    			if (_juntaCampoReportePND != null)
    		{
    			_juntaCampoReportePND.CollectionChanged -= FixupJuntaCampoReportePND;
    			_juntaCampoReportePND.CollectionChanged += FixupJuntaCampoReportePND;
    			}
    		}
    
    
        protected virtual void ClearNavigationProperties()
        {
            JuntaReportePnd.Clear();
            JuntaCampoReportePND.Clear();
        }

        #endregion

        #region Association Fixup
    
        private void FixupJuntaReportePnd(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (JuntaReportePnd item in e.NewItems)
                {
                    item.TipoRechazo = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("JuntaReportePnd", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (JuntaReportePnd item in e.OldItems)
                {
                    if (ReferenceEquals(item.TipoRechazo, this))
                    {
                        item.TipoRechazo = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("JuntaReportePnd", item);
                    }
                }
            }
        }
    
        private void FixupJuntaCampoReportePND(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (JuntaCampoReportePND item in e.NewItems)
                {
                    item.TipoRechazo = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("JuntaCampoReportePND", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (JuntaCampoReportePND item in e.OldItems)
                {
                    if (ReferenceEquals(item.TipoRechazo, this))
                    {
                        item.TipoRechazo = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("JuntaCampoReportePND", item);
                    }
                }
            }
        }

        #endregion

    }
}
