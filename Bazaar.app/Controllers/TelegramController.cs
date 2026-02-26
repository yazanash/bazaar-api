using Bazaar.app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
        public async Task<IActionResult> ReceiveUpdate([FromBody] JsonElement rawJson)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var update = JsonSerializer.Deserialize<Update>(rawJson.GetRawText(), options);

                if (update != null)
                {
                    await _telegramBotService.HandleUpdate(update);
                }
            }
            catch (Exception)
            {
            }
            return Ok();
        }
    }
}
