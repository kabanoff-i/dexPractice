using Microsoft.AspNetCore.Mvc;
using CurrencyService;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        CurrencyService.CurrencyService CurrencyService;
        public CurrencyController()
        {
            CurrencyService = new CurrencyService.CurrencyService();
        }
        [HttpGet("Exchange")]
        public async Task<CurrencyResponse> Exchange(string to, string from, int amount)
        {
            CurrencyData data = new CurrencyData() { To = to, From = from, Amount = amount };
            CurrencyResponse response = await CurrencyService.GetCurrency(data);
            return response;
        }
    }
}
