using Bas.D20FlashCards.Pathfinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bas.D20FlashCards.Tests
{
    [TestClass]
    public sealed class D20PFSrdParserTests
    {
        private Uri baseUri = new Uri("http://www.d20.com");
        private D20PFSrdParser defaultParser;

        [TestInitialize]
        public void Initialize()
        {
            this.defaultParser = new D20PFSrdParser(this.baseUri);
        }

        #region CanParse
        [TestMethod]
        public void CanParse_UriIsNull_ThrowsArgumentNullException()
        {
            ParserTests.CanParse_UriIsNull_ThrowsArgumentNullException(defaultParser);
        }

        [TestMethod]
        public void CanParse_UriIsRelative_ThrowsArgumentException()
        {
            ParserTests.CanParse_UriIsRelative_ThrowsArgumentException(defaultParser);
        }

        [TestMethod]
        public void CanParse_UriHasSameBaseUri_ReturnsTrue()
        {
            ParserTests.CanParse_UriHasSameBaseUri_ReturnsTrue(defaultParser, new Uri(this.baseUri, "/somefeat"));
        }

        [TestMethod]
        public void CanParse_UriHasDifferentBaseUri_ReturnsFalse()
        {
            ParserTests.CanParse_UriHasDifferentBaseUri_ReturnsFalse(defaultParser, new Uri("http://www.callofcthulhu.com/somefeat"));
        }

        #endregion

        #region Parse
        [TestMethod]
        public void Parse_ResponseArgumentIsNull_ThrowsArgumentNullException()
        {
            ParserTests.Parse_ResponseArgumentIsNull_ThrowsArgumentNullException(defaultParser);
        }

        [TestMethod]
        public void Parse_ResponseArgumentIsEmptyOrWhitespace_ThrowsArgumentException()
        {
            ParserTests.Parse_ResponseArgumentIsEmptyOrWhitespace_ThrowsArgumentException(defaultParser);
        }

        [TestMethod]
        public void Parse_ResponseIsUnknown_ReturnsNull()
        {
            ParserTests.Parse_ResponseIsUnknown_ReturnsNull(defaultParser);
        }

        [TestMethod]
        [DeploymentItem("d20pfsrd_feat_response.xml")]
        public void Parse_ResponseIsFeat_ReturnsFeat()
        {
            // Arrange
            var response = File.ReadAllText(@"Pathfinder\\d20pfsrd_feat_response.xml");

            // Act
            var card = defaultParser.Parse(response);

            // Assert          
            Assert.IsNotNull(card);
            Assert.AreEqual(CardType.Feat, card.CardType);
        }
        #endregion
    }
}
