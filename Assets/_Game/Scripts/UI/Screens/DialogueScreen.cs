using System.Collections;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI.Screens
{
    public class DialogueScreen : BaseScreen
    {
        [SerializeField] private float textSpeed = 0.01f;
    
        [Header("References")]
        [SerializeField] private TMP_Text characterNameComp;
        [SerializeField] private TMP_Text textComp;
        [SerializeField] private GameObject textHint;
        
        [HideInInspector] public bool dialogueInProcess {get; private set; }
        private bool IsTyping = false;
        
        [HideInInspector] public bool IsPhraseCompleted => !IsTyping;
        
        
        
        public void PlayPhrase(string text, string characterName)
        {
            characterNameComp.text = characterName;
            StartCoroutine(TypeText(text));
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public void StartDialogueIfNotStarted()
        {
            if (!dialogueInProcess)
            {
                dialogueInProcess = true;
                Show();
            }
        }

        public void EndDialogue()
        {
            if (dialogueInProcess)
            {
                dialogueInProcess = false;
                Hide();
            }
        }


        

        private void Update()
        {
            // Подсказка "нажать Пробел для продолжения"
            textHint.SetActive(IsPhraseCompleted);
        }
        
        private IEnumerator TypeText(string text)
        {
            IsTyping = true;
            textComp.text = "";
            int index = 0;
        
            while (IsTyping)
            {
                textComp.text += text[index];
                yield return new WaitForSeconds(textSpeed);
            
                index++;
                if (index >= text.Length)
                {
                    IsTyping = false;
                    break;
                }
            }
            textComp.text = text; // чтобы текст 100% отображался полностью
        }
    }
}