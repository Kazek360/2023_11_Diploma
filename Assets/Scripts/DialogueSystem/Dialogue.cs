using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public DialogueSO Info;
    private int _currentSentenceIndex;
    
    public Dialogue(DialogueSO info)
    {
        Info = info;
        _currentSentenceIndex = 0;
    }
}
