/*Copyright (c) 2009 T.Evelyn (evescode@gmail.com) 

All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1.Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2.Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in 
 the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 

THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS 

BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 

GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 

LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 

DAMAGE.*/


using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Data;

namespace CircularGaugeControl
{

    /// <summary>
    /// Converts the given color to a SolidColorBrush
    /// </summary>
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value == null) return BindingNotification.UnsetValue;
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            return BindingNotification.UnsetValue;
        }
    }


    /// <summary>   
    /// A type converter for converting image offset into render transform  
    /// </summary>   
    public class ImageOffsetConverter : IValueConverter
    {
        public object Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value == null) return BindingNotification.UnsetValue;
            double dblVal = (double)value;
            var tt = new TranslateTransform
            {
                Y = dblVal
            };
            return tt;
        }

        public object ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            return BindingNotification.UnsetValue;
        }
    }


    /// <summary>
    /// Converts radius to diameter
    /// </summary>
    public class RadiusToDiameterConverter : IValueConverter
    {
        public object Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value == null) return BindingNotification.UnsetValue;
            double dblVal = (double)value;

            return dblVal * 2;
        }

        public object ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value == null) return BindingNotification.UnsetValue;
            double dblVal = (double)value;

            return dblVal / 2;
        }
    }

    /// <summary>
    /// Calculates the pointer position
    /// </summary>
    public class PointerCenterConverter : IValueConverter
    {
        public object Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value == null) return BindingNotification.UnsetValue;
            double dblVal = (double)value;
            var tg = new TransformGroup();
            var rt = new RotateTransform();
            var tt = new TranslateTransform
            {
                X = dblVal / 2
            };
            tg.Children.Add(rt);
            tg.Children.Add(tt);

            return tg;
        }

        public object ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            return BindingNotification.UnsetValue;
        }
    }

    /// <summary>
    /// Calculates the range indicator light position
    /// </summary>
    public class RangeIndicatorLightPositionConverter : IValueConverter
    {
        public object Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value == null) return BindingNotification.UnsetValue;
            double dblVal = (double)value;
            var tg = new TransformGroup();
            var rt = new RotateTransform();
            var tt = new TranslateTransform
            {
                Y = dblVal
            };
            tg.Children.Add(rt);
            tg.Children.Add(tt);

            return tg;
        }

        public object ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            return BindingNotification.UnsetValue;
        }
    }

    /// <summary>
    /// Converts the given Size to height and width
    /// </summary>
    public class SizeConverter : IValueConverter
    {
        public object Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value == null) return BindingNotification.UnsetValue;
            double i = 0;
            Size s = (Size)value;
            if (parameter?.ToString() == "Height")
            {
                i = s.Height;
            }
            else if (parameter?.ToString() == "Width")
            {
                i = s.Width;
            }

            return i;

        }

        public object ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            return BindingNotification.UnsetValue;
        }
    }

    /// <summary>
    /// Scaling factor for drawing the glass effect.
    /// </summary>
    public class GlassEffectWidthConverter : IValueConverter
    {
        public object Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value == null) return BindingNotification.UnsetValue;
            double dbl = (double)value;
            return dbl * 2 * 0.94;

        }

        public object ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            return BindingNotification.UnsetValue;
        }
    }

    /// <summary>
    /// Converts background color to Gradient effect
    /// </summary>
    public class BackgroundColorConverter : IValueConverter
    {
        public object Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value == null) return BindingNotification.UnsetValue;
            Color c = (Color)value;
            var radBrush = new RadialGradientBrush();
            var g1 = new GradientStop
            {
                Color = Color.FromArgb(0xFF, 0xAF, 0xB2, 0xB0)
            };
            var g2 = new GradientStop
            {
                Offset = 0.982,
                Color = c
            };
            radBrush.GradientStops.Add(g1);
            radBrush.GradientStops.Add(g2);
            return radBrush;

        }

        public object ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            return BindingNotification.UnsetValue;
        }
    }
}
