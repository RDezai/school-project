using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectmanagement
{
    public class Projekt
    {
        public int projektID;
        public string projektBezeichnung = "";

        public static Projekt GetDatabaseObject(SQLiteDataReader reader)
        {
            Projekt project = new Projekt();
            project.projektID = reader.GetInt32(0);
            project.projektBezeichnung = reader.GetString(1);

            return project;
        }

        public static string GetTableName()
        {
            return "Projekte";
        }

        public override string ToString()
        {
            return $"Projekt: (ProjektID: {projektID}, Bezeichnung: {projektBezeichnung})\n";
        }

    }
}