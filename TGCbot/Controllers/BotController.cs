using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TGCbot.Services;

namespace TGCbot.Controllers
{
    [ApiController]
    [Route("api/message/update")]
    public class BotController : ControllerBase
    {
        private readonly ICommandExecutor _commandExecutor;

        public BotController(ICommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update == null) return Ok();

            try
            {
                await _commandExecutor.Execute(update);
            }
            catch (Exception e)
            {
                return Ok();
            }
            
            return Ok();
        }
    }
}
