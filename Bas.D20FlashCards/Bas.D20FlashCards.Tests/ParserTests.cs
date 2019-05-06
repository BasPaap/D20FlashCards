using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Tests
{
    [TestClass]
    public abstract class ParserTests
    {
        #region CanParse
        [TestMethod]
        public void CanParse_UriIsRelative_ThrowsArgumentException(Parser parser)
        {
            // Arrange
            // Act          
            // Assert          
            var exception = Assert.ThrowsException<ArgumentException>(() => parser.CanParse(new Uri(string.Empty)));
            Assert.AreEqual("uri", exception.ParamName);
        }

        [TestMethod]
        public void CanParse_UriIsNull_ThrowsArgumentNullException(Parser parser)
        {
            // Arrange
            // Act          
            // Assert          
            var exception = Assert.ThrowsException<ArgumentNullException>(() => parser.CanParse(null));
            Assert.AreEqual("uri", exception.ParamName);
        }
        #endregion
    }
}
