using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Tests
{
    [TestClass]
    public sealed class FeatTests
    {
        #region Default constructor
        [TestMethod]
        public void Constructor_IsCalled_CardTypeIsFeat()
        {
            // Arrange
            // Act
            var feat = new Feat();

            // Assert          
            Assert.AreEqual(CardType.Feat, feat.CardType);
        }
        #endregion

        #region ToString()
        [TestMethod]
        public void ToString_WithoutTitle_ReturnsFeat()
        {
            // Arrange
            var feat = new Feat();

            // Act
            var value = feat.ToString();

            // Assert          
            Assert.AreEqual("Feat \"\"", value);
        }
        
        [TestMethod]
        public void ToString_WithTitle_ReturnsFeatAndTitle()
        {
            // Arrange
            var feat = new Feat { Title = "Title" };

            // Act
            var value = feat.ToString();

            // Assert          
            Assert.AreEqual("Feat \"Title\"", value);
        }
        #endregion
    }
}
