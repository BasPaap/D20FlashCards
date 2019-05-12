﻿using Bas.D20FlashCards.Pathfinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bas.D20FlashCards.Client.Services
{
    public class CardsService
    {
        private readonly List<Parser> parsers = new List<Parser>();

        public CardsService(IEnumerable<Parser> parsers)
        {
            this.parsers.AddRange(parsers);
        }

        public async Task<ICollection<Card>> GetCardsAsync(string uriText)
        {
            //var httpClient = new HttpClient();
            //var lines = uriText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            //var uris = from l in lines
            //           where Uri.IsWellFormedUriString(l, UriKind.Absolute)
            //           select new Uri(l);

            //foreach (var uri in uris)
            //{
            //    foreach (var parser in parsers)
            //    {
            //        if (parser.CanParse(uri))
            //        {
            //            var response = await httpClient.GetStringAsync(uri);
            //            var card = parser.Parse(response);
            //            Status += card?.Name;
            //            break;
            //        }
            //    }
            //}

            throw new NotImplementedException();
        }

        public string Status { get; set; }
    }
}
