using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private PlayerController _pl;
    public GameObject dialogueUI;
    public GameObject handbookUI;
    public GameObject popupUI;
    public bool popupOk = false;
    
    [Header("Sounds")]
    [SerializeField] AudioClip HandbookOpenClip;
    [SerializeField] AudioClip HandbookCloseClip;

    public void closeUI(UiType uiType)
    {
        switch(uiType)
        {
            case UiType.Dialogue: dialogueUI.SetActive(false); break;
            case UiType.Handbook: handbookUI.SetActive(false); _pl.PlayPlayerSound(HandbookCloseClip); break;
            case UiType.Popup: popupUI.SetActive(false); break;
        }
    }
    public void openUI(UiType uiType)
    {
        switch (uiType)
        {
            case UiType.Dialogue: dialogueUI.SetActive(true); break;
            case UiType.Handbook: handbookUI.SetActive(true); _pl.PlayPlayerSound(HandbookOpenClip); break;
            case UiType.Popup: popupUI.SetActive(true); break;
        }
    }

    public void toggleUI(UiType uiType)
    {
    if (IsOppened(uiType)) { closeUI(uiType); } else { openUI(uiType); }
    }
    public bool IsOppened(UiType uiType)
    {
#pragma warning disable CS0618
        switch (uiType)
        {
            case UiType.Dialogue: return dialogueUI.active;
            case UiType.Handbook: return handbookUI.active;
            case UiType.Popup: return popupUI.active;
#pragma warning restore CS0618 // Тип или член устарел
            default: return false;
        }
    }


    public void PopupChoose(bool choose)
    {
        popupOk = choose;
        closeUI(UiType.Popup);
    }
    public void OpenPopup() { openUI(UiType.Popup); }
}

public enum UiType
{
    None,
    Dialogue,
    Handbook,
    Popup
}