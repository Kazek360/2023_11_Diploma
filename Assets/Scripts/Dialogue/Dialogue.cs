
namespace Dialogue
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
        public readonly DialogueInfo info;
        public State state;
        private int _defaultPriority;
        public int currentPriority;
    
        public Dialogue(DialogueInfo info)
        {
            this.info = info;
            state = State.CannotTrigger;
            id = this.info.name;

            _defaultPriority = info.dialogueType switch
            {
                DialogueInfo.Type.QuestProgression => 3,
                DialogueInfo.Type.QuestStarter => 2,
                DialogueInfo.Type.Repeatable => 1,
                _ => 1
            };
            
            currentPriority = _defaultPriority;
        }
    }
}

