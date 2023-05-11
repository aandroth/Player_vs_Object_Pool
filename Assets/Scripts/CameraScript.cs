using UnityEngine;

public class CameraScript : MonoBehaviour 
{
    [SerializeField] new Camera camera;
    [SerializeField] GameObject Player;
    [SerializeField] Transform target;
    [SerializeField] Vector2 viewSize;
    [SerializeField] Bounds cameraMovementArea;
    [SerializeField] float transitionSpeed;
    [SerializeField] float smoothing;

    Vector2 transitionDirection, oldPos; // Transition
    float originalOrthographicSize;

    //Vector2 CameraViewSize => camera.ViewportToWorldPoint(Vector2.one) - camera.ViewportToWorldPoint(Vector2.zero);
    Vector2 CameraViewSize => new Vector2
    {
        y = 2f * camera.orthographicSize,
        x = 2f * camera.orthographicSize * camera.aspect
    };

    void Awake()
    {
        originalOrthographicSize = camera.orthographicSize;
        state = CAMERA_STATE.IN_ROOM;
    }
	
	void LateUpdate() 
    {
        var new_pos = transform.position;

        switch (state)
        {
            case CAMERA_STATE.IN_ROOM:
                new_pos = new Vector3
                (
                    target.transform.position.x,
                    target.transform.position.y,
                    transform.position.z
                );
                new_pos = Vector3.Lerp(transform.position, new_pos, 1f);
                new_pos.x = Mathf.Clamp(new_pos.x, cameraMovementArea.min.x, cameraMovementArea.max.x);
                new_pos.y = Mathf.Clamp(new_pos.y, cameraMovementArea.min.y, cameraMovementArea.max.y);
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
    
    void UpdateCameraSize(Bounds bounds)
    {
        camera.orthographicSize = originalOrthographicSize;

        var currentViewSize = CameraViewSize;
        if (currentViewSize.x > bounds.size.x)
        {
            camera.orthographicSize /= currentViewSize.x / bounds.size.x;
            currentViewSize = CameraViewSize;
        }
        if (currentViewSize.y > bounds.size.y)
        {
            camera.orthographicSize /= currentViewSize.y / bounds.size.y;
            currentViewSize = CameraViewSize;
        }
    }
    void UpdateCameraMovementLimit(Bounds bounds)
    {
        var tempCameraViewSize = CameraViewSize;
        cameraMovementArea = new Bounds(bounds.center, bounds.size - new Vector3(tempCameraViewSize.x, tempCameraViewSize.y));
        Debug.Log($"<color=purple>[CameraScript]</color> UpdateCameraMovementLimit: cameraMovementArea.size={cameraMovementArea.size}");
    }

    public void setNewLimitsUppDwnLftRht(Bounds bounds)
    {
        Debug.Log($"<color=purple>[CameraScript]</color> setNewLimitsUppDwnLftRht: bounds={bounds}");
        UpdateCameraSize(bounds);
        UpdateCameraMovementLimit(bounds);
    }

    public void beginRoomTransitionInDirection(Vector2 dir)
    {
        oldPos = transform.position;
        transitionDirection = dir;
        //state = CAMERA_STATE.TRANSITION_ROOM;
    }

    public enum CAMERA_STATE { IN_ROOM, TRANSITION_ROOM }
    private CAMERA_STATE state = CAMERA_STATE.IN_ROOM;
}
