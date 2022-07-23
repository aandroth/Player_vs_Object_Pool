using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHolder : MonoBehaviour
{
    public List<PushBlock> pushBlockList;
    public List<Vector2> pushBlockSolutionList;
    public bool puzzleIsSolved = false;

    void Start()
    {
        if (pushBlockList.Count == 0)
            this.enabled = false;
    }

    public void resetPuzzleBlockPositions()
    {

        Debug.Log("PuzzleHolder: resetPuzzleBlockPositions()");
        for (int ii = 0; ii < pushBlockList.Count; ++ii)
        {
            pushBlockList[ii].resetBlockToStartingCoordsAndPos();
        }
    }

    public void checkIfPuzzleBecameSolved()
    {
        Debug.Log("PuzzleHolder: checkIfPuzzleBecameSolved()");
        if (!puzzleIsSolved && puzzleBecameSolved())
        {
            Debug.Log("PuzzleHolder: LockPuzzlePositions()");
            LockPuzzlePositions();
            puzzleIsSolved = true;
        }
    }

    private bool puzzleBecameSolved()
    {
        List<Vector2> tempPushBlockSolutionList;
        {
            Vector2[] tempArr = new Vector2[pushBlockSolutionList.Count];
            pushBlockSolutionList.CopyTo(tempArr);

            tempPushBlockSolutionList = new List<Vector2>(tempArr);
        }
        
        for(int ii=0; ii<pushBlockList.Count; ++ii)
        {
            for(int jj=0; jj<tempPushBlockSolutionList.Count; ++jj)
            {
                if(pushBlockList[ii].currentPositionX == tempPushBlockSolutionList[jj].x &&
                    pushBlockList[ii].currentPositionY == tempPushBlockSolutionList[jj].y)
                {
                    tempPushBlockSolutionList.RemoveAt(jj);
                    if (tempPushBlockSolutionList.Count == 0)
                        return true;
                    break;
                }
            }
        }
        if (tempPushBlockSolutionList.Count == 0)
        {
            return true;
        }
        return false;
    }

    private void LockPuzzlePositions()
    {
        for (int ii = 0; ii < pushBlockList.Count; ++ii)
        {
            pushBlockList[ii].setCurrentCoordsAndPosAsStarting();
        }
    }
}
