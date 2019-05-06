using Bas.D20FlashCards.Pathfinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Tests
{
    [TestClass]
    public sealed class ArchivesOfNethysParserTests
    {
        private const string baseUri = "aonpd.com";

        private const string archivesOfNethysBaseUri = "https://aonprd.com";

        #region CanParse
        [TestMethod]
        public void CanParse_UriIsRelative_ThrowsArgumentException()
        {
            // Arrange
            // Act          
            // Assert          
            var exception = Assert.ThrowsException<ArgumentException>(() => ArchivesOfNethysParser.CanParse(new Uri(string.Empty), new Uri(archivesOfNethysBaseUri, UriKind.Absolute)));
            Assert.AreEqual("uri", exception.ParamName);
        }

        [TestMethod]
        public void CanPase_UriIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            // Act          
            // Assert          
            var exception = Assert.ThrowsException<ArgumentNullException>(() => ArchivesOfNethysParser.CanParse(null, new Uri(archivesOfNethysBaseUri, UriKind.Absolute)));
            Assert.AreEqual("uri", exception.ParamName);
        }
        #endregion
    }
}
