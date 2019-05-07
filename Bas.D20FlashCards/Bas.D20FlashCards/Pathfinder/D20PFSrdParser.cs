﻿using Bas.D20FlashCards.Extensions;
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
            var (articleTitle, articleContentElement) = GetArticleTitleAndContentElement(response);

            if (articleTitle == null || articleContentElement == null)
            {
                return null;
            }

            var feat = new Feat()
            {
                Name = articleTitle,
                Description = ((string)articleContentElement.Elements("p").FirstOrDefault())?.Trim(),
                Prerequisite = GetFeatProperty("Prerequisite", articleContentElement),
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
                Description = articleContentElement.Elements("p").FirstOrDefault()?.ToString().Substring("<p>", "</p>").Trim(),
                Action = GetSkillProperty("Action", articleContentElement),
                Check = GetSkillProperty("Check", articleContentElement),
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
            var propertyXml = articleContentElement.Elements("p").FirstOrDefault(p => (string)p.Elements("b").FirstOrDefault() == propertyName)?.ToString();
            var propertyValue = propertyXml?.Substring("<p>", "</p>");

            return propertyValue?.Replace($"<b>{propertyName}</b>: ", string.Empty).Replace(Environment.NewLine, string.Empty).Trim();
        }

        private static string GetSpecialSkillProperty(XElement articleContentElement)
        {
            return articleContentElement.Elements("h3").FirstOrDefault(h => h?.Value.Trim() == "Modifiers")?.ElementsAfterSelf()?.FirstOrDefault()?.ToString().Replace(Environment.NewLine, string.Empty).Trim();            
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
            var propertyXml = articleContentElement.Elements("h3").FirstOrDefault(h => h?.Value.Trim() == propertyName)?.NextNode?.ToString();
            return propertyXml?.Substring($"<p>", $"</p>")?.Trim();
        }
    }
}
