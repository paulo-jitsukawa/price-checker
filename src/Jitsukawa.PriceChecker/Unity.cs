namespace Jitsukawa.PriceChecker
{
    internal record Unity
    {
        internal string Ticker { get; set; } = null!;
        internal double Price { get; set; }
    }
}
