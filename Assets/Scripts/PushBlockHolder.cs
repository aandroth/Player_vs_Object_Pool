using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlockHolder : MonoBehaviour
{
    public void DeactivateCollidersOnChildren()
    {
        var children = gameObject.GetComponentsInChildren<PushBlock>();

        for (int ii = 0; ii < children.Length; ++ii)
        {
            children[ii].DeactivateCollisionBoxes();
        }
    }
    public void ActivateCollidersOnChildren()
    {
        var children = gameObject.GetComponentsInChildren<PushBlock>();

        for (int ii = 0; ii < children.Length; ++ii)
        {
            children[ii].ActivateCollisionBoxes();
        }
    }
}
