using System.ComponentModel;
using System;
using System.Globalization;
using System.Resources;
using System.Web.UI;
using Telerik.Web.UI;
using Mimo.Framework.Common;

namespace Mimo.Framework.WebControls
{
    [ToolboxData("<{0}:MappableDatePicker runat=server></{0}:MappableDatePicker>")]
    public class MappableDatePicker: RadDatePicker, IMappableField
    {
        public MappableDatePicker()
        {
            EnableEmbeddedSkins = false;           
        }

        [Serializable]
        private struct ControlProperties
        {
            public string EntityPropertyName;
            public bool EmptyIsNull;
        }

        private ControlProperties _properties;

        protected override void OnInit(EventArgs e)
        {
            Page.RegisterRequiresControlState(this);
            ResourceManager temp = new ResourceManager("Resources.RadDatePicker.Main", System.Reflection.Assembly.Load("App_GlobalResources"));
            DatePopupButton.ToolTip = temp.GetString("PickButton", new CultureInfo(LanguageHelper.CustomCulture));
            base.OnInit(e);
        }

        protected override void LoadControlState(object savedState)
        {
            object[] currentState = (object[])savedState;
            _properties = (ControlProperties)currentState[0];
            base.LoadControlState(currentState[1]);
        }

        protected override object SaveControlState()
        {
            object[] state = new object[2];
            state[0] = _properties;
            state[1] = base.SaveControlState();
            return state;
        }


        [Category("Mapping")]
        public string EntityPropertyName
        {
            get
            {
                return _properties.EntityPropertyName;
            }
            set
            {
                _properties.EntityPropertyName = value;
                SaveControlState();
            }
        }

        [DefaultValue(false)]
        [Category("Mapping")]
        public bool EmptyIsNull
        {
            get
            {
                return _properties.EmptyIsNull;
            }
            set
            {
                _properties.EmptyIsNull = value;
                SaveControlState();
            }
        }

        public void Map(object entity)
        {
            SelectedDate = ReflectionUtils.GetDateTimeValue(entity, EntityPropertyName);
        }

        public void Unmap(object entity)
        {
            ReflectionUtils.SetValueFromDateTime(entity, SelectedDate, EntityPropertyName);
        }        

        public void EstablecerFechas(DateTime FechaMin, DateTime FechaMax)
        {
            this.MinDate = FechaMin;
            this.MaxDate = FechaMax;
        }
    }
}
