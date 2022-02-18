namespace Jitsukawa.PriceChecker
{
    internal record Rule
    {
        internal int Id { get; set; }

        internal string Ticker { get; set; } = null!;

        internal double Start { get; set; }

        internal double End { get; set; }

        internal string Message { get; set; } = null!;
    }
}
