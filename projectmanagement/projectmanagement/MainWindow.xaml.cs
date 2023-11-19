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

                    string selectQuery = "SELECT * FROM Mitarbeiter";

                    using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                int employeeID = reader.GetInt32(0);
                                string firstName = reader.GetString(1);
                                string lastName = reader.GetString(2);
                                //string position = reader.GetString(3);

                                textBlock.Text += ($"MitarbeiterID: {employeeID}, Vorname: {firstName}, Nachname: {lastName}\n");
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception exception) { textBlock.Text=("Datenbankverbindung fehlgeschlagen. " + connectionString + "\n"+ exception); }

            DBRet.Content = textBlock;
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
