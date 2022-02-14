using ExchangeRateViewer.API.Security;
using ExchangeRateViewer.API.Security.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExchangeRateViewer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IApiKeyGenerator _keyGenerator;

        public AuthorizationController(IApiKeyGenerator keyGenerator)
        {
            _keyGenerator = keyGenerator;
        }

        [HttpGet("generateApiKey")]
        public async Task<ActionResult<ApiKeyResult>> GenerateApiKey()
        {
            var key = await _keyGenerator.GenerateKey();

            return Ok(key);
        }
    }
}
