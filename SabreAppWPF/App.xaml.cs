using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SQLite;
using System.Windows.Controls;
using System.Globalization;
using Windows.Storage;
using Windows.ApplicationModel;

namespace SabreAppWPF
{
    public static class GlobalVariable
    {
        public static string GetAppVersion()
        {
            PackageVersion currentVersion = Package.Current.Id.Version;
            return $"{currentVersion.Major}.{currentVersion.Minor}.{currentVersion.Build}.{currentVersion.Revision}";
        }

        public static readonly string currentDbName = "Sabre-1.0.7.0.db";
        public static readonly string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, currentDbName);

        public static readonly CultureInfo culture = new CultureInfo("fr-FR");

        public static readonly Dictionary<string, string> specialCharacter = new Dictionary<string, string>()
        {
            {"CheckMark", "✓"},
            {"Cross", "❌" }
        };
        public static bool isLightMode = true;
    }
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }
    }
}
