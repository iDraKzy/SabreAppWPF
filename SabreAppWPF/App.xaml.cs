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

namespace SabreAppWPF
{
    public static class GlobalVariable
    {
        public static readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Sabre.db");

        public static readonly CultureInfo culture = new CultureInfo("fr-FR");
    }
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }
}
