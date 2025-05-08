using XNode;

[NodeTint("#4CAF50")]
public class StartNode : Node {

	[Output] public BaseNode output;

	public void Start()
	{
		GetNextNode()?.Execute();
	}
	
	public new BaseNode GetNextNode() {
		NodePort port = GetOutputPort("output");
		if (!port.IsConnected) return null;
		return port.Connection.node as BaseNode;
	}

}