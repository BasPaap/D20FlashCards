using Bas.D20FlashCards.Pathfinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Bas.D20FlashCards.Tests
{
    [TestClass]
    public sealed class ArchivesOfNethysParserTests
    {
        private const string featResponseFileName = "aon_feat_response.txt";
        private const string skillResponseFileName = "aon_skill_response.txt";
        private readonly Uri baseUri = new Uri("http://www.aon.com");
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

        [TestMethod]
        public void Parse_ResponseIsUnknown_ReturnsNull()
        {
            ParserTests.Parse_ResponseIsUnknown_ReturnsNull(defaultParser);
        }

        [TestMethod]
        [DeploymentItem(featResponseFileName)]
        [DataRow(featResponseFileName, 
            typeof(Feat), 
            "Armor Proficiency, Medium (Combat)",
            "You are skilled at wearing medium armor.",
            "Light Armor Proficiency.",
            "See Armor Proficiency, Light.",
            "See Armor Proficiency, Light.",
            "Barbarians, clerics, druids, fighters, paladins, and rangers automatically have Medium Armor Proficiency as a bonus feat. They need not select it.")]
        public void Parse_ResponseIsFeat_ReturnsFeat(string fileName,
                                                     Type instanceType,
                                                     string name,
                                                     string description,
                                                     string prerequisite,
                                                     string benefit,
                                                     string normal,
                                                     string special)
        {
            // Arrange
            var response = File.ReadAllText($"Pathfinder\\{fileName}");

            // Act
            var card = defaultParser.Parse(response);

            // Assert          
            Assert.IsNotNull(card);
            Assert.IsInstanceOfType(card, instanceType);
            Assert.AreEqual(name, card.Name);

            var feat = card as Feat;
            Assert.AreEqual(description, feat.Description);
            Assert.AreEqual(prerequisite, feat.Prerequisite);
            Assert.AreEqual(benefit, feat.Benefit);
            Assert.AreEqual(normal, feat.Normal);
            Assert.AreEqual(special, feat.Special);
        }

        [TestMethod]
        [DeploymentItem(skillResponseFileName)]
        [DataRow(skillResponseFileName, 
            typeof(Skill),
            "Profession (Wis, Trained only)",
            "You are skilled at a specific job. Like Craft, Knowledge, and Perform, Profession is actually a number of separate skills. You could have several Profession skills, each with its own ranks. While a Craft skill represents ability in creating an item, a Profession skill represents an aptitude in a vocation requiring a broader range of less specific knowledge. The most common Profession skills are architect, baker, barrister, brewer, butcher, clerk, cook, courtesan, driver, engineer, farmer, fisherman, gambler, gardener, herbalist, innkeeper, librarian, merchant, midwife, miller, miner, porter, sailor, scribe, shepherd, stable master, soldier, tanner, trapper, and woodcutter.",
            "You can earn half your Profession check result in gold pieces per week of dedicated work. You know how to use the tools of your trade, how to perform the profession’s daily tasks, how to supervise helpers, and how to handle common problems. You can also answer questions about your Profession. Basic questions are DC 10, while more complex questions are DC 15 or higher.",
            "Not applicable. A single check generally represents a week of work.",
            "Varies. An attempt to use a Profession skill to earn income cannot be retried. You are stuck with whatever weekly wage your check result brought you. Another check may be made after a week to determine a new income for the next period of time. An attempt to accomplish some specific task can usually be retried.",
            "A gnome gets a +2 bonus on a Craft or Profession skill of her choice.",
            "Untrained laborers and assistants (that is, characters without any ranks in Profession) earn an average of 1 silver piece per day.")]
        public void Parse_ResponseIsSkill_ReturnsSkill(string fileName,
                                                     Type instanceType,
                                                     string name,
                                                     string description,
                                                     string check,
                                                     string action,
                                                     string tryAgain,
                                                     string special,
                                                     string untrained)
        {
            // Arrange
            var response = File.ReadAllText($"Pathfinder\\{fileName}");

            // Act
            var card = defaultParser.Parse(response);

            // Assert          
            Assert.IsNotNull(card);
            Assert.IsInstanceOfType(card, instanceType);
            Assert.AreEqual(name, card.Name);

            var skill = card as Skill;
            Assert.AreEqual(description, skill.Description);
            Assert.AreEqual(check, skill.Check);
            Assert.AreEqual(action, skill.Action);
            Assert.AreEqual(tryAgain, skill.TryAgain);
            Assert.AreEqual(special, skill.Special);
            Assert.AreEqual(untrained, skill.Untrained);
        }
        #endregion
    }
}
