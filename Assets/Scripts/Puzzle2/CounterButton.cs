using UnityEngine;
using TMPro; // Only needed if you use TextMeshPro

public class CounterButton : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;
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
    }

    private void UpdateCounterText()
    {
        if (counterText != null)
        {
            counterText.text = counter.ToString();
        }
    }
}
