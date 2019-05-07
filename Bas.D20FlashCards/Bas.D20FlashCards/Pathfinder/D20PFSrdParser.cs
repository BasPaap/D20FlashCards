using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Pathfinder
{
    public sealed class D20PFSrdParser : Parser
    {
        private readonly Uri baseUri;

        public D20PFSrdParser(Uri baseUri)
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
