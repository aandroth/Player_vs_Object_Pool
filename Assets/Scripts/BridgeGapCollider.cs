using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeGapCollider : MonoBehaviour
{
    public void ActivateCollider()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void DeactivateColliderIfOverPushBlock()
    {
        Collider2D[] allOverlappingColliders = new Collider2D[16];
        Collider2D col2d = gameObject.GetComponent<Collider2D>();

        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;

        int overlapCount = Physics2D.OverlapCollider(col2d, contactFilter, allOverlappingColliders);
        Debug.Log("overlapCount: " + overlapCount);

        for (int ii = 0; ii < overlapCount; ++ii)
        {
            if (allOverlappingColliders[ii].GetComponent<PushBlock>() != null)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                return;
            }
        }
    }
}
