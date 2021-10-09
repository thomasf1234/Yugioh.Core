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
  Description VARCHAR NOT NULL
);
",

@"
CREATE TABLE IF NOT EXISTS Artwork (
  CardId INTEGER NOT NULL,
  Ordinal INTEGER NOT NULL,
  Image BLOB,
  FOREIGN KEY(CardId) REFERENCES Card(CardId),
  UNIQUE(CardId, Ordinal) ON CONFLICT REPLACE
);
",

@"
CREATE TABLE IF NOT EXISTS Product (
  ProductId VARCHAR PRIMARY KEY NOT NULL, 
  Title VARCHAR NOT NULL,
  SetSize INTEGER NOT NULL,
  --Image BLOB,
  LaunchDate DATETIME
);
",

@"
CREATE TABLE IF NOT EXISTS ProductCard (
    Code VARCHAR PRIMARY KEY NOT NULL,
    ProductId VARCHAR NOT NULL, 
    CardId INTEGER NOT NULL,
    ArtworkOrdinal INTEGER, --NOT NULL, 
    Rarity VARCHAR NOT NULL,
    --Rarity TINYINT, -- Binary [GR][SecretRare][UltimateRare][]
    Passcode CHAR(8),
    FOREIGN KEY(ProductId) REFERENCES Product(ProductId),
    FOREIGN KEY(CardId) REFERENCES Card(CardId)
);
"
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
