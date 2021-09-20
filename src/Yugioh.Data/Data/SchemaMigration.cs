using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace Yugioh.Data.Data
{
    public class SchemaMigration
    {
        private readonly SqliteConnection _sqliteConnection;

        public SchemaMigration(SqliteConnection sqliteConnection)
        {
            _sqliteConnection = sqliteConnection;
        }

        public async Task ApplyAsync()
        {
            string[] queries = new string[]
            {
@"
CREATE TABLE IF NOT EXISTS Card (
  CardId INTEGER PRIMARY KEY NOT NULL, 
  Type TINYINT NOT NULL, -- Monster, Spell, Trap
  Name VARCHAR NOT NULL, 
  Attribute TINYINT NOT NULL,
  Level TINYINT, 
  Rank TINYINT, 
  LinkRating TINYINT,
  LinkArrows TINYINT, -- Binary [TL][T][TR][R][BR][B][BL][L] so 10010000 is Top-Left + Right
  PendulumScale TINYINT, 
  PendulumDescription VARCHAR,
  Property TINYINT, 
  MonsterTypes TINYINT, -- [Link][Pendulum][Xyz][Synchro][Fusion][Ritual][Effect][Normal]
  Race VARCHAR, --https://yugioh.fandom.com/wiki/Type#English
  Abilities TINYINT, -- Binary [Tuner][DarkTuner][Toon][Union][Spirit][Gemini][Flip]
  Attack VARCHAR, 
  Defense VARCHAR, 
  Description VARCHAR NOT NULL,
  Passcode CHAR(8)
);
",

@"
CREATE TABLE IF NOT EXISTS Artwork (
  ArtworkId integer PRIMARY KEY NOT NULL,
  CardId integer NOT NULL,
  Alternate BOOLEAN NOT NULL,
  Image BLOB,
  FOREIGN KEY(CardId) REFERENCES Card(CardId)
);
"//,

/*@"
CREATE TABLE IF NOT EXISTS CardSet (
  Number VARCHAR PRIMARY KEY NOT NULL, 
  CardId INTEGER NOT NULL,
  ArtworkId INTEGER NOT NULL, 
  Rarity TINYINT, -- Binary [GR][SecretRare][UltimateRare][]
  FOREIGN KEY(CardId) REFERENCES Card(CardId),
  FOREIGN KEY(ArtworkId) REFERENCES Artwork(ArtworkId)
);
"*/
        };

            using (var transaction = _sqliteConnection.BeginTransaction())
            {
                foreach (string query in queries)
                {
                    var sqlCommand = _sqliteConnection.CreateCommand();
                    sqlCommand.CommandText = query;
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                transaction.Commit();
            }
        }
    }
}
