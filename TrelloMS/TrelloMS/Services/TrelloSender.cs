using System.Linq;
using System.Threading.Tasks;
using Manatee.Trello;
using Microsoft.Extensions.Configuration;
using Card = TrelloMS.Entities.Card;

namespace TrelloMS.Services
{
    public class TrelloSender
    {
        private readonly IConfiguration _configuration;

        public TrelloSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task AddNewCard(Card card)
        {
            var factory = new TrelloFactory();
            var auth = new TrelloAuthorization
            {
                AppKey = _configuration["AppKey"],
                UserToken = _configuration["UserToken"]
            };
            var board = factory.Board(_configuration["BoardId"], auth);
            await board.Lists.Refresh();

            var list = board.Lists.FirstOrDefault(l => l.Name == "idea");
            await list.Cards.Add(card.Name, description: card.Description);
        }
    }
}