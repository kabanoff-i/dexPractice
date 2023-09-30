using Newtonsoft.Json;

namespace CurrencyService
{
    public class CurrencyService
    {
        public async Task<CurrencyResponse> GetCurrency(CurrencyData data)
        {
            string baseUrl = "https://www.amdoren.com/api/currency.php";

            UriBuilder uriBuilder = new UriBuilder(baseUrl);

            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            query["api_key"] = data._api_key;
            query["from"] = data.From;
            query["to"] = data.To;
            query["amount"] = data.Amount.ToString();

            uriBuilder.Query = query.ToString();

            string finalUrl = uriBuilder.ToString();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(finalUrl);
                string message = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CurrencyResponse>(message);
            }
        }
    }
}