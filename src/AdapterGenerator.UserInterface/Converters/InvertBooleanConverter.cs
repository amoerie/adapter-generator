using System;
using System.Globalization;
using System.Windows.Data;

namespace AdapterGenerator.UserInterface.Converters {
  [ValueConversion(typeof(bool), typeof(bool))]
  public class InvertBooleanConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var original = (bool) value;
      return !original;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      var original = (bool) value;
      return !original;
    }
  }
}