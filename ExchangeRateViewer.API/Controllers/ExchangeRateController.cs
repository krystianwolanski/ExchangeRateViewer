using ExchangeRateViewer.API.Filters;
using ExchangeRateViewer.Application.DTO.External;
using ExchangeRateViewer.Application.Queries;
using ExchangeRateViewer.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateViewer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ExchangeRateController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ApiKeyAuth]
        [ResponseCache(Duration = 60 * 30)]
        public async Task<ActionResult<IEnumerable<ExchangeRateDto>>> GetExchangeRates(
            [FromQuery] Dictionary<string, string> currencyCodes,
            [FromQuery]DateTime startDate,
            [FromQuery]DateTime endDate)
        {
            var exchangeRates = await _queryDispatcher
                .QueryAsync(new GetExchangeRates(currencyCodes, startDate, endDate));

            return Ok(exchangeRates);
        }
    }
}
