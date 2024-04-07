

public class DialogueMarker
{ 
    public DialogueMarkerSO info;
    public bool _isTriggered;

    public DialogueMarker(DialogueMarkerSO info)
    {
        _isTriggered = false;
        this.info = info;
    }
}