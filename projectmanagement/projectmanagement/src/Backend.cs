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

        public struct GanttEntry
        {
            public int phaseID;
            public string phaseName;
            public int startTime;
            public int duration;
            public int width;
        }
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

        public static List<Employee> GetEmployeesList()
        {
            string table = Employee.GetTableName();
            string selectQuery = "SELECT * FROM " + table;

            return ExecuteQuery(table, selectQuery, Employee.GetDatabaseObject);
        }

        public static List<Project> GetProjectList()
        {
            string table = Project.GetTableName();
            string selectQuery = "SELECT * FROM " + table;

            return ExecuteQuery(table, selectQuery, Project.GetDatabaseObject);
        }

        public static List<Projectphases> GetAllProjectPhases()
        {
            List<Projectphases> allPhases = new List<Projectphases>();

            try
            {
                string selectQuery = $"SELECT * FROM {Projectphases.GetTableName()}";
                SQLiteCommand command = new SQLiteCommand(selectQuery, backend.connection);

                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Projectphases phase = Projectphases.GetDatabaseObject(reader);
                    allPhases.Add(phase);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Abrufen aller Projektphasen: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return allPhases;
        }


        public static void DeleteEmployee(Employee employee)
        {
            try
            {
                string deleteQuery = $"DELETE FROM {Employee.GetTableName()} WHERE Vorname = @Vorname AND Nachname = @Nachname";
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

        public static void AddSaveEmployeeToDatabase(Employee employee)
        {
            try
            {
                string insertQuery = $"INSERT INTO {Employee.GetTableName()} (Vorname, Nachname, Tel_Nr, Abteilung) " +
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

        public static void UpdateEmployee(Employee employee)
        {
            try
            {
                string updateQuery = $"UPDATE {Employee.GetTableName()} SET Tel_Nr = @Tel_Nr, Abteilung = @Abteilung WHERE Vorname = @Vorname AND Nachname = @Nachname";

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
                string insertQuery = $"INSERT INTO {Project.GetTableName()} (Name, Verantwortlicher, Startdatum, Enddatum) " +
                                     $"VALUES (@Name, @Verantwortlicher, @Startdatum, @Enddatum)";

                ExecuteNonQuery(insertQuery, new List<SQLiteParameter>
        {
            new SQLiteParameter("@Name", project.Name),
            new SQLiteParameter("@Verantwortlicher", project.Verantwortlicher), // Corrected the parameter name
            new SQLiteParameter("@Startdatum", project.Startdatum.ToString("yyyy-MM-dd")), // Added date formatting
            new SQLiteParameter("@Enddatum", project.Enddatum.ToString("yyyy-MM-dd")) // Added date formatting
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
                string updateQuery = $"UPDATE {Project.GetTableName()} SET Name = @Name, " +
                                     "Verantwortlicher = @Verantwortlicher, " +
                                     "Startdatum = @Startdatum, Enddatum = @Enddatum " +
                                     "WHERE Proj_ID = @Proj_ID";

                ExecuteNonQuery(updateQuery, new List<SQLiteParameter>
        {
            new SQLiteParameter("@Name", project.Name),
            new SQLiteParameter("@Verantwortlicher", project.Verantwortlicher ?? (object)DBNull.Value),
            new SQLiteParameter("@Startdatum", project.Startdatum.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@Enddatum", project.Enddatum.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@Proj_ID", project.Proj_ID)
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
                string deleteQuery = $"DELETE FROM {Project.GetTableName()} WHERE Proj_ID = @Proj_ID";
                ExecuteNonQuery(deleteQuery, new List<SQLiteParameter>
        {
            new SQLiteParameter("@Proj_ID", project.Proj_ID)
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
                string insertQuery = $"INSERT INTO {Projectphases.GetTableName()} (Kennung, Bezeichnung, Proj_ID, Dauer, Vorgaenger) " +
                                     "VALUES (@Kennung, @Bezeichnung, @Proj_ID, @Dauer, @Vorgaenger)";

                ExecuteNonQuery(insertQuery, new List<SQLiteParameter>
        {
            new SQLiteParameter("@Kennung", projectPhase.Kennung),
            new SQLiteParameter("@Bezeichnung", projectPhase.Bezeichnung),
            new SQLiteParameter("@Proj_ID", projectPhase.Proj_ID),
            new SQLiteParameter("@Dauer", projectPhase.Dauer),
            new SQLiteParameter("@Vorgaenger", projectPhase.Vorgaenger)
        });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Hinzufügen der Projektphase: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         public static List<GanttEntry> GetGanttList(int projectID)
        {
            List<Projectphases> projectPhaseList = GetProjectPhasesOfProject(projectID);
            List<GanttEntry> ganttList = new();

            int totalWidth = 0;

            foreach (Projectphases projectPhase in projectPhaseList)
            {
                GanttEntry entry = new GanttEntry();
                entry.phaseID = projectPhase.PhasID;
                entry.duration = projectPhase.Dauer;
                entry.phaseName = projectPhase.Bezeichnung;

                if (projectPhase.Vorgaenger == -1)
                {
                    entry.startTime = 0;
                }
                else
                {
                    GanttEntry predecessor = ganttList.Find((GanttEntry ganttEntry) => ganttEntry.phaseID == projectPhase.Vorgaenger);
                    entry.startTime = predecessor.startTime + predecessor.width;
                }

                int entryWidth = entry.startTime + entry.duration;

                if (totalWidth < entryWidth)
                {
                    totalWidth = entryWidth;
                }

                entry.width = totalWidth - entry.startTime;
                ganttList.Add(entry);
            }

            return ganttList;
        }

        public static List<Projectphases> GetProjectPhasesOfProject(int projectID)
        {
            List<Projectphases> projectPhaseList = new();
            try
            {
                string selectQuery = $"SELECT * FROM {Projectphases.GetTableName()} WHERE Proj_ID = @ProjID";
                SQLiteCommand command = new SQLiteCommand(selectQuery, backend.connection);
                command.Parameters.AddWithValue("@ProjID", projectID);

                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Projectphases projectPhase = Projectphases.GetDatabaseObject(reader);
                    projectPhaseList.Add(projectPhase);
                }

                return projectPhaseList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Abrufen der Details zur Projektphase: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return projectPhaseList;
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
                string insertQuery = $"INSERT INTO {Projectphases.GetTableName()} (Kennung, Bezeichnung, Proj_ID, Dauer, Vorgaenger) " +
                                     "VALUES (@Kennung, @Bezeichnung, @Proj_ID, @Dauer, @Vorgaenger)";
                 
                ExecuteNonQuery(insertQuery, new List<SQLiteParameter>
        {
            new SQLiteParameter("@Kennung", newPhase.Kennung),
            new SQLiteParameter("@Bezeichnung", newPhase.Bezeichnung),
            new SQLiteParameter("@Proj_ID", newPhase.Proj_ID),
            new SQLiteParameter("@Dauer", newPhase.Dauer),
            new SQLiteParameter("@Vorgaenger", newPhase.Vorgaenger)
        });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Hinzufügen einer neuen Projektphase: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

      
    }
}
