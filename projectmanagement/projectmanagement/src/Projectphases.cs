using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectmanagement.src
{
    public class Projectphases
    {
        public int PhasID { get; set; }
        public string Kennung { get; set; } //for Nummer
        public string Bezeichnung { get; set; } //for Phase
        public int Proj_ID { get; set; }
        public int Dauer { get; set; }
        public string Vorgaenger { get; set; }

        public static Projectphases GetDatabaseObject(SQLiteDataReader reader)
        {
            Projectphases phase = new Projectphases();

            // Map only the necessary columns
            phase.Kennung = Convert.ToString(reader["Kennung"]); // Assuming Kennung is a string
            phase.Bezeichnung = Convert.ToString(reader["Bezeichnung"]); // Assuming Phasenbezeichnung is a string
            phase.Dauer = Convert.ToInt32(reader["Dauer"]); // Assuming Dauer is an integer
            phase.Vorgaenger = reader.IsDBNull(reader.GetOrdinal("Vorgaenger")) ? null : Convert.ToString(reader["Vorgaenger"]);

            return phase;
        }


        public static string GetTableName()
        {
            return "Projektphase";
        }

        public override string ToString()
        {
            //return $"Phase: (ID: {PhasID}, Kenn: {Kennung}, Bez: {Bezeichnung}, Proj {ProjID}, Dauer {Dauer}, Vorgaenger {Vorgaenger})\n";
            return $"Phase: (ID: {PhasID}, Kenn: {Kennung}, Bez: {Bezeichnung}\n";
        }
    }


}
