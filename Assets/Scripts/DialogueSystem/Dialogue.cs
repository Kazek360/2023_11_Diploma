
//Class storing NPC dialogue info and state of dialogue.
namespace DialogueSystem
{
    public class Dialogue
    {
        public readonly DialogueSO Info;
        public DialogueState State;
        private int _defaultPriority;
        public int currentPriority;
    
        public Dialogue(DialogueSO info)
        {
            Info = info;
            State = DialogueState.NotUnlocked;
            if (Info.relatedQuest is not null)
            {
                _defaultPriority = Info.relatedQuest.questCategory switch
                {
                    QuestCategory.MainQuest => 3,
                    QuestCategory.SideQuest => 2,
                    _ => 1
                };
            }
            else
            {
                _defaultPriority = 1;
            }
            currentPriority = _defaultPriority;
        }
    }
}

