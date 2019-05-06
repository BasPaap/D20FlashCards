using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Tests
{
    [TestClass]
    public static class ParserTests
    {
        public static void CanParse_UriIsRelative_ThrowsArgumentException(Parser parser)
        {
            // Arrange
            // Act          
            // Assert          
            var exception = Assert.ThrowsException<ArgumentException>(() => parser.CanParse(new Uri(string.Empty)));
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
