using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.NPC;
using _Game.Scripts.UI.Screens;
using GoT._Game.Scripts.Utils;
using NaughtyAttributes;
using UnityEngine;
using XNode;

[NodeTint("#9150C6"), NodeWidth(300)]
    public class HandbookChooseNode : BaseNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public BaseNode input;
        
        [SerializeField, CharacterSelector] private NpcData[] availableNpcs;

        private HandbookScreen _handbookScreen;
        private List<NodePort> _dynamicPorts = new();
        
        public override void Execute()
        {
            dialogueGraph.DialogueScreen.EndDialogue();
            G.ui.ShowScreen<HandbookScreen>();
            _handbookScreen = G.ui.GetScreen<HandbookScreen>();
            _handbookScreen.ShowChooseButton();
            _handbookScreen.onChoose.AddListener(MoveNext);
        }

        public override void MoveNext()
        {
            BaseNode nextNode = GetConnections().GetValueOrDefault(_handbookScreen.selectedNpcId);
            
            G.ui.HideScreen<HandbookScreen>();
                
            _handbookScreen.onChoose.RemoveListener(MoveNext);
            nextNode.Execute();
        }

        protected override void Init()
        {
            base.Init();
            UpdatePorts();
        }
        
        private void OnValidate()
        {
            UpdatePorts();
        }

        private void UpdatePorts()
        {
            // Удаляем старые динамические порты
            foreach (var port in _dynamicPorts)
            {
                RemoveDynamicPort(port);
            }
            _dynamicPorts.Clear();

            // Создаем новые порты для выбранных ключей
            foreach (NpcData npcData in availableNpcs)
            {
                    NodePort newPort = AddDynamicOutput(
                        typeof(BaseNode),
                        ConnectionType.Override,
                        TypeConstraint.None,
                        $"({npcData.ID.Substring(0,3)}...) {npcData.npcName}"
                    );
                    _dynamicPorts.Add(newPort);
            }
        }
        public Dictionary<string, BaseNode> GetConnections()
        {
            var connections = new Dictionary<string, BaseNode>();
        
            foreach (var port in _dynamicPorts)
            {
                if (!port.IsConnected) continue;
            
                string key = availableNpcs.First(x => x.npcName == port.fieldName).ID;
                
                connections[key] = port.GetConnections().First().node as BaseNode;
            }
        
            return connections;
        }
    }