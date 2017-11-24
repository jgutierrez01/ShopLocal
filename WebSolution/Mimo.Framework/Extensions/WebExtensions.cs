using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Data;
using Mimo.Framework.WebControls;
using Telerik.Web.UI;

namespace Mimo.Framework.Extensions
{
    public static class WebExtensions
    {
        public static void IterateRecursively(this ControlCollection controlCollection, Action<Control> codeToExecute)
        {
            foreach (Control control in controlCollection)
            {
                codeToExecute(control);
                
                if (control.HasControls())
                {
                    control.Controls.IterateRecursively(codeToExecute);
                }
            }
        }

        public static void IterateRecursivelyStopOnIMappableAndUserControl(this ControlCollection controlCollection, Action<Control> codeToExecute)
        {
            foreach (Control control in controlCollection)
            {
                codeToExecute(control);

                if (control is IMappable && control is UserControl)
                {
                    return;
                }

                if (control.HasControls())
                {
                    control.Controls.IterateRecursivelyStopOnIMappableAndUserControl(codeToExecute);
                }
            }
        }

        #region RadComboBox extensions

        private static void BindToEnties(this RadComboBox comboBox, IEnumerable<BaseEntity> entities, int? selected, bool insertEmptyRow, string textField, string valueField)
        {
            comboBox.DataSource = entities;
            comboBox.DataTextField = textField ?? "Text";
            comboBox.DataValueField = valueField ?? "Value";
            comboBox.DataBind();

            if (insertEmptyRow) comboBox.Items.Insert(0, new RadComboBoxItem("", "-1"));
            if (selected == null) return;
            RadComboBoxItem item = comboBox.Items.FindItemByValue(selected.ToString());
            if (item != null) item.Selected = true;
        }

        public static void BindToEntiesWithEmptyRow(this RadComboBox comboBox, IEnumerable<BaseEntity> entities, int? selected)
        {
            BindToEnties(comboBox, entities, selected, true, null, null);
        }

        public static void BindToEnties(this RadComboBox comboBox, IEnumerable<BaseEntity> entities, int? selected)
        {
            BindToEnties(comboBox, entities, selected, false, null, null);
        }

        public static void BindToEntiesWithEmptyRow(this RadComboBox comboBox, IEnumerable<BaseEntity> entities)
        {
            BindToEnties(comboBox, entities, null, true, null, null);
        }

        public static void BindToEnties(this RadComboBox comboBox, IEnumerable<BaseEntity> entities)
        {
            BindToEnties(comboBox, entities, null, false, null, null);
        }

        public static void BindToEntiesWithEmptyRow(this RadComboBox comboBox, IEnumerable<BaseEntity> entities, int? selected, string textField, string valueField)
        {
            BindToEnties(comboBox, entities, selected, true, textField, valueField);
        }

        public static void BindToEnties(this RadComboBox comboBox, IEnumerable<BaseEntity> entities, int? selected, string textField, string valueField)
        {
            BindToEnties(comboBox, entities, selected, false, textField, valueField);
        }

        public static void BindToEntiesWithEmptyRow(this RadComboBox comboBox, IEnumerable<BaseEntity> entities, string textField, string valueField)
        {
            BindToEnties(comboBox, entities, null, true, textField, valueField);
        }

        public static void BindToEnties(this RadComboBox comboBox, IEnumerable<BaseEntity> entities, string textField, string valueField)
        {
            BindToEnties(comboBox, entities, null, false, textField, valueField);
        }

        public static bool IsItem(this GridItem item)
        {
            return item.ItemType == GridItemType.Item || item.ItemType == GridItemType.AlternatingItem;
        }

        public static bool IsHeader(this GridItem item)
        {
            return item.ItemType == GridItemType.Header || item.ItemType == GridItemType.GroupHeader || item.ItemType == GridItemType.THead;
        }

        #endregion

        #region DropDown Extensions

        private static void BindToEnties(this DropDownList dropDownList, IEnumerable<BaseEntity> entities, int? selected, bool insertEmptyRow, string textField, string valueField)
        {
            dropDownList.DataSource = entities.OrderBy(x => x.Text);
            dropDownList.DataTextField = textField ?? "Text";
            dropDownList.DataValueField = valueField ?? "ID";
            dropDownList.DataBind();

            if (insertEmptyRow)
            {
                dropDownList.Items.Insert(0, new ListItem(string.Empty, string.Empty));
            }

            if (selected == null)
            {
                return;
            }
            
            ListItem item = dropDownList.Items.FindByValue(selected.ToString());

            if (item != null)
            {
                item.Selected = true;
            }
        }

        public static void BindToEnties(this DropDownList dropDownList, IEnumerable<BaseEntity> entities, int? selected)
        {
            BindToEnties(dropDownList, entities, selected, false, null, null);
        }

        public static void BindToEnties(this DropDownList dropDownList, IEnumerable<BaseEntity> entities)
        {
            BindToEnties(dropDownList, entities, null, false, null, null);
        }

        public static void BindToEntiesWithEmptyRow(this DropDownList dropDownList, IEnumerable<BaseEntity> entities, int? selected)
        {
            BindToEnties(dropDownList, entities, selected, true, null, null);
        }

        public static void BindToEntiesWithEmptyRow(this DropDownList dropDownList, IEnumerable<BaseEntity> entities)
        {
            BindToEnties(dropDownList, entities, null, true, null, null);
        }

        public static void BindToEnties(this DropDownList dropDownList, IEnumerable<BaseEntity> entities, int? selected, string textField, string valueField)
        {
            BindToEnties(dropDownList, entities, selected, false, textField, valueField);
        }

        public static void BindToEnties(this DropDownList dropDownList, IEnumerable<BaseEntity> entities, string textField, string valueField)
        {
            BindToEnties(dropDownList, entities, null, false, textField, valueField);
        }

        public static void BindToEntiesWithEmptyRow(this DropDownList dropDownList, IEnumerable<BaseEntity> entities, int? selected, string textField, string valueField)
        {
            BindToEnties(dropDownList, entities, selected, true, textField, valueField);
        }

        public static void BindToEntiesWithEmptyRow(this DropDownList dropDownList, IEnumerable<BaseEntity> entities, string textField, string valueField)
        {
            BindToEnties(dropDownList, entities, null, true, textField, valueField);
        }

        public static void BindToEnumerable<T>(this DropDownList dropDownList, IEnumerable<T> dataSource, string textField, string valueField, bool insertEmptyRow, int? selected)
        {
            dropDownList.DataSource = dataSource;
            dropDownList.DataTextField = textField ?? "Text";
            dropDownList.DataValueField = valueField ?? "ID";
            dropDownList.DataBind();

            if (insertEmptyRow)
            {
                dropDownList.Items.Insert(0, string.Empty);
            }

            if (selected == null)
            {
                return;
            }
            
            ListItem item = dropDownList.Items.FindByValue(selected.ToString());
            
            if (item != null)
            {
                item.Selected = true;
            }
        }

        public static void BindToEnumerableWithEmptyRow<T>(this DropDownList dropDownList, IEnumerable<T> dataSource, string textField, string valueField, int? selected)
        {
            BindToEnumerable<T>(dropDownList, dataSource, textField, valueField, true, selected);
        }
        
        #endregion

        #region Repeater Extensions

        public static bool IsItem(this RepeaterItem item)
        {
            return item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem;
        }

        public static bool IsHeader(this RepeaterItem item)
        {
            return item.ItemType == ListItemType.Header;
        }

        #endregion
    }
}
