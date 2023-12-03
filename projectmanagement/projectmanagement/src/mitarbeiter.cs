using System.Data.SQLite;

namespace projectmanagement
{
    public class Mitarbeiter
    {
        //public int MitarbeiterID { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Tel_Nr { get; set; }
        public string Abteilung { get; set; }

        public static Mitarbeiter GetDatabaseObject(SQLiteDataReader reader)
        {
            Mitarbeiter mitarbeiter = new Mitarbeiter();
            //mitarbeiter.MitarbeiterID = reader.GetInt32(0);
            mitarbeiter.Vorname = reader.GetString(1);
            mitarbeiter.Nachname = reader.GetString(2);
            mitarbeiter.Tel_Nr = reader.GetString(3);
            mitarbeiter.Abteilung = reader.GetString(4);

            return mitarbeiter;
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
