using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UESAN.ExchangePro.CORE.Core.Interfaces;

namespace UESAN.ExchangePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoCambioController : ControllerBase
    {
        private readonly ITipoCambioService _tipoCambioService;

        public TipoCambioController(ITipoCambioService tipoCambioService)
        {
            _tipoCambioService = tipoCambioService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _tipoCambioService.GetTipoCambio();
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}