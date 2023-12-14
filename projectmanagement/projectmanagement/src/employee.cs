using System.Data.SQLite;

namespace projectmanagement
{
    public class Employee
    {
        public int MitarbeiterID { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Tel_Nr { get; set; }
        public string Abteilung { get; set; }

        public static Employee GetDatabaseObject(SQLiteDataReader reader)
        {
            Employee employee = new Employee();
            employee.MitarbeiterID = reader.GetInt32(0);
            employee.Vorname = reader.GetString(1);
            employee.Nachname = reader.GetString(2);
            employee.Tel_Nr = reader.GetInt32(3).ToString();
            employee.Abteilung = reader.GetString(4);

            return employee;
        }

        public static string GetTableName()
        {
            return "Mitarbeiter";
        }

        public override string ToString()
        {
            return $"Vorname: {Vorname}, Nachname: {Nachname}, Tel_Nr: {Tel_Nr}, Abteilung: {Abteilung})\n";
        }
    }
}
