using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoomScript : MonoBehaviour
{
    public GameObject ct; // Collision Tiles of the room
    public float topMostPos, botMostPos, leftMostPos, rightMostPos;

    public GameObject creepHolder;
    public CreepDataObject[] creepDataObjects;
    public bool hasPuzzle, puzzleSolved;
    public List<PuzzleHolder> puzzleHolders;

    public bool furyLightActive, summonLightActive;

    public bool isBossRoom;

    public UnityEvent events;

    [HideInInspector]
    public GameObject bossObject;

    void Start()
    {
        UnityEngine.Vector2 ctCenter = ct.GetComponent<TilemapCollider2D>().bounds.center;
        UnityEngine.Vector2 ctExtents = ct.GetComponent<TilemapCollider2D>().bounds.extents;

        topMostPos = ctCenter.y + ctExtents.y;
        botMostPos = ctCenter.y - ctExtents.y;
        leftMostPos = ctCenter.x - ctExtents.x;
        rightMostPos = ctCenter.x + ctExtents.x;

        creepDataObjects = creepHolder.GetComponentsInChildren<CreepDataObject>();
    }

    public void PlayerEntersRoom()
    {
        Debug.Log("Player entered room");
        if (!isBossRoom)
        {
            if (puzzleHolders.Count > 0)
            {
                ResetPuzzles();
            }
            startAllCreeps();
        }
        else
        {
            StartBossBattle();
        }
        events.Invoke();
    }

    public void ResetPuzzles()
    {
        Debug.Log("ResetPuzzles()");
        for (int ii=0; ii<puzzleHolders.Count; ++ii)
        {
            Debug.Log("ResetPuzzles()"+ii);
            puzzleHolders[ii].resetPuzzleBlockPositions();
        }
    }
    
    public void setCameraLimits(GameObject cam)
    {
        UnityEngine.Vector2 ctCenter = ct.GetComponent<TilemapCollider2D>().bounds.center;
        UnityEngine.Vector2 ctExtents = ct.GetComponent<TilemapCollider2D>().bounds.extents;

        cam.GetComponent<CameraScript>().setNewLimitsUppDwnLftRht(topMostPos, botMostPos, leftMostPos, rightMostPos);
    }
    
    public Vector4 getCameraLimits()
    {
        UnityEngine.Vector2 ctCenter = ct.GetComponent<TilemapCollider2D>().bounds.center;
        UnityEngine.Vector2 ctExtents = ct.GetComponent<TilemapCollider2D>().bounds.extents;

        return new Vector4(topMostPos, botMostPos, leftMostPos, rightMostPos);
    }

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
        Debug.Log("in startAllCreeps");
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

    public void StartBossBattle()
    {
        //bossObject.SetActive(true);
    }
}