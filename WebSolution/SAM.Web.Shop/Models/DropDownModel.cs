using System;

namespace SAM.Web.Shop.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DropDownModel
    {
        public DropDownConfiguration Config { get; set; }
        public DropDownItem[] Items { get; set; }

        public DropDownModel() : this (new DropDownConfiguration()) { }

        public DropDownModel(DropDownConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            Config = config;
            Items = new DropDownItem[0];
        }

        public string RenderValidationAttributtes()
        {
            if (!Config.IsRequired)
            {
                return string.Empty;
            }

            return string.Format(@"data-val=""true"" data-val-required=""{0}""", Config.RequiredErrorMessage);
        }

        public string RenderSelected(DropDownItem item)
        {
            if (MatchesSelection(item) || IsSingleItem())
            {
                return "selected=\"selected\"";
            }

            return string.Empty;
        }

        private bool IsSingleItem()
        {
            return (Config.PreselectIfOnlyOne && Items.Length == 1);
        }

        private bool MatchesSelection(DropDownItem item)
        {
            return (Config.SelectedId != null && Config.SelectedId.Equals(item.Id, StringComparison.OrdinalIgnoreCase));
        }

        public string RenderErrorPrefix()
        {
            if (Config.HasErrors)
            {
                return "input-validation-error ";
            }

            return string.Empty;
        }
    }
}