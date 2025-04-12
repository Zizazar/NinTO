using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.PackageManager;
using UnityEngine;
using static CoffeeMakerSystem.CoffeeMakerController;
using static Unity.Burst.Intrinsics.X86.Avx;

public class CoffeeContoller : MonoBehaviour
{
    public GameObject glowButton;
    public AudioClip _BrewSound;
    public AudioClip _buttonClickSound;

    public GameObject fluid;
    public CoffeeGiveTriger coffeeGiveTriger;
    public HintsController hints;

    [Header("Triggers")]
    public CupTrigger cupTrigger;
    public SnapPoint dispenserSnap;
    public DispenserTrigger2 dispenserTrigger2;
    public GrinderTrigger grinderTrigger;

    private Renderer _glowButRenderer;
    private Outline[] outlineComponents;
    private AudioSource _AudioSource;
    private bool shouldShowHints = true;

    private bool cupIn;
    public enum CoffeeState
    {
        None,
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

    public CoffeeState currentState = CoffeeState.None;
    private bool isButtonPressed;

    private void Start()
    {
        _glowButRenderer = glowButton.GetComponent<Renderer>();
        outlineComponents = FindObjectsOfType<Outline>();
        _AudioSource = GetComponent<AudioSource>();
        DisableAllHighlights();
    }

    public void StartCoffeeMaking()
    {
        StartCoroutine(CoffeeGameLoop());
    }

    private IEnumerator CoffeeGameLoop()
    {
        UpdateState(CoffeeState.SelectCup);
        if (shouldShowHints) hints.showHint("Поместите любую чашку помеченную зелёным в кофе-машину");

        yield return new WaitUntil(() => cupTrigger.IsCupIn());
        SetButtonColor(new Color(255, 200, 0));
        UpdateState(CoffeeState.MoveDispenserToGrinder);
        if (shouldShowHints) hints.showHint("Возьмите ложку и зачерпните кофе из Гриндера");

        yield return new WaitUntil(() => grinderTrigger.IsSpoonIn());
        UpdateState(CoffeeState.CollectCoffee);
        if (shouldShowHints) hints.showHint("Насыпте кофе в портафильтер");

        yield return new WaitUntil(() => dispenserSnap.IsSnapped());
        dispenserTrigger2.SetSpoonIn(false);
        SetButtonColor(new Color(255, 200, 0));
        UpdateState(CoffeeState.ReturnDispenserToMachine);
        if (shouldShowHints) hints.showHint("Насыпте кофе в портафильтер");

        yield return new WaitUntil(() => isButtonPressed);
        UpdateState(CoffeeState.BrewCoffee);
        isButtonPressed = false;
        SetButtonColor(new Color(255, 100, 0));

        yield return new WaitWhile(() => _AudioSource.isPlaying);
        fluid.SetActive(true);
        PlaySound(_BrewSound);

        yield return new WaitUntil(() => !_AudioSource.isPlaying);
        fluid.SetActive(false);
        SetButtonColor(new Color(0, 255, 0));
        cupTrigger.FillCup();
        UpdateState(CoffeeState.Finish);
        yield return new WaitUntil(() => IsCoffeeDone());
        UpdateState(CoffeeState.GiveCoffee);
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
                break;

            case CoffeeState.CollectCoffee:
                HighlightObjectWithTag("Dispenser");
                HighlightObjectWithTag("Machine");
                break;

            case CoffeeState.ReturnDispenserToMachine:
                HighlightObjectWithTag("Button");
                break;

            case CoffeeState.BrewCoffee:
                HighlightObjectWithTag("CupFilled");
                break;
            case CoffeeState.Finish:
                HighlightObjectWithTag("CupFilled");
                
                break;

            case CoffeeState.GiveCoffee:
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


    public bool IsCoffeeDone()
    {
        return (coffeeGiveTriger.IsCoffeeIn() && (currentState == CoffeeState.GiveCoffee));
    }
}
