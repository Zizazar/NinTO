using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;
using XNode;

[CreateAssetMenu(menuName = "Dialogue/DialogueGraph")]
public class DialogueGraph : NodeGraph
{
    public UnityEvent onDialogueEnd;
    
    public DialogueNode currentDialogueNode;
    
    
    public void Start()
    {
        onDialogueEnd ??= new UnityEvent();
        
        StartNode startNode = (StartNode)nodes.Find(node => node is StartNode startNode);
        startNode.Start();
    }

    // Ивент вызывается когда нужно перейти к следующей фразе
    public void NextPhrase()
    {
        if (currentDialogueNode)
            if (currentDialogueNode.NextNode() is not null)
            {
                currentDialogueNode.NextNode().Execute();
            }
            else
            {
                G.dialogueController.EndDialogue();
            }
    }
}
