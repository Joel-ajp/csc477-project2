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
        }
        else if (piecesFound >= 0 && piecesFound < 3)
        {
            //Close gate/replace lock
            print("Puzzle 1 unsolved: " + piecesFound + " piece registered");
            Lock.SetActive(true);
        }
        else
        {
            print("ERR: Puzzle 1 has negative/too many pieces");
        }
    }
}
