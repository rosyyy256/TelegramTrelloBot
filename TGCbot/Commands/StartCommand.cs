using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TGCbot.Services;

namespace TGCbot.Commands
{
    public class StartCommand : BaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly IUserService _userService;

        public StartCommand(IUserService userService, TelegramBot botClient)
        {
            _userService = userService;
            _botClient = botClient.GetBotClientAsync().Result;
        }

        public override string Name => CommandNames.StartCommand;
        
        public override async Task ExecuteAsync(Update update)
        {
            var user = await _userService.GetOrCreate(update);
            
            await _botClient.SendTextMessageAsync(
                user.ChatId, 
                $"Привет, {user.FirstName}", 
                Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}