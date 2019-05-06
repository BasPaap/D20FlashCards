using Bas.D20FlashCards.Pathfinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Tests
{
    [TestClass]
    public sealed class D20PFSrdParserTests : ParserTests
    {
        private Uri baseUri = new Uri("http://www.d20.com");

        #region CanParse
        [TestMethod]
        public void CanParse_UriIsNull_ThrowsArgumentNullException()
        {
            var parser = new D20PFSrdParser(this.baseUri);
            CanParse_UriIsNull_ThrowsArgumentNullException(parser);
        }

        [TestMethod]
        public void CanParse_UriIsRelative_ThrowsArgumentException()
        {
            var parser = new D20PFSrdParser(this.baseUri);
            CanParse_UriIsRelative_ThrowsArgumentException(parser);
        }

        [TestMethod]
        public void CanParse_UriHasSameBaseUri_ReturnsTrue()
        {
            var parser = new D20PFSrdParser(this.baseUri);
            CanParse_UriHasSameBaseUri_ReturnsTrue(parser, new Uri(this.baseUri, "/somefeat"));
        }

        [TestMethod]
        public void CanParse_UriHasDifferentBaseUri_ReturnsFalse()
        {
            var parser = new D20PFSrdParser(this.baseUri);
            CanParse_UriHasDifferentBaseUri_ReturnsFalse(parser, new Uri("http://www.callofcthulhu.com/somefeat"));
        }

        #endregion
    }
}
