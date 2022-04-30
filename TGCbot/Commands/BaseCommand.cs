using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TGCbot.Commands
{
    public abstract class BaseCommand
    {
        public abstract string Name { get; }
        public abstract Task ExecuteAsync(Update update);

        public virtual bool Contains(string value)
        {
            return value.Contains(Name);
        }
    }
}