using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using TGCbot.Entities;

namespace TGCbot.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        
        public UserService(DataContext context)
        {
            _context = context;
        }
        
        public async Task<AppUser> GetOrCreate(Update update)
        {
            var chat = update.Message.Chat;
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ChatId == chat.Id);

            if (user != null) return user;
            
            var appUser = new AppUser
            {
                Username = chat.Username,
                ChatId = chat.Id,
                FirstName = chat.FirstName,
                LastName = chat.LastName
            };

            var result = await _context.Users.AddAsync(appUser);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
    }
}