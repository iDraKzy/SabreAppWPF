using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SQLite;

namespace SabreAppWPF
{
    public static class GlobalVariable
    {
        public static readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Sabre.db");

    }
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }
}
