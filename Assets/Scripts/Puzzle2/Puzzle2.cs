using UnityEngine;
using UnityEngine.UI;

public class TrainSeatCombinationPuzzle : MonoBehaviour
{
    [System.Serializable]
    public class SeatSymbol
    {
        public int seatNumber;
        public Sprite symbolSprite;
        public string symbolName;
    }

    [Header("Puzzle Configuration")]
    public SeatSymbol[] availableSeats;
    public Image[] symbolDisplays;
    public Text[] digitInputs;
    public Button submitButton;

    [Header("Combination Lock")]
    public int[] correctCombination = new int[3];
    private int[] currentCombination = new int[3];

    [Header("Feedback")]
    public GameObject successPanel;
    public AudioSource successSound;
    public AudioSource errorSound;

    private void Start()
    {
        // Randomize the correct combination using seat numbers
        SetUpRandomCombination();

        // Setup submit button listener
        submitButton.onClick.AddListener(CheckCombination);

        // Initialize UI elements
        InitializeSymbolDisplays();
    }

    private void SetUpRandomCombination()
    {
        // Randomly select 3 unique seats for the combination
        for (int i = 0; i < 3; i++)
        {
            SeatSymbol randomSeat = availableSeats[Random.Range(0, availableSeats.Length)];
            correctCombination[i] = randomSeat.seatNumber;
        }

        // Debug log to help developers track the solution
        Debug.Log($"Correct Combination: {correctCombination[0]}, {correctCombination[1]}, {correctCombination[2]}");
    }

    private void InitializeSymbolDisplays()
    {
        // Randomize the symbols for each input position
        for (int i = 0; i < 3; i++)
        {
            // Find the seat corresponding to the correct combination digit
            SeatSymbol matchingSeat = System.Array.Find(availableSeats, 
                seat => seat.seatNumber == correctCombination[i]);
            
            // Set the symbol display
            symbolDisplays[i].sprite = matchingSeat.symbolSprite;
        }
    }

    public void UpdateDigit(int inputIndex, bool increment)
    {
        // Increment or decrement the digit (0-9)
        int currentValue = int.Parse(digitInputs[inputIndex].text);
        
        if (increment)
        {
            currentValue = (currentValue + 1) % 10;
        }
        else
        {
            currentValue = (currentValue - 1 + 10) % 10;
        }

        digitInputs[inputIndex].text = currentValue.ToString();
        currentCombination[inputIndex] = currentValue;
    }

    private void CheckCombination()
    {
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

        if (isCorrect)
        {
            // Puzzle solved successfully
            successSound.Play();
            successPanel.SetActive(true);
            // You can add additional logic here, like unlocking the door
        }
        else
        {
            // Incorrect combination
            errorSound.Play();
        }
    }

    // Optional: Method to reset the puzzle
    public void ResetPuzzle()
    {
        SetUpRandomCombination();
        InitializeSymbolDisplays();
        
        // Reset digit inputs
        for (int i = 0; i < 3; i++)
        {
            digitInputs[i].text = "0";
            currentCombination[i] = 0;
        }
    }
}