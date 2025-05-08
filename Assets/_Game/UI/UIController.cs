using System;
using System.Collections;
using _Game.Legacy.Player;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private PlayerController _pl;
    public GameObject dialogueUI;
    public GameObject handbookUI;
    public GameObject handbookUIButton;
    public GameObject popupUI;
    public GameObject escUI;
    public GameObject settingsUI;
    public GameObject FadeUI;
    public AudioMixer MainMixer;
    public bool popupOk = false;

    public Slider musicSlider;
    public Slider masterSlider;

    public UnityEvent<int> onHandbookChosen;

    private void Awake()
    {
        onHandbookChosen = new UnityEvent<int>();
        
        onHandbookChosen.AddListener((index) => Close(UiType.HandbookChoose));
    }

    public void Close(UiType uiType)
    {
        switch(uiType)
        {
            case UiType.Dialogue: dialogueUI.SetActive(false); break;
            case UiType.Handbook: handbookUI.SetActive(false); AudioManager.Instance.PlaySound("handbook_close"); break;
            case UiType.HandbookChoose: handbookUI.SetActive(false); AudioManager.Instance.PlaySound("handbook_close"); break;
            case UiType.EscMenu: escUI.SetActive(false); break;
            case UiType.Settings: settingsUI.SetActive(false); break;
            case UiType.Popup: popupUI.SetActive(false); break;
        }
    }
    public void Open(UiType uiType)
    {
        switch (uiType)
        {
            case UiType.Dialogue: dialogueUI.SetActive(true); break;
            case UiType.Handbook: handbookUI.SetActive(true); AudioManager.Instance.PlaySound("handbook_open"); break;
            case UiType.HandbookChoose: handbookUI.SetActive(true); handbookUIButton.SetActive(true); AudioManager.Instance.PlaySound("handbook_open"); break;
            case UiType.EscMenu: escUI.SetActive(true); break;
            case UiType.Settings: settingsUI.SetActive(true); break;
            case UiType.Popup: popupUI.SetActive(true); break;
        }
    }

    public void Toggle(UiType uiType)
    {
    if (IsOpened(uiType)) { Close(uiType); } else { Open(uiType); }
    }
    public bool IsOpened(UiType uiType)
    {
        switch (uiType)
        {
            case UiType.Dialogue: return dialogueUI.activeSelf;
            case UiType.Handbook: return handbookUI.activeSelf;
            case UiType.EscMenu: return escUI.activeSelf;
            case UiType.Settings: return settingsUI.activeSelf;
            case UiType.Popup: return popupUI.activeSelf;
            default: return false;
        }
    }


    public void PopupChoose(bool choose)
    {
        popupOk = choose;
        if (popupOk) {
            StartCoroutine(FadeBlack());
        }
        Close(UiType.Popup);
    }

    public void OpenPopup() { Open(UiType.Popup); }

    public IEnumerator FadeBlack()
    {
        FadeUI.SetActive(true);
        Image image = FadeUI.GetComponent<Image>();
        while (image.color.a < 1)
        {
            MainMixer.SetFloat("Music", Mathf.Lerp(0, -40, image.color.a));
            MainMixer.SetFloat("Radio", Mathf.Lerp(0, -40, image.color.a));
            yield return null;
            
            image.color += new Color(0,0,0, 0.005f);
        }

        SceneManager.LoadScene(2);
    }

    public void CloseEscMenu()
    {
        Close(UiType.EscMenu);
    }
    public void OpenSettings()
    {
        Open(UiType.Settings);
    }

    public void ExitMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void SetMusicVolume()
    {
        MainMixer.SetFloat("Music", musicSlider.value);
    }
    public void SetMasterVolume()
    {
        MainMixer.SetFloat("Master", masterSlider.value);
    }
}

public enum UiType
{
    None,
    Dialogue,
    Handbook,
    HandbookChoose,
    EscMenu,
    Settings,
    Popup
}