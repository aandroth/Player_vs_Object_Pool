using System;
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
    public float attackPreTime;
    public float attackAnimTime;
    public float attackPostTime;
    public IEnumerator attackExecutionEnumerator;
    public GameObject attackArea;
    public GameObject damageArea;
    public Animator animator;

    public Vector2 attackAreaOffset;
    public Vector2 attackAreaScale;
    public Vector2 damageAreaOffset;
    public Vector2 damageAreaScale;

    public delegate float BeginAttack();

    public float startMarker = 0, endMarker = 0;
    //////////////////////////////////////

    public abstract Vector2 TravelTo(Vector2 dest, Vector2 selfPos, bool isInHorizDirection);

    public IEnumerator ExecuteAttack()
    {
        yield return new WaitForSeconds(attackPreTime);
        Debug.Log("BeginAttackDefinition: Start attacking the player!");
        yield return new WaitForSeconds(attackAnimTime);
        Debug.Log("BeginAttackDefinition: Ended attacking the player!");
        yield return new WaitForSeconds(attackPostTime);
    }

    public abstract GameObject GetAttackArea();
    public abstract GameObject GetDamageArea();
}
