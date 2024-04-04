using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    public static class SkillEvents
    {
        public static event Action<string> OnSkillUnlock;
        public static void UnlockSkill(string id)
        {
            OnSkillUnlock?.Invoke(id);
        }
    }
}

