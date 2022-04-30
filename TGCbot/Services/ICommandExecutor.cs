using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TGCbot.Services
{
    public interface ICommandExecutor
    {
        Task Execute(Update update);
    }
}