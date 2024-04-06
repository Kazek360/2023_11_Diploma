using System;

namespace Skill.Events
{
    public static class SkillTreeEvents
    {
        public static event Action<string> OnNodeActivation;
        public static void ActivateNode(string id)
        {
            OnNodeActivation?.Invoke(id);
        }
    }
}
