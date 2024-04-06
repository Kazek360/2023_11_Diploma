using System;

namespace Skill.Events
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

