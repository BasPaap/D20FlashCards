using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards.Pathfinder
{
    public sealed class D20PFSrdParser : Parser
    {
        private readonly Uri baseUri;
        private readonly Uri featsUri;
        private readonly Uri skillsUri;

        public D20PFSrdParser(Uri baseUri, Uri featsUri, Uri skillsUri)
        {
            this.baseUri = baseUri;
            this.featsUri = featsUri;
            this.skillsUri = skillsUri;
        }

        public override bool CanParse(Uri uri)
        {
            TestUriValidity(uri);

            return baseUri.Authority == uri.Authority;
        }

        protected override Type GetCardType(string response)
        {
            if (ResponseContainsBaseUri(response, this.featsUri))
            {
                return typeof(Feat);
            }

            if (ResponseContainsBaseUri(response, this.skillsUri))
            {
                return typeof(Skill);
            }

            return null;
        }

        private bool ResponseContainsBaseUri(string response, Uri relativeUri)
        {
            var baseUriPosition = response.IndexOf(new Uri(baseUri, relativeUri).ToString());

            if (baseUriPosition < 0)
            {
                return false;
            }

            const string baseUriElementStart = "<base href=\"";
            var baseElementPosition = baseUriPosition - baseUriElementStart.Length;

            if (baseElementPosition < 0 || response.Substring(baseElementPosition, baseUriElementStart.Length) != baseUriElementStart)
            {
                return false;
            }

            return true;
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
