using global::projectmanagement.src;
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

        private Backend(SQLiteConnection connection)
        {
            this.connection = connection;
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
            string connectionString = GetConnectionString();
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            backend = new Backend(connection);
        }

        private static string GetConnectionString()
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            int projectManagementPosition = path.IndexOf("projectmanagement");
            path = path.Substring(0, projectManagementPosition);
            return "Data Source=" + path + "projectmanagement\\database\\database.db";
        }

        public static void CloseConnection()
        {
            if (backend != null && backend.connection != null)
            {
                backend.connection.Close();
            }
        }

        public static List<Mitarbeiter> GetMitarbeiterList()
        {
            if (backend.connection != null)
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
            else
            {
                // Handle null connection, e.g., log an error or throw an exception
                return new List<Mitarbeiter>(); // or return null, depending on your error handling strategy
            }
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
                command.Parameters.AddWithValue("@Nachname", employee.Nachname);
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
        public static void UpdateEmployee(Mitarbeiter employee)
        {
            try
            {
                string updateQuery = $"UPDATE {Mitarbeiter.GetTableName()} SET Tel_Nr = @Tel_Nr, Abteilung = @Abteilung WHERE Vorname = @Vorname AND Nachname = @Nachname";

                SQLiteCommand command = new SQLiteCommand(updateQuery, backend.connection);
                command.Parameters.AddWithValue("@Tel_Nr", employee.Tel_Nr);
                command.Parameters.AddWithValue("@Abteilung", employee.Abteilung);
                command.Parameters.AddWithValue("@Vorname", employee.Vorname);
                command.Parameters.AddWithValue("@Nachname", employee.Nachname);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating employee: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void SaveProjectToDatabase(Projekt project)
        {
            try
            {
                string insertQuery = $"INSERT INTO {Projekt.GetTableName()} (ProjektBezeichnung, VerantwortlichePersonalnummer, VonDatum, BisDatum) " +
                                     $"VALUES (@ProjektBezeichnung, @VerantwortlichePersonalnummer, @VonDatum, @BisDatum)";

                SQLiteCommand command = new SQLiteCommand(insertQuery, backend.connection);
                command.Parameters.AddWithValue("@ProjektBezeichnung", project.ProjektBezeichnung);
                command.Parameters.AddWithValue("@VerantwortlichePersonalnummer", project.VerantwortlichePersonalnummer);
                command.Parameters.AddWithValue("@VonDatum", project.VonDatum);
                command.Parameters.AddWithValue("@BisDatum", project.BisDatum);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving project to the database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void UpdateProject(Projekt project)
        {
            try
            {
                string updateQuery = $"UPDATE {Projekt.GetTableName()} SET ProjektBezeichnung = @ProjektBezeichnung, " +
                                     "VerantwortlichePersonalnummer = @VerantwortlichePersonalnummer, " +
                                     "VonDatum = @VonDatum, BisDatum = @BisDatum " +
                                     "WHERE ProjektID = @ProjektID";

                SQLiteCommand command = new SQLiteCommand(updateQuery, backend.connection);
                command.Parameters.AddWithValue("@ProjektBezeichnung", project.ProjektBezeichnung);
                command.Parameters.AddWithValue("@VerantwortlichePersonalnummer", project.VerantwortlichePersonalnummer);
                command.Parameters.AddWithValue("@VonDatum", project.VonDatum.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@BisDatum", project.BisDatum.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@ProjektID", project.projektID);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void DeleteProject(Projekt project)
        {
           try
           {
                string deleteQuery = $"DELETE FROM {Projekt.GetTableName()} WHERE ProjektID = @ProjektID";
                SQLiteCommand command = new SQLiteCommand(deleteQuery, backend.connection);
                command.Parameters.AddWithValue("@ProjektID", project.projektID);
                command.ExecuteNonQuery();
           }
           catch (Exception ex)
           {
               MessageBox.Show($"Error deleting project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
           }
        }
        public static void AddProjectPhase(Projektphasen projectPhase)
        {
            try
            {
                // Fügen Sie die Projektphase zur Datenbank hinzu
                string insertQuery = $"INSERT INTO {Projektphasen.GetTableName()} (Kennung, Bezeichnung, ProjID, Dauer, Vorg) " +
                                     "VALUES (@Kennung, @Bezeichnung, @ProjID, @Dauer, @Vorg)";

                SQLiteCommand command = new SQLiteCommand(insertQuery, backend.connection);
                command.Parameters.AddWithValue("@Kennung", projectPhase.Kennung);
                command.Parameters.AddWithValue("@Bezeichnung", projectPhase.Bezeichnung);
                command.Parameters.AddWithValue("@ProjID", projectPhase.ProjID);
                command.Parameters.AddWithValue("@Dauer", projectPhase.Dauer);
                command.Parameters.AddWithValue("@Vorg", projectPhase.Vorg);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding project phase: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static Projektphasen GetProjectPhaseDetails(int phaseID)
        {
            try
            {
                // Abrufen der Details einer bestimmten Projektphase aus der Datenbank
                string selectQuery = $"SELECT * FROM {Projektphasen.GetTableName()} WHERE PhasID = @PhasID";
                SQLiteCommand command = new SQLiteCommand(selectQuery, backend.connection);
                command.Parameters.AddWithValue("@PhasID", phaseID);

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // Erstellen und zurückgeben Sie ein Projektphasen-Objekt aus den Daten
                    return Projektphasen.GetDatabaseObject(reader);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting project phase details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }
        public static void AddNewProjectPhase(Projektphasen newPhase)
        {
            try
            {
                // Fügen Sie die neue Projektphase zur Datenbank hinzu
                string insertQuery = $"INSERT INTO {Projektphasen.GetTableName()} (Kennung, Bezeichnung, ProjID, Dauer, Vorg) " +
                                     "VALUES (@Kennung, @Bezeichnung, @ProjID, @Dauer, @Vorg)";

                SQLiteCommand command = new SQLiteCommand(insertQuery, backend.connection);
                command.Parameters.AddWithValue("@Kennung", newPhase.Kennung);
                command.Parameters.AddWithValue("@Bezeichnung", newPhase.Bezeichnung);
                command.Parameters.AddWithValue("@ProjID", newPhase.ProjID);
                command.Parameters.AddWithValue("@Dauer", newPhase.Dauer);
                command.Parameters.AddWithValue("@Vorg", newPhase.Vorg);

                command.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding new project phase: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
}
