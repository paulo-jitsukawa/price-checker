using System.Globalization;
using System.Text.RegularExpressions;

namespace Jitsukawa.PriceChecker
{
    internal class Rules
    {
        private readonly string _path = null!;

        internal readonly List<Rule> Items = new();

        internal Rules(string path) => _path = path;

        internal void Load()
        {
            var id = 0;
            var rowNumber = 0;
            var colNumber = 0;
            string? content;

            try
            {
                content = File.ReadAllText(_path);
            }
            catch (Exception ex)
            {
                throw new Fail($"Não foi possível abrir o arquivo {_path}.", ex);
            }

            try
            {
                foreach (var row in content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                {
                    var cols = row.Split(';');
                    rowNumber++;

                    if (!cols[0].Trim().StartsWith("#"))
                    {
                        var rule = new Rule() { Id = ++id };

                        colNumber = 1;
                        rule.Ticker = cols[0].Trim().Trim('"').ToUpperInvariant();
                        if (!Regex.Match(rule.Ticker, "\\w{4}(3|4|5|6|11)B{0,1}").Success)
                        {
                            throw new Fail($"Ação ou FII inválido na linha {rowNumber} do arquivo {_path}: {rule.Ticker}.");
                        }

                        colNumber++;
                        rule.Start = double.Parse(cols[1].Trim(), CultureInfo.InvariantCulture);

                        colNumber++;
                        rule.End = double.Parse(cols[2].Trim(), CultureInfo.InvariantCulture);

                        colNumber++;
                        rule.Message = cols[3].Trim().Trim('"');

                        Items.Add(rule);
                    }
                }
            }
            catch (Fail ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Fail($"Erro na coluna {colNumber} da linha {rowNumber} do arquivo {_path}.", ex);
            }
        }
    }
}
