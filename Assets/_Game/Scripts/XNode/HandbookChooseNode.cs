using System.Collections.Generic;

namespace _Game.Scripts.XNode
{
    [NodeTint("#6CD9C8")]
    public class HandbookChooseNode : BaseNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public BaseNode input;
        
        [Output(dynamicPortList = true)] public List<BaseNode> output;

        public override void Execute()
        {
            G.ui.Close(UiType.Dialogue);
            G.ui.Open(UiType.HandbookChoose);
            
            
        }
        public void SelectCharacter(int index) {
            if (index >= 0 && index < output.Count) {
                BaseNode nextNode = output[index];
                G.ui.Close(UiType.HandbookChoose);
                nextNode.Execute();
            }
        }
        
    }
}