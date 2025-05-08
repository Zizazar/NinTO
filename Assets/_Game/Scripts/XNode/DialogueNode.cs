using Unity.VisualScripting;
using UnityEngine;
using XNode;

public class DialogueNode : BaseNode
{
    
    [Input(ShowBackingValue.Never, ConnectionType.Override)] public BaseNode input;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public BaseNode output;
    
    public G.Characters character;
    [TextArea] public string text;
    public override void Execute()
    {
        if (graph is not DialogueGraph dialogueGraph)
        {
            Debug.LogError( "Dialogue node is not in a DialogueGraph" );
            return;
        }

        G.dialogueController.StartDialogueIfNotStarted();
        
        dialogueGraph.currentDialogueNode = this;
        G.dialogueController.PlayPhrase(text, G.GetCharacterName(character));
    }
    
    public override object GetValue(NodePort port) {
        return output;
    }
}

