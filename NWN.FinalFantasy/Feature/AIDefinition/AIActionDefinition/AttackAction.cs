﻿using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript;
using NWN.FinalFantasy.Service.AIService;

namespace NWN.FinalFantasy.Feature.AIDefinition.AIActionDefinition
{
    public class AttackAction: IAIAction
    {
        public void Action(uint creature, params uint[] targets)
        {
            var target = targets[0];

            NWScript.ActionAttack(target);
        }
    }
}