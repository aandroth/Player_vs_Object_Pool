using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public delegate void BeginAttackDel();
    BeginAttackDel beginAttackDel;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // perform attack by calling attack in the creep script through the delegate
        if(collision.tag == "Player")
            beginAttackDel.Invoke();
    }

    public void SetBeginAttackDelegate(BeginAttackDel beginAttackDelegate)
    {
        beginAttackDel = beginAttackDelegate;
    }

    //public void SetAttackArea(Vector3 position, BoxCollider2D collider)
    //{
    //    transform.position = position;
    //    gameObject.GetComponent<BoxCollider2D>().
    //}
}
