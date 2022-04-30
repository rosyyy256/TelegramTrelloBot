using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace TGCbot.Services
{
    public class TelegramBot
    {
        private TelegramBotClient _botClient;
        private readonly IConfiguration _configuration;

        public TelegramBot(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (_botClient != null)
            {
                return _botClient;
            }

            _botClient = new TelegramBotClient(_configuration["Token"]);
            
            var hook = $"{_configuration["Url"]}/api/message/update";
            await _botClient.SetWebhookAsync(hook);
            
            return _botClient;
        }
    }
}