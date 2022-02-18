using Jitsukawa.PriceChecker;

Console.WriteLine();
Console.WriteLine(" * * * * * * * * * * * * * * * * * * * * * * * * * *");
Console.WriteLine(" * PriceChecker v1.0                    17/02/2022 *");
Console.WriteLine(" * http://github.com/paulo-jitsukawa/price-checker *");
Console.WriteLine(" * * * * * * * * * * * * * * * * * * * * * * * * * *");
Console.WriteLine();

try
{
    var path = string.Empty;
    var arguments = Environment.GetCommandLineArgs();
    if (arguments.Length < 2 || !File.Exists(path = arguments[1]))
    {
        do
        {
            Console.Write(" Informe o endereço de seu arquivo de regras e pressione ENTER: ");
            path = Console.ReadLine();
        } while (!File.Exists(path));
    }

    Console.Write(" Carregando regras... ");
    var rules = new Rules(path);
    rules.Load();
    Console.WriteLine("OK");

    Console.Write(" Extraindo preços...  ");
    var ativos = new Units();
    await ativos.LoadAcoes();
    await ativos.LoadFIIs();
    Console.WriteLine("OK");

    Console.Write(" Aplicando regras...  ");
    var results = new SortedList<int, string>();
    foreach (var a in ativos.Items)
    {
        rules.Items
            .Where(r => r.Ticker == a.Ticker)
            .ToList()
            .ForEach(r =>
            {
                if (a.Price >= r.Start && a.Price <= r.End)
                {
                    results.Add(r.Id, $"{r.Id,3}. {r.Ticker} ({r.Start} <= {a.Price} <= {r.End}): {r.Message}");
                }
            });
    }
    Console.WriteLine("OK");
    Console.WriteLine();

    foreach (var result in results)
    {
        Console.WriteLine(result.Value);
    }
}
catch (Fail ex)
{
    Console.WriteLine($"ERRO: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"ERRO: Ocorreu um erro inesperado ({ex.Message}).");
}
finally
{
    Console.WriteLine();
    Console.WriteLine(" Pressione ESC para sair.");
    while (Console.ReadKey().Key != ConsoleKey.Escape) { }
}
