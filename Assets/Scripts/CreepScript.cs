using UnityEngine;

public class CreepScript : MonoBehaviour {

    public enum ENEMY_STATE { ATTACKING, DAMAGING, CHASING, TAKING_DAMAGE, REVIVING, DEAD}
    public ENEMY_STATE state;
    Factions.FACTION _faction;

    public int objectPoolIndex;
    public GameObject Player;
    public GameObject Boss;
    public Creep_Behaviour creepBehaviour;
    public float attackDistance;
    public bool isInHorizDirection = true;
    public CircleCollider2D attackRange;

    bool isWeapon, showDamageSprite;
    public bool isAttacking, isBeingDamaged, isAlive;
    public float m_speed, attackTimer, attackTimerLimit, damageTimer, damageTimerLimit, takeDamageTimer, takeDamageTimerLimit;
    public int m_health, m_healthMax, m_damage;
    public uint m_objectPoolIndex;
    bool movingX;

    public delegate void ReportDeath(CreepScript cs, int h);
    public ReportDeath sendBackDataOnRelease;

    public GameObject attackArea;
    public GameObject damageArea;

    //bool isWeapon, showDamageSprite;
    //private Vector3 prev_pos;
    //public Vector2 target;
    //Vector2 prevPos_V2;
    //public float distToTravel;

    //// CREEP BEAST MOVE PARAMS
    //public float startMarker = 0, endMarker = 0;
    //public bool isInHorizDirection = true;
    ///// ///////////////////////////////////

    // Use this for initialization
    void Start () {
        state = ENEMY_STATE.CHASING;
        isBeingDamaged = false;
        isAlive = true;
        isAttacking = false;

        if (Random.Range(0, 2) == 1)
            movingX = true;
        else
            movingX = false;
	}

    private void OnEnable()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        state = ENEMY_STATE.CHASING;
        isBeingDamaged = false;
        isAlive = true;
        isAttacking = false;

        if (Random.Range(0, 2) == 1)
            movingX = true;
        else
            movingX = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Die();
        }

        switch (state)
        {
            case ENEMY_STATE.CHASING:

                GetComponent<Rigidbody2D>().velocity = creepBehaviour.TravelTo(Player.transform.position, transform.position, isInHorizDirection);
                if (GetComponent<Rigidbody2D>().velocity.x == 0 && GetComponent<Rigidbody2D>().velocity.y == 0)
                {
                    isInHorizDirection = !isInHorizDirection;
                }
                checkFacing();
                break;
            case ENEMY_STATE.ATTACKING:

                if (attackTimer >= attackTimerLimit)
                {
                    attackTimer = 0;
                    damageArea.SetActive(true);
                    state = ENEMY_STATE.DAMAGING;
                }
                else
                {
                    attackTimer += Time.deltaTime;
                }
                break;
            case ENEMY_STATE.DAMAGING:

                if (damageTimer >= damageTimerLimit)
                {
                    damageTimer = 0;
                    damageArea.SetActive(false);
                    state = ENEMY_STATE.CHASING;
                }
                else
                {
                    damageTimer += Time.deltaTime;
                }
                break;
            case ENEMY_STATE.TAKING_DAMAGE:

                takeDamageTimer += Time.deltaTime;
                if (takeDamageTimer > takeDamageTimerLimit)
                {
                    state = ENEMY_STATE.CHASING;
                    takeDamageTimer = 0;
                    isBeingDamaged = false;
                    showDamageSprite = false;
                }
                else
                {
                    //if (damageTimer < damageTimerLimit * 0.25)
                    //    showDamageSprite = true;
                    //else if (damageTimer >= damageTimerLimit * 0.25 && damageTimer < damageTimerLimit * 0.5)
                    //    showDamageSprite = false;
                    //else
                    //    showDamageSprite = true;
                }
                break;
            case ENEMY_STATE.REVIVING:
                break;
            case ENEMY_STATE.DEAD:
                break;

        }

        // Limits on velocity
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > 10 || Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > 10)
        {
            Vector2 new_speed = GetComponent<Rigidbody2D>().velocity;

            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > 5)
                new_speed.x = Mathf.Sign(new_speed.x) * 5;
            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > 5)
                new_speed.y = Mathf.Sign(new_speed.y) * 5;
            GetComponent<Rigidbody2D>().velocity = new_speed;
        }
    }

    private void checkFacing()
    {
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y))
        {
            if(GetComponent<Rigidbody2D>().velocity.x > 0 && transform.rotation.eulerAngles.z != 90)
            {
                Quaternion q = Quaternion.identity;
                q.eulerAngles = new Vector3(0, 0, 270);
                transform.rotation = q;
            }
            else if (GetComponent<Rigidbody2D>().velocity.x < 0 && transform.rotation.eulerAngles.z != 270)
            {
                Quaternion q = Quaternion.identity;
                q.eulerAngles = new Vector3(0, 0, 90);
                transform.rotation = q;
            }
        }
        else if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) < Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y))
        {
            if (GetComponent<Rigidbody2D>().velocity.y > 0 && transform.rotation.eulerAngles.z != 0)
            {
                Quaternion q = Quaternion.identity;
                q.eulerAngles = new Vector3(0, 0, 0);
                transform.rotation = q;
            }
            else if (GetComponent<Rigidbody2D>().velocity.y < 0 && transform.rotation.eulerAngles.z != 180)
            {
                Quaternion q = Quaternion.identity;
                q.eulerAngles = new Vector3(0, 0, 180);
                transform.rotation = q;
            }
        }
    }

    public void takeDamage(int damage)
    {
        if (state != ENEMY_STATE.DEAD && state != ENEMY_STATE.TAKING_DAMAGE)
        {
            state = ENEMY_STATE.TAKING_DAMAGE;
            m_health -= damage;
            isBeingDamaged = true;

            if (m_health <= 0)
            {
                Die();
            }
            else
                knockBack(transform.position - transform.position, 4);
        }
    }

    void knockBack(Vector2 direction, float force)
    {
        Vector2 dir = transform.position - Player.transform.position;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            dir = new Vector2(Mathf.Sign(dir.x) * m_speed * force, 0);
        }
        else // (Mathf.Abs(dir.x) <= Mathf.Abs(dir.y))
        {
            dir = new Vector2(0, Mathf.Sign(dir.y) * m_speed * force);
        }

        GetComponent<Rigidbody2D>().velocity = dir;
    }

    public void beginPreAttack()
    {
        state = ENEMY_STATE.ATTACKING;
    }

    public int getDamage() { return m_damage; }


    void Die()
    {
        Debug.Log("Enemy died");
        state = ENEMY_STATE.DEAD;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        isAlive = false;
        isBeingDamaged = false;
        isAttacking = false;
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        ReleaseToObjectPool();
        if (Boss != null)
        {
            Boss.GetComponent<BossScript>().decrementCreepAndCheckIfAnyAlive();
        }
    }

    public void BeginRevive()
    {
        state = ENEMY_STATE.REVIVING;
        isAlive = true;
    }

    public void Revive()
    {
        isAlive = true;
        m_health = m_healthMax;
        state = ENEMY_STATE.CHASING;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        transform.rotation = Quaternion.identity;
    }

    public void ChangeBehavior(Creep_Behaviour newBehavior)
    {
        creepBehaviour = newBehavior;

        SetAttackAreaCollider(creepBehaviour.GetAttackArea());
        attackArea.GetComponent<AttackArea>().SetBeginAttackDelegate(creepBehaviour.BeginAttackDefinition);
        SetDamageAreaCollider(creepBehaviour.GetDamageArea());
    }

    public void SetAttackAreaCollider(GameObject attArea)
    {
        attackArea.transform.localScale = attArea.transform.localScale;
        attackArea.transform.localPosition = attArea.transform.localPosition;
    }

    public void SetDamageAreaCollider(GameObject dmgArea)
    {
        damageArea.transform.localScale = dmgArea.transform.localScale;
        damageArea.transform.localPosition = dmgArea.transform.localPosition;
    }

    public void ReleaseToObjectPool()
    {
        Debug.Log("Creep releasing data at " + m_health + " hp.");
        sendBackDataOnRelease(gameObject.GetComponent<CreepScript>(), m_health);
        gameObject.SetActive(false);
    }
}
