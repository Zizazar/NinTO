using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public abstract class BaseNode : Node
{
    // <summary>
    // Код выполняющийся нодой. Вызывать base.Execute() после выполнения логики.
    // </summary>
    internal DialogueGraph dialogueGraph => graph as DialogueGraph;
    
    public abstract void Execute();
    public abstract void MoveNext();
}
