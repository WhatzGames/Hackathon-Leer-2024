using System.Text;
using Microsoft.Data.Sqlite;

namespace Hackathon2024.SqlHelper ;

    public class PossibleWordList()
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public List<string> GetPossibleWordList(string word, List<char> wrongGuesses)
        {
            List<string> _possibleWordList = [];
            var sql = BuildSql(word, wrongGuesses);
            try
            {
                _sb.Clear();
                using var connection = DbConfiguration.GetDatabaseConnection();
                connection.Open();
                using var command = new SqliteCommand(sql, connection);
                //Console.WriteLine($"request for {word} with {string.Join(',', wrongGuesses)}");
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    _possibleWordList.Add(reader.GetString(1));
                }
            }
            catch (SqliteException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            //Console.WriteLine($"WORD: {_possibleWordList.Count} mögliche Wörter gefunden");
            return _possibleWordList;
        }

        private string BuildSql(string word, List<char> wrongGuesses)
        {
            _sb.Append(GetSelectStatement(word));
            if (wrongGuesses.Count > 0)
            {
                for (int i = 0; i < wrongGuesses.Count; i++)
                {
                    var c = wrongGuesses[i].ToString()
                                           .ToUpper();
                    _sb.Append($" AND word NOT LIKE '%{c}%'");
                }
            }


            return _sb.ToString();
        }

        public List<string> GetPossibleWordList(List<char> wrongGuessed)
        {
            List<string> _possibleWordList = [];
            var sql = BuildGlobalSql(wrongGuessed);
            try
            {
                _sb.Clear();
                using var connection = DbConfiguration.GetDatabaseConnection();
                connection.Open();
                using var command = new SqliteCommand(sql, connection);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    _possibleWordList.Add(reader.GetString(1));
                }
            }
            catch (SqliteException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            //Console.WriteLine($"GLOBAL {_possibleWordList.Count} mögliche Wörter gefunden");
            return _possibleWordList;
        }

        public string BuildGlobalSql(List<char> wrongGuesses)
        {
            _sb.Append($"SELECT * FROM {DbConfiguration.GetTableName()}");

            if (wrongGuesses.Count > 0)
            {
                _sb.Append(" WHERE ");
            }
            for (int i = 0; i < wrongGuesses.Count; i++)
            {
                if (i > 0)
                {
                    _sb.Append(" AND ");
                }
                var c = wrongGuesses[i].ToString()
                                       .ToUpper();
                _sb.Append($" word NOT LIKE '%{c}%'");
            }

            return _sb.ToString();
        }

        private string GetSelectStatement(string word)
        {
            return "SELECT * FROM " + DbConfiguration.GetTableName() + " WHERE "
                   + DbConfiguration.GetColumnName() + " LIKE '%" + word + "%'";
        }

        public List<char> GetWrongGuesses(string word, List<char> guessed)
        {
            var wrongGuesses = new List<char>();
            foreach (char c in guessed)
            {
                if (!word.Contains(c))
                {
                    wrongGuesses.Add(c);
                }
            }

            return wrongGuesses;
        }
    }