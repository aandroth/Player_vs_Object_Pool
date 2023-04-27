using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public enum CAMERA_STATE { IN_ROOM, TRANSITION_ROOM }
    private CAMERA_STATE state = CAMERA_STATE.IN_ROOM;

    public GameObject Player;

    public Transform target;
    public float viewWidth, viewHeight;
    public float upperLimit, lowerLimit, leftLimit, rightLimit;
    public float smoothing;

    // Transition
    private Vector2 transitionDirection, oldPos;
    public float transitionSpeed;

	// Use this for initialization
	void Start ()
    {
        state = CAMERA_STATE.IN_ROOM;
        viewHeight = 2f * gameObject.GetComponent<Camera>().orthographicSize;
        viewWidth = viewHeight * gameObject.GetComponent<Camera>().aspect;
    }
	
	// Update is called once per frame
	void LateUpdate () {

        Vector3 new_pos = transform.position;

        switch (state)
        {
            case CAMERA_STATE.IN_ROOM:

                new_pos = new Vector3(target.transform.position.x,
                                        target.transform.position.y,
                                        transform.position.z);
                new_pos = Vector3.Lerp(transform.position, new_pos, 1f);

                new_pos.x = Mathf.Clamp(new_pos.x, leftLimit, rightLimit);
                new_pos.y = Mathf.Clamp(new_pos.y, lowerLimit, upperLimit);
                break;
            //case CAMERA_STATE.TRANSITION_ROOM:
            //    if (transitionDirection.x != 0)
            //    {
            //        new_pos.x += transitionDirection.x * Time.deltaTime * transitionSpeed;
            //        if (Math.Abs(new_pos.x - oldPos.x) >= viewWidth) {
            //            state = CAMERA_STATE.IN_ROOM;
            //        }
            //    }
            //    else
            //    {
            //        new_pos.y += transitionDirection.y * Time.deltaTime * transitionSpeed;
            //        if (Math.Abs(new_pos.y - oldPos.y) >= viewHeight)
            //        {
            //            state = CAMERA_STATE.IN_ROOM;
            //        }
            //    }
            //    break;
        }

        transform.position = Vector3.Lerp(transform.position, new_pos, smoothing);
    }

    public void setNewLimitsUppDwnLftRht(float upp, float dwn, float lft, float rht)
    {
        viewHeight = 2f * gameObject.GetComponent<Camera>().orthographicSize;
        viewWidth = viewHeight * gameObject.GetComponent<Camera>().aspect;

        upperLimit = upp - viewHeight * 0.5f;
        lowerLimit = dwn + viewHeight * 0.5f;
        leftLimit  = lft + viewWidth * 0.5f;
        rightLimit = rht - viewWidth * 0.5f;
    }

    public void setNewLimitsUppDwnLftRht(Vector4 newLimits)
    {
        viewHeight = 2f * gameObject.GetComponent<Camera>().orthographicSize;
        viewWidth = viewHeight * gameObject.GetComponent<Camera>().aspect;

        upperLimit = newLimits.x - viewHeight * 0.5f;
        lowerLimit = newLimits.y + viewHeight * 0.5f;
        leftLimit  = newLimits.z + viewWidth * 0.5f;
        rightLimit = newLimits.w - viewWidth * 0.5f;
    }

    public void beginRoomTransitionInDirection(Vector2 dir)
    {
        oldPos = transform.position;
        transitionDirection = dir;
        //state = CAMERA_STATE.TRANSITION_ROOM;
    }
}
