using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintsController : MonoBehaviour
{
    public Sprite[] keySprites;
    public Image keybindImage;
    public TMP_Text textComp;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showHint(string message, float delay = 5, BindKey bindKey = BindKey.None)
    {
        textComp.text = message;
        if ((int)bindKey > -1)
        {
            keybindImage.sprite = keySprites[(int)bindKey];
        } else
        {
            keybindImage.sprite = null;
        }
        _animator.Play("Show");
        Invoke("hideHint", delay);
    }

    public void hideHint()
    {
        _animator.Play("Hide");
    }
}

public enum BindKey {
    None = -1, A, D, E, J, Mouse0, Mouse1
}