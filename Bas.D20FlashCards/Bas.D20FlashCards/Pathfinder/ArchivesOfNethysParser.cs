using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
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
            const string tableStartTag = "<table ";
            const string tableEndTag = "</table>";

            var tableContents = response.Substring(tableStartTag, tableEndTag);
            if (string.IsNullOrWhiteSpace(tableContents))
            {
                return null;
            }

            var tableElement = GetTableElement($"{tableStartTag}{tableContents}{tableEndTag}");
            var propertyNodes = from n in tableElement.Descendants("span").FirstOrDefault()?.Nodes()
                                where !(n is XElement) || 
                                    (((XElement)n).Name != "a" && ((XElement)n).Name != "h1" &&((XElement)n).Name != "br")
                                select n;

            var feat = new Feat()
            {
                Name = ((string)tableElement.Descendants("h1").FirstOrDefault())?.Trim(),
                Description = (propertyNodes.Skip(1).Take(1).Single() as XText).Value,
                Benefit = GetFeatPropertyValue("Benefit", propertyNodes),
                Normal = GetFeatPropertyValue("Normal", propertyNodes),
                Prerequisites = GetFeatPropertyValue("Prerequisites", propertyNodes),
                Special = GetFeatPropertyValue("Special", propertyNodes)
            };

            return feat;
        }

        private static string GetFeatPropertyValue(string propertyName, IEnumerable<XNode> propertyNodes)
        {
            foreach (var propertyNode in propertyNodes)
            {
                if (propertyNode.NodeType == XmlNodeType.Element && (propertyNode as XElement).Value == propertyName)
                {
                    return (propertyNode.NextNode as XText).Value.Substring(1).Trim();
                }
            }

            return null;
        }

        private static XElement GetTableElement(string tableContents)
        {
            try
            {
                const string imageElementStart = "<img src=";
                const string imageElementEnd = ">";
                var imageHtml = $"{imageElementStart}{tableContents.Substring(imageElementStart, imageElementEnd)}{imageElementEnd}";
                
                return XElement.Parse(tableContents.Replace(imageHtml, string.Empty));
            }
            catch (XmlException)
            {
                return null;
            }
        }

        protected override Skill GetSkill(string response)
        {
            return new Skill();
        }        
    }
}
