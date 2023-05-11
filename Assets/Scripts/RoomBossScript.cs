using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomBossScript : MonoBehaviour
{
    public GameObject ct; // Collision Tiles of the room

    public GameObject creepHolder;
    public CreepDataObject[] creepDataObjects;
    public bool hasPuzzle, puzzleSolved;
    public List<PuzzleHolder> puzzleHolders;

    public bool furyLightActive, summonLightActive;

    public Bounds Bounds => ct.GetComponent<TilemapCollider2D>().bounds;

    void Start()
    {
        creepDataObjects = creepHolder.GetComponentsInChildren<CreepDataObject>();
    }

    public void PlayerEntersRoom()
    {
        Debug.Log("Player entered room");
        if (puzzleHolders.Count > 0)
        {
            ResetPuzzles();
        }
        startAllCreeps();
    }

    //public void PlayerExitsRoom()
    //{
    //    Debug.Log("Player exited room");
    //    stopAllCreeps();
    //}

    public void ResetPuzzles()
    {
        Debug.Log("ResetPuzzles()");
        for (int ii = 0; ii < puzzleHolders.Count; ++ii)
        {
            Debug.Log("ResetPuzzles()" + ii);
            puzzleHolders[ii].resetPuzzleBlockPositions();
        }
    }

    public void setCameraLimits(GameObject cam)
    {
        cam.GetComponent<CameraScript>().setNewLimitsUppDwnLftRht(Bounds);
    }

    public Bounds getCameraLimits() => Bounds;

    public float getCenterX()
    {
        return ct.GetComponent<TilemapCollider2D>().bounds.center.x;
    }
    public float getCenterY()
    {
        return ct.GetComponent<TilemapCollider2D>().bounds.center.y;
    }

    public void stopAllCreeps()
    {
        for (int ii = 0; ii < creepDataObjects.Length; ++ii)
        {
            if (creepDataObjects[ii].isAlive)
            {
                creepDataObjects[ii].releaseCreepGameObjectFromPool();
            }
        }
        creepHolder.SetActive(false);
    }
    public void startAllCreeps()
    {
        creepHolder.SetActive(true);
        Debug.Log("Starting creeps");
        for (int ii = 0; ii < creepDataObjects.Length; ++ii)
        {
            if (true)
            {
                creepDataObjects[ii].refreshCreepData();
            }
            creepDataObjects[ii].getCreepGameObjectFromPool();
        }
    }
}
