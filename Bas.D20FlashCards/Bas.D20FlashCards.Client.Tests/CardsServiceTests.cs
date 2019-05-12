using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Bas.D20FlashCards.Client.Services;
using Bas.D20FlashCards.Pathfinder;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.IO;

namespace Bas.D20FlashCards.Client.Tests
{
    [TestClass]
    public class CardsServiceTests
    {
        private const string aonFeatResponseFileName = "aon_feat_response.txt";
        private const string aonSkillResponseFileName = "aon_skill_response.txt";
        private const string d20pfsrdFeatResponseFileName = "d20pfsrd_feat_response.txt";
        private const string d20pfsrdSkillResponseFileName = "d20pfsrd_skill_response.txt";

        private CardsService cardsService;
        private readonly Dictionary<Uri, Card> cardsByUri = new Dictionary<Uri, Card>();
        private readonly List<Parser> parsers = new List<Parser>(new Parser[] { new ArchivesOfNethysParser(new Uri("https://aon.com"), " - Archives of Nethys: Pathfinder RPG Database"),
                                                                                new D20PFSrdParser(new Uri("https://www.d20.com"), new Uri("/feats", UriKind.Relative), new Uri("/skills", UriKind.Relative))
        });

        [TestInitialize]
        public void Initialize()
        {
            this.cardsService = new CardsService(this.parsers, null, false);
        }

        #region GetCardsAsync

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("May the force\r\nbe with you.")]
        public async Task GetCardsAsync_UriTextDoesNotContainUris_ReturnsEmptyCollection(string uriText)
        {
            // Arrange
            // Act
            var result = await this.cardsService.GetCardsAsync(uriText);

            // Assert          
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DeploymentItem(aonFeatResponseFileName)]
        [DeploymentItem(aonSkillResponseFileName)]
        [DeploymentItem(d20pfsrdSkillResponseFileName)]
        [DeploymentItem(d20pfsrdFeatResponseFileName)]
        [DataRow("https://aon.com/feat", "https://aon.com/skill", null, "Armor Proficiency, Medium (Combat)", "Profession (Wis; Trained Only)", null, aonFeatResponseFileName, aonSkillResponseFileName, null)]
        [DataRow("https://www.d20.com/feats/feat", "May the force be with you", "https://www.d20.com/skills/skill", "Armor Proficiency, Medium (Combat)", null, "Profession (Wis, Trained only)", d20pfsrdFeatResponseFileName, null, d20pfsrdSkillResponseFileName)]
        [DataRow("https://www.d20.com/feats/feat", "https://aon.com/skill", "https://www.d20.com/skills/skill", "Armor Proficiency, Medium (Combat)", "Profession (Wis; Trained Only)", "Profession (Wis, Trained only)", d20pfsrdFeatResponseFileName, aonSkillResponseFileName, d20pfsrdSkillResponseFileName)]
        [DataRow("You", "smell", "a wumpus", null, null, null, null, null, null)]
        public async Task GetCardsAsync_UriTextContainsUris_ReturnsCardsForUrisAndIgnoresRest(string firstUriText,
            string secondUriText,
            string thirdUriText,
            string firstCardName,
            string secondCardName,
            string thirdCardName,
            string firstFileName,
            string secondFileName,
            string thirdFileName)
        {
            // Arrange
            var uriText = $"{firstUriText}{Environment.NewLine}{secondUriText}{Environment.NewLine}{thirdUriText}";
            var testMessageHandler = new TestMessageHandler();

            int numUris = 0;
            if (Uri.TryCreate(firstUriText, UriKind.Absolute, out Uri firstUri))
            {
                testMessageHandler.AddResponseMessageToReturnForUri(firstUri, new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(File.ReadAllText(firstFileName))
                });
                numUris++;
            }

            if (Uri.TryCreate(secondUriText, UriKind.Absolute, out Uri secondUri))
            {
                testMessageHandler.AddResponseMessageToReturnForUri(secondUri, new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(File.ReadAllText(secondFileName))
                });
                numUris++;                
            }

            if (Uri.TryCreate(thirdUriText, UriKind.Absolute, out Uri thirdUri))
            {
                testMessageHandler.AddResponseMessageToReturnForUri(thirdUri, new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(File.ReadAllText(thirdFileName))
                });
                numUris++;
            }

            var testCardsService = new CardsService(this.parsers, testMessageHandler, false);

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
