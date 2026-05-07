using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Windows.UI;

namespace AirportApp.Converters
{
    public class BoolToActiveBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isActive)
            {
                if (isActive)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 43, 184, 192));
                }
                else
                {
                    return new SolidColorBrush(Microsoft.UI.Colors.Transparent);
                }
            }
            return new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToInactiveBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isActive)
            {
                if (isActive)
                {
                    return new SolidColorBrush(Microsoft.UI.Colors.Transparent);
                }
                else
                {
                    return new SolidColorBrush(Color.FromArgb(255, 43, 184, 192));
                }
            }
            return new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToActiveBorderBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isActive)
            {
                return isActive ? null : new SolidColorBrush(Color.FromArgb(255, 63, 63, 70));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToActiveBorderThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isActive)
            {
                return isActive ? new Thickness(0) : new Thickness(1);
            }
            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
