using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class terminalState : MonoBehaviour{
    public GameObject[] UIPanels;

    void Start() {
        ShowUI(0);
    }

    public void ShowUI(int index) {
        foreach (GameObject panel in UIPanels) {
            panel.SetActive(false);
        }

        UIPanels[index].SetActive(true);
    }
}
