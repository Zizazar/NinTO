using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndManager : MonoBehaviour
{

    public Image _fade;
    public Image _back;

    public Sprite _win;
    public Sprite _lose;

    public GameObject _button;

    void Start()
    {
        _back.sprite = (PlayerPrefs.GetInt("SelectedNPC") == 0) ? _win : _lose;
        StartCoroutine(Fade());
    }
    private IEnumerator Fade()
    {
        while (_fade.color.a > 0)
        {
            yield return null;
            _fade.color -= new Color(0, 0, 0, 0.005f);
        }
        yield return new WaitForSeconds(5f);
        _button.SetActive(true);
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
