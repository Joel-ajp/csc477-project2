using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1Key : MonoBehaviour
{
    //Tracks number of puzzle pieces in place
    public static int piecesFound;
    //GameObject of lock that will be enabled/disabled
    public static GameObject Lock;
    //Amimation control
    [SerializeField] private Animator animController1;
    [SerializeField] private Animator animController2;
    [SerializeField] private Animator animController3;

    // Start is called before the first frame update
    void Start()
    {
        piecesFound = 0;
        try
        {
            Lock = GameObject.Find("Lock");
        }
        catch (Exception e)
        {
            print("ERR: Cannot find lock GameObject - " + e);
        }
    }

    //Called when piece is detected being placed
    public void incrementPuzzle()
    {
        piecesFound++;
        determineKey();
    }

    //Called when piece is detected being removed
    public void decrementPuzzle()
    {
        piecesFound--;
        determineKey();
    }

    //Called when piecesFound is changed
    //Opens turnstyles if amount of pieces is 3 (puzzle complete)
    public void determineKey()
    {
        if(piecesFound == 3)
        {
            //Open gate/remove lock
            print("Puzzle 1 solved");
            Lock.SetActive(false);
            animController1.SetBool("puzzle_1_completed", true);
            animController2.SetBool("puzzle_1_completed", true);
            animController3.SetBool("puzzle_1_completed", true);
        }
        else if (piecesFound >= 0 && piecesFound < 3)
        {
            //Close gate/replace lock
            print("Puzzle 1 unsolved: " + piecesFound + " piece registered");
            Lock.SetActive(true);
            animController1.SetBool("puzzle_1_completed", false);
            animController2.SetBool("puzzle_1_completed", false);
            animController3.SetBool("puzzle_1_completed", false);
        }
        else
        {
            print("ERR: Puzzle 1 has negative/too many pieces");
        }
    }
}
