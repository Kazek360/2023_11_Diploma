namespace StorySystems.Dialogues
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
                DialogueInfo.Type.MainQuest => 5, 
                DialogueInfo.Type.SideQuest => 4,
                DialogueInfo.Type.QuestStarter => 3,
                DialogueInfo.Type.Unrepeatable => 2,
                DialogueInfo.Type.Repeatable => 1,
                _ => 1
            };
            
            currentPriority = _defaultPriority;
        }
    }
}

