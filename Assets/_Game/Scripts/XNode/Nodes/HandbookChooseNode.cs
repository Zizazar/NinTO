using System.Collections.Generic;
using _Game.Scripts.UI.Screens;

[NodeTint("#6CD9C8")]
    public class HandbookChooseNode : BaseNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public BaseNode input;
        [Output(dynamicPortList = true)] public List<BaseNode> output;

        private HandbookScreen _handbookScreen;
        
        public override void Execute()
        {
            (graph as DialogueGraph).DialogueScreen.EndDialogue();
            G.ui.ShowScreen<HandbookScreen>();
            _handbookScreen = G.ui.GetScreen<HandbookScreen>();
            _handbookScreen.ShowChooseButton();
            _handbookScreen.onChoose.AddListener(SelectCharacter);
        }
        
        public void SelectCharacter(int index) {
            if (index >= 0 && index < output.Count) {
                BaseNode nextNode = output[index];
                G.ui.HideScreen<HandbookScreen>();
                
                _handbookScreen.onChoose.RemoveListener(SelectCharacter);
                nextNode.Execute();
            }
        }
        
    }