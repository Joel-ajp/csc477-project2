using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleID : MonoBehaviour
{
    [SerializeField] int _puzzleID;

    //retrieves ID of puzzle piece
    public int GetID()
    {
        return _puzzleID;
    }
}
