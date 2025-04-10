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
                    break;

                case CoffeeState.PlaceCupOnMachine:
                    HighlightObjectWithTag("CupTarget");
                    break;

                case CoffeeState.MoveDispenserToGrinder:
                    HighlightObjectWithTag("Dispenser");
                    HighlightObjectWithTag("Grinder");
                    break;

                case CoffeeState.CollectCoffee:
                    HighlightObjectWithTag("Spoon");
                    HighlightObjectWithTag("Grinder");
                    break;

                case CoffeeState.PutCoffeeInDispenser:
                    HighlightObjectWithTag("Spoon");
                    HighlightObjectWithTag("Dispenser");
                    break;

                case CoffeeState.ReturnDispenserToMachine:
                    HighlightObjectWithTag("Dispenser");
                    HighlightObjectWithTag("DispenserTarget");
                    break;

                case CoffeeState.BrewCoffee:
                    HighlightObjectWithTag("BrewButton");
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
                    if (objectTag == "CupTarget")
                    {
                        UpdateState(CoffeeState.MoveDispenserToGrinder);
                    }
                    break;

                case CoffeeState.MoveDispenserToGrinder:
                    if (objectTag == "Dispenser" || objectTag == "Grinder")
                    {
                        UpdateState(CoffeeState.CollectCoffee);
                    }
                    break;

                case CoffeeState.CollectCoffee:
                    if (objectTag == "Spoon" || objectTag == "Grinder")
                    {
                        UpdateState(CoffeeState.PutCoffeeInDispenser);
                    }
                    break;

                case CoffeeState.PutCoffeeInDispenser:
                    if (objectTag == "Spoon" || objectTag == "Dispenser")
                    {
                        UpdateState(CoffeeState.ReturnDispenserToMachine);
                    }
                    break;

                case CoffeeState.ReturnDispenserToMachine:
                    if (objectTag == "Dispenser" || objectTag == "DispenserTarget")
                    {
                        UpdateState(CoffeeState.BrewCoffee);
                    }
                    break;

                case CoffeeState.BrewCoffee:
                    if (objectTag == "BrewButton")
                    {
                        UpdateState(CoffeeState.Finish);
                    }
                    break;
            }
        }
    }
}