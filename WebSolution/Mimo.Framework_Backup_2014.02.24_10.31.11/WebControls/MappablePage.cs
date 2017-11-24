using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Diagnostics;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;

namespace Mimo.Framework.WebControls
{
    public class MappablePage : Page
    {
        protected virtual void Map(object entity)
        {
            Controls.IterateRecursivelyStopOnIMappableAndUserControl(c =>
            {
                if (c is IMappableField)
                {
                    ((IMappableField)c).Map(entity);
                }
            });
        }

        protected virtual void Unmap(object entity)
        {
            Controls.IterateRecursivelyStopOnIMappableAndUserControl(c =>
            {
                if (c is IMappableField)
                {
                    ((IMappableField)c).Unmap(entity);
                }
            });
        }

        protected void RenderErrors(BaseValidationException ex, string validationGroup)
        {
            foreach (string detail in ex.Details)
            {
                var cv = new CustomValidator
                {
                    ErrorMessage = detail,
                    IsValid = false,
                    Display = ValidatorDisplay.None,
                    ValidationGroup = validationGroup
                };
                Page.Form.Controls.Add(cv);
            }        
        }

        protected void RenderErrors(BaseValidationException ex)
        {
            RenderErrors(ex, string.Empty);
        }

        protected void RenderErrors(string message)
        {
            RenderErrors(new BaseValidationException(message), string.Empty);
        }

        protected void RenderErrors(string message, string validationGroup)
         {
             RenderErrors(new BaseValidationException(message), validationGroup);    
         }

        protected void RenderErrors(List<string> messages, string validationGroup)
         {
             RenderErrors(new BaseValidationException(messages), validationGroup);
         }

        protected void RenderErrors(List<string> messages)
         {
             RenderErrors(new BaseValidationException(messages), string.Empty);
         }
    }
}
