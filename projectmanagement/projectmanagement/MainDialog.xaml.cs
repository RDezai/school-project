using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
// MainDialog.xaml.cs
using projectmanagement;

namespace YourNamespace
{
    public partial class MainDialog : Window
    {
        public MainDialog()
        {
            InitializeComponent();
            TextBlock textBlock = new TextBlock();
            string connectionString = GetConnectionString();

            try
            {
                textBlock.Text = "Verbindungsaufbau\n";
                SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();
                ShowProjektTable(connection, textBlock);
                connection.Close();
            }
            catch (Exception exception) { textBlock.Text = ("Datenbankverbindung fehlgeschlagen. " + connectionString + "\n" + exception); }

        }

        private void MitarbeiterButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the EmployeeWindow
            EmployeeWindow employeeWindow = new EmployeeWindow();
            employeeWindow.Show();
        }

        public void ShowMitarbeiterTable(SQLiteConnection connection, TextBlock textBlock)
        {
            string table = Mitarbeiter.GetTableName();
            string selectQuery = "SELECT * FROM " + table;

            SQLiteCommand command = new SQLiteCommand(selectQuery, connection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                textBlock.Text += Mitarbeiter.GetDatabaseObject(reader);
            }
        }

        public void ShowProjektTable(SQLiteConnection connection, TextBlock textBlock)
        {
            string table = Projekt.GetTableName();
            string selectQuery = "SELECT * FROM " + table;

            SQLiteCommand command = new SQLiteCommand(selectQuery, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                textBlock.Text += Projekt.GetDatabaseObject(reader);
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

