using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards
{
    public abstract class Parser
    {
        public Card Parse(string response)
        {
            throw new NotImplementedException();
        }

        protected abstract CardType GetCardType(string response);
        protected abstract Feat GetFeat(string response);
        protected abstract Skill GetSkill(string response);
    }
}
