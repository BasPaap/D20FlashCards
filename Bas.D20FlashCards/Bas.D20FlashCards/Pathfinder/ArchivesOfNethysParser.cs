using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Pathfinder
{
    public sealed class ArchivesOfNethysParser : Parser
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

        public static void CanParse(Uri uri, Uri baseUri)
        {
            throw new NotImplementedException();
        }
    }
}
