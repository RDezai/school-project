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
        public int projektID;
        public string ProjektBezeichnung { get; set; }
        public int VerantwortlichePersonalnummer { get; set; }
        public DateTime VonDatum { get; set; }
        public DateTime BisDatum { get; set; }

        public static Project GetDatabaseObject(SQLiteDataReader reader)
        {
            Project project = new Project();
            project.projektID = reader.GetInt32(0);
            project.ProjektBezeichnung = reader.GetString(1);

            // Check if the column is DBNull before reading
            if (!reader.IsDBNull(2))
            {
                project.VerantwortlichePersonalnummer = reader.GetInt32(2);
            }

            // Check if the column is DBNull before reading
            if (!reader.IsDBNull(3))
            {
                project.VonDatum = reader.GetDateTime(3);
            }

            // Check if the column is DBNull before reading
            if (!reader.IsDBNull(4))
            {
                project.BisDatum = reader.GetDateTime(4);
            }

            return project;
        }

        public static string GetTableName()
        {
            return "Projekte";
        }

        public override string ToString()
        {
            return $"Projekt: (ProjektID: {projektID}, Bezeichnung: {ProjektBezeichnung})\n";
        }
    }

}