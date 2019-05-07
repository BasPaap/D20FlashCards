﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Bas.D20FlashCards
{
    public abstract class Parser
    {
        public Card Parse(string response)
        {
            throw new NotImplementedException();
        }

        public abstract bool CanParse(Uri uri);
        protected abstract Type GetCardType(string response);
        protected abstract Feat GetFeat(string response);
        protected abstract Skill GetSkill(string response);
    }
}
