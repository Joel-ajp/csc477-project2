using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class passwordCheck : MonoBehaviour{
    public string correctPass = "TAKEBACKCONTROL";
    public bool isLocked = true;
    public TMP_Text trainControlText;

    public void CheckPass(string text) {
        if (text == correctPass) {
            isLocked = false;
            trainControlText.text = "  >  TRAIN CONTROLS - [UNLOCKED]";
            Debug.Log("Train Controls Unlocked");
            // call the code that opens the door
        } else {
            Debug.Log("This is not correct");
            // change to a red alert screen 
        }
    }
}
