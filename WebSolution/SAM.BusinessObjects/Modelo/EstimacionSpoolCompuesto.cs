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
    
    [Serializable]
    public partial class EstimacionSpoolCompuesto : INotifyComplexPropertyChanging, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int WorkstatusSpoolID
        {
            get { return _workstatusSpoolID; }
            set
            {
                if (_workstatusSpoolID != value)
                {
                    OnComplexPropertyChanging();
                    _workstatusSpoolID = value;
                    OnPropertyChanged("WorkstatusSpoolID");
                }
            }
        }
        private int _workstatusSpoolID;
    
        [DataMember]
        public int OrdenTrabajoSpoolID
        {
            get { return _ordenTrabajoSpoolID; }
            set
            {
                if (_ordenTrabajoSpoolID != value)
                {
                    OnComplexPropertyChanging();
                    _ordenTrabajoSpoolID = value;
                    OnPropertyChanged("OrdenTrabajoSpoolID");
                }
            }
        }
        private int _ordenTrabajoSpoolID;
    
        [DataMember]
        public bool Embarcado
        {
            get { return _embarcado; }
            set
            {
                if (_embarcado != value)
                {
                    OnComplexPropertyChanging();
                    _embarcado = value;
                    OnPropertyChanged("Embarcado");
                }
            }
        }
        private bool _embarcado;
    
        [DataMember]
        public bool TieneLiberacionDimensional
        {
            get { return _tieneLiberacionDimensional; }
            set
            {
                if (_tieneLiberacionDimensional != value)
                {
                    OnComplexPropertyChanging();
                    _tieneLiberacionDimensional = value;
                    OnPropertyChanged("TieneLiberacionDimensional");
                }
            }
        }
        private bool _tieneLiberacionDimensional;
    
        [DataMember]
        public bool LiberadoPintura
        {
            get { return _liberadoPintura; }
            set
            {
                if (_liberadoPintura != value)
                {
                    OnComplexPropertyChanging();
                    _liberadoPintura = value;
                    OnPropertyChanged("LiberadoPintura");
                }
            }
        }
        private bool _liberadoPintura;
    
        [DataMember]
        public Nullable<decimal> Pdis
        {
            get { return _pdis; }
            set
            {
                if (_pdis != value)
                {
                    OnComplexPropertyChanging();
                    _pdis = value;
                    OnPropertyChanged("Pdis");
                }
            }
        }
        private Nullable<decimal> _pdis;
    
        [DataMember]
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (_nombre != value)
                {
                    OnComplexPropertyChanging();
                    _nombre = value;
                    OnPropertyChanged("Nombre");
                }
            }
        }
        private string _nombre;
    
        [DataMember]
        public string NumeroControl
        {
            get { return _numeroControl; }
            set
            {
                if (_numeroControl != value)
                {
                    OnComplexPropertyChanging();
                    _numeroControl = value;
                    OnPropertyChanged("NumeroControl");
                }
            }
        }
        private string _numeroControl;
    
        [DataMember]
        public Nullable<int> EstimacionSpoolID
        {
            get { return _estimacionSpoolID; }
            set
            {
                if (_estimacionSpoolID != value)
                {
                    OnComplexPropertyChanging();
                    _estimacionSpoolID = value;
                    OnPropertyChanged("EstimacionSpoolID");
                }
            }
        }
        private Nullable<int> _estimacionSpoolID;
    
        [DataMember]
        public Nullable<int> EstimacionID
        {
            get { return _estimacionID; }
            set
            {
                if (_estimacionID != value)
                {
                    OnComplexPropertyChanging();
                    _estimacionID = value;
                    OnPropertyChanged("EstimacionID");
                }
            }
        }
        private Nullable<int> _estimacionID;
    
        [DataMember]
        public Nullable<int> ConceptoEstimacionID
        {
            get { return _conceptoEstimacionID; }
            set
            {
                if (_conceptoEstimacionID != value)
                {
                    OnComplexPropertyChanging();
                    _conceptoEstimacionID = value;
                    OnPropertyChanged("ConceptoEstimacionID");
                }
            }
        }
        private Nullable<int> _conceptoEstimacionID;
    
        [DataMember]
        public string NumeroEstimacion
        {
            get { return _numeroEstimacion; }
            set
            {
                if (_numeroEstimacion != value)
                {
                    OnComplexPropertyChanging();
                    _numeroEstimacion = value;
                    OnPropertyChanged("NumeroEstimacion");
                }
            }
        }
        private string _numeroEstimacion;

        #endregion

        #region ChangeTracking
    
        private void OnComplexPropertyChanging()
        {
            if (_complexPropertyChanging != null)
            {
                _complexPropertyChanging(this, new EventArgs());
            }
        }
    
        event EventHandler INotifyComplexPropertyChanging.ComplexPropertyChanging { add { _complexPropertyChanging += value; } remove { _complexPropertyChanging -= value; } }
        private event EventHandler _complexPropertyChanging;
    
        private void OnPropertyChanged(String propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged { add { _propertyChanged += value; } remove { _propertyChanged -= value; } }
        private event PropertyChangedEventHandler _propertyChanged;
    
        public static void RecordComplexOriginalValues(String parentPropertyName, EstimacionSpoolCompuesto complexObject, ObjectChangeTracker changeTracker)
        {
            if (String.IsNullOrEmpty(parentPropertyName))
            {
                throw new ArgumentException("String parameter cannot be null or empty.", "parentPropertyName");
            }
    
            if (changeTracker == null)
            {
                throw new ArgumentNullException("changeTracker");
            }
            changeTracker.RecordOriginalValue(String.Format(CultureInfo.InvariantCulture, "{0}.WorkstatusSpoolID", parentPropertyName), complexObject == null ? null : (object)complexObject.WorkstatusSpoolID);
            changeTracker.RecordOriginalValue(String.Format(CultureInfo.InvariantCulture, "{0}.OrdenTrabajoSpoolID", parentPropertyName), complexObject == null ? null : (object)complexObject.OrdenTrabajoSpoolID);
            changeTracker.RecordOriginalValue(String.Format(CultureInfo.InvariantCulture, "{0}.Embarcado", parentPropertyName), complexObject == null ? null : (object)complexObject.Embarcado);
            changeTracker.RecordOriginalValue(String.Format(CultureInfo.InvariantCulture, "{0}.TieneLiberacionDimensional", parentPropertyName), complexObject == null ? null : (object)complexObject.TieneLiberacionDimensional);
            changeTracker.RecordOriginalValue(String.Format(CultureInfo.InvariantCulture, "{0}.LiberadoPintura", parentPropertyName), complexObject == null ? null : (object)complexObject.LiberadoPintura);
            changeTracker.RecordOriginalValue(String.Format(CultureInfo.InvariantCulture, "{0}.Pdis", parentPropertyName), complexObject == null ? null : (object)complexObject.Pdis);
            changeTracker.RecordOriginalValue(String.Format(CultureInfo.InvariantCulture, "{0}.Nombre", parentPropertyName), complexObject == null ? null : (object)complexObject.Nombre);
            changeTracker.RecordOriginalValue(String.Format(CultureInfo.InvariantCulture, "{0}.NumeroControl", parentPropertyName), complexObject == null ? null : (object)complexObject.NumeroControl);
            changeTracker.RecordOriginalValue(String.Format(CultureInfo.InvariantCulture, "{0}.EstimacionSpoolID", parentPropertyName), complexObject == null ? null : (object)complexObject.EstimacionSpoolID);
            changeTracker.RecordOriginalValue(String.Format(CultureInfo.InvariantCulture, "{0}.EstimacionID", parentPropertyName), complexObject == null ? null : (object)complexObject.EstimacionID);
            changeTracker.RecordOriginalValue(String.Format(CultureInfo.InvariantCulture, "{0}.ConceptoEstimacionID", parentPropertyName), complexObject == null ? null : (object)complexObject.ConceptoEstimacionID);
            changeTracker.RecordOriginalValue(String.Format(CultureInfo.InvariantCulture, "{0}.NumeroEstimacion", parentPropertyName), complexObject == null ? null : (object)complexObject.NumeroEstimacion);
        }

        #endregion

    }
}
