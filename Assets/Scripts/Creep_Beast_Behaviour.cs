using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creep/Behavior/Beast")]
public class Creep_Beast_Behaviour : Creep_Behaviour
{
    public Coroutine attackEnumerator;

    public void OnEnable()
    {
        //attackArea.GetComponent<AttackArea>()
    }

    public void StartAttack()
    {

    }

    public IEnumerator AttackSequence()
    {
        yield return null;
    }

    public override Vector2 TravelTo(Vector2 dest, Vector2 selfPos, bool isInHorizDirection)
    {
        Vector2 returnVec = new Vector2(0, 0);

        float xDist = dest.x - selfPos.x;
        float yDist = dest.y - selfPos.y;

        if (isInHorizDirection)
        {
            if (Math.Abs(yDist) > 2 * Math.Abs(xDist))
            {
                return returnVec;
            }
            returnVec = new Vector2(Mathf.Sign(xDist), 0);
        }
        else
        {
            if (Math.Abs(xDist) > 2 * Math.Abs(yDist))
            {
                return returnVec;
            }
            returnVec = new Vector2(0, Mathf.Sign(yDist));
        }
        return returnVec;
    }

    public override GameObject GetAttackArea()
    {
        return attackArea;
    }

    public override GameObject GetDamageArea()
    {
        return attackArea;
    }
}
