using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour, IDamageable
{
    public bool isAttacking, isTransitioning, showDamageSprite, isBeingDamaged, isAlive, isWeapon, isPlayerAlly, onCoolDown, freezeControls;

    public float attackTimer, attackTimerLimit, 
                damageTimer, damageTimerLimit, 
                swordCoolDownTimer, swordCoolDownTimerLimit,
                roomTransitionTimer, roomTransitionTimeLimit;
    public int m_speed, m_damage;

    public GameObject weapon;
    public GameObject playerSprite;

    private Vector3 prev_pos;
    public Vector2 newRoomPos, newRoomDir;

    public int m_Health { get; set; }

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
                PlayAttackAnimation();
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
        m_Health -= damage;
        isBeingDamaged = true;
        isAttacking = false;
        weapon.SetActive(false);

        if (m_Health <= 0)
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
            }
            else if (Input.GetKey(KeyCode.S))
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, -m_speed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-m_speed, GetComponent<Rigidbody2D>().velocity.y);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(m_speed, GetComponent<Rigidbody2D>().velocity.y);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                playerSprite.GetComponent<Animator>().Play("PlayerWalkUp");
                weapon.transform.eulerAngles = (new Vector3(0, 0, 90));
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                playerSprite.GetComponent<Animator>().Play("PlayerWalkDown");
                weapon.transform.eulerAngles = (new Vector3(0, 0, 270));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                playerSprite.GetComponent<Animator>().Play("PlayerWalkSide");
                weapon.transform.eulerAngles = (new Vector3(0, 0, 0));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                playerSprite.GetComponent<Animator>().Play("PlayerWalkSide");
                weapon.transform.eulerAngles = (new Vector3(0, 0, 0));
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
                    playerSprite.GetComponent<Animator>().Play("IdleUp");
                else
                    PlayWalkAnimAfterKeyUp();
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D))
                    playerSprite.GetComponent<Animator>().Play("IdleDown");
                else
                    PlayWalkAnimAfterKeyUp();
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
                    playerSprite.GetComponent<Animator>().Play("IdleSide");
                else
                    PlayWalkAnimAfterKeyUp();
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
                    playerSprite.GetComponent<Animator>().Play("IdleSide");
                else
                    PlayWalkAnimAfterKeyUp();
            }
        }
    }

    public void PlayWalkAnimAfterKeyUp()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            playerSprite.GetComponent<Animator>().Play("PlayerWalkDown");
            weapon.transform.eulerAngles = (new Vector3(0, 0, 270));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            playerSprite.GetComponent<Animator>().Play("PlayerWalkSide");
            weapon.transform.eulerAngles = (new Vector3(0, 0, 0));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            playerSprite.GetComponent<Animator>().Play("PlayerWalkSide");
            weapon.transform.eulerAngles = (new Vector3(0, 0, 0));
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            playerSprite.GetComponent<Animator>().Play("PlayerWalkUp");
            weapon.transform.eulerAngles = (new Vector3(0, 0, 90));
        }
    }

    public void PlayAttackAnimation()
    {
        switch (weapon.transform.rotation.eulerAngles.z)
        {
            case 0:
            case 180:
            case 360:
                playerSprite.GetComponent<Animator>().Play("AttackSideAnim");
                break;
            case 90:
                playerSprite.GetComponent<Animator>().Play("AttackUpAnim");
                break;
            case 270:
                playerSprite.GetComponent<Animator>().Play("AttackDownAnim");
                break;
        }
    }

    void Die()
    {
        Debug.Log("<color=red>[PlayerScript]</color> Die");
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        isAlive = false;
        isBeingDamaged = false;
        isAttacking = false;
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

        Debug.Log("<color=red>[PlayerScript]</color> Die: reloading game");
        SceneManager.LoadScene(0);
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