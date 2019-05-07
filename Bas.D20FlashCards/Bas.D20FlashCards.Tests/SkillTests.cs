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
        #region ToString()
        [TestMethod]
        public void ToString_WithEmptyName_ReturnsSkill()
        {
            // Arrange
            var skill = new Skill();

            // Act
            var value = skill.ToString();

            // Assert          
            Assert.AreEqual("Skill", value);
        }

        [TestMethod]
        public void ToString_WithName_ReturnsSkillAndName()
        {
            // Arrange
            var skill = new Skill { Name = "Name" };

            // Act
            var value = skill.ToString();

            // Assert          
            Assert.AreEqual("Skill \"Name\"", value);
        }
        #endregion
    }
}
