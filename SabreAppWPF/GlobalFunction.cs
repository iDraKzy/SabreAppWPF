using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using System.Windows;

namespace SabreAppWPF
{
    static public class GlobalFunction
    {

        static public SQLiteCommand OpenDbConnection()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + GlobalVariable.path + ";UseUTF16Encoding=True;");
            connection.Open();
            SQLiteCommand cmd = new SQLiteCommand(connection);
            return cmd;
        }
        static public MainWindow GetMainWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    return (MainWindow)window;
                }
            }
            return null;
        }

        static public int ReturnTimestamp(DateTime date)
        {
            return (int)new DateTimeOffset(date).ToUnixTimeSeconds();
        }
    }
}
