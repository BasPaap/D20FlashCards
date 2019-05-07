using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards
{
    public abstract class Parser
    {
        public Card Parse(string response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (string.IsNullOrWhiteSpace(response))
            {
                throw new ArgumentException($"{nameof(response)} is empty or whitespace.", nameof(response));
            }

            switch (GetCardType(response).Name)
            {
                case nameof(Skill):
                    GetSkill(response);
                    break;
                case nameof(Feat):
                    GetFeat(response);
                    break;
                default:
                    break;
            }

            return null;
        }

        protected void TestUriValidity(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (!uri.IsAbsoluteUri)
            {
                throw new ArgumentException("Uri is not absolute.", nameof(uri));
            }
        }

        public abstract bool CanParse(Uri uri);
        protected abstract Type GetCardType(string response);
        protected abstract Feat GetFeat(string response);
        protected abstract Skill GetSkill(string response);
    }
}
