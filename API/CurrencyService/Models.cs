namespace CurrencyService
{

    public class CurrencyResponse
    {
        public int Error { get; set; }
        public string ErrorMessage { get; set; }
        public double Amount { get; set; }
    }
    public class CurrencyData
    {
        public string _api_key = "";
        public string To { get; set; }
        public string From { get; set; }
        public int Amount { get;set; }
    }
}
