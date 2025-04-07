using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backButton : MonoBehaviour {
    public GameObject HomeScreen;
    public GameObject ClassifiedScreen;
    public GameObject Journal1;
    public GameObject Journal2;
    public GameObject Journal3;
    public GameObject DataLogScreen;
    public GameObject GovernmentEntityUI;
    public GameObject JournalEntries;
    public GameObject TrainControls;

    public void goBack() {
        if (ClassifiedScreen.activeSelf) {
            ClassifiedScreen.SetActive(false);
            JournalEntries.SetActive(true);
        } else if (Journal1.activeSelf) {
            Journal1.SetActive(false);
            JournalEntries.SetActive(true);
        } else if (Journal2.activeSelf) {
            Journal2.SetActive(false);
            JournalEntries.SetActive(true);
        } else if (Journal3.activeSelf) {
            Journal3.SetActive(false);
            JournalEntries.SetActive(true);
        } else if (JournalEntries.activeSelf) {
            JournalEntries.SetActive(false);
            HomeScreen.SetActive(true);
        } else if (DataLogScreen.activeSelf) {
            DataLogScreen.SetActive(false);
            HomeScreen.SetActive(true);
        } else if (GovernmentEntityUI.activeSelf) {
            GovernmentEntityUI.SetActive(false);
            HomeScreen.SetActive(true);
        } else if (TrainControls.activeSelf) {
            TrainControls.SetActive(false);
            HomeScreen.SetActive(true);
        }
    }
}
