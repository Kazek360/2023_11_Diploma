using Ink.Runtime;
using Other;

namespace Dialogues
{
    public class InkExternalFunctions
    {
        public void Bind(Story story)
        {
            story.BindExternalFunction("addItem", 
                (string id, int amount) => GameConfig.PlayerInventory.AddItem(id, amount));
            story.BindExternalFunction("removeItem", 
                (string id, int amount) => GameConfig.PlayerInventory.RemoveItem(id, amount));
            story.BindExternalFunction("changeNestorState", 
                (string id, bool isAppearing) => Nestor.ChangeNestorState(id, isAppearing));
        }
        
        public void Unbind(Story story)
        {
            story.UnbindExternalFunction("addItem");
            story.UnbindExternalFunction("removeItem");
            story.UnbindExternalFunction("changeNestorState");
        }
    }
}