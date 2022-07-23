using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creep/Behavior/Robot")]
public class Creep_Robot_Behaviour: Creep_Behaviour
{
    public bool movingInHoriz = true;
    public override Vector2 TravelTo(Vector2 dest, Vector2 selfPos, bool isInHorizDirection)
    {
        Vector2 returnVec = new Vector2(0, 0);

        float xDist = dest.x - selfPos.x;
        float yDist = dest.y - selfPos.y;

        if (isInHorizDirection)
        {
            if (Math.Abs(xDist) <= attackDistance)
            {
                return returnVec;
            }

            returnVec = new Vector2(Mathf.Sign(xDist), 0);
        }
        else// yDist > xDist
        {
            if (Math.Abs(yDist) <= attackDistance)
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
        return damageArea;
    }
}
