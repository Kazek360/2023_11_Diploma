using System;

namespace Skill.Events
{
    public static class SkillTreeEvents
    {
        public static event Action<string, int> OnNodeActivation;
        public static void ActivateNode(string id, int skillPoints)
        {
            OnNodeActivation?.Invoke(id, skillPoints);
        }
    }
}
