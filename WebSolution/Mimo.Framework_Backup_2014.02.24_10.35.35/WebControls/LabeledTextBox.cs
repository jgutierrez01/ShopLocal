using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;

namespace Mimo.Framework.WebControls
{
    [ValidationProperty("Text")]
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:LabeledTextBox runat=server></{0}:LabeledTextBox>")]
    public class LabeledTextBox : MappableControl, INamingContainer
    {
        private TextBox _textBox;
        private Label _label;

        protected override void CreateChildControls()
        {
            _textBox = new TextBox
            {
                ID = "box",
            };

            _label = new Label
            {
                Text = string.Empty,
                ID = "lbl",
                AssociatedControlID = _textBox.ID
            };

            Controls.Add(_label);
            Controls.Add(_textBox);
        }

        public override string ClientID
        {
            get
            {
                return _textBox.ClientID;
            }
        }

        [Browsable(true)]
        public string Label
        {
            get
            {
                EnsureChildControls();
                return _label.Text;
            }
            set
            {
                EnsureChildControls();
                _label.Text = value;
            }
        }

        [Browsable(true)]
        public string Text
        {
            get
            {
                EnsureChildControls();
                return _textBox.Text;
            }
            set
            {
                EnsureChildControls();
                _textBox.Text = value;
            }
        }

        [Browsable(true)]
        public int MaxLength
        {
            get
            {
                EnsureChildControls();
                return _textBox.MaxLength;
            }
            set
            {
                EnsureChildControls();
                _textBox.MaxLength = value;
            }
        }

        [Browsable(true)]
        public bool ReadOnly
        {
            get
            {
                EnsureChildControls();
                return _textBox.ReadOnly;
            }
            set
            {
                EnsureChildControls();
                _textBox.ReadOnly = value;
            }
        }

        [Browsable(true)]
        public bool Enabled
        {
            get
            {
                EnsureChildControls();
                return _textBox.Enabled;
            }
            set
            {
                EnsureChildControls();
                _textBox.Enabled = value;
            }
        }

        [Browsable(true)]
        public TextBoxMode TextMode
        {
            get
            {
                EnsureChildControls();
                return _textBox.TextMode;
            }
            set
            {
                EnsureChildControls();
                _textBox.TextMode = value;
            }
        }

        [Browsable(true)]
        public int Rows
        {
            get
            {
                EnsureChildControls();
                return _textBox.Rows;
            }
            set
            {
                EnsureChildControls();
                _textBox.Rows = value;
            }
        }

        [Browsable(true)]
        public int Columns
        {
            get
            {
                EnsureChildControls();
                return _textBox.Columns;
            }
            set
            {
                EnsureChildControls();
                _textBox.Columns = value;
            }
        }

        [Browsable(true)]
        public string CssTextBox
        {
            get
            {
                EnsureChildControls();
                return _textBox.CssClass;
            }
            set
            {
                EnsureChildControls();
                _textBox.CssClass = value;
            }
        }

        [Browsable(true)]
        public string CssLabel
        {
            get
            {
                EnsureChildControls();
                return _label.CssClass;
            }
            set
            {
                EnsureChildControls();
                _label.CssClass = value;
            }
        }

        public override void Map(object entity)
        {
            Text = ReflectionUtils.GetStringValue(entity, EntityPropertyName);
        }

        public override void Unmap(object entity)
        {
            ReflectionUtils.SetValueFromString(entity, Text, EntityPropertyName, EmptyIsNull);
        }

    }
}
