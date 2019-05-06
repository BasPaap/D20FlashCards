using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Bas.D20FlashCards.Tests
{
    [TestClass]
    public sealed class SkillTests
    {
        #region Default constructor
        [TestMethod]
        public void Constructor_IsCalled_CardTypeIsSkill()
        {
            // Arrange
            // Act
            var skill = new Skill();

            // Assert          
            Assert.AreEqual(CardType.Skill, skill.CardType);
        }
        #endregion

        #region ToString()
        [TestMethod]
        public void ToString_WithEmptyTitle_ReturnsSkill()
        {
            // Arrange
            var skill = new Skill();

            // Act
            var value = skill.ToString();

            // Assert          
            Assert.AreEqual("Skill \"\"", value);
        }

        [TestMethod]
        public void ToString_WithTitle_ReturnsSkillAndTitle()
        {
            // Arrange
            var skill = new Skill { Title = "Title" };

            // Act
            var value = skill.ToString();

            // Assert          
            Assert.AreEqual("Skill \"Title\"", value);
        }
        #endregion
    }
}
