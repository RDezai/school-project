using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectmanagement
{
    public class Project
    {
        public int projectID;
        public string ProjektBezeichnung { get; set; }
        public string Verantwortlicher { get; set; }
        public DateTime Startdatum { get; set; }
        public DateTime Enddatum { get; set; }

        public static Project GetDatabaseObject(SQLiteDataReader reader)
        {
            Project project = new Project();
            project.projectID = reader.GetInt32(0);
            project.ProjektBezeichnung = reader.GetString(1);

            // Check if the column is DBNull before reading
            /*if (!reader.IsDBNull(2))
            {
                project.Verantwortlicher = reader.GetInt32(2).ToString();
            }*/

            // Check if the column is DBNull before reading
            if (!reader.IsDBNull(3))
            {
                project.Startdatum = reader.GetDateTime(3);
            }

            // Check if the column is DBNull before reading
            if (!reader.IsDBNull(4))
            {
                project.Enddatum = reader.GetDateTime(4);
            }

            return project;
        }

        public static string GetTableName()
        {
            return "Projekte";
        }

        public override string ToString()
        {
            return $"Projekt: (ProjektID: {projectID}, Bezeichnung: {ProjektBezeichnung})\n";
        }
    }

}