using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    public bool isNotColliding;

    // Start is called before the first frame update
    void Start()
    {
        isNotColliding = true;
    }

    public void OnCollisionStay2D(Collision2D other)
    {
        isNotColliding = false;
    }
    public void OnCollisionExit2D(Collision2D other)
    {
        isNotColliding = true;
    }
}
