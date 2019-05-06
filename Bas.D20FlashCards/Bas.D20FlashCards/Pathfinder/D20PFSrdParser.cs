using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Pathfinder
{
    public sealed class D20PFSrdParser : Parser
    {
        public D20PFSrdParser(Uri baseUri)
        {            
        }

        public override bool CanParse(Uri uri)
        {
            throw new NotImplementedException();
        }

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
