using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;

namespace SabreAppWPF.LightDark
{
    public static class AppTheme
    {
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        private static string textColor;
        private static string backgroundColor;
        private static string buttonHoverColor;
        private static string buttonClickColor;
        private static string borderColor;
        private static string buttonTextDisabledColor;

        public static string TextColor 
        {
            get { return textColor; }
            set
            {
                textColor = value;
                NotifyStaticPropertyChanged("TextColor");
            }
        }
        public static string BackgroundColor 
        { 
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                NotifyStaticPropertyChanged("BackgroundColor");
            }
        }

        public static string ButtonHoverColor
        {
            get { return buttonHoverColor; }
            set
            {
                buttonHoverColor = value;
                NotifyStaticPropertyChanged("ButtonHoverColor");
            }
        }

        public static string ButtonClickColor
        {
            get { return buttonClickColor; }
            set 
            {
                buttonClickColor = value;
                NotifyStaticPropertyChanged("ButtonClickColor");
            }
        }

        public static string BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                NotifyStaticPropertyChanged("BorderColor");
            }
        }

        public static string ButtonTextDisabledColor
        {
            get { return buttonTextDisabledColor; }
            set
            {
                buttonTextDisabledColor = value;
                NotifyStaticPropertyChanged("ButtonTextDisabledColor");
            }
        }
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            if (StaticPropertyChanged != null)
                StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
    }
}
