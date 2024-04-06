namespace Skill
{
    public class SkillTreeNode
    {
        public SkillTreeNodeSO Info;

        public enum State
        {
            CannotBeActivated,
            NotActivated,
            Activated
        }
        public State state;
    
        public SkillTreeNode(SkillTreeNodeSO info)
        {
            Info = info;
            state = State.CannotBeActivated;
        }
    }
}
