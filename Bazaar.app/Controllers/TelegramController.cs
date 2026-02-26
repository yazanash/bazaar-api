using Bazaar.app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        private readonly TelegramBotService _telegramBotService;

        public TelegramController(TelegramBotService telegramBotService)
        {
            _telegramBotService = telegramBotService;
        }
        [HttpPost("update")]
        public async Task<IActionResult> ReceiveUpdate([FromBody] Update update)
        {
            await _telegramBotService.HandleUpdate(update);
            return Ok();
        }
    }
}
