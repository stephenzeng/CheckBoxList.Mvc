using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CheckBoxList.Mvc.Common;

namespace CheckBoxList.Mvc.Html
{
    public static class EnumCheckBoxListExtensions
    {
        public static MvcHtmlString EnumCheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            where TProperty : IEnumerable
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var name = ExpressionHelper.GetExpressionText(expression);
            var func = expression.Compile();
            var enumList = func(htmlHelper.ViewData.Model);

            var enumType = enumList.GetType().IsGenericType
                ? enumList.GetType().GetGenericArguments()[0]
                : enumList.GetType().GetElementType();

            if (!enumType.IsEnum)
                throw new ArgumentException();

            var availableValues = new List<Tuple<string, int, bool>>();
            foreach (var e in Enum.GetValues(enumType))
            {
                var selected = enumList.Cast<object>().Any(s => s.ToString() == e.ToString());
                availableValues.Add(new Tuple<string, int, bool>(GetDisplayName(e), (int)e, selected));
            }

            return SelectInternal(htmlHelper, name, availableValues, null);
        }

        private static MvcHtmlString SelectInternal(this HtmlHelper htmlHelper, string name, IEnumerable<Tuple<string, int, bool>> list, IDictionary<string, object> htmlAttributes)
        {
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException("name");
            }

            var listItemBuilder = new StringBuilder();
            foreach (var t in list)
            {
                listItemBuilder.AppendLine("<div>");
                var checkBox = string.Format(@"<input name=""{0}"" type=""checkbox"" value=""{1}"" {2} />", fullName, t.Item2, t.Item3 ? @"checked=""checked""" : string.Empty);
                listItemBuilder.AppendLine(checkBox);
                listItemBuilder.AppendLine(t.Item1);
                listItemBuilder.AppendLine("</div>");
            }

            var tagBuilder = new TagBuilder("div")
            {
                InnerHtml = listItemBuilder.ToString()
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.GenerateId(fullName);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        private static string GetDisplayName(object value)
        {
            var type = value.GetType();
            var member = type.GetMember(value.ToString());

            var displayAttributes = member[0].GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (displayAttributes != null && displayAttributes.Any())
                return displayAttributes.First().Name;

            var descriptionAttributes = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (descriptionAttributes != null && descriptionAttributes.Any())
                return descriptionAttributes.First().Description;

            return value.ToString();
        }
    }
}
