using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

    public bool isAttacking;
    public Transform pushBackPoint;
    
    public int m_damage;
    float attackTimer = 0;
    float attackTimerLimit;

    void Start()
    {
        attackTimer = 0;
        attackTimerLimit = 0.5f;
        isAttacking = false;
        gameObject.SetActive(false);
    }

	// Use this for initialization
	void OnEnable () {
        attackTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (isAttacking)
        {
            if (attackTimer >= attackTimerLimit)
            {
                isAttacking = false;
                attackTimer = 0;
                gameObject.SetActive(false);
            }
            else
            {
                attackTimer += Time.deltaTime;
            }
        }
    }

    public void Attack(float timeLimit)
    {
        attackTimerLimit = timeLimit;
        isAttacking = true;
    }

    public int getDamage() { return m_damage; }
}
