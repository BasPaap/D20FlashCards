using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Tests
{
    [TestClass]
    public abstract class ParserTests
    {
        protected void CanParse_UriIsRelative_ThrowsArgumentException(Parser parser)
        {
            // Arrange
            // Act          
            // Assert          
            var exception = Assert.ThrowsException<ArgumentException>(() => parser.CanParse(new Uri(string.Empty)));
            Assert.AreEqual("uri", exception.ParamName);
        }

        protected void CanParse_UriIsNull_ThrowsArgumentNullException(Parser parser)
        {
            // Arrange
            // Act          
            // Assert          
            var exception = Assert.ThrowsException<ArgumentNullException>(() => parser.CanParse(null));
            Assert.AreEqual("uri", exception.ParamName);
        }

        protected void CanParse_UriHasSameBaseUri_ReturnsTrue(Parser parser, Uri uri)
        {
            // Arrange
            // Act
            var result = parser.CanParse(uri);

            // Assert
            Assert.IsTrue(result);
        }

        protected void CanParse_UriHasDifferentBaseUri_ReturnsFalse(Parser parser, Uri uri)
        {
            // Arrange
            // Act
            var result = parser.CanParse(uri);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
