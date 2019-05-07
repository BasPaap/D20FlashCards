using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Tests
{
    [TestClass]
    public sealed class FeatTests
    {
        #region ToString()
        [TestMethod]
        public void ToString_WithoutName_ReturnsFeat()
        {
            // Arrange
            var feat = new Feat();

            // Act
            var value = feat.ToString();

            // Assert          
            Assert.AreEqual("Feat \"\"", value);
        }
        
        [TestMethod]
        public void ToString_WithName_ReturnsFeatAndName()
        {
            // Arrange
            var feat = new Feat { Name = "Name" };

            // Act
            var value = feat.ToString();

            // Assert          
            Assert.AreEqual("Feat \"Name\"", value);
        }
        #endregion
    }
}
