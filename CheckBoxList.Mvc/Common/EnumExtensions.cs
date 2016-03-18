using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CheckBoxList.Mvc.Common
{
    public static class EnumExtensions
    {
        public static string GetEnumDisplayName<T>(this T value)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be enum type");

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

        public static IEnumerable<CheckBoxListItem> ToCheckboxListItems<T>(this IEnumerable<T> list) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enum type");
            }

            var availableValues = Enum.GetValues(typeof(T)).Cast<T>();

            return availableValues.Select(e => new CheckBoxListItem()
            {
                Text = e.GetEnumDisplayName(),
                Value = Convert.ToInt32(e).ToString(),
                IsChecked = list != null && list.Contains(e),
            });
        }

        public static IEnumerable<T> ToArray<T>(this IEnumerable<CheckBoxListItem> checkboxList) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enum type");
            }

            return checkboxList
                .Where(c => c.IsChecked)
                .Select(c => (T)Enum.Parse(typeof(T), c.Value));
        }
    }
}
