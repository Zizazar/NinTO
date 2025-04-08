using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUI;
    public TMP_Text characterNameComp;
    public TMP_Text textComp;
    public float textSpeed = 0.01f;

    private State curState = State.COMPLITED;

    [SerializeField] private int coffeeQuality = 0;
    [SerializeField] private bool wrongCoffee = false;

    private Queue<Phrase> currentPhrases;

    private enum State
    {
        COMPLITED,
        PLAYING
    }

    void Awake()
    {
        currentPhrases = new Queue<Phrase>();
    }

    public void StartDialogue(BasicDialog dialogue)
    {
        currentPhrases.Clear();

        if (dialogue is DoneDialog)
        {
            DoneDialog doneDialogue = (DoneDialog)dialogue;
            if (wrongCoffee)
            {
                addPhrasesToQueue(doneDialogue.wrongCoffeePhrases);
            }
            else
            {
                switch (coffeeQuality)
                {
                    case 0: 
                        addPhrasesToQueue(doneDialogue.badCoffeeTimePhrases);
                        break;
                    case 1:
                        addPhrasesToQueue(doneDialogue.normCoffeeTimePhrases);
                        break;
                    case 2:
                        addPhrasesToQueue(doneDialogue.perfectCoffeeTimePhrases);
                        break;
                }
            }
        }
        dialogueUI.SetActive(true);
        PlayNextPhrase();
    }

    private void addPhrasesToQueue(Phrase[] phrases)
    {
        foreach (Phrase phrase in phrases) 
            currentPhrases.Enqueue(phrase);
    }

    public void PlayNextPhrase()
    {
        if (currentPhrases.Count == 0)
        {
            EndDialogue();
            return;
        }
        var phrase = currentPhrases.Dequeue();

        StartCoroutine(TypeText(phrase.text));
        characterNameComp.color = phrase.speaker.nameColor;
        characterNameComp.text = phrase.speaker.speakerName;
        }

    void EndDialogue()
    {
        dialogueUI.SetActive(false);
    }

    public bool IsCompleted()
    {
        return curState == State.COMPLITED;
    }

    private IEnumerator TypeText(string text) // Посимвольное показывание текста
    {
        curState = State.PLAYING;
        textComp.text = "";
        int index = 0;
        while (curState != State.COMPLITED)
        {
            textComp.text += text[index];
            yield return new WaitForSeconds(textSpeed);
            index++;
            if (index >= text.Length)
            {
                curState = State.COMPLITED;
                break;
            }
        }
    }
}
