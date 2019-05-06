using Bas.D20FlashCards.Pathfinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Tests
{
    [TestClass]
    public sealed class D20PFSrdParserTests
    {
        private const string d20PFSrdBaseUri = "https://aonprd.com";

        #region CanParse
        [TestMethod]
        public void CanParse_UriIsRelative_ThrowsArgumentException()
        {
            // Arrange
            // Act          
            // Assert          
            var exception = Assert.ThrowsException<ArgumentException>(() => D20PFSrdParser.CanParse(new Uri(string.Empty), new Uri(d20PFSrdBaseUri, UriKind.Absolute)));
            Assert.AreEqual("uri", exception.ParamName);
        }

        [TestMethod]
        public void CanPase_UriIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            // Act          
            // Assert          
            var exception = Assert.ThrowsException<ArgumentNullException>(() => D20PFSrdParser.CanParse(null, new Uri(d20PFSrdBaseUri, UriKind.Absolute)));
            Assert.AreEqual("uri", exception.ParamName);
        }
        #endregion
    }
}
