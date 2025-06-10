using _Game.Scripts.UI.Screens;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using XNode;

public class DialogueNode : BaseNode
{
    
    [Input(ShowBackingValue.Never)] public BaseNode input;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public BaseNode output;
    
    public Characters character;
    [ResizableTextArea]
    public string text;
    
    public override void Execute()
    {

        dialogueGraph.DialogueScreen.StartDialogueIfNotStarted();
        
        dialogueGraph.DialogueScreen.PlayPhrase(text, G.GetCharacterName(character));
        G.input.Dialogue.Next.performed += MoveNext;
    }

    private void MoveNext(InputAction.CallbackContext ctx)
    {
        MoveNext();
    }

    public override void MoveNext()
    {
        G.input.Dialogue.Next.performed -= MoveNext;

        NodePort port = GetOutputPort("output");
        if (port.Connection != null) {
            (port.Connection.node as BaseNode)?.Execute();
        } else {
            dialogueGraph.DialogueScreen.EndDialogue();
        }
    }
}

