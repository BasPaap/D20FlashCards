﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Bas.D20FlashCards.Client.Services;
using Bas.D20FlashCards.Pathfinder;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;

namespace Bas.D20FlashCards.Client.Tests
{
    [TestClass]
    public class CardsServiceTests
    {
        private CardsService cardsService;
        private readonly Dictionary<Uri, Card> cardsByUri = new Dictionary<Uri, Card>();
        private readonly List<Parser> parsers = new List<Parser>(new Parser[] { new ArchivesOfNethysParser(new Uri("https://aon.com/"), " - Archives of Nethys: Pathfinder RPG Database"),
                                                                                new D20PFSrdParser(new Uri("https://www.d20.com"), new Uri("/feats"), new Uri("/skills"))
        });

        [TestInitialize]
        public void Initialize()
        {
            this.cardsService = new CardsService(this.parsers);
        }
        
        #region GetCardsAsync

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("May the force\n\rbe with you.")]
        public async Task GetCardsAsync_UriTextDoesNotContainUris_ReturnsNull(string uriText)
        {
            // Arrange
            // Act
            var result = await this.cardsService.GetCardsAsync(uriText);

            // Assert          
            Assert.IsNull(result);
        }
        
        [TestMethod]
        [DataRow("https://aon.com/feat", "https://aon.com/skill", null, "Armor Proficiency, Medium (Combat)", "Profession (Wis; Trained Only)", null)]
        [DataRow("https://www.d20.com/feats/feat", "May the force be with you", "https://www.d20.com/skills/skill", "Armor Proficiency, Medium (Combat)", null, "Profession (Wis, Trained only)")]
        [DataRow("https://www.d20.com/feats/feat", "https://aon.com/skill", "https://www.d20.com/skills/skill", "Armor Proficiency, Medium (Combat)", "Profession (Wis; Trained Only)", "Profession (Wis, Trained only)")]
        [DataRow("You", "smell", "a wumpus", null, null, null)]
        public async Task GetCardsAsync_UriTextContainsUris_ReturnsCardsForUrisAndIgnoresRest(string firstUriText, string secondUriText, string thirdUriText, string firstCardName, string secondCardName, string thirdCardName)
        {
            // Arrange
            var uriText = $"{firstUriText}{Environment.NewLine}{secondUriText}{Environment.NewLine}{thirdUriText}";
            var testMessageHandler = new TestMessageHandler();

            int numUris = 0;
            if (Uri.TryCreate(firstUriText, UriKind.Absolute, out Uri firstUri))
            {
                testMessageHandler.AddResponseMessageToReturnForUri(firstUri, new HttpResponseMessage());
                numUris++;
            }

            if (Uri.TryCreate(secondUriText, UriKind.Absolute, out Uri secondUri))
            {
                testMessageHandler.AddResponseMessageToReturnForUri(secondUri, new HttpResponseMessage());
                numUris++;
            }

            if (Uri.TryCreate(thirdUriText, UriKind.Absolute, out Uri thirdUri))
            {
                testMessageHandler.AddResponseMessageToReturnForUri(thirdUri, new HttpResponseMessage());
                numUris++;
            }

            var testCardsService = new CardsService(this.parsers, testMessageHandler);

            // Act
            var cards = await testCardsService.GetCardsAsync(uriText);

            // Assert
            var cardNames = cards.Select(c => c.Name);
            Assert.AreEqual(numUris, cards.Count);
            
            if (!string.IsNullOrWhiteSpace(firstCardName))
            {
                Assert.IsTrue(cardNames.Contains(firstCardName));
            }
            else
            {
                Assert.IsFalse(cardNames.Contains(firstCardName));
            }

            if (!string.IsNullOrWhiteSpace(secondCardName))
            {
                Assert.IsTrue(cardNames.Contains(secondCardName));
            }
            else
            {
                Assert.IsFalse(cardNames.Contains(secondCardName));
            }

            if (!string.IsNullOrWhiteSpace(thirdCardName))
            {
                Assert.IsTrue(cardNames.Contains(thirdCardName));
            }
            else
            {
                Assert.IsFalse(cardNames.Contains(thirdCardName));
            }
        }
        #endregion
    }
}
