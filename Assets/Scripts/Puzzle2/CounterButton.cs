using UnityEngine;
using TMPro;

public class CounterButton : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private TrainSeatCombinationPuzzle puzzleScript; // Reference to the puzzle script
    [SerializeField] private int counterIndex; // Which counter this is (0, 1, or 2)
    
    private int counter = 1;

    private void Start()
    {
        UpdateCounterText();
    }

    public void IncreaseCounter()
    {
        counter++;
        if (counter > 8)
        {
            counter = 1;
        }
        UpdateCounterText();
        
        // Notify the puzzle script about the change
        if (puzzleScript != null)
        {
            puzzleScript.UpdateDigit(counterIndex, true); // true means increment
        }
    }
    
    public void DecreaseCounter()
    {
        counter--;
        if (counter < 1)
        {
            counter = 8;
        }
        UpdateCounterText();
        
        // Notify the puzzle script about the change
        if (puzzleScript != null)
        {
            puzzleScript.UpdateDigit(counterIndex, false); // false means decrement
        }
    }

    private void UpdateCounterText()
    {
        if (counterText != null)
        {
            counterText.text = counter.ToString();
        }
    }
    
    // Method to set the counter value directly (useful for initialization)
    public void SetCounter(int value)
    {
        if (value >= 1 && value <= 8)
        {
            counter = value;
            UpdateCounterText();
        }
    }
    
    // Getter for the current counter value
    public int GetCounter()
    {
        return counter;
    }
}