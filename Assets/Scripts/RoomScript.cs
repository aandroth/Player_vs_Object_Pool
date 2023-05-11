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
    [SerializeField]
    public List<GameObject> roomItems;

    public bool furyLightActive, summonLightActive;

    public bool isBossRoom;

    public UnityEvent events;

    [HideInInspector]
    public GameObject bossObject;

    public Bounds Bounds => ct.GetComponent<TilemapCollider2D>().bounds;

    void Start()
    {
        creepDataObjects = creepHolder.GetComponentsInChildren<CreepDataObject>();
    }

    public void PlayerEntersRoom()
    {
        Debug.Log("<color=purple>[RoomScript]</color> PlayerEntersRoom");
        if (!isBossRoom)
        {
            if (puzzleHolders.Count > 0)
            {
                ResetPuzzles();
            }
            if (roomItems.Count > 0)
            {
                ResetItems();
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
        Debug.Log("<color=purple>[RoomScript]</color> ResetPuzzles");
        for (int i = 0; i < puzzleHolders.Count; ++i)
        {
            Debug.Log("<color=purple>[RoomScript]</color> ResetPuzzles: i=" + i);
            puzzleHolders[i].resetPuzzleBlockPositions();
        }
    }

    public void ResetItems()
    {
        Debug.Log("<color=purple>[RoomScript]</color> ResetItems");
        for (int i = 0; i < roomItems.Count; ++i)
        {
            Debug.Log("<color=purple>[RoomScript]</color> ResetItems: i=" + i);
            roomItems[i].GetComponent<I_RoomItems>().RoomReset();
        }
    }
    
    public void setCameraLimits(GameObject cam)
    {
        cam.GetComponent<CameraScript>().setNewLimitsUppDwnLftRht(getCameraLimits());
    }
    
    public Bounds getCameraLimits()
    {
        return Bounds;
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
        Debug.Log("<color=purple>[RoomScript]</color> startAllCreeps");
        creepHolder.SetActive(true);
        for (int i = 0; i < creepDataObjects.Length; ++i)
        {
            creepDataObjects[i].refreshCreepData();
            creepDataObjects[i].getCreepGameObjectFromPool();
        }
    }

    public void StartBossBattle()
    {
        //bossObject.SetActive(true);
    }
}