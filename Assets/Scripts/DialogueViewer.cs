using UnityEngine;

public class DialogueViewer : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;

    int currentLine = 0;

    void Awake()
    {
        Debug.Assert(dialogue, gameObject.name + ": No dialogue assigned!");
    }

    [ContextMenu("Print Dialogue")]
    string NextLine()
    {
        if (currentLine >= dialogue.lines.Length)
        {
            Debug.Log("End of dialogue");
            ResetDialogue();
            UserInterface.Instance.InteractionPrompt("");
            return null;
        }

        var line = dialogue.lines[currentLine];

        currentLine++;
        return line.text;
    }

    public void PrintDialogue()
    {
        UserInterface.Instance.SmallPrompt(NextLine(), 4f);
    }


    [ContextMenu("Reset Dialogue")]
    void ResetDialogue() => currentLine = 0;

}

