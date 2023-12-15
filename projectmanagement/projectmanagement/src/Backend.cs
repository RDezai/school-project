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


        public static List<T> ExecuteQuery<T>(string tableName, string selectQuery, Func<SQLiteDataReader, T> getObject)
        {
            List<T> resultList = new List<T>();
            try
            {
                SQLiteCommand command = new SQLiteCommand(selectQuery, backend.connection);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    T databaseObject = getObject(reader);
                    resultList.Add(databaseObject);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Ausführen der Abfrage für Tabelle {tableName}: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return resultList;
        }

        public static void ExecuteNonQuery(string query, List<SQLiteParameter> parameters)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(query, backend.connection);
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Ausführen der Nicht-Abfrage: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static List<employee> GetEmployeesList()
        {
            string table = employee.GetTableName();
            string selectQuery = "SELECT * FROM " + table;

            return ExecuteQuery(table, selectQuery, employee.GetDatabaseObject);
        }

        public static List<Project> GetProjectList()
        {
            string table = Project.GetTableName();
            string selectQuery = "SELECT * FROM " + table;

            return ExecuteQuery(table, selectQuery, Project.GetDatabaseObject);
        }

        public static List<Projectphases> GetProjectPhasenList()
        {
            string table = Projectphases.GetTableName();
            string selectQuery = "SELECT * FROM " + table;

            return ExecuteQuery(table, selectQuery, Projectphases.GetDatabaseObject);
        }

        public static void DeleteEmployee(employee employee)
        {
            try
            {
                string deleteQuery = $"DELETE FROM {employee.GetTableName()} WHERE Vorname = @Vorname AND Nachname = @Nachname";
                ExecuteNonQuery(deleteQuery, new List<SQLiteParameter>
        {
            new SQLiteParameter("@Vorname", employee.Vorname),
            new SQLiteParameter("@Nachname", employee.Nachname)
        });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Löschen des Mitarbeiters: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void AddSaveEmployeeToDatabase(employee employee)
        {
            try
            {
                string insertQuery = $"INSERT INTO {employee.GetTableName()} (Vorname, Nachname, Tel_Nr, Abteilung) " +
                                     $"VALUES (@Vorname, @Nachname, @Tel_Nr, @Abteilung)";

                ExecuteNonQuery(insertQuery, new List<SQLiteParameter>
        {
            new SQLiteParameter("@Vorname", employee.Vorname),
            new SQLiteParameter("@Nachname", employee.Nachname),
            new SQLiteParameter("@Tel_Nr", employee.Tel_Nr),
            new SQLiteParameter("@Abteilung", employee.Abteilung)
        });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern des Mitarbeiters in der Datenbank: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void UpdateEmployee(employee employee)
        {
            try
            {
                string updateQuery = $"UPDATE {employee.GetTableName()} SET Tel_Nr = @Tel_Nr, Abteilung = @Abteilung WHERE Vorname = @Vorname AND Nachname = @Nachname";

                ExecuteNonQuery(updateQuery, new List<SQLiteParameter>
        {
            new SQLiteParameter("@Vorname", employee.Vorname),
            new SQLiteParameter("@Nachname", employee.Nachname),
            new SQLiteParameter("@Tel_Nr", employee.Tel_Nr),
            new SQLiteParameter("@Abteilung", employee.Abteilung)
        });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Aktualisieren des Mitarbeiters: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public static void SaveProjectToDatabase(Project project)
        {
            try
            {
                string insertQuery = $"INSERT INTO {Project.GetTableName()} (ProjektBezeichnung, VerantwortlichePersonalnummer, VonDatum, BisDatum) " +
                                     $"VALUES (@ProjektBezeichnung, @VerantwortlichePersonalnummer, @VonDatum, @BisDatum)";

                ExecuteNonQuery(insertQuery, new List<SQLiteParameter>
        {
            new SQLiteParameter("@ProjektBezeichnung", project.ProjektBezeichnung),
            new SQLiteParameter("@VerantwortlichePersonalnummer", project.VerantwortlichePersonalnummer),
            new SQLiteParameter("@VonDatum", project.VonDatum),
            new SQLiteParameter("@BisDatum", project.BisDatum)
        });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern des Projekts in der Datenbank: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public static void UpdateProject(Project project)
        {
            try
            {
                string updateQuery = $"UPDATE {Project.GetTableName()} SET ProjektBezeichnung = @ProjektBezeichnung, " +
                                     "VerantwortlichePersonalnummer = @VerantwortlichePersonalnummer, " +
                                     "VonDatum = @VonDatum, BisDatum = @BisDatum " +
                                     "WHERE ProjektID = @ProjektID";

                ExecuteNonQuery(updateQuery, new List<SQLiteParameter>
        {
            new SQLiteParameter("@ProjektBezeichnung", project.ProjektBezeichnung),
            new SQLiteParameter("@VerantwortlichePersonalnummer", project.VerantwortlichePersonalnummer),
            new SQLiteParameter("@VonDatum", project.VonDatum.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@BisDatum", project.BisDatum.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@ProjektID", project.projektID)
        });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Aktualisieren des Projekts: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void DeleteProject(Project project)
        {
            try
            {
                string deleteQuery = $"DELETE FROM {Project.GetTableName()} WHERE ProjektID = @ProjektID";
                ExecuteNonQuery(deleteQuery, new List<SQLiteParameter>
        {
            new SQLiteParameter("@ProjektID", project.projektID)
        });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Löschen des Projekts: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void AddProjectPhase(Projectphases projectPhase)
        {
            try
            {
                string insertQuery = $"INSERT INTO {Projectphases.GetTableName()} (Kennung, Bezeichnung, ProjID, Dauer, Vorg) " +
                                     "VALUES (@Kennung, @Bezeichnung, @ProjID, @Dauer, @Vorg)";

                ExecuteNonQuery(insertQuery, new List<SQLiteParameter>
        {
            new SQLiteParameter("@Kennung", projectPhase.Kennung),
            new SQLiteParameter("@Bezeichnung", projectPhase.Bezeichnung),
            new SQLiteParameter("@ProjID", projectPhase.ProjID),
            new SQLiteParameter("@Dauer", projectPhase.Dauer),
            new SQLiteParameter("@Vorg", projectPhase.Vorg)
        });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Hinzufügen der Projektphase: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static Projectphases GetProjectPhaseDetails(int phaseID)
        {
            try
            {
                string selectQuery = $"SELECT * FROM {Projectphases.GetTableName()} WHERE PhasID = @PhasID";
                SQLiteCommand command = new SQLiteCommand(selectQuery, backend.connection);
                command.Parameters.AddWithValue("@PhasID", phaseID);

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return Projectphases.GetDatabaseObject(reader);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Abrufen der Details zur Projektphase: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        public static void AddNewProjectPhase(Projectphases newPhase)
        {
            try
            {
                string insertQuery = $"INSERT INTO {Projectphases.GetTableName()} (Kennung, Bezeichnung, ProjID, Dauer, Vorg) " +
                                     "VALUES (@Kennung, @Bezeichnung, @ProjID, @Dauer, @Vorg)";

                ExecuteNonQuery(insertQuery, new List<SQLiteParameter>
        {
            new SQLiteParameter("@Kennung", newPhase.Kennung),
            new SQLiteParameter("@Bezeichnung", newPhase.Bezeichnung),
            new SQLiteParameter("@ProjID", newPhase.ProjID),
            new SQLiteParameter("@Dauer", newPhase.Dauer),
            new SQLiteParameter("@Vorg", newPhase.Vorg)
        });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Hinzufügen einer neuen Projektphase: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        internal static void DeleteProject(Projectphases selectedPhase)
        {
            throw new NotImplementedException();
        }
    }
}
