using Memiz.Api.DTOs;
using Memiz.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Memiz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost("generate")]
        public IActionResult Generate([FromBody] GenerateCardsRequestDto request)
        {
            var result = _cardService.GenerateCards(request);
            return Ok(result);
        }
    }
}
