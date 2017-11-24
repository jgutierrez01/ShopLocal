
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SAM.Web.Shop.Models;
using Resources.Models;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
using SAM.Web.Shop.Resources.Views.WorkStatus;
using System;
using SAM.Entities;
using System.Collections.Generic;


namespace SAM.Web.Shop.Factories
{
    public static class DropDownConfigurationFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DropDownConfiguration GetRequired(SearchControlNumberModel model, ModelStateDictionary modelState)
        {
            DropDownConfiguration config = new DropDownConfiguration
            {
                LabelText = CommonStrings.Project,
                Name = "ProjectId",
                IsRequired = true,
                RequiredErrorMessage = SearchStrings.Project_Required_ErrorMessage,
                IncludeEmptyOption = true,
                PreselectIfOnlyOne = true
            };


            if (model != null && model.ProjectId > 0)
            {
                config.SelectedId = model.ProjectId.ToString(CultureInfo.CurrentCulture);
            }
            if (model != null && model.SearchType == null)
            {
                model.SearchType = "cn";
            }
            if (model != null && string.IsNullOrEmpty(model.DateProcess))
            {
                model.DateProcess = DateTime.Now.ToShortDateString();
            }


            if (!modelState.IsValid && modelState[config.Name].Errors.Any())
            {
                config.HasErrors = true;
                config.Errors = modelState[config.Name].Errors;
            }


            return config;
        }


        public static DropDownConfiguration GetRequiredCuadrante(SQModel model, ModelStateDictionary modelState, int opc)
        {            
            DropDownConfiguration config = new DropDownConfiguration
            {                
                LabelText = "Cuadrante",                
                //Name = "QuadrantIdCADD",
                Name = opc == 1 ? "QuadrantIdCADD" : "QuadrantIdNCADD",
                IsRequired = true,
                RequiredErrorMessage = SearchStrings.QuadrantName_Required_ErrorMessage,
                IncludeEmptyOption = true,
                PreselectIfOnlyOne = true//,
                //SelectedId = model.QuadrantIdCADD.ToString()
            };
            try
            {
                if (!modelState.IsValid && modelState[config.Name].Errors.Any())
                {
                    config.HasErrors = true;
                    config.Errors = modelState[config.Name].Errors;
                }
            }
            catch (Exception e)
            {
                return config;
            }

            return config;
        }

        public static DropDownConfiguration GetRequiredADD(SQModel model, ModelStateDictionary modelState)
        {
            DropDownConfiguration config = new DropDownConfiguration
            {
                LabelText = CommonStrings.Project,
                Name = "ProjectIdADD",
                IsRequired = true,
                RequiredErrorMessage = SearchStrings.Project_Required_ErrorMessage,
                IncludeEmptyOption = true,
                PreselectIfOnlyOne = true
            };


            if (model != null && model.ProjectIdADD > 0)
            {
                config.SelectedId = model.ProjectIdADD.ToString(CultureInfo.CurrentCulture);
            }
            if (model != null && model.SearchTypeADD == null)
            {
                model.SearchTypeADD = "c";
            }
            try
            {
                if (modelState.IsValid)
                {
                    if (!modelState.IsValid && modelState[config.Name].Errors.Any())
                    {
                        config.HasErrors = true;
                        config.Errors = modelState[config.Name].Errors;
                    }
                }
                else
                {
                    config.HasErrors = true;

                    if (modelState[config.Name] != null)
                        config.Errors = modelState[config.Name].Errors;
                }
            }
            catch (Exception e)
            {
                return config;
            }            
            return config;
        }

        public static DropDownConfiguration GetRequiredEdit(SQModel model, ModelStateDictionary modelState)
        {
            DropDownConfiguration config = new DropDownConfiguration
            {
                LabelText = CommonStrings.Project,
                Name = "ProjectIdEditar",
                IsRequired = true,
                RequiredErrorMessage = SearchStrings.Project_Required_ErrorMessage,
                IncludeEmptyOption = true,
                PreselectIfOnlyOne = true                
            };


            if (model != null && model.ProjectIdEditar > 0)
            {
                config.SelectedId = model.ProjectIdEditar.ToString(CultureInfo.CurrentCulture);
            }
            if (model != null && model.SearchTypeEdit == null)
            {
                model.SearchTypeEdit = "c";
            }
            try
            {
                if (modelState.IsValid)
                {
                    if (!modelState.IsValid && modelState[config.Name].Errors.Any())
                    {
                        config.HasErrors = true;
                        config.Errors = modelState[config.Name].Errors;
                    }

                }
                else
                {
                    config.HasErrors = true;

                    if (modelState[config.Name] != null)
                        config.Errors = modelState[config.Name].Errors;
                }
            }
            catch (Exception)
            {
                return config;                
            }
            
            return config;
        }

        public static DropDownConfiguration GetRequired(ShipmentOutModel model, ModelStateDictionary modelState)
        {
            DropDownConfiguration config = new DropDownConfiguration
            {
                LabelText = WorkStatusStrings.NumberShipment,
                Name = "ShipmentId",
                IsRequired = true,
                RequiredErrorMessage = WorkStatusStrings.NumberShipment_Required_ErrorMessage,
                IncludeEmptyOption = true,
                PreselectIfOnlyOne = true
            };


            if (model != null && model.ShipmentId > 0)
            {
                config.SelectedId = model.ShipmentId.ToString(CultureInfo.CurrentCulture);
            }


            if (!modelState.IsValid && modelState[config.Name].Errors.Any())
            {
                config.HasErrors = true;
                config.Errors = modelState[config.Name].Errors;
            }


            return config;
        }


        public static DropDownConfiguration GetRequired(WorkstatusModel model)
        {
            DropDownConfiguration config = new DropDownConfiguration
            {
                LabelText = WorkStatusStrings.TypeReport,
                Name = "TypeReportId",
                IsRequired = false,
                IncludeEmptyOption = true,
                PreselectIfOnlyOne = true
            };


            if (model != null && model.TypeReportId >= 0)
            {
                config.SelectedId = model.TypeReportId.ToString(CultureInfo.CurrentCulture);
            }

            return config;
        }

        public static DropDownConfiguration GetRequired(LocationModel model, ModelStateDictionary modelState)
        {
            DropDownConfiguration config = new DropDownConfiguration
            {
                LabelText = WorkStatusStrings.Quadrant,
                Name = "QuadrantId",
                IncludeEmptyOption = true,
                PreselectIfOnlyOne = true

            };


            if (model != null && model.QuadrantId >= 0)
            {
                config.SelectedId = model.QuadrantId.ToString(CultureInfo.CurrentCulture);
            }

            if (!modelState.IsValid && modelState[config.Name].Errors.Any())
            {
                config.HasErrors = true;
                config.Errors = modelState[config.Name].Errors;
            }

            return config;
        }
    }
}



