using Bas.D20FlashCards.Pathfinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bas.D20FlashCards.Client.Services
{
    public class CardsService
    {
        private List<Parser> parsers = new List<Parser>()
        {
            new ArchivesOfNethysParser(new Uri(""), ""),
            new D20PFSrdParser(new Uri(""), new Uri(""), new Uri(""))
        };

        public async Task GetCardsAsync(string uriText)
        {
            var httpClient = new HttpClient();
            var lines = uriText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var uris = from l in lines
                       where Uri.IsWellFormedUriString(l, UriKind.Absolute)
                       select new Uri(l);

            foreach (var uri in uris)
            {
                foreach (var parser in parsers)
                {
                    if (parser.CanParse(uri))
                    {
                        var response = await httpClient.GetStringAsync(uri);
                        var card = parser.Parse(response);
                        Status += card?.Name;
                        break;
                    }
                }
            }

        }

        public string Status { get; set; }
    }
}
