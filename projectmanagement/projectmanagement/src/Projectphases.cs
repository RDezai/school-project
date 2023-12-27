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
        public int PhasID;
        public string Kennung = "";
        public string Bezeichnung = "";
        public int ProjID;
        public int Dauer;
        public int Vorg;

        public static Projectphases GetDatabaseObject(SQLiteDataReader reader)
        {
            Projectphases Phase = new Projectphases();
            Phase.PhasID = reader.GetInt32(0);
            Phase.Kennung = reader.GetString(1);
            Phase.Bezeichnung = reader.GetString(2);
            Phase.ProjID = reader.GetInt32(3);  // Use index 3 for ProjID
            Phase.Dauer = reader.GetInt32(4);

            if (reader.IsDBNull(5)) {
                Phase.Vorg = -1;
            }
            else {
                Phase.Vorg = reader.GetInt32(5);
            }

            return Phase;
        }

        public static string GetTableName()
        {
            return "Projektphase";
        }

        public override string ToString()
        {
            //return $"Phase: (ID: {PhasID}, Kenn: {Kennung}, Bez: {Bezeichnung}, Proj {ProjID}, Dauer {Dauer}, Vorg {Vorg})\n";
            return $"Phase: (ID: {PhasID}, Kenn: {Kennung}, Bez: {Bezeichnung}\n";
        }
    }


}
