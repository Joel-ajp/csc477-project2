using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class passwordCheck : MonoBehaviour{
    public TMP_InputField inputField;
    public string correctPass = "takebackcontrol";

    public void CheckPass() {
        if (inputField.text == correctPass) {
            // call the code that opens the door
        } else {
            // change to a red alert screen 
        }
    }
}
