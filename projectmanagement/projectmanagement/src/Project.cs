﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectmanagement
{
    public class Project
    {
        public int Proj_ID;
        public string Name { get; set; }
        public string Verantwortlicher { get; set; }
        public DateTime Startdatum { get; set; }
        public DateTime Enddatum { get; set; }
  

        public static Project GetDatabaseObject(SQLiteDataReader reader)
        {
            Project project = new Project();
            project.Proj_ID = reader.GetInt32(0);
            project.Name = reader.GetString(1);

            try
            {
                if (!reader.IsDBNull(2))
                {
                    project.Verantwortlicher = reader.GetString(2);
                }
            }
            catch (InvalidCastException ex)
            {
                // Handle the exception or log the error.
                // You can also set a default value for "Verantwortlicher" in case of a cast error.
                project.Verantwortlicher = "Unknown"; // Set a default value.
            }


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
            return $"Projekt: (ProjektID: {Proj_ID}, Bezeichnung: {Name})\n";
        }
    }

}