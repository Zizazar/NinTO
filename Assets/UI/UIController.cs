using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
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

    [Header("Sounds")]
    [SerializeField] AudioClip HandbookOpenClip;
    [SerializeField] AudioClip HandbookCloseClip;

    public void closeUI(UiType uiType)
    {
        switch(uiType)
        {
            case UiType.Dialogue: dialogueUI.SetActive(false); break;
            case UiType.Handbook: handbookUI.SetActive(false); _pl.PlayPlayerSound(HandbookCloseClip); break;
            case UiType.HandbookChoose: handbookUI.SetActive(false); _pl.PlayPlayerSound(HandbookCloseClip); break;
            case UiType.EscMenu: escUI.SetActive(false); break;
            case UiType.Settings: settingsUI.SetActive(false); break;
            case UiType.Popup: popupUI.SetActive(false); break;
        }
    }
    public void openUI(UiType uiType)
    {
        switch (uiType)
        {
            case UiType.Dialogue: dialogueUI.SetActive(true); break;
            case UiType.Handbook: handbookUI.SetActive(true); _pl.PlayPlayerSound(HandbookOpenClip); break;
            case UiType.HandbookChoose: handbookUI.SetActive(true); handbookUIButton.SetActive(true); break;
            case UiType.EscMenu: escUI.SetActive(true); break;
            case UiType.Settings: settingsUI.SetActive(true); break;
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
            case UiType.EscMenu: return escUI.active;
            case UiType.Settings: return settingsUI.active;
            case UiType.Popup: return popupUI.active;
#pragma warning restore CS0618 // Тип или член устарел
            default: return false;
        }
    }


    public void PopupChoose(bool choose)
    {
        popupOk = choose;
        if (popupOk) {
            StartCoroutine(FadeBlack());
        }
        closeUI(UiType.Popup);
    }

    public void OpenPopup() { openUI(UiType.Popup); }

    public IEnumerator FadeBlack()
    {
        FadeUI.SetActive(true);
        Image _image = FadeUI.GetComponent<Image>();
        while (_image.color.a < 1)
        {
            MainMixer.SetFloat("Music", Mathf.Lerp(0, -40, _image.color.a));
            MainMixer.SetFloat("Radio", Mathf.Lerp(0, -40, _image.color.a));
            yield return null;
            
            _image.color += new Color(0,0,0, 0.001f);
        }

        SceneManager.LoadScene(2);
    }

    public void CloseEscMenu()
    {
        closeUI(UiType.EscMenu);
    }
    public void OpenSettings()
    {
        openUI(UiType.Settings);
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