using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeGapColliderHolder : MonoBehaviour
{
    public void DeactivateCollidersOnChildrenIfOverPushBlock()
    {
        var children = gameObject.GetComponentsInChildren<BridgeGapCollider>();

        for(int ii=0; ii<children.Length; ++ii)
        {
            children[ii].DeactivateColliderIfOverPushBlock();
        }
    }
    public void ActivateCollidersOnChildren()
    {
        var children = gameObject.GetComponentsInChildren<BridgeGapCollider>();

        for(int ii=0; ii<children.Length; ++ii)
        {
            children[ii].ActivateCollider();
        }
    }
}
