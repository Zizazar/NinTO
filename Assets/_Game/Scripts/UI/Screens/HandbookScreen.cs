using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.UI.Components;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Game.Scripts.UI.Screens
{
    public class HandbookScreen : BaseScreen
    {
        public UnityEvent<int> onChoose;
        public int selectedNpc = -1;
        
        [Header("References")]
        [SerializeField] private Button chooseButton;
        [SerializeField] private TMP_Text npcName;
        [SerializeField] private TMP_Text npcDialogues;
        [SerializeField] private List<CharacterButton> npcCards;
        
        private CharacterButton _selectedButton;
        
        private void Awake()
        {
            chooseButton.onClick.AddListener(
                () => onChoose.Invoke(selectedNpc)
                );
            foreach (CharacterButton characterButton in npcCards)
            {
                characterButton.OnClick.AddListener(
                    () => ChooseCharacter(characterButton)
                    );
            }
        }

        private void OnDestroy()
        {
            chooseButton.onClick.RemoveAllListeners();
            onChoose.RemoveAllListeners();
        }

        private void OnEnable()
        {
            foreach (var characterButton in npcCards)
            {
                if (characterButton.NpcIndex < G.openedNpcsCount)
                    characterButton.Show();
                
            }
        }

        // TODO: Соеденить выбор персонажей сдесь и в диалоговом графе
        private void ChooseCharacter(CharacterButton characterButton)
        {
            if (_selectedButton == characterButton) return;
            
            Debug.Log("Choose character: " + characterButton.name);
            // Обратная анимация если уже что-то выбрано
            _selectedButton?.Deselect();
            
            _selectedButton = characterButton;
            _selectedButton.Select();
            // TODO: Изменить звук выбора
            AudioManager.Instance.PlaySound("handbook_open", 1.5f);
        }

        public void ShowChooseButton()
        {
            chooseButton.gameObject.SetActive(true);
        }

        public override void Show()
        {
            AudioManager.Instance.PlaySound("handbook_open");
            base.Show();
        }

        public override void Hide()
        {
            AudioManager.Instance.PlaySound("handbook_close");
            base.Hide();
        }
    }
}