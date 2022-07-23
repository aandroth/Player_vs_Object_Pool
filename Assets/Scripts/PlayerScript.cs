using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public bool isAttacking, isTransitioning, showDamageSprite, isBeingDamaged, isAlive, isWeapon, isPlayerAlly, onCoolDown, freezeControls;

    public float attackTimer, attackTimerLimit, 
                damageTimer, damageTimerLimit, 
                swordCoolDownTimer, swordCoolDownTimerLimit,
                roomTransitionTimer, roomTransitionTimeLimit;
    public int m_speed, m_health, m_damage;

    public GameObject weapon;

    private Vector3 prev_pos;
    public Vector2 newRoomPos, newRoomDir;

    // Use this for initialization
    void Start () {
        isAttacking = false;
        onCoolDown = false;
        showDamageSprite = false;
        isBeingDamaged = false;
        isAlive = true;
        isWeapon = false;
        isPlayerAlly = true;

        weapon.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        prev_pos = transform.position;

        if (freezeControls)
        {
            // Hold player position until game controller gives back control
        }
        else if (!isAlive)
        {
            transform.Rotate(new Vector3(0, 1, 0));
            return;
        }
        else if (isTransitioning)
        {
            roomTransitionTimer -= Time.deltaTime;

            if(roomTransitionTimer <= 0)
            {
                roomTransitionTimer = roomTransitionTimeLimit;
                isTransitioning = false;
            }

            //Vector2 newPos = transform.position;
            //if(newRoomPos.x != 0)
            //{
            //    newPos.x += newRoomDir.x * transitionSpeed;
            //    if ((newRoomDir.x == 1 && newPos.x >= newRoomPos.x) ||
            //        (newRoomDir.x == -1 && newPos.x <= newRoomPos.x))
            //    {
            //        Debug.Log("end Transitioning");
            //        newPos.x = newRoomPos.x;
            //        isTransitioning = false;
            //    }
            //}
            //else
            //{
            //    newPos.y += newRoomDir.y * transitionSpeed;
            //    if ((newRoomDir.y == 1 && newPos.y >= newRoomPos.y) ||
            //        (newRoomDir.y == -1 && newPos.y <= newRoomPos.y))
            //    {
            //        Debug.Log("end Transitioning");
            //        newPos.y = newRoomPos.y;
            //        isTransitioning = false;
            //    }
            //}
            //transform.position = newPos;
        }
        else if (isBeingDamaged)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageTimerLimit)
            {
                damageTimer = 0;
                isBeingDamaged = false;
                showDamageSprite = false;
            }
            else
            {
                if (damageTimer < damageTimerLimit * 0.25)
                    showDamageSprite = true;
                else if (damageTimer >= damageTimerLimit * 0.25 && damageTimer < damageTimerLimit * 0.5)
                    showDamageSprite = false;
                else
                    showDamageSprite = true;
            }
        }
        if (!weapon.GetComponent<WeaponScript>().isActiveAndEnabled)
        {
            if (Input.GetKey(KeyCode.L))
            {
                // Spawn sword and attack
                weapon.SetActive(true);
                weapon.GetComponent<WeaponScript>().Attack(attackTimerLimit);
                return;
            }
            TravelTo();
        }

        // Limits on velocity
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > 5)
            GetComponent<Rigidbody2D>().velocity *= 0.5f;
        else if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > 5)
            GetComponent<Rigidbody2D>().velocity *= 0.5f;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Creep")
        {
            isBeingDamaged = true;
            knockBack(other.collider.transform.position - transform.position, 4);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<PushBlock>() != null)
        {
            if (Input.GetKey(KeyCode.W))
            {
                other.gameObject.GetComponent<PushBlock>().pushedUp();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                other.gameObject.GetComponent<PushBlock>().pushedDown();
            }
            else if (Input.GetKey(KeyCode.A))
            {
                other.gameObject.GetComponent<PushBlock>().pushedLeft();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                other.gameObject.GetComponent<PushBlock>().pushedRight();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        m_health -= damage;
        isBeingDamaged = true;
        isAttacking = false;
        weapon.SetActive(false);

        if (m_health <= 0)
        {
            isAlive = false;
            Die();
        }
    }

    public void knockBack(Vector2 direction, float force)
    {
        GetComponent<Rigidbody2D>().AddForce(direction * force);
    }

    int getDamage()
    {
        return m_damage;
    }

    void TravelTo()
    {
        if (!isAttacking)
        {
            if (Input.GetKey(KeyCode.W))
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, m_speed);
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, -m_speed);
                transform.rotation = Quaternion.Euler(0, 0, 270);
            }

            if (Input.GetKey(KeyCode.A))
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-m_speed, GetComponent<Rigidbody2D>().velocity.y);
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(m_speed, GetComponent<Rigidbody2D>().velocity.y);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    void Die()
    {
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        isAlive = false;
        isBeingDamaged = false;
        isAttacking = false;
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
    }

    public void initiateTransitionToNewRoomAtPos(Vector3 newPos)
    {
        isTransitioning = true;
        //    newRoomPos = newPos;
        //    newRoomDir = new Vector2((newRoomPos.x - transform.position.x), (newRoomPos.y - transform.position.y)).normalized;
        transform.position = newPos;
    }

    public float getHeight()
    {
        Vector2 extents = gameObject.GetComponent<Collider2D>().bounds.extents;
        return extents.y * 2f;
    }
}