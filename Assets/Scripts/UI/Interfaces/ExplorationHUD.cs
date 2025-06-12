using UI;
using UI.Interfaces;

public class ExplorationHUD : UserInterface
{
    public override UserInterfaceState State => UserInterfaceState.Exploration;


    public void OpenInventory()
    {
        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Inventory);
    }

    public void OpenSkillTree()
    {
        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.SkillTree);
    }
}
