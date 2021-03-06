﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Tests.Mocks
{
    sealed class ParserMock : Parser
    {
        private readonly Type cardTypeToReturn;
        
        public bool IsGetFeatCalled { get; set; }
        public bool IsGetSkillCalled { get; set; }
        public string ResponsePassedToGetCardType { get; set; }
        public string ResponsePassedToGetFeat { get; set; }
        public string ResponsePassedToGetSkill { get; set; }

        public ParserMock(Type cardTypeToReturn)
        {
            this.cardTypeToReturn = cardTypeToReturn;            
        }
        public override bool CanParse(Uri uri)
        {
            return true;
        }

        protected override Type GetCardType(string response)
        {
            ResponsePassedToGetCardType = response;
            return this.cardTypeToReturn;
        }

        protected override Feat GetFeat(string response)
        {
            ResponsePassedToGetFeat = response;
            IsGetFeatCalled = true;
            return null;
        }

        protected override Skill GetSkill(string response)
        {
            ResponsePassedToGetSkill = response;
            IsGetSkillCalled = true;
            return null;
        }
    }
}
