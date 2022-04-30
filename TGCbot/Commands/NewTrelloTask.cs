using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TGCbot.Entities;
using TGCbot.Services;

namespace TGCbot.Commands
{
    public class NewTrelloTask : BaseCommand
    {
        private readonly RabbitMqPublisher _publisher;
        private readonly TelegramBotClient _botClient;
        
        public NewTrelloTask(TelegramBot botClient)
        {
            _publisher = new RabbitMqPublisher();
            _botClient = botClient.GetBotClientAsync().Result;
        }

        public override string Name => CommandNames.TrelloTask;
        public override async Task ExecuteAsync(Update update)
        {
            var message = update.Message;
            var text = message.Text;
            var splitText = text.Split(new[] {"--", "—"}, StringSplitOptions.RemoveEmptyEntries);
            var card = new Card();

            try
            {
                card.Name = splitText[0].Replace(Name, "");
                card.Description = splitText[1];
            }
            catch (Exception e)
            {
                await _botClient.SendTextMessageAsync(message.Chat.Id, "Ошибка в запросе");
                return;
            }

            _publisher.SendMessage(card);

            await _botClient.SendTextMessageAsync(message.Chat.Id, "Задача создана");
        }
    }
}