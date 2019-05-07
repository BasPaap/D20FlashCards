using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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
            const string titleElementStartTag = "<title>";
            const string titleElementEndTag = "</title>";
            
            var titleElementPosition = response.IndexOf(titleElementStartTag);
            var titleElementEndTagPosition = response.IndexOf(titleElementEndTag);

            if (titleElementPosition < 0 || titleElementEndTagPosition == 0 || titleElementPosition > titleElementEndTagPosition)
            {
                return false;
            }

            var pageTitlePosition = titleElementPosition + titleElementStartTag.Length;
            var pageTitleLength = titleElementEndTagPosition - pageTitlePosition;

            var pageTitle = response.Substring(pageTitlePosition, pageTitleLength);
            Debug.Assert(pageTitle.Length == pageTitleLength);

            if (!pageTitle.Contains($"{title}{this.titleSuffix}"))
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
