using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Primitives;

namespace Hackathon2024.SqlHelper ;

    public partial class PossibleWordList()
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
                Console.WriteLine($"request for {word} with {string.Join(',', wrongGuesses)}");
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

            Console.WriteLine($"WORD: {_possibleWordList.Count} mögliche Wörter gefunden");
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

        [GeneratedRegex("([A-Z]_{1,2}[A-Z])|([A-Z]_{1,3})|(_{1,3}[A-Z])")]
        private partial Regex SegmentRegex();

        public List<string> GetPossibleSegments(string word, List<char> guesses)
        {
            HashSet<string> possibleSegments = [];
            foreach (Match match in SegmentRegex().Matches(word))
            {
                foreach (Capture capture in match.Captures)
                {
                    string sql = BuildSegmentSql(capture, guesses);
                    try
                    {
                        _sb.Clear();
                        using var connection = DbConfiguration.GetDatabaseConnection();
                        connection.Open();
                        using var command = new SqliteCommand(sql, connection);

                        using var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            possibleSegments.Add(reader.GetString(0));
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
                }
            }
            Console.WriteLine("Following words were found: {0}", string.Join(',', possibleSegments));
            return possibleSegments.ToList();
        }

        private string BuildSegmentSql(Capture matchCapture, List<char> guesses)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT letter_pair FROM letter_indices WHERE ");
            builder.Append($"letter_pair LIKE '{matchCapture.Value}' ");

            int start = matchCapture.ValueSpan.IndexOf('_');
            int end = matchCapture.ValueSpan.LastIndexOf('_') + 1;
            int count = matchCapture.ValueSpan.Count('_');

            var startSlice = matchCapture.ValueSpan[..start];
            var endSlice = matchCapture.ValueSpan[end..];

            foreach (var guess in guesses)
            {
                for (int i = 0; i < count; i++)
                {
                    builder.Append($"AND letter_pair NOT LIKE '{startSlice}");
                    int j = 0;
                    while (j < i)
                    {
                        builder.Append('_');
                        j++;
                    }
                    j++;
                    builder.Append(guess);
                    while (j < count)
                    {
                        builder.Append('_');
                        j++;
                    }
                    builder.Append($"{endSlice}' ");
                }
            }
            
            builder.Append("ORDER BY occurences DESC ");
            builder.Append("LIMIT 1");
            return builder.ToString();
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

            Console.WriteLine($"GLOBAL {_possibleWordList.Count} mögliche Wörter gefunden");
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