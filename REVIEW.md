# Code Review Report

## 1. Architecture and Global State (`G.cs`, `Main.cs`)
*   **Global Static Class `G`**: Current implementation acts as a Service Locator. While convenient, it creates tight coupling and makes the state mutable from anywhere.
    *   *Recommendation*: Consider a Dependency Injection framework (like Zenject) or at least make setters private/internal to restrict state modification.
*   **Hardcoded Names**: Character names are hardcoded in `G.GetCharacterName`.
    *   *Recommendation*: Move this to a configuration file or directly into a ScriptableObject.
*   **Initialization Order**: `Main.Start` handles many dependencies manually. This is brittle if initialization order changes.

## 2. Input System
*   **Mixed Input Systems**: The project uses both the New Input System (Actions) and legacy `UnityEngine.Input`.
    *   *Locations*: `PlayerController.cs` (KeyCode.V), `WaitingStartDialogueNpcState.cs` (KeyCode.E), `PlayerInteraction.cs` (Input.mousePosition).
    *   *Recommendation*: Fully migrate to the New Input System to support multiple input devices and rebindable controls.

## 3. NPC and State Machine
*   **Redundant Naming**: Classes like `ComingNpcNpcState` have redundant "Npc" in their names.
*   **Logic Error in `LeavingCoffeeNpcNpcState`**:
    ```csharp
    private void OnNpcCome() {
        _stateMachine.ChangeState<WaitingStartDialogueNpcState>();
    }
    ```
    NPCs transition back to waiting for dialogue instead of leaving/despawning.
*   **State Allocations**: `ChangeState<T>` creates a `new T()` on every transition, causing GC pressure.
    *   *Recommendation*: Cache state instances within the StateMachine.

## 4. Dialogue and UI Systems
*   **Event Listener Leaks**:
    *   `DialogueNode.cs`: Subscribes to input but only unsubscribes in `MoveNext`. Interruptions will leave dangling subscriptions.
    *   `HandbookChooseNode.cs`: `onChoose` listener is not removed if the handbook is closed without making a choice.
*   **UI Performance**: `UIController` uses `List.Find` with type checks (`is`) frequently.
    *   *Recommendation*: Use a `Dictionary<Type, BaseScreen>` for O(1) lookup.

## 5. Clean Code and Standards
*   **File/Class Mismatch**: `LeavingNpcState.cs` contains `LeavingCoffeeNpcNpcState`.
*   **Namespace Inconsistency**: Mixed use of `GoT._Game` and `_Game` prefixes.
*   **Private Field Naming**: Inconsistent use of underscores for private members across different classes.

---
**Summary**: The project has a solid foundation using xNode and StateMachines, but requires refactoring to address technical debt, memory leaks, and input system inconsistencies.
