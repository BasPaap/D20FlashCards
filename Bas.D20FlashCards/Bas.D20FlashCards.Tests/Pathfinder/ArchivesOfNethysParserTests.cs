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
        private Uri baseUri = new Uri("http://www.aon.com");
        private ArchivesOfNethysParser defaultParser;

        [TestInitialize]
        public void Initialize()
        {
            this.defaultParser = new ArchivesOfNethysParser(baseUri);
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
        #endregion
    }
}
