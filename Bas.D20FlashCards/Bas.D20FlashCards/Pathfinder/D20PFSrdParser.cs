using Bas.D20FlashCards.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

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
            var baseUriInResponse = response.Substring("<base href=\"", "\">");
            var indexOfRelativeUri = baseUriInResponse?.IndexOf(relativeUri.ToString(), StringComparison.OrdinalIgnoreCase);

            if (indexOfRelativeUri == null || indexOfRelativeUri < 0)
            {
                return false;
            }

            return true;
        }

        protected override Feat GetFeat(string response)
        {
            var (articleTitle, articleContentElement) = GetArticleTitleAndContentElement(response);

            if (articleTitle == null || articleContentElement == null)
            {
                return null;
            }

            var feat = new Feat()
            {
                Name = articleTitle,
                Description = ((string)articleContentElement.Elements("p").FirstOrDefault())?.Trim(),
                Prerequisites = GetFeatProperty("Prerequisite", articleContentElement),
                Benefit = GetFeatProperty("Benefit", articleContentElement),
                Normal = GetFeatProperty("Normal", articleContentElement),
                Special = GetFeatProperty("Special", articleContentElement)
            };

            return feat;
        }

        protected override Skill GetSkill(string response)
        {
            var (articleTitle, articleContentElement) = GetArticleTitleAndContentElement(response);

            if (articleTitle == null || articleContentElement == null)
            {
                return null;
            }

            var skill = new Skill()
            {
                Name = articleTitle,
                Description = articleContentElement.Elements("p").FirstOrDefault()?.Value.Trim(),
                Action = GetSkillProperty("Action", articleContentElement),
                Check = GetCheckSkillProperty(articleContentElement),
                Special = GetSpecialSkillProperty(articleContentElement),
                TryAgain = GetRetryProperty(articleContentElement),
                Untrained = GetSkillProperty("Untrained", articleContentElement)
            };

            return skill;
        }

        private static (string, XElement) GetArticleTitleAndContentElement(string response)
        {
            const string articleStartTag = "<article ";
            const string articleEndTag = "</article>";

            var articleContents = response.Substring(articleStartTag, articleEndTag);
            if (string.IsNullOrWhiteSpace(articleContents))
            {
                return (null, null);
            }

            try
            {
                const string nbspEntityName = "&nbsp;";
                const string nbspEntityNumber = "&#160;";
                var articleElement = XElement.Parse($"{articleStartTag}{articleContents}{articleEndTag}".Replace(nbspEntityName, nbspEntityNumber));
                var articleContentElement = articleElement.Descendants("div").FirstOrDefault(d => ((string)d.Attribute("class"))?.Contains("article-content") == true);

                return ((string)articleElement.Element("h1"), articleContentElement);
            }
            catch (XmlException)
            {
                return (null, null);
            }
        }

        private static string GetFeatProperty(string propertyName, XElement articleContentElement)
        {
            var propertyValue = articleContentElement.Elements("p").FirstOrDefault(p => (string)p.Elements("b").FirstOrDefault() == propertyName).Value.Replace($"{propertyName}: ", string.Empty).Trim();
            return propertyValue;
        }

        private static string GetSpecialSkillProperty(XElement articleContentElement)
        {
            var specialDescriptionElements = articleContentElement.Elements("h3").FirstOrDefault(h => h?.Value.Trim() == "Modifiers")?.ElementsAfterSelf()?.Descendants("li");
            
            var specialDescriptionBuilder = new StringBuilder();
            foreach (var specialDescriptionElement in specialDescriptionElements)
            {

                specialDescriptionElement.Elements("b").Remove();
                specialDescriptionBuilder.Append(specialDescriptionElement.Value?.Trim());
            }

            return specialDescriptionBuilder.ToString();
        }

        private static string GetCheckSkillProperty(XElement articleContentElement)
        {
            var elementsContainingCheckDescriptions = articleContentElement.Elements("h3").FirstOrDefault()?.ElementsAfterSelf();

            var checkDescriptionBuilder = new StringBuilder();

            foreach (var element in elementsContainingCheckDescriptions)
            {
                if (element.Name == "p")
                {
                    checkDescriptionBuilder.Append(element.Value?.Trim());
                }
                else if (element.Name == "h3")
                {
                    break;
                }
            }

            return checkDescriptionBuilder.ToString();
        }

        private static string GetRetryProperty(XElement articleContentElement)
        {
            const string propertyName = "Retry?";
            var propertyXml = articleContentElement.Elements("p").FirstOrDefault(p => (string)p.Elements("b").FirstOrDefault() == propertyName)?.ToString();
            var propertyValue = propertyXml?.Substring("<p>", "</p>");

            return propertyValue?.Replace($"<b>{propertyName}</b> ", string.Empty).Replace(Environment.NewLine, string.Empty).Trim();
        }


        private static string GetSkillProperty(string propertyName, XElement articleContentElement)
        {
            var propertyValue = articleContentElement.Elements("h3").FirstOrDefault(h => h?.Value.Trim() == propertyName)?.ElementsAfterSelf()?.FirstOrDefault()?.Value.Trim();
            return propertyValue;
        }
    }
}
