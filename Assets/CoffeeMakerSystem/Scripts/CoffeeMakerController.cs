using UnityEngine;

namespace CoffeeMakerSystem
{
    public class CoffeeMakerController : MonoBehaviour
    {
        public enum CoffeeState
        {
            SelectCup,
            PlaceCupOnMachine,
            MoveDispenserToGrinder,
            CollectCoffee,
            PutCoffeeInDispenser,
            ReturnDispenserToMachine,
            BrewCoffee,
            Finish
        }

        public CoffeeState currentState = CoffeeState.SelectCup;

        private Outline[] outlineComponents;

        void Start()
        {
            outlineComponents = FindObjectsOfType<Outline>();
            UpdateState(currentState);
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

        public void OnObjectPlaced(string objectTag)
        {
            switch (currentState)
            {
                case CoffeeState.SelectCup:
                    if (objectTag == "Cup")
                    {
                        UpdateState(CoffeeState.PlaceCupOnMachine);
                    }
                    break;

                case CoffeeState.PlaceCupOnMachine:
                    if (objectTag == "Dispenser")
                    {
                        UpdateState(CoffeeState.MoveDispenserToGrinder);
                    }
                    break;

                case CoffeeState.MoveDispenserToGrinder:
                    if (objectTag == "Spoon")
                    {
                        UpdateState(CoffeeState.CollectCoffee);
                    }
                    break;

                case CoffeeState.CollectCoffee:
                    if (objectTag == "Dispenser")
                    {
                        UpdateState(CoffeeState.ReturnDispenserToMachine);
                    }
                    break;

                case CoffeeState.ReturnDispenserToMachine:
                    if (objectTag == "Button")
                    {
                        UpdateState(CoffeeState.BrewCoffee);
                    }
                    break;

                case CoffeeState.BrewCoffee:
                    if (objectTag == "Cup")
                    {
                        UpdateState(CoffeeState.Finish);
                    }
                    break;
            }
        }
    }
}