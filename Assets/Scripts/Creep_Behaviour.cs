using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creep_Behaviour : ScriptableObject
{
    protected Vector3 prev_pos;
    public Vector2 target;
    protected Vector2 prevPos_V2;
    public float distToTravel;
    public float distTraveled;
    public float attackDistance;
    public float attackAnimTime;
    public GameObject attackArea;
    public GameObject damageArea;

    public Vector2 attackAreaOffset;
    public Vector2 attackAreaScale;
    public Vector2 damageAreaOffset;
    public Vector2 damageAreaScale;

    public delegate float BeginAttack();

    public float startMarker = 0, endMarker = 0;
    //////////////////////////////////////

    public abstract Vector2 TravelTo(Vector2 dest, Vector2 selfPos, bool isInHorizDirection);

    public float BeginAttackDefinition()
    {
        // Activate the damage area after a specified time
        // Damage area is specific to each enemy behavior
        Debug.Log("BeginAttackDefinition: Attack the player!");
        return attackAnimTime;
    }
    public abstract GameObject GetAttackArea();
    public abstract GameObject GetDamageArea();
}
