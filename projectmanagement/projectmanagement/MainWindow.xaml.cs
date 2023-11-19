using projectmanagement.src;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace projectmanagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TextBlock textBlock = new TextBlock();
            string connectionString = GetConnectionString();

            try
            {
                textBlock.Text = "Verbindungsaufbau\n";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    ShowProjektTable(connection, textBlock);
                    connection.Close();
                }
            }
            catch (Exception exception) { textBlock.Text=("Datenbankverbindung fehlgeschlagen. " + connectionString + "\n"+ exception); }

            DBRet.Content = textBlock;
        }

        public void ShowMitarbeiterTable(SQLiteConnection connection, TextBlock textBlock)
        {
            string table = Mitarbeiter.GetTableName();
            string selectQuery = "SELECT * FROM " + table;

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        textBlock.Text +=  Mitarbeiter.GetDatabaseObject(reader);
                    }
                }
            }
        }

        public void ShowProjektTable(SQLiteConnection connection, TextBlock textBlock)
        {
            string table = Projekt.GetTableName();
            string selectQuery = "SELECT * FROM " + table;

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        textBlock.Text += Projekt.GetDatabaseObject(reader);
                    }
                }
            }
        }

        private static string GetConnectionString()
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            int projectManagementPosition = path.IndexOf("projectmanagement");
            path = path.Substring(0, projectManagementPosition);
            return "Data Source=" + path + "projectmanagement\\database\\database.db";
        }
    }
}
