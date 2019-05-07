using Bas.D20FlashCards.Pathfinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bas.D20FlashCards.Tests
{
    [TestClass]
    public sealed class D20PFSrdParserTests
    {
        private const string featResponseFileName = "d20pfsrd_feat_response.txt";
        private const string skillResponseFileName = "d20pfsrd_skill_response.txt";
        private readonly Uri baseUri = new Uri("https://www.d20.com");
        private readonly Uri featsUri = new Uri("/feats", UriKind.Relative);
        private readonly Uri skillsUri = new Uri("/skills", UriKind.Relative);
        private D20PFSrdParser defaultParser;

        [TestInitialize]
        public void Initialize()
        {
            this.defaultParser = new D20PFSrdParser(this.baseUri, this.featsUri, this.skillsUri);
        }

        #region CanParse
        [TestMethod]
        public void CanParse_UriIsNull_ThrowsArgumentNullException()
        {
            ParserTests.CanParse_UriIsNull_ThrowsArgumentNullException(defaultParser);
        }

        [TestMethod]
        public void CanParse_UriIsMalFormed_ThrowsUriFormatException()
        {
            ParserTests.CanParse_UriIsMalFormed_ThrowsUriFormatException(default);
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
            "<a href=\"https://www.d20pfsrd.com/feats/combat-feats/armor-proficiency-light\">Light Armor Proficiency</a>.", 
            "See <a href=\"https://www.d20pfsrd.com/feats/combat-feats/armor-proficiency-light\">Armor Proficiency, Light</a>.", 
            "See <a href=\"https://www.d20pfsrd.com/feats/combat-feats/armor-proficiency-light\">Armor Proficiency, Light</a>.", 
            "<a href=\"https://www.d20pfsrd.com/classes/core-classes/barbarian\">Barbarians</a>, <a href=\"https://www.d20pfsrd.com/classes/core-classes/cleric\">clerics</a>, <a href=\"https://www.d20pfsrd.com/classes/core-classes/druid\">druids</a>, <a href=\"https://www.d20pfsrd.com/classes/core-classes/fighter\">fighters</a>, <a href=\"https://www.d20pfsrd.com/classes/core-classes/paladin\">paladins</a>, and <a href=\"https://www.d20pfsrd.com/classes/core-classes/ranger\">rangers</a> automatically have Medium Armor Proficiency as a bonus feat. They need not select it.")]
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
            "You are skilled at a specific job. Like <a href=\"https://www.d20pfsrd.com/skills/craft\">Craft</a>, <a href=\"https://www.d20pfsrd.com/skills/knowledge\">Knowledge</a>, and <a href=\"https://www.d20pfsrd.com/skills/perform\">Perform</a>, Profession is actually a number of separate skills. You could have several Profession skills, each with its own ranks. While a Craft skill represents ability in creating an item, a Profession skill represents an aptitude in a vocation requiring a broader range of less specific knowledge. The most common Profession skills are architect, baker, barrister, brewer, butcher, clerk, cook, courtesan, driver, engineer, farmer, fisherman, gambler, gardener, herbalist, innkeeper, librarian, merchant, midwife, miller, miner, porter, sailor, scribe, shepherd, stable master, soldier, tanner, trapper, and woodcutter.",
            "You can earn half your Profession check result in gold pieces per week of dedicated work. You know how to use the tools of your trade, how to perform the profession&#8217;s daily tasks, how to supervise helpers, and how to handle common problems. You can also answer questions about your Profession. Basic questions are DC 10, while more complex questions are DC 15 or higher.",
            "Not applicable. A single check generally represents a week of work.",
            "Varies. An attempt to use a Profession skill to earn income cannot be retried. You are stuck with whatever weekly wage your check result brought you. Another check may be made after a week to determine a new income for the next period of time. An attempt to accomplish some specific task can usually be retried.",
            "<ul>  <li>    <b>Race</b> A <a href=\"https://www.d20pfsrd.com/races/core-races/gnome\">gnome</a> gets a +2 bonus on a <a href=\"https://www.d20pfsrd.com/skills/craft\">Craft</a> or Profession skill of her choice.                          </li></ul>",
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
            Assert.AreEqual(action, skill.Action);
            Assert.AreEqual(tryAgain, skill.TryAgain);
            Assert.AreEqual(special, skill.Special);
            Assert.AreEqual(untrained, skill.Untrained);
            Assert.AreEqual(check, skill.Check);
        }
        #endregion

        #region GetCardType
        [TestMethod]
        public void GetCardType_ResponseIsUnknownCardType_ReturnsNull()
        {
            // Arrange
            // Act
            var card = defaultParser.Parse("Unknown card type response.");

            // Assert          
            Assert.IsNull(card);
        }

        [TestMethod]
        [DeploymentItem(skillResponseFileName)]
        [DeploymentItem(featResponseFileName)]
        [DataRow(skillResponseFileName, typeof(Skill))]
        [DataRow(featResponseFileName, typeof(Feat))]
        public void GetCardType_ResponseIsKnownCardType_ReturnsCorrectCardType(string fileName, Type cardType)
        {
            // Arrange
            var response = File.ReadAllText($"Pathfinder\\{fileName}");

            // Act
            var card = defaultParser.Parse(response);

            // Assert          
            Assert.IsInstanceOfType(card, cardType);
        }
        #endregion
    }
}
