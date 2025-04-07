using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class screenButtons : MonoBehaviour
{
    public TMP_Text buttonText;
    private bool toggleOn = true;
    private bool toggleDoors = false;

    public void enableButtons() {
        toggleOn = !toggleOn;
        buttonText.text = toggleOn ? "[ENABLED]" : "[DISABLED]";
    }

    public void openButtons() {
        toggleDoors = !toggleDoors;
        buttonText.text = toggleDoors ? "[CLOSED]" : "[OPENED]";
    }
}
