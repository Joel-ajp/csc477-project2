using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class TrainSeatCombinationPuzzle : MonoBehaviour
{
    [System.Serializable]
    public class SeatSymbol
    {
        public GameObject symbolObject; // GameObject representation of the symbol
        public int benchNumber; // Hard-coded bench number
        public string symbolName; // For debugging/reference
    }

    [Header("Puzzle Configuration")]
    public SeatSymbol[] puzzleSymbols; // Predefined symbols with bench assignments
    public GameObject[] symbolDisplayObjects; // GameObjects to display the symbols
    public CounterButton[] counterButtons; // References to the counter button scripts
    public DoorController doorController; // Drag door here in Inspector
    
    [Header("Objects To Disable")]
    public MonoBehaviour[] scriptsToDisableOnComplete;
    public GameObject[] objectsToDisableOnComplete;
    public Collider[] collidersToDisableOnComplete;

    [Header("Combination Lock")]
    private int[] correctCombination = new int[3]; // Will be populated from puzzleSymbols
    private int[] currentCombination = new int[3];

    [Header("Feedback")]
    public GameObject successPanel;
    public AudioSource successSound;
    public AudioSource errorSound;

    [Header("Puzzle Complete Event")]
    public UnityEvent onPuzzleComplete;

    private bool isPuzzleCompleted = false;
    private bool isCheckingCombination = false;

    private void Start()
    {
        // Setup the puzzle with hard-coded values
        SetUpPuzzle();
        
        // Initialize UI elements
        InitializeSymbolDisplays();
    }

    private void Update()
    {
        // Check combination automatically every frame
        if (!isPuzzleCompleted && !isCheckingCombination)
        {
            // Update current combination from counter buttons
            UpdateCurrentCombinationFromCounters();
            
            // Check if the combination is correct
            CheckCombination();
        }
    }

    private void UpdateCurrentCombinationFromCounters()
    {
        // Read current values from counter buttons
        if (counterButtons != null)
        {
            for (int i = 0; i < counterButtons.Length && i < 3; i++)
            {
                if (counterButtons[i] != null)
                {
                    currentCombination[i] = counterButtons[i].GetCounter();
                }
            }
        }
    }

    private void SetUpPuzzle()
    {
        // Validate setup
        if (puzzleSymbols == null || puzzleSymbols.Length < 3)
        {
            Debug.LogError("Not enough puzzle symbols defined. Need at least 3.");
            return;
        }

        // Extract the correct combination from the hard-coded values
        for (int i = 0; i < 3; i++)
        {
            if (i < puzzleSymbols.Length)
            {
                correctCombination[i] = puzzleSymbols[i].benchNumber;
            }
        }
        
        // Debug log to help with testing
        Debug.Log($"Puzzle Solution - " +
                  $"Symbol 1: {puzzleSymbols[0].symbolName} on Bench {puzzleSymbols[0].benchNumber}, " +
                  $"Symbol 2: {puzzleSymbols[1].symbolName} on Bench {puzzleSymbols[1].benchNumber}, " +
                  $"Symbol 3: {puzzleSymbols[2].symbolName} on Bench {puzzleSymbols[2].benchNumber}");
    }

    private void InitializeSymbolDisplays()
    {
        // Make sure we have the correct number of display objects
        if (symbolDisplayObjects == null || symbolDisplayObjects.Length < 3)
        {
            Debug.LogError("Missing required symbol display objects.");
            return;
        }

        // Setup each symbol display
        for (int i = 0; i < 3; i++)
        {
            // Enable the corresponding symbol GameObject if it exists
            if (i < puzzleSymbols.Length && puzzleSymbols[i].symbolObject != null)
            {
                // Option 1: Enable the display object
                if (symbolDisplayObjects[i] != null)
                {
                    symbolDisplayObjects[i].SetActive(true);
                }
            }
            
            // Initialize counter buttons if they exist
            if (counterButtons != null && i < counterButtons.Length && counterButtons[i] != null)
            {
                // Set initial value to 1
                counterButtons[i].SetCounter(1);
                currentCombination[i] = 1;
            }
        }
    }

    public void UpdateDigit(int inputIndex, bool increment)
    {
        if (isPuzzleCompleted || inputIndex >= 3)
            return;

        // This is called from the CounterButton script
        // The counter script already updates its own value and text
        // We just need to update our internal tracking
        
        // Read the new value directly from the counter button
        if (counterButtons != null && inputIndex < counterButtons.Length && counterButtons[inputIndex] != null)
        {
            currentCombination[inputIndex] = counterButtons[inputIndex].GetCounter();
        }
    }

    private void CheckCombination()
    {
        isCheckingCombination = true;
        
        bool isCorrect = true;
        
        // Check if all digits match the correct combination
        for (int i = 0; i < 3; i++)
        {
            if (currentCombination[i] != correctCombination[i])
            {
                isCorrect = false;
                break;
            }
        }
        
        if (isCorrect && !isPuzzleCompleted)
        {
            // Puzzle solved successfully
            CompletePuzzle();
        }
        
        isCheckingCombination = false;
    }

    private void CompletePuzzle()
    {

        isPuzzleCompleted = true;

        // your existing success sound, success panel, disable logic...
        doorController?.RaiseDoor();
        
        // Play success sound if available
        if (successSound != null)
        {
            successSound.Play();
        }
        
        // Show success panel if available
        if (successPanel != null)
        {
            successPanel.SetActive(true);
        }
        
        // Disable interactive objects
        DisableInteractableObjects();
        
        // Trigger the puzzle complete event
        if (onPuzzleComplete != null)
        {
            onPuzzleComplete.Invoke();
        }
    }

    private void DisableInteractableObjects()
    {
        // Disable scripts
        if (scriptsToDisableOnComplete != null)
        {
            foreach (MonoBehaviour script in scriptsToDisableOnComplete)
            {
                if (script != null)
                {
                    script.enabled = false;
                }
            }
        }
        
        // Disable GameObjects
        if (objectsToDisableOnComplete != null)
        {
            foreach (GameObject obj in objectsToDisableOnComplete)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
        
        // Disable Colliders
        if (collidersToDisableOnComplete != null)
        {
            foreach (Collider collider in collidersToDisableOnComplete)
            {
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }
        }
    }
}