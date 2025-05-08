using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BaseNode : Node
{
    
    
    // <summary>
    // Код выполняющийся нодой. Вызывать base.Execute() после выполнения логики.
    // </summary>
    public virtual void Execute() {}

    public virtual BaseNode NextNode()
    {
        if (!GetPort("output").IsConnected) return null;
        return GetPort("output").Connection.node as BaseNode;
    }
}
