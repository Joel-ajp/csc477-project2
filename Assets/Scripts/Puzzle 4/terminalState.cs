using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class terminalState : MonoBehaviour{
    public GameObject[] UIPanels;
    public passwordCheck passwordScript;

    void Start() {
        ShowUI(0);
    }

    public void ShowUI(int index) {
        foreach (GameObject panel in UIPanels) {
            panel.SetActive(false);
        }

        if ((index == 8) && (passwordScript.isLocked)) {
            UIPanels[0].SetActive(true);
            return;
        }

        UIPanels[index].SetActive(true);
    }
}
