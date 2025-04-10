using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1Key : MonoBehaviour
{
    public static int piecesFound;
    [SerializeField] private GameObject Lock;

    [SerializeField] private Animator animController1;
    [SerializeField] private Animator animController2;
    [SerializeField] private Animator animController3;

    [Header("Set 1: Doors and their movement offsets")]
    [SerializeField] private List<Transform> doorSet1;
    [SerializeField] private List<Vector3> positionOffsets1;
    [SerializeField] private List<Vector3> rotationOffsets1;

    [Header("Set 2: Doors and their movement offsets")]
    [SerializeField] private List<Transform> doorSet2;
    [SerializeField] private List<Vector3> positionOffsets2;
    [SerializeField] private List<Vector3> rotationOffsets2;

    // Store original states
    private List<Vector3> originalPositions1 = new List<Vector3>();
    private List<Quaternion> originalRotations1 = new List<Quaternion>();
    private List<Vector3> originalPositions2 = new List<Vector3>();
    private List<Quaternion> originalRotations2 = new List<Quaternion>();

    void Start()
    {
        piecesFound = 0;
        try
        {
            Lock = GameObject.Find("Lock");
        }
        catch (Exception e)
        {
            Debug.LogError("ERR: Cannot find lock GameObject - " + e);
        }

        ValidateInputs(doorSet1, positionOffsets1, rotationOffsets1, "Set 1");
        ValidateInputs(doorSet2, positionOffsets2, rotationOffsets2, "Set 2");

        StoreOriginalTransforms(doorSet1, originalPositions1, originalRotations1);
        StoreOriginalTransforms(doorSet2, originalPositions2, originalRotations2);
    }

    private void ValidateInputs(List<Transform> doors, List<Vector3> posOffsets, List<Vector3> rotOffsets, string setName)
    {
        if (doors.Count != posOffsets.Count || doors.Count != rotOffsets.Count)
        {
            Debug.LogWarning($"Mismatch in number of doors, position offsets, or rotation offsets for {setName}.");
        }
    }

    private void StoreOriginalTransforms(List<Transform> doors, List<Vector3> posList, List<Quaternion> rotList)
    {
        foreach (var door in doors)
        {
            posList.Add(door.position);
            rotList.Add(door.rotation);
        }
    }

    public void incrementPuzzle()
    {
        piecesFound++;
        determineKey();
    }

    public void decrementPuzzle()
    {
        piecesFound--;
        determineKey();
    }

    public void determineKey()
    {
        if (piecesFound == 3)
        {
            Debug.Log("Puzzle 1 solved");
            Lock.SetActive(false); // This disables the entire GameObject

            // Optional: just disable the collider instead
            Collider lockCollider = Lock.GetComponent<Collider>();
            if (lockCollider != null)
                lockCollider.enabled = false;

            SetAnimations(true);
            MoveDoorsByOffset(doorSet1, positionOffsets1, rotationOffsets1);
            MoveDoorsByOffset(doorSet2, positionOffsets2, rotationOffsets2);
        }

        else if (piecesFound >= 0 && piecesFound < 3)
        {
            Debug.Log($"Puzzle 1 unsolved: {piecesFound} piece(s) registered");
            Lock.SetActive(true);
            SetAnimations(false);
            RestoreOriginalTransforms(doorSet1, originalPositions1, originalRotations1);
            RestoreOriginalTransforms(doorSet2, originalPositions2, originalRotations2);
        }
        else
        {
            Debug.LogError("ERR: Puzzle 1 has negative/too many pieces");
        }
    }

    private void SetAnimations(bool puzzleCompleted)
    {
        animController1.SetBool("puzzle_1_completed", puzzleCompleted);
        animController2.SetBool("puzzle_1_completed", puzzleCompleted);
        animController3.SetBool("puzzle_1_completed", puzzleCompleted);
    }

    private void MoveDoorsByOffset(List<Transform> doors, List<Vector3> posOffsets, List<Vector3> rotOffsets)
    {
        for (int i = 0; i < doors.Count; i++)
        {
            if (doors[i] != null)
            {
                doors[i].position += posOffsets[i];
                doors[i].rotation *= Quaternion.Euler(rotOffsets[i]);
            }
        }
    }

    private void RestoreOriginalTransforms(List<Transform> doors, List<Vector3> originalPositions, List<Quaternion> originalRotations)
    {
        for (int i = 0; i < doors.Count; i++)
        {
            if (doors[i] != null)
            {
                doors[i].position = originalPositions[i];
                doors[i].rotation = originalRotations[i];
            }
        }
    }
}
