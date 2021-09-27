using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace AdapterGenerator.UserInterface.Converters {
  internal class ListToStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (targetType != typeof(string)) throw new InvalidOperationException("The target must be a String");

      return string.Join("\r\n", ((ObservableCollection<string>) value).ToArray());
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      if (targetType != typeof(ObservableCollection<string>))
        throw new InvalidOperationException("The target must be an ObservableCollection<string>");

      var list = new ObservableCollection<string>();

      foreach (var s in value.ToString().Split('\r')) {
        var val = s.Replace('\n', ' ');
        val = val.Trim();
        if (!string.IsNullOrEmpty(val)) list.Add(val);
      }

      return list;
    }
  }
}