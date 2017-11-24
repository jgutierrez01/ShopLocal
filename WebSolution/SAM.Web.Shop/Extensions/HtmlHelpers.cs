using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace SAM.Web.Shop.Extensions
{
    public static class HtmlHelpers
    {
        private const string CalendarWrapper =
            @"<div class=""input-group date"">
                {0}
                <span class=""input-group-addon""><i class=""glyphicon glyphicon-th""></i></span>
            </div>";


        /// <summary>
        /// Convenience method that reads metadata from attributes in order to render more appropriate HTML.
        /// The supported attributes are:
        /// - Required: Adds or appends the "required" class to the textbox
        /// - StringLength: Adds the maxlength attribute to the markup
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString StyledTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            IDictionary<string, object> attributes = MergeWithDataAnnotations(expression, htmlAttributes);
            AppendCss(attributes, "form-control");

            return htmlHelper.TextBoxFor(expression, attributes);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString StyledLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            IDictionary<string, object> attributes = htmlAttributes != null ? new RouteValueDictionary(htmlAttributes) : new RouteValueDictionary();
            AppendCss(attributes, "control-label");

            return htmlHelper.LabelFor(expression, attributes);
        }

        /// <summary>
        /// Convenience method that reads metadata from attributes in order to render more appropriate HTML.
        /// The supported attributes are:
        /// - Required: Adds or appends the "required" class to the textbox
        /// - StringLength: Adds the maxlength attribute to the markup
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString StyledPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            IDictionary<string, object> attributes = MergeWithDataAnnotations(expression, htmlAttributes);
            AppendCss(attributes, "form-control");

            return htmlHelper.PasswordFor(expression, attributes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            IDictionary<string, object> attributes = MergeWithDataAnnotations(expression, htmlAttributes);
            attributes.Add("readonly", "true");
            AppendCss(attributes, "form-control");

            string dateFormat = "dd/MM/yyyy";

            if(Thread.CurrentThread.CurrentCulture.Name.StartsWith("en", StringComparison.OrdinalIgnoreCase))
            {
                dateFormat = "MM/dd/yyyy";
            }

            MvcHtmlString textBox = htmlHelper.TextBoxFor(expression, "{0:" + dateFormat + "}", attributes);

            return MvcHtmlString.Create(string.Format(CalendarWrapper, textBox.ToHtmlString()));
        }

        /// <summary>
        /// Reads the meta-data of the property and renders the appropriate html
        /// based on that.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        private static IDictionary<string, object> MergeWithDataAnnotations<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            MemberExpression member = expression.Body as MemberExpression;

            MaxLengthAttribute lengthAttr = member.Member
                                                  .GetCustomAttributes(typeof(MaxLengthAttribute), false)
                                                  .FirstOrDefault() as MaxLengthAttribute;

            RequiredAttribute reqAttr = member.Member
                                              .GetCustomAttributes(typeof(RequiredAttribute), false)
                                              .FirstOrDefault() as RequiredAttribute;

            IDictionary<string, object> attributes = htmlAttributes != null ? new RouteValueDictionary(htmlAttributes) : new RouteValueDictionary();

            if (lengthAttr != null)
            {
                attributes.Add("maxlength", lengthAttr.Length);
            }

            if (reqAttr != null)
            {
                AppendOrAddRequiredCss(attributes);
            }

            return attributes;
        }

        /// <summary>
        /// If the key "class" is already contained in the attributes dictionary then it appends the string "required-field" to it.
        /// Otherwise it creates the key with the value "required"
        /// </summary>
        /// <param name="attributes"></param>
        private static void AppendOrAddRequiredCss(IDictionary<string, object> attributes)
        {
            AppendCss(attributes, "required-field");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="cssClass"></param>
        private static void AppendCss(IDictionary<string, object> attributes, string cssClass)
        {
            if (attributes.ContainsKey("class"))
            {
                attributes["class"] = (attributes["class"] as string) + " " + cssClass;
            }
            else
            {
                attributes["class"] = cssClass;
            }
        }
    }
}