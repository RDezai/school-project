using projectmanagement.src;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace projectmanagement
{
    public class Backend
    {
        private static Backend? backend;
        private SQLiteConnection connection;

        public Backend()
        {
            connection = new SQLiteConnection(GetConnectionString());
            try
            {
                Console.WriteLine("Verbindungsaufbau...");
                connection.Open();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Verbindungsaufbau fehlgeschlagen: " + exception);
            }
        }

        public static void CreateBackend()
        {
            backend = new Backend();
        }

        private static string GetConnectionString()
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            int projectManagementPosition = path.IndexOf("projectmanagement");
            path = path.Substring(0, projectManagementPosition);
            return "Data Source=" + path + "projectmanagement\\database\\database.db";
        }

        public static List<Mitarbeiter> GetMitarbeiterList()
        {
            string table = Mitarbeiter.GetTableName();
            string selectQuery = "SELECT * FROM " + table;

            SQLiteCommand command = new SQLiteCommand(selectQuery, backend.connection);
            SQLiteDataReader reader = command.ExecuteReader();

            List<Mitarbeiter> mitarbeiterList = new List<Mitarbeiter>();
            while (reader.Read())
            {
                Mitarbeiter databaseMitarbeiter = Mitarbeiter.GetDatabaseObject(reader);
                mitarbeiterList.Add(databaseMitarbeiter);
            }

            return mitarbeiterList;
        }

        public static List<Projekt> GetProjectList()
        {
            string table = Projekt.GetTableName();
            string selectQuery = "SELECT * FROM " + table;

            SQLiteCommand command = new SQLiteCommand(selectQuery, backend.connection);
            SQLiteDataReader reader = command.ExecuteReader();

            List<Projekt> projectList = new List<Projekt>();
            while (reader.Read())
            {
                Projekt databaseProject = Projekt.GetDatabaseObject(reader);
                projectList.Add(databaseProject);
            }

            return projectList;
        }

        public static List<Projektphasen> GetProjectPhasenList()
        {
            string table = Projektphasen.GetTableName();
            string selectQuery = "SELECT * FROM " + table;

            SQLiteCommand command = new SQLiteCommand(selectQuery, backend.connection);
            SQLiteDataReader reader = command.ExecuteReader();

            List<Projektphasen> projectPhasenList = new List<Projektphasen>();
            while (reader.Read())
            {
                Projektphasen databaseProjektPhasen = Projektphasen.GetDatabaseObject(reader);
                projectPhasenList.Add(databaseProjektPhasen);
            }

            return projectPhasenList;
        }

        public static void DeleteEmployee(Mitarbeiter employee)
        {
            try
            {
                string deleteQuery = $"DELETE FROM {Mitarbeiter.GetTableName()} WHERE Vorname = @Vorname AND Nachname = @Nachname";
                SQLiteCommand command = new SQLiteCommand(deleteQuery, backend.connection);
                command.Parameters.AddWithValue("@Vorname", employee.Vorname);
                command.Parameters.AddWithValue("@Vorname", employee.Nachname);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting employee: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void SaveEmployeeToDatabase(Mitarbeiter employee)
        {
            try
            {
                string insertQuery = $"INSERT INTO {Mitarbeiter.GetTableName()} (Vorname, Nachname, Tel_Nr, Abteilung) " +
                                     $"VALUES (@Vorname, @Nachname, @Tel_Nr, @Abteilung)";

                SQLiteCommand command = new SQLiteCommand(insertQuery, backend.connection);
                command.Parameters.AddWithValue("@Vorname", employee.Vorname);
                command.Parameters.AddWithValue("@Nachname", employee.Nachname);
                command.Parameters.AddWithValue("@Tel_Nr", employee.Tel_Nr);
                command.Parameters.AddWithValue("@Abteilung", employee.Abteilung);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving employee to the database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
