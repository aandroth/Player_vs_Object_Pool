using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushBlockTrigger : MonoBehaviour
{
    //public Vector2 moveDirAllowed;
    public UnityEvent turnOnPushBlockMove;
    public UnityEvent turnOffPushBlockMove;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;

        turnOnPushBlockMove.Invoke();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;

        turnOffPushBlockMove.Invoke();
    }
}
