using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Creep/Behavior/Humanoid")]
public class Creep_Humanoid_Behavior : Creep_Behaviour
{
    public override Vector2 TravelTo(Vector2 target, Vector2 selfPos, bool isInHorizDirection)
    {
        Vector2 returnVec = new Vector2(0, 0);

        //if ((Mathf.Abs(distToTravel.x) > 0.3) && Mathf.Abs(distToTravel.x) - Mathf.Abs(distToTravel.y * 10) > 0)
        //    returnVec = new Vector2(Mathf.Sign(distToTravel.x), 0);
        //else if (Mathf.Abs(distToTravel.y) > 0.3)
        //    returnVec = new Vector2(0, Mathf.Sign(distToTravel.y));


        // HUMANOID MOVE SYNTAX
        if (distTraveled >= distToTravel)
        {

            if (Mathf.Abs(target.x - selfPos.x) > 0.5f)
            {
                returnVec.x = Mathf.Sign(target.x - selfPos.x);

            }
            if (Mathf.Abs(target.y - selfPos.y) > 0.5f)
            {
                if (returnVec.x > 0)
                {
                    returnVec.x = 0.5f * Mathf.Sign(target.x - selfPos.x);
                    returnVec.y = 0.5f * Mathf.Sign(target.y - selfPos.y);
                }
                else
                {
                    returnVec.y = Mathf.Sign(target.y - selfPos.y);
                }
            }
        }
        //returnVec *= m_speed;
        //prevPos_V2 = transform.position;

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
