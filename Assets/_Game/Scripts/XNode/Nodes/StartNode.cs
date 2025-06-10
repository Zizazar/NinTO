using UnityEngine;
using XNode;

[NodeTint("#4CAF50")]
public class StartNode : BaseNode {

	[Output(ShowBackingValue.Never, ConnectionType.Override)] public BaseNode output;

	public override void MoveNext()
	{
		NodePort port = GetOutputPort("output");
		if (port.Connection != null) {
			(port.Connection.node as BaseNode)?.Execute();
		}
		else
		{
			Debug.LogWarning("No output port connected to start node");
		}
	}

	public override void Execute() => MoveNext();

}