
namespace DialogueSystem
{
    //Class storing NPC dialogue info and state of dialogue.
    public class Dialogue
    {
        public enum State
        {
            CannotTrigger,
            CanTrigger,
            Finished
        }

        public string id;
        public readonly DialogueSO info;
        public State state;
        private int _defaultPriority;
        public int currentPriority;
    
        public Dialogue(DialogueSO info)
        {
            this.info = info;
            state = State.CannotTrigger;
            id = this.info.name;

            if (info.relatedQuest is null)
            {
                _defaultPriority = 1;
            }
            else
            {
                _defaultPriority = info.relatedQuest.questCategory switch
                {
                    QuestCategory.MainQuest => 3,
                    QuestCategory.SideQuest => 2,
                    _ => 1
                }; 
            }
            currentPriority = _defaultPriority;
        }
    }
}

