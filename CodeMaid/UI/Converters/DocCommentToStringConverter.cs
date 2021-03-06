#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Xml.Linq;

namespace SteveCadwallader.CodeMaid.UI.Converters
{
    /// <summary>
    /// Converts the specified doc comment into a simpler string.
    /// </summary>
    public class DocCommentToStringConverter : IValueConverter
    {
        /// <summary>
        /// A default instance of the <see cref="DocCommentToStringConverter" />.
        /// </summary>
        public static DocCommentToStringConverter Default = new DocCommentToStringConverter();

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = value as string;
            if (string.IsNullOrEmpty(str)) return string.Empty;

            try
            {
                var xElement = XElement.Parse(str);

                var summaryTag = xElement.Descendants("summary").FirstOrDefault();
                if (summaryTag == null) return string.Empty;

                // Get the Inner XML for the summary tag.
                var result = GetInnerXML(summaryTag);

                // Replace para tags with two new lines.
                result = Regex.Replace(result, @"</?para ?/?> ?", Environment.NewLine + Environment.NewLine);

                // Reduce three of more new lines down to two.
                result = Regex.Replace(result, @"(\r?\n){3,}", Environment.NewLine + Environment.NewLine);

                // Trim off any leading/trailing whitespace including newlines.
                result = result.Trim();

                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the inner XML for the specified XElement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The inner XML for the specified XElement.</returns>
        private static string GetInnerXML(XElement element)
        {
            var reader = element.CreateReader();
            reader.MoveToContent();
            return reader.ReadInnerXml();
        }
    }
}