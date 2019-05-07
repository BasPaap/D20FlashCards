﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards
{
    public abstract class Parser
    {
        public Card Parse(string response)
        {
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
                throw new ArgumentException(nameof(uri));
            }
        }

        public abstract bool CanParse(Uri uri);
        protected abstract Type GetCardType(string response);
        protected abstract Feat GetFeat(string response);
        protected abstract Skill GetSkill(string response);
    }
}
