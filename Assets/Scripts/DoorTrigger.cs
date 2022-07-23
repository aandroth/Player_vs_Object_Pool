using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorTrigger : MonoBehaviour
{
    public GameObject myRoom, attachedRoom, attachedDoorTrigger, myDoor;
    public GameObject player, cam, spawnInRoomPoint;
    public float doorOpenTime, doorOpenTimeMax;
    public Vector2 newRoomDir;

    public bool doorIsOpen = true, doorIsLocked = false;

    public void Update()
    {
        if (doorIsOpen)
        {
            doorOpenTime -= Time.deltaTime;
            if (doorOpenTime <= 0)
            {
                doorIsOpen = false;
                myDoor.SetActive(true);
            }
        }
    }

    private void triggerCameraPanToNewRoom()
    {
        cam.GetComponent<CameraScript>().setNewLimitsUppDwnLftRht(attachedRoom.GetComponent<RoomScript>().getCameraLimits());
        //attachedRoom.GetComponent<RoomScript>().setCameraLimits(cam);
        //cam.GetComponent<CameraScript>().beginRoomTransitionInDirection(newRoomDir);
    }

    private void triggerPlayerMoveToNewRoom()
    {
        Debug.Log("triggering initiateTransitionToNewRoomAtPos");
        Vector2 newPos = attachedDoorTrigger.GetComponent<DoorTrigger>().getSpawnPointInRoom();
        //newPos.x = newRoomDir.x * player.GetComponent<PlayerScript>().getHeight()*3;
        //newPos.y = newRoomDir.y * player.GetComponent<PlayerScript>().getHeight()*3;
        //newPos.y += attachedDoors.GetComponent<DoorTrigger>().getDoorHeight();
        //newPos.y += getDoorHeight();
        
        player.GetComponent<PlayerScript>().initiateTransitionToNewRoomAtPos(newPos);
    }

    private float getDoorHeight()
    {
        //UnityEngine.Vector2 ctCenter = myDoor.GetComponent<TilemapRenderer>().bounds.center;
        UnityEngine.Vector2 ctExtents = myDoor.GetComponent<TilemapRenderer>().bounds.extents;
        return ctExtents.y * 2f;
    }
    private float getDoorWidth()
    {
        float doorWidth = 0;
        return doorWidth;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!doorIsLocked && collision.tag == "Player")
        {
            doorIsOpen = true;
            doorOpenTime = doorOpenTimeMax;
            myDoor.SetActive(false);
            triggerCameraPanToNewRoom();
            triggerPlayerMoveToNewRoom();
            myRoom.GetComponent<RoomScript>().stopAllCreeps();
            attachedRoom.GetComponent<RoomScript>().PlayerEntersRoom();
        }
    }

    public Vector2 getSpawnPointInRoom()
    {
        return spawnInRoomPoint.gameObject.transform.position;
    }
}
