using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace projectmanagement.src
{
    public class Mitarbeiter
    {
        public int employeeID;
        public string firstName = "";
        public string lastName = "";

        public static Mitarbeiter GetDatabaseObject(SQLiteDataReader reader)
        {
         Mitarbeiter mitarbeiter = new Mitarbeiter();
         mitarbeiter.employeeID = reader.GetInt32(0);
         mitarbeiter.firstName = reader.GetString(1);
         mitarbeiter.lastName = reader.GetString(2);

          return mitarbeiter;
        }

        public static string GetTableName()
        {
            return "Mitarbeiter";
        }

        public override string ToString()
        {
            return $"Mitarbeiter: (MitarbeiterID: {employeeID}, Vorname: {firstName}, Nachname: {lastName})\n";
        }
    }
}
