using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class buttonHandler : MonoBehaviour{
    public TMP_InputField inputField;

    public void onButtonClick (string letter) {
        inputField.text += letter;
    }

    public void onButtonDelete() {
        if (!string.IsNullOrEmpty(inputField.text)) {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }
}
