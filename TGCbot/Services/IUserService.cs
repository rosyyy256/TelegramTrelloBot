using System.Threading.Tasks;
using Telegram.Bot.Types;
using TGCbot.Entities;

namespace TGCbot.Services
{
    public interface IUserService
    {
        public Task<AppUser> GetOrCreate(Update update);
    }
}