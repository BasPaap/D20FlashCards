using Bas.D20FlashCards.Pathfinder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bas.D20FlashCards.Client.Services
{
    public class CardsService
    {
        private readonly List<Parser> parsers = new List<Parser>();
        private readonly HttpClient httpClient;
        private readonly bool bypassCors;

        public CardsService(IEnumerable<Parser> parsers, HttpMessageHandler httpMessageHandler = null, bool bypassCors = true)
        {
            this.httpClient = httpMessageHandler == null ? new HttpClient() : new HttpClient(httpMessageHandler);
            this.httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            this.parsers.AddRange(parsers);
            this.bypassCors = bypassCors;
        }

        public async Task<ICollection<Card>> GetCardsAsync(string uriText)
        {
            var cards = new Collection<Card>();

            var lines = uriText?.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
                        
            var uris = from l in lines
                       where Uri.IsWellFormedUriString(l, UriKind.Absolute)
                       select new Uri(l);
                        
            foreach (var uri in uris)
            {
                foreach (var parser in parsers)
                {
                    if (parser.CanParse(uri))
                    {
                        var requestUri = this.bypassCors ? new Uri($"https://cors-anywhere.herokuapp.com/{uri.ToString()}") : uri;
                        var response = await this.httpClient.GetStringAsync(requestUri);
                        var card = parser.Parse(response);

                        if (card != null)
                        {
                            cards.Add(card);
                            break;
                        }                        
                    }
                }
            }

            return cards;
        }
    }
}
