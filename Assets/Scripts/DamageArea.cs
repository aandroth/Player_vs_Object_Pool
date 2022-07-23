using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour {

    public GameObject parent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon" &&
            !parent.GetComponent<CreepScript>().isBeingDamaged)
        {
            parent.GetComponent<CreepScript>().takeDamage(collision.GetComponent<WeaponScript>().getDamage());
        }
        else if (collision.tag == "Player")
        {
            parent.GetComponent<CreepScript>().isAttacking = true;
            collision.GetComponent<PlayerScript>().TakeDamage(parent.GetComponent<CreepScript>().getDamage());
            //parent.GetComponent<CreepScript>().knockBack(collision.transform.position - transform.position, 4);
        }
    }
}
