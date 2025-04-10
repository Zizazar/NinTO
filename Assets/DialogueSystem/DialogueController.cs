using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.XR;
using System.Linq;

public class DialogueController : MonoBehaviour
{
    public TMP_Text characterNameComp;
    public TMP_Text textComp;
    public float textSpeed = 0.01f;
    public PlayerController playerController;
    public UIController uiController;
    public HandbookController handbookController;
    [HideInInspector] public bool dialogueInProcess = false;

    private State curState = State.COMPLITED;

    [SerializeField] public int coffeeQuality = 0;
    [SerializeField] public bool wrongCoffee = false;

    public List<string> allPhrases;

    public string allCurrent;

    public AudioClip DialoguePlayerSound;
    public AudioClip DialogueClientSound;

    private Queue<Phrase> currentPhrases;
    private string playerName;
    private NpcController _npcController;

    private enum State
    {
        COMPLITED,
        PLAYING
    }

    void Awake()
    {
        currentPhrases = new Queue<Phrase>();
        playerName = playerController.playerName;
    }

    public void StartDialogue(BasicDialog dialogue, NpcController npcController)
    {
        dialogueInProcess = true;
        currentPhrases.Clear();
        _npcController = npcController;

        if (dialogue is OrderDialog)
        {
            OrderDialog orderDialogue = (OrderDialog)dialogue;
            addPhrasesToQueue(orderDialogue.phrases);
        }

        if (dialogue is DoneDialog)
        {
            DoneDialog doneDialogue = (DoneDialog)dialogue;
            if (wrongCoffee)
            {
                addPhrasesToQueue(doneDialogue.wrongCoffeePhrases);
            }
            else
            {
                switch (coffeeQuality) // Лень думать
                {
                    case 0: 
                        addPhrasesToQueue(doneDialogue.badCoffeeTimePhrases);
                        break;
                    case 1:
                        addPhrasesToQueue(doneDialogue.badCoffeeTimePhrases);
                        addPhrasesToQueue(doneDialogue.normCoffeeTimePhrases);
                        break;
                    case 2:
                        addPhrasesToQueue(doneDialogue.badCoffeeTimePhrases);
                        addPhrasesToQueue(doneDialogue.normCoffeeTimePhrases);
                        addPhrasesToQueue(doneDialogue.perfectCoffeeTimePhrases);
                        break;
                }
            }
        }
        PlayNextPhrase();
    }

    private void addPhrasesToQueue(Phrase[] phrases)
    {
        foreach (Phrase phrase in phrases)
        {
            currentPhrases.Enqueue(phrase);
            _npcController.saidPhrases += (phrase.speaker == CharacterType.Player) ? "<i></b><color=#000b>" : "</i><b><color=#000>" + phrase.text + "\n";
        }
    }

    public void PlayNextPhrase()
    {
        if (currentPhrases.Count == 0)
        {
            EndDialogue();
            
            return;
        }
        var phrase = currentPhrases.Dequeue();

        if (phrase.speaker == CharacterType.Normal)
        {
            characterNameComp.text = _npcController.npcData.npcName;

        }
        else if (phrase.speaker == CharacterType.Player)
        {
            characterNameComp.text = playerController.playerName;

        }
        StartCoroutine(TypeText(phrase.text, phrase.speaker));
    }

        void EndDialogue()
        {
            dialogueInProcess = false;
            _npcController.Stage++; // Переход в 1 и 3 состояние
            _npcController.stageChanged = true;
            uiController.closeUI(UiType.Dialogue);
        }

    public void DialogueNext()
    {
        if (dialogueInProcess)
        {
            if (curState == State.COMPLITED) {
                PlayNextPhrase();
            }
            else
            {
                curState = State.COMPLITED;
            }
        }
    }

    public bool IsCompleted()
    {
        return curState == State.COMPLITED;
    }

    private IEnumerator TypeText(string text, CharacterType speaker) // Посимвольное показывание текста
    {
        curState = State.PLAYING;
        textComp.text = "";
        int index = 0;
        while (curState != State.COMPLITED)
        {
            textComp.text += text[index];
            if (speaker == CharacterType.Player) playerController.PlayPlayerSound(DialoguePlayerSound);
            if (speaker == CharacterType.Normal) playerController.PlayPlayerSound(DialogueClientSound);
            yield return new WaitForSeconds(textSpeed);
            index++;
            if (index >= text.Length)
            {
                curState = State.COMPLITED;
                break;
            }
        }
        textComp.text = text;
    }
}
