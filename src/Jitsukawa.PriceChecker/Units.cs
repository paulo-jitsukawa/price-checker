using HtmlAgilityPack;
using System.Globalization;

namespace Jitsukawa.PriceChecker
{
    internal class Units
    {
        private readonly HttpClient client = new();

        internal readonly List<Unity> Items = new();

        internal Units()
        {
            client.DefaultRequestHeaders.Add
            (
                "user-agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36 OPR/83.0.4254.46"
            );
        }

        internal async Task LoadAcoes()
        {
            string? content;
            try
            {
                content = await client.GetStringAsync("https://www.fundamentus.com.br/resultado.php");
            }
            catch (HttpRequestException ex)
            {
                throw new Fail($"Não foi possível obter os dados da fonte. Certifique-se de estar conectado à Internet.", ex);
            }

            var html = new HtmlDocument();
            html.LoadHtml(content);

            foreach (var tr in html.DocumentNode.SelectNodes("//table/tbody/tr"))
            {
                try
                {
                    var list = tr.InnerText.Replace(" ", "").Replace(".", "").Replace(",", ".").Split('\n');
                    Items.Add(new Unity
                    {
                        Ticker = list[1],
                        Price = double.Parse(list[2], CultureInfo.InvariantCulture)
                    });
                }
                catch (Exception ex)
                {
                    throw new Fail($"Não foi possível processar a entrada:{Environment.NewLine}{tr.InnerText}", ex);
                }
            }
        }

        internal async Task LoadFIIs()
        {
            string? content;
            try
            {
                content = await client.GetStringAsync("https://www.fundamentus.com.br/fii_resultado.php");
            }
            catch (HttpRequestException ex)
            {
                throw new Fail($"Não foi possível obter os dados da fonte. Certifique-se de estar conectado à Internet.", ex);
            }

            var html = new HtmlDocument();
            html.LoadHtml(content);

            foreach (var tr in html.DocumentNode.SelectNodes("//table/tbody/tr"))
            {
                try
                {
                    var list = tr.InnerText.Replace(" ", "").Replace(".", "").Replace(",", ".").Split('\n');
                    Items.Add(new Unity
                    {
                        Ticker = list[1],
                        Price = double.Parse(list[3], CultureInfo.InvariantCulture)
                    });
                }
                catch (Exception ex)
                {
                    throw new Fail($"Não foi possível processar a entrada:{Environment.NewLine}{tr.InnerText}", ex);
                }
            }
        }
    }
}
