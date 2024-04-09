

public class DialogueMarker
{ 
    public FlagSO info;
    public bool _isTriggered;

    public DialogueMarker(FlagSO info)
    {
        _isTriggered = false;
        this.info = info;
    }
}