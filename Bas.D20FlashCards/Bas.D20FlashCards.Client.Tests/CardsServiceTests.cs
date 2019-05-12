using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Bas.D20FlashCards.Client.Services;
using Bas.D20FlashCards.Pathfinder;

namespace Bas.D20FlashCards.Client.Tests
{
    [TestClass]
    public class CardsServiceTests
    {
        private CardsService cardsService;

        [TestInitialize]
        public void Initialize()
        {
            this.cardsService = new CardsService(new Parser[]
            {
                new ArchivesOfNethysParser(new Uri("https://aonprd.com/"), " - Archives of Nethys: Pathfinder RPG Database"),
                new D20PFSrdParser(new Uri("https://www.d20pfsrd.com"), new Uri("/feats"), new Uri("/skills"))
            });
        }


    }
}
