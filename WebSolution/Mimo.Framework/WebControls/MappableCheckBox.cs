using System.ComponentModel;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;

namespace Mimo.Framework.WebControls
{
    [SupportsEventValidation]
    [ToolboxData("<{0}:MappableCheckBox runat=server></{0}:MappableCheckBox>")]
    public class MappableCheckBox : CheckBox, IMappableField
    {
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
            bool? selected = ReflectionUtils.GetBooleanValue(entity, EntityPropertyName);

            if (selected.HasValue)
            {
                Checked = selected.Value;
            }
        }

        public void Unmap(object entity)
        {
            ReflectionUtils.SetValueFromBoolean(entity, Checked, EntityPropertyName);
        }
    }
}
