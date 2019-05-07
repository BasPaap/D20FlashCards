using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Pathfinder
{
    public sealed class ArchivesOfNethysParser : Parser
    {
        private readonly Uri baseUri;

        public ArchivesOfNethysParser(Uri baseUri)
        {
            this.baseUri = baseUri;
        }

        public override bool CanParse(Uri uri)
        {
            TestUriValidity(uri);

            return baseUri.Authority == uri.Authority;
        }

        protected override Type GetCardType(string response)
        {
            return null;
        }

        protected override Feat GetFeat(string response)
        {
            return new Feat();
        }

        protected override Skill GetSkill(string response)
        {
            return new Skill();
        }        
    }
}
