using Bas.D20FlashCards.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Tests
{
    [TestClass]
    public class ParserTests
    {
        #region Parse

        [TestMethod]
        [DataRow(typeof(Feat), true, false)]
        [DataRow(typeof(Skill), false, true)]
        public void Parse_GetCardTypeReturnsCardType_CallsGetFunctionForCardType(Type cardType, bool isGetFeatCalled, bool isGetSkillCalled)
        {
            // Arrange
            var parser = new ParserMock(cardType);

            // Act
            parser.Parse(string.Empty);

            // Assert          
            Assert.AreEqual(isGetFeatCalled, parser.IsGetFeatCalled);
            Assert.AreEqual(isGetSkillCalled, parser.IsGetSkillCalled);
        }


        [TestMethod]
        [DataRow(typeof(Feat), "Feat Response", "Feat Response", null)]
        [DataRow(typeof(Skill), "Skill Response", null, "Skill Response")]
        public void Parse_ValidResponseIsPassedToParse_GetCardTypeAndGetFunctionForCardTypeAreCalledWithSameResponse(Type cardType, string response, string featResponse, string skillResponse)
        {
            // Arrange
            var parser = new ParserMock(cardType);

            // Act
            parser.Parse(response);

            // Assert          
            Assert.AreEqual(featResponse, parser.ResponsePassedToGetFeat);
            Assert.AreEqual(skillResponse, parser.ResponsePassedToGetSkill);
            Assert.AreEqual(response, parser.ResponsePassedToGetCardType);
        }
        #endregion

        public static void CanParse_UriIsMalFormed_ThrowsUriFormatException(Parser parser)
        {
            // Arrange
            // Act          
            // Assert          
            var exception = Assert.ThrowsException<UriFormatException>(() => parser.CanParse(new Uri(string.Empty)));            
        }

        public static void CanParse_UriIsRelative_ThrowsArgumentException(Parser parser)
        {
            // Arrange
            // Act          
            // Assert          
            var exception = Assert.ThrowsException<ArgumentException>(() => parser.CanParse(new Uri("/something")));
            Assert.AreEqual("uri", exception.ParamName);
        }

        public static void CanParse_UriIsNull_ThrowsArgumentNullException(Parser parser)
        {
            // Arrange
            // Act          
            // Assert          
            var exception = Assert.ThrowsException<ArgumentNullException>(() => parser.CanParse(null));
            Assert.AreEqual("uri", exception.ParamName);
        }

        public static void CanParse_UriHasSameBaseUri_ReturnsTrue(Parser parser, Uri uri)
        {
            // Arrange
            // Act
            var result = parser.CanParse(uri);

            // Assert
            Assert.IsTrue(result);
        }

        public static void CanParse_UriHasDifferentBaseUri_ReturnsFalse(Parser parser, Uri uri)
        {
            // Arrange
            // Act
            var result = parser.CanParse(uri);

            // Assert
            Assert.IsFalse(result);
        }

        public static void Parse_ResponseArgumentIsNull_ThrowsArgumentNullException(Parser parser)
        {
            // Arrange
            // Act
            // Assert          
            var exception = Assert.ThrowsException<ArgumentNullException>(() => parser.Parse(null));
            Assert.AreEqual("response", exception.ParamName);
        }

        public static void Parse_ResponseArgumentIsEmptyOrWhitespace_ThrowsArgumentException(Parser parser)
        {
            // Arrange
            // Act          
            // Assert          
            var exception = Assert.ThrowsException<ArgumentException>(() => parser.Parse(string.Empty));
            Assert.AreEqual("response", exception.ParamName);
        }

        public static void Parse_ResponseIsUnknown_ReturnsNull(Parser parser)
        {
            // Arrange
            var response = "MacArthur's Park is melting in the dark, all the sweet green icing flowing down.";

            // Act
            var card = parser.Parse(response);

            // Assert          
            Assert.IsNull(card);
        }
    }
}
