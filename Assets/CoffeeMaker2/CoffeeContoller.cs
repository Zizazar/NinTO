using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static CoffeeMakerSystem.CoffeeMakerController;
using static Unity.Burst.Intrinsics.X86.Avx;

public class CoffeeContoller : MonoBehaviour
{
    public GameObject glowButton;
    public AudioClip _BrewSound;
    public AudioClip _buttonClickSound;

    public GameObject cup;
    public GameObject fullCup;

    [Header("Triggers")]
    public CupTrigger cupTrigger;
    public DispenserTrigger dispenserTrigger;
    public DispenserTrigger2 dispenserTrigger2;
    public GrinderTrigger grinderTrigger;

    private Renderer _glowButRenderer;
    private Outline[] outlineComponents;
    private AudioSource _AudioSource;

    private bool cupIn;
    public enum CoffeeState
    {
        SelectCup,
        PlaceCupOnMachine,
        MoveDispenserToGrinder,
        CollectCoffee,
        PutCoffeeInDispenser,
        ReturnDispenserToMachine,
        BrewCoffee,
        Finish,
        GiveCoffee
    }

    public CoffeeState currentState = CoffeeState.SelectCup;
    private bool isButtonPressed;

    private void Start()
    {
        _glowButRenderer = glowButton.GetComponent<Renderer>();
        outlineComponents = FindObjectsOfType<Outline>();
        _AudioSource = GetComponent<AudioSource>();
        StartCoroutine(CoffeeGameLoop());

    }

    private IEnumerator CoffeeGameLoop()
    {
        UpdateState(CoffeeState.SelectCup);

        yield return new WaitUntil(() => cupTrigger.IsCupIn());
        SetButtonColor(new Color(255, 200, 0));
        UpdateState(CoffeeState.MoveDispenserToGrinder);

        yield return new WaitUntil(() => grinderTrigger.IsSpoonIn());
        UpdateState(CoffeeState.CollectCoffee);

        yield return new WaitUntil(() => dispenserTrigger2.IsSpoonIn());
        dispenserTrigger2.SetSpoonIn(false);
        SetButtonColor(new Color(255, 200, 0));
        UpdateState(CoffeeState.ReturnDispenserToMachine);

        yield return new WaitUntil(() => isButtonPressed);
        UpdateState(CoffeeState.BrewCoffee);
        isButtonPressed = false;
        SetButtonColor(new Color(255, 100, 0));
        yield return new WaitWhile(() => _AudioSource.isPlaying);
        PlaySound(_BrewSound);
        yield return new WaitUntil(() => !_AudioSource.isPlaying);
        SetButtonColor(new Color(0, 255, 0));
        cup.SetActive(false);
        fullCup.SetActive(true);
        UpdateState(CoffeeState.Finish);
        yield break;
    }

    public void UpdateState(CoffeeState newState)
    {
        DisableAllHighlights();
        currentState = newState;

        switch (currentState)
        {
            case CoffeeState.SelectCup:
                HighlightObjectWithTag("Cup");
                HighlightObjectWithTag("CupTarget");
                break;

            case CoffeeState.PlaceCupOnMachine:
                HighlightObjectWithTag("Dispenser");
                HighlightObjectWithTag("Grinder");
                break;

            case CoffeeState.MoveDispenserToGrinder:
                HighlightObjectWithTag("Spoon");
                HighlightObjectWithTag("GrinderWafla");
                HighlightObjectWithTag("Dispenser");
                break;

            case CoffeeState.CollectCoffee:
                HighlightObjectWithTag("Dispenser");
                HighlightObjectWithTag("Machine");
                break;

            case CoffeeState.ReturnDispenserToMachine:
                HighlightObjectWithTag("Button");
                break;

            case CoffeeState.BrewCoffee:
                HighlightObjectWithTag("Cup");
                break;

            case CoffeeState.Finish:
                DisableAllHighlights();
                break;
        }

    }

    void HighlightObjectWithTag(string tag)
    {
        foreach (Outline outline in outlineComponents)
        {
            if (outline.gameObject.CompareTag(tag))
            {
                outline.enabled = true;
            }
        }
    }

    void DisableAllHighlights()
    {
        foreach (Outline outline in outlineComponents)
        {
            outline.enabled = false;
        }
    }

    public void OnButtonClick()
    {
        PlaySound(_buttonClickSound);
        if (currentState == CoffeeState.ReturnDispenserToMachine)
        {
            isButtonPressed = true;
        }
    }

    public void SetButtonColor(Color color)
    {
        _glowButRenderer.materials[1].color = color;
    }

    private void PlaySound( AudioClip clip)
    {
        _AudioSource.clip = clip;
        _AudioSource.Play();
    }

}
