using System;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueLine[] lines;
}

[Serializable]
public class DialogueLine
{
    [TextArea(3, 5)]
    public string text;
    public Choice[] choices;
}

[Serializable]
public class Choice
{
    public string text;
    public UnityEvent action;
}


