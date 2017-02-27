using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AntragsVerwaltung.Helpers.Converters
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "?";
            }
            var original = value.ToString();
            switch (original)
            {
                case "deleted":
                    return "LÖSCHEN";
                case "modified":
                    return "ÄNDERN";
                case "added":
                    return "HINZUFÜGEN";
                default:
                    return "?";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
