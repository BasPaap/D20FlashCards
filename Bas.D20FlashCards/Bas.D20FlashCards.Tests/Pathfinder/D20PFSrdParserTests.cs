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
        private const string weaponFinesseFeatResponseFileName = "d20_pfsrd_feat_response_weaponfinesse.txt";
        private const string acrobaticsSkillResponseFileName = "d20_pfsrd_skill_response_acrobatics.txt";
        private const string appraiseSkillResponseFileName = "d20_pfsrd_skill_response_appraise.txt";
        private const string climbSkillResponseFileName = "d20_pfsrd_skill_response_climb.txt";
        private const string craftSkillResponseFileName = "d20_pfsrd_skill_response_craft.txt";
        private const string disableDeviceSkillResponseFileName = "d20_pfsrd_skill_response_disabledevice.txt";
        private const string featResponseFileName = "d20pfsrd_feat_response.txt";
        private const string skillResponseFileName = "d20pfsrd_skill_response.txt";
        private readonly Uri baseUri = new Uri("https://www.d20.com");
        private readonly Uri featsUri = new Uri("/feats", UriKind.Relative);
        private readonly Uri skillsUri = new Uri("/skills", UriKind.Relative);
        private D20PFSrdParser defaultParser;
        private D20PFSrdParser upperCaseUriParser;

        [TestInitialize]
        public void Initialize()
        {
            this.defaultParser = new D20PFSrdParser(this.baseUri, this.featsUri, this.skillsUri);
            this.upperCaseUriParser = new D20PFSrdParser(this.baseUri, 
                new Uri(this.featsUri.ToString().ToUpper(), UriKind.Relative), 
                new Uri(this.skillsUri.ToString().ToUpper(), UriKind.Relative));
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
        [DeploymentItem(weaponFinesseFeatResponseFileName)]
        [DataRow(weaponFinesseFeatResponseFileName, 
            typeof(Feat),
            "Weapon Finesse (Combat)",
            "You are trained in using your agility in melee combat, as opposed to brute strength.", 
            null,
            "With a light weapon, elven curve blade, rapier, whip, or spiked chain made for a creature of your size category, you may use your Dexterity modifier instead of your Strength modifier on attack rolls. If you carry a shield, its armor check penalty applies to your attack rolls.", 
            null,
            "Natural weapons are considered light weapons.")]
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
            Assert.AreEqual(prerequisite, feat.Prerequisites);
            Assert.AreEqual(benefit, feat.Benefit);
            Assert.AreEqual(normal, feat.Normal);
            Assert.AreEqual(special, feat.Special);
        }

        [TestMethod]
        [DeploymentItem(skillResponseFileName)]
        [DeploymentItem(acrobaticsSkillResponseFileName)]
        [DeploymentItem(appraiseSkillResponseFileName)]
        [DeploymentItem(climbSkillResponseFileName)]
        [DeploymentItem(craftSkillResponseFileName)]
        [DeploymentItem(disableDeviceSkillResponseFileName)]
        [DataRow(skillResponseFileName, 
            typeof(Skill),
            "Profession (Wis, Trained only)", 
            "You are skilled at a specific job. Like Craft, Knowledge, and Perform, Profession is actually a number of separate skills. You could have several Profession skills, each with its own ranks. While a Craft skill represents ability in creating an item, a Profession skill represents an aptitude in a vocation requiring a broader range of less specific knowledge. The most common Profession skills are architect, baker, barrister, brewer, butcher, clerk, cook, courtesan, driver, engineer, farmer, fisherman, gambler, gardener, herbalist, innkeeper, librarian, merchant, midwife, miller, miner, porter, sailor, scribe, shepherd, stable master, soldier, tanner, trapper, and woodcutter.",
            "You can earn half your Profession check result in gold pieces per week of dedicated work. You know how to use the tools of your trade, how to perform the profession’s daily tasks, how to supervise helpers, and how to handle common problems. You can also answer questions about your Profession. Basic questions are DC 10, while more complex questions are DC 15 or higher.",
            "Not applicable. A single check generally represents a week of work.",
            "Varies. An attempt to use a Profession skill to earn income cannot be retried. You are stuck with whatever weekly wage your check result brought you. Another check may be made after a week to determine a new income for the next period of time. An attempt to accomplish some specific task can usually be retried.",
            "A gnome gets a +2 bonus on a Craft or Profession skill of her choice.",
            "Untrained laborers and assistants (that is, characters without any ranks in Profession) earn an average of 1 silver piece per day.")]
        [DataRow(acrobaticsSkillResponseFileName,
            typeof(Skill),
            "Acrobatics (Dex; Armor Check Penalty)",
            "You can keep your balance while traversing narrow or treacherous surfaces. You can also dive, flip, jump, and roll, avoiding attacks and confusing your opponents.",
            "First, you can use Acrobatics to move on narrow surfaces and uneven ground without falling. A successful check allows you to move at half speed across such surfaces—only one check is needed per round. Use the following table to determine the base DC, which is then modified by the Acrobatics skill modifiers noted below. While you are using Acrobatics in this way, you are considered flat-footed and lose your Dexterity bonus to your AC (if any). If you take damage while using Acrobatics, you must immediately make another Acrobatics check at the same DC to avoid falling or being knocked prone. In addition, you can move through a threatened square without provoking an attack of opportunity from an enemy by using Acrobatics. When moving in this way, you move at half speed. You can move at full speed by increasing the DC of the check by 10. You cannot use Acrobatics to move past foes if your speed is reduced due to carrying a medium or heavy load or wearing medium or heavy armor. If an ability allows you to move at full speed under such conditions, you can use Acrobatics to move past foes. You can use Acrobatics in this way while prone, but doing so requires a full-round action to move 5 feet, and the DC is increased by 5. If you attempt to move through an enemy’s space and fail the check, you lose the move action and provoke an attack of opportunity. Finally, you can use the Acrobatics skill to make jumps or to soften a fall. The base DC to make a jump is equal to the distance to be crossed (if horizontal) or four times the height to be reached (if vertical). These DCs double if you do not have at least 10 feet of space to get a running start. The only Acrobatics modifiers that apply are those concerning the surface you are jumping from. If you fail this check by 4 or less, you can attempt a DC 20 Reflex save to grab hold of the other side after having missed the jump. If you fail by 5 or more, you fail to make the jump and fall (or land prone, in the case of a vertical jump).",
            "None. An Acrobatics check is made as part of another action or as a reaction to a situation.",
            null,
            "If you have 3 or more ranks in Acrobatics, you gain a +3 dodge bonus to AC when fighting defensively instead of the usual +2, and a +6 dodge bonus to AC when taking the total defense action instead of the usual +4. f you have the Acrobatic feat, you get a + 2 bonus on all Acrobatics checks.If you have 10 or more ranks in Acrobatics, the bonus increases to + 4.",
            null)]
        [DataRow(appraiseSkillResponseFileName,
            typeof(Skill),
            "Appraise (Int)",
            "You can evaluate the monetary value of an object.",
            "A DC 20 Appraise check determines the value of a common item. If you succeed by 5 or more, you also determine if the item has magic properties, although this success does not grant knowledge of the magic item’s abilities. If you fail the check by less than 5, you determine the price of that item to within 20% of its actual value. If you fail this check by 5 or more, the price is wildly inaccurate, subject to GM discretion. Particularly rare or exotic items might increase the DC of this check by 5 or more. You can also use this check to determine the most valuable item visible in a treasure hoard.The DC of this check is generally 20 but can increase to as high as 30 for a particularly large hoard.",
            "Appraising an item takes 1 standard action. Determining the most valuable object in a treasure hoard takes 1 full-round action.",
            "Additional attempts to Appraise an item reveal the same result.",
            "Dwarves get a +2 bonus to determine on Appraise checks made to determine the price of non-magical goods that contain precious metals or gemstones. A spellcaster with a raven familiar gains a + 3 bonus on Appraise checks.",
            null)]
        [DataRow(climbSkillResponseFileName,
            typeof(Skill),
            "Climb (Str; Armor Check Penalty)",
            "You are skilled at scaling vertical surfaces, from smooth city walls to rocky cliffs.",
            "With a successful Climb check, you can advance up, down, or across a slope, wall, or other steep incline (or even across a ceiling, provided it has handholds) at one-quarter your normal speed. A slope is considered to be any incline at an angle measuring less than 60 degrees; a wall is any incline at an angle measuring 60 degrees or more. A Climb check that fails by 4 or less means that you make no progress, and one that fails by 5 or more means that you fall from whatever height you have already attained. The DC of the check depends on the conditions of the climb. Compare the task with those on the following table to determine an appropriate DC. You need both hands free to climb, but you may cling to a wall with one hand while you cast a spell or take some other action that requires only one hand.While climbing, you can’t move to avoid a blow, so you lose your Dexterity bonus to AC(if any).You also can’t use a shield while climbing.Anytime you take damage while climbing, make a Climb check against the DC of the slope or wall.Failure means you fall from your current height and sustain the appropriate falling damage.",
            "Climbing is part of movement, so it’s generally part of a move action (and may be combined with other types of movement in a move action). Each move action that includes any climbing requires a separate Climb check. Catching yourself or another falling character doesn’t take an action.",
            null,
            "You can use a rope to haul a character upward (or lower a character) through sheer strength. You can lift double your maximum load in this manner. A creature with a climb speed has a + 8 racial bonus on all Climb checks.The creature must make a Climb check to climb any wall or slope with a DC higher than 0, but it can always choose to take 10, even if rushed or threatened while climbing.If a creature with a climb speed chooses an accelerated climb(see above), it moves at double its climb speed(or at its land speed, whichever is slower) and makes a single Climb check at a –5 penalty.Such a creature retains its Dexterity bonus to Armor Class(if any) while climbing, and opponents get no special bonus to their attacks against it.It cannot, however, use the run action while climbing. If you have the Athletic feat, you get a + 2 bonus on Climb checks.If you have 10 or more ranks in Climb, the bonus increases to + 4. Any creature of Tiny or smaller size should use its Dex modifier instead of its Str modifier for Climb and Swim checks(see FAQ).",
            null)]
        [DataRow(craftSkillResponseFileName,
            typeof(Skill),
            "Craft (Int)",
            "You are skilled in the creation of a specific group of items, such as armor or weapons. Like Knowledge, Perform, and Profession, Craft is actually a number of separate skills. You could have several Craft skills, each with its own ranks. The most common Craft skills are alchemy, armor, baskets, books, bows, calligraphy, carpentry, cloth, clothing, glass, jewelry, leather, locks, paintings, pottery, sculptures, ships, shoes, stonemasonry, traps, and weapons. A Craft skill is specifically focused on creating something.If nothing is created by the endeavor, it probably falls under the heading of a Profession skill.",
            null,
            "Craft checks are made by the day or week (see above).",
            "Yes, but if you fail a check by 4 or less, you make no progress this week (or day, see below). If you miss by 5 or more, you ruin half the raw materials and have to pay half the original raw material cost again.",
            "ll crafts require artisan’s tools to give the best chance of success. If improvised tools are used, the check is made with a –2 penalty. On the other hand, masterwork artisan’s tools provide a +2 circumstance bonus on the check. In some cases, the fabricate spell can be used to achieve the results of a Craft check with no actual check involved.You must still make an appropriate Craft check when using the spell to make articles requiring a high degree of craftsmanship. A successful Craft check related to woodworking in conjunction with the casting of the ironwood spell enables you to make wooden items that have the strength of steel. When casting the spell minor creation, you must succeed on an appropriate Craft check to make a complex item. You can make checks by the day instead of by the week if desired.In this case your progress (check result × DC) should be divided by the number of days in a week.",
            null)]
        [DataRow(disableDeviceSkillResponseFileName,
            typeof(Skill),
            "Disable Device (Dex; Armor Check Penalty; Trained Only)",
            "You are skilled at disarming traps and opening locks. In addition, this skill lets you sabotage simple mechanical devices, such as catapults, wagon wheels, and doors.",
            null,
            "The amount of time needed to make a Disable Device check depends on the task: Simple Device: Disabling a simple device takes 1 round and is a full - round action. Complex / Intricate Device: An intricate or complex device requires 1d4 or 2d4 rounds.Attempting to open a lock is a full - round action.",
            "Yes. You can retry checks made to open locks.",
            "If you have the Deft Hands feat, you get a +2 bonus on Disable Device skill checks. If you have 10 or more ranks in Disable Device, the bonus increases to +4.",
            null)]
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
        [DeploymentItem(acrobaticsSkillResponseFileName)]
        [DeploymentItem(appraiseSkillResponseFileName)]
        [DeploymentItem(climbSkillResponseFileName)]
        [DeploymentItem(craftSkillResponseFileName)]
        [DeploymentItem(disableDeviceSkillResponseFileName)]
        [DeploymentItem(weaponFinesseFeatResponseFileName)]
        [DataRow(skillResponseFileName, typeof(Skill))]
        [DataRow(acrobaticsSkillResponseFileName, typeof(Skill))]
        [DataRow(appraiseSkillResponseFileName, typeof(Skill))]
        [DataRow(climbSkillResponseFileName, typeof(Skill))]
        [DataRow(craftSkillResponseFileName, typeof(Skill))]
        [DataRow(disableDeviceSkillResponseFileName, typeof(Skill))]
        [DataRow(featResponseFileName, typeof(Feat))]
        [DataRow(weaponFinesseFeatResponseFileName, typeof(Feat))]
        public void GetCardType_UriDiffersInCase_ReturnsCorrectCardType(string fileName, Type cardType)
        {
            // Arrange
            var response = File.ReadAllText($"Pathfinder\\{fileName}");
            
            // Act
            var card = upperCaseUriParser.Parse(response);

            // Assert          
            Assert.IsInstanceOfType(card, cardType);
        }

        [TestMethod]
        [DeploymentItem(skillResponseFileName)]
        [DeploymentItem(featResponseFileName)]
        [DeploymentItem(acrobaticsSkillResponseFileName)]
        [DeploymentItem(appraiseSkillResponseFileName)]
        [DeploymentItem(climbSkillResponseFileName)]
        [DeploymentItem(craftSkillResponseFileName)]
        [DeploymentItem(disableDeviceSkillResponseFileName)]
        [DeploymentItem(weaponFinesseFeatResponseFileName)]
        [DataRow(skillResponseFileName, typeof(Skill))]
        [DataRow(acrobaticsSkillResponseFileName, typeof(Skill))]
        [DataRow(appraiseSkillResponseFileName, typeof(Skill))]
        [DataRow(climbSkillResponseFileName, typeof(Skill))]
        [DataRow(craftSkillResponseFileName, typeof(Skill))]
        [DataRow(disableDeviceSkillResponseFileName, typeof(Skill))]
        [DataRow(featResponseFileName, typeof(Feat))]
        [DataRow(weaponFinesseFeatResponseFileName, typeof(Feat))]
        public void GetCardType_ResponseIsKnownCardType_ReturnsCorrectCardType(string fileName, Type cardType)
        {
            // Arrange
            var response = File.ReadAllText($"Pathfinder\\{fileName}");

            // Act
            var card = defaultParser.Parse(response);

            // Assert          
            Assert.IsInstanceOfType(card, cardType);
        }

        [TestMethod]
        [DeploymentItem(skillResponseFileName)]
        [DeploymentItem(featResponseFileName)]
        [DeploymentItem(acrobaticsSkillResponseFileName)]
        [DeploymentItem(appraiseSkillResponseFileName)]
        [DeploymentItem(climbSkillResponseFileName)]
        [DeploymentItem(craftSkillResponseFileName)]
        [DeploymentItem(disableDeviceSkillResponseFileName)]
        [DeploymentItem(weaponFinesseFeatResponseFileName)]
        [DataRow(skillResponseFileName, typeof(Skill))]
        [DataRow(acrobaticsSkillResponseFileName, typeof(Skill))]
        [DataRow(appraiseSkillResponseFileName, typeof(Skill))]
        [DataRow(climbSkillResponseFileName, typeof(Skill))]
        [DataRow(craftSkillResponseFileName, typeof(Skill))]
        [DataRow(disableDeviceSkillResponseFileName, typeof(Skill))]
        [DataRow(featResponseFileName, typeof(Feat))]
        [DataRow(weaponFinesseFeatResponseFileName, typeof(Feat))]
        public void GetCardType_SchemeIsDifferentButValid_ReturnsCorrectCardType(string fileName, Type cardType)
        {
            // Arrange
            var httpSchemeUri = new Uri(this.baseUri.ToString().Replace(this.baseUri.Scheme, "http"));
            var httpParser = new D20PFSrdParser(httpSchemeUri, this.featsUri, this.skillsUri);
            var response = File.ReadAllText($"Pathfinder\\{fileName}");

            // Act
            var card = httpParser.Parse(response);

            // Assert          
            Assert.IsInstanceOfType(card, cardType);
        }
        #endregion
    }
}
