using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HandbookController : MonoBehaviour
{
    public NpcData[] npcsData;
    public GameObject[] npcCards;

    public TMP_Text npcName;
    public TMP_Text npcDialogues;

    public int selectedNPC = -1;
    public int openedNpcsCount = 0;

    public List<string> npcPhrases;


    void OnEnable()
    {
        for (int i = 0; i < openedNpcsCount; i++)
        {
            GameObject npcCard = npcCards[i];
            if (selectedNPC == i) npcCard.GetComponent<Animator>().Play("Pressed");
            npcCard.SetActive(true);
        }
    }

    public void SelectNpc(int buttonId)
    {
        if (selectedNPC != -1 && selectedNPC != buttonId) npcCards[selectedNPC].GetComponent<Animator>().Play("Normal");
        selectedNPC = buttonId;
        PlayerPrefs.SetInt("SelectedNPC", selectedNPC);
        Animator _anim = npcCards[buttonId].GetComponent<Animator>();

        _anim.Play("Pressed");

        npcName.text = npcsData[buttonId].npcName;
        npcDialogues.text = npcPhrases[buttonId];
    }   
}
