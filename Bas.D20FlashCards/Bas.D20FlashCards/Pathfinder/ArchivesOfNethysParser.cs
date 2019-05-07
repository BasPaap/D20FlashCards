using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Bas.D20FlashCards.Extensions;

namespace Bas.D20FlashCards.Pathfinder
{
    public sealed class ArchivesOfNethysParser : Parser
    {
        private readonly Uri baseUri;
        private readonly string titleSuffix;

        public ArchivesOfNethysParser(Uri baseUri, string titleSuffix)
        {
            this.baseUri = baseUri;
            this.titleSuffix = titleSuffix;
        }

        public override bool CanParse(Uri uri)
        {
            TestUriValidity(uri);

            return baseUri.Authority == uri.Authority;
        }

        protected override Type GetCardType(string response)
        {
            const string featsResponseTitle = "Feats";
            const string skillsResponseTitle = "Skills";

            if (ResponseContainsTitle(response, featsResponseTitle))
            {
                return typeof(Feat);
            }

            if (ResponseContainsTitle(response, skillsResponseTitle))
            {
                return typeof(Skill);
            }

            return null;
        }

        private bool ResponseContainsTitle(string response, string title)
        {
            var pageTitle = response.Substring("<title>", "</title>");
            
            if (pageTitle == null || !pageTitle.Contains($"{title}{this.titleSuffix}"))
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
