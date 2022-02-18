namespace Jitsukawa.PriceChecker
{
    internal class Fail : Exception
    {
        internal Fail(string message, Exception? inner = null) : base(message, inner) { }
    }
}
