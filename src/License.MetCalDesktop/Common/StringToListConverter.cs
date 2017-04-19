using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using License.MetCalDesktop.Model;

namespace License.MetCalDesktop.Common
{
    /// <summary>
    /// String to list object converter logic 
    /// </summary>
    public class StringToListConverter : IValueConverter
    {
        /// <summary>
        /// It converts string data into object 
        /// </summary>
        /// <param name="value">input value</param>
        /// <param name="targetType">target type</param>
        /// <param name="parameter">parameter type</param>
        /// <param name="culture">culture info</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var featureList = (ICollection<Feature>)value;
            if (featureList != null && featureList.Count > 0)
            {
                StringBuilder str = new StringBuilder();
                foreach (var feature in featureList)
                    str.Append(feature.Name + ", ");
                return str.ToString().Remove(str.Length - 2);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Convert back to string logic
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="targetType">target type</param>
        /// <param name="parameter">parameter info</param>
        /// <param name="culture">culture info</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
