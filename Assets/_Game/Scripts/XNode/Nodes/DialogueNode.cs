using _Game.Scripts.UI.Screens;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
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
        if (graph is not DialogueGraph dialogueGraph)
        {
            Debug.LogError( "Dialogue node is not in a DialogueGraph" );
            return;
        }

        dialogueGraph.DialogueScreen.StartDialogueIfNotStarted();
        
        dialogueGraph.currentDialogueNode = this;
        dialogueGraph.DialogueScreen.PlayPhrase(text, G.GetCharacterName(character));
    }
    
    public override object GetValue(NodePort port) {
        return output;
    }
}

