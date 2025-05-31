using Ink.Runtime;

namespace Dialogues
{
    public class InkExternalFunctions
    {
        public void Bind(Story story)
        {
            story.BindExternalFunction("addItem", (string id, int amount) => 
                GameConfig.PlayerInventory.AddItem(id, amount));
            story.BindExternalFunction("removeItem", (string id, int amount) => 
                GameConfig.PlayerInventory.RemoveItem(id, amount));
        }
        
        public void Unbind(Story story)
        {
            story.UnbindExternalFunction("addItem");
            story.UnbindExternalFunction("removeItem");
        }
    }
}