using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DesignGyakorlas.Models.Converters
{
    public class PolarToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LinearGradientBrush lgb = new LinearGradientBrush();
            lgb.StartPoint = new Point(0,0);
            lgb.EndPoint = new Point(1,1);
           

            double? number = (double?)value;
            if (number > 0)
            {
                lgb.GradientStops.Add(new GradientStop(Color.FromRgb(0, 206, 24), 1.0));
                lgb.GradientStops.Add(new GradientStop(Color.FromRgb(98, 247, 116),0.0));
                return lgb;
            }
            else if (number < 0)
            {
                lgb.GradientStops.Add(new GradientStop(Color.FromRgb(244, 0, 0), 1.0));
                lgb.GradientStops.Add(new GradientStop(Color.FromRgb(255, 209, 209), 0.15));
                return lgb;
            } 
            else
                return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
