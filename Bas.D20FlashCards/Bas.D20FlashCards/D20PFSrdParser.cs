using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards
{
    public sealed class D20PFSrdParser : Parser
    {
        protected override CardType GetCardType(string response)
        {
            throw new NotImplementedException();
        }

        protected override Feat GetFeat(string response)
        {
            throw new NotImplementedException();
        }

        protected override Skill GetSkill(string response)
        {
            throw new NotImplementedException();
        }
    }
}
