using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public AudioClip music;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayAction()
    {
        SceneManager.LoadScene(1); // Выбрать при билде
    }
    public void OpenSettings()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
