using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;

namespace Mimo.Framework.WebControls
{
    [ValidationProperty("Text")]
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:RequiredLabeledTextBox runat=server></{0}:RequiredLabeledTextBox>")]
    public class RequiredLabeledTextBox : MappableControl, INamingContainer
    {
        private Label _label;
        private TextBox _textBox;
        private Literal _litStar;
        private RequiredFieldValidator _reqValidator;

        protected override void CreateChildControls()
        {
            _textBox = new TextBox
            {
                ID = "box",
                CssClass = "required"
            };

            _label = new Label
            {
                Text = string.Empty,
                ID = "lbl",
                AssociatedControlID = _textBox.ID
            };

            _reqValidator = new RequiredFieldValidator
            {
                ID = "req",
                Display = ValidatorDisplay.None,
                ControlToValidate = _textBox.ID
            };

            _litStar = new Literal
            {
                Text = "<span class=\"required\">*</span>"
            };


            Controls.Add(_label);
            Controls.Add(_textBox);
            Controls.Add(_litStar);
            Controls.Add(_reqValidator);
        }

        public override string ClientID
        {
            get
            {
                return _textBox.ClientID;
            }
        }

        [Browsable(true)]
        public string ErrorMessage
        {
            get
            {
                EnsureChildControls();
                return _reqValidator.ErrorMessage;
            }
            set
            {
                EnsureChildControls();
                _reqValidator.ErrorMessage = value;
            }
        }

        [Browsable(true)]
        public string ValidationGroup
        {
            get
            {
                EnsureChildControls();
                return _reqValidator.ValidationGroup;
            }
            set
            {
                EnsureChildControls();
                _reqValidator.ValidationGroup = value;
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
        public Unit Width
        {
            get
            {
                EnsureChildControls();
                return _textBox.Width;
            }
            set
            {
                EnsureChildControls();
                _textBox.Width = value;
            }
        }

        [Browsable(true)]
        public string CssClass
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
                _reqValidator.Enabled = value;
                _litStar.Visible = value;
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
