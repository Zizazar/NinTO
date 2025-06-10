using _Game.Scripts.UI.Screens;
using UnityEngine;
using UnityEngine.Events;
using XNode;

[CreateAssetMenu(menuName = "Dialogue/DialogueGraph")]
public class DialogueGraph : NodeGraph
{
    public UnityEvent onDialogueEnd;
    public UnityEvent onNextPhrase;
    public UnityEvent onHandbookChoose;
    
    public DialogueScreen DialogueScreen {get; private set;}
    
    public void Start()
    {
        onDialogueEnd ??= new UnityEvent();

        DialogueScreen = G.ui.GetScreen<DialogueScreen>();
        
        StartNode startNode = (StartNode)nodes.Find(node => node is StartNode startNode);
        startNode.Execute();
    }
}
