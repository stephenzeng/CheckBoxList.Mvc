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

namespace CheckBoxList.Mvc.Html
{
    public static class CheckBoxListExtensions
    {
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<CheckBoxListItem> checkboxList)
        {
            return CheckBoxListHelper(htmlHelper, name, checkboxList, null);
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<CheckBoxListItem> checkboxList, object htmlAttributes)
        {
            return CheckBoxListHelper(htmlHelper, name, checkboxList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<CheckBoxListItem> checkboxList, IDictionary<string, object> htmlAttributes)
        {
            return CheckBoxListHelper(htmlHelper, name, checkboxList, htmlAttributes);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            where TProperty : IEnumerable<CheckBoxListItem>
        {
            return CheckBoxListFor(htmlHelper, expression, null);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            where TProperty : IEnumerable<CheckBoxListItem>
        {
            return CheckBoxListFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
            where TProperty : IEnumerable<CheckBoxListItem>
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var name = ExpressionHelper.GetExpressionText(expression);

            var func = expression.Compile();
            var checkboxList = func(htmlHelper.ViewData.Model) as IEnumerable<CheckBoxListItem>;

            return CheckBoxListHelper(htmlHelper, name, checkboxList, htmlAttributes);
        }

        public static MvcHtmlString EnumCheckBoxList<T>(this HtmlHelper htmlHelper, string name, IEnumerable<T> list) where T : struct
        {
            return EnumCheckBoxList(htmlHelper, name, list, null);
        }

        public static MvcHtmlString EnumCheckBoxList<T>(this HtmlHelper htmlHelper, string name, IEnumerable<T> list, object htmlAttributes) where T : struct
        {
            return EnumCheckBoxList(htmlHelper, name, list, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString EnumCheckBoxList<T>(this HtmlHelper htmlHelper, string name, IEnumerable<T> list, IDictionary<string, object> htmlAttributes) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enum type");

            //text, value and if selected
            var tupleList = new List<Tuple<string, int, bool>>();
            foreach (var value in Enum.GetValues(typeof(T)).Cast<T>())
            {
                var selected = list.Contains(value);
                tupleList.Add(new Tuple<string, int, bool>(GetDisplayName(value), Convert.ToInt32(value), selected));
            }

            return EnumCheckBoxListHelper(htmlHelper, name, tupleList, htmlAttributes);
        }

        public static MvcHtmlString EnumCheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            where TProperty : IEnumerable
        {
            return EnumCheckBoxListFor(htmlHelper, expression, null);
        }

        public static MvcHtmlString EnumCheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            where TProperty : IEnumerable
        {
            return EnumCheckBoxListFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString EnumCheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
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
                throw new ArgumentException("Must be a list of enum type");

            var tupleList = new List<Tuple<string, int, bool>>();
            foreach (var value in Enum.GetValues(enumType))
            {
                var selected = enumList.Cast<object>().Any(s => s.ToString() == value.ToString());
                tupleList.Add(new Tuple<string, int, bool>(GetDisplayName(value), (int)value, selected));
            }

            return EnumCheckBoxListHelper(htmlHelper, name, tupleList, htmlAttributes);
        }

        private static MvcHtmlString CheckBoxListHelper(HtmlHelper htmlHelper, string name, IEnumerable<CheckBoxListItem> checkboxList, IDictionary<string, object> htmlAttributes)
        {
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException("name");
            }

            var listItemBuilder = BuildCheckBoxListItems(htmlHelper, name, checkboxList.ToList());

            var tagBuilder = new TagBuilder("div")
            {
                InnerHtml = listItemBuilder.ToString()
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.GenerateId(fullName);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
        
        private static StringBuilder BuildCheckBoxListItems(this HtmlHelper htmlHelper, string name, IList<CheckBoxListItem> list)
        {
            var listItemBuilder = new StringBuilder();

            for (var i = 0; i < list.Count(); i++)
            {
                var item = list[i];

                var checkbox = htmlHelper.CheckBox(GetChildControlName(name, i, "IsChecked"), item.IsChecked);
                var text = htmlHelper.Hidden(GetChildControlName(name, i, "Text"), item.Text);
                var value = htmlHelper.Hidden(GetChildControlName(name, i, "Value"), item.Value);

                var sb = new StringBuilder();
                sb.AppendLine("<div>");
                sb.AppendLine(checkbox.ToHtmlString());
                sb.AppendLine(text.ToHtmlString());
                sb.AppendLine(value.ToHtmlString());
                sb.AppendLine(HttpUtility.HtmlEncode(item.Text));
                sb.AppendLine("</div>");

                listItemBuilder.AppendLine(sb.ToString());
            }

            return listItemBuilder;
        }

        private static string GetChildControlName(string parentName, int index, string childName)
        {
            return string.Format("{0}[{1}].{2}", parentName, index, childName);
        }
        
        private static MvcHtmlString EnumCheckBoxListHelper(HtmlHelper htmlHelper, string name, IEnumerable<Tuple<string, int, bool>> list, IDictionary<string, object> htmlAttributes)
        {
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException("name");
            }

            var listItemBuilder = BuildEnumCheckBoxListItems(fullName, list);

            var tagBuilder = new TagBuilder("div")
            {
                InnerHtml = listItemBuilder.ToString()
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.GenerateId(fullName);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        private static MvcHtmlString BuildEnumCheckBoxListItems(string name, IEnumerable<Tuple<string, int, bool>> list)
        {
            var listItemBuilder = new StringBuilder();
            foreach (var t in list)
            {
                listItemBuilder.AppendLine("<div>");
                var checkBox = string.Format(@"<input name=""{0}"" type=""checkbox"" value=""{1}"" {2} />", name, t.Item2, t.Item3 ? @"checked=""checked""" : string.Empty);
                listItemBuilder.AppendLine(checkBox);
                listItemBuilder.AppendLine(t.Item1);
                listItemBuilder.AppendLine("</div>");
            }

            return new MvcHtmlString(listItemBuilder.ToString());
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
