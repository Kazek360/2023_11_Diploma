
namespace UI.Interfaces
{
    public class SettingsMenu : UserInterface
    {
        public override GameState State => GameState.Settings;
        
        protected new void Awake()
        {
            base.Awake();
        }
    }
 
}
