using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossState {STARTING,  WAITING, REVIVING_CREEPS, REVIVING, EXITING_LEFT, EXITING_RIGHT, DYING};

public class BossScript : MonoBehaviour {

    public BossState state;
    public GameObject m_BossCinematic;
    public GameObject C0, C1, C2;
    public ObjectPool objectPool;
    public List<Creep_Behaviour> creepBehaviorList;
    public List<CreepDataObject> creepDataList;

    public bool isAttacking, showDamageSprite, isBeingDamaged, isAlive, isWeapon, isPlayerAlly;

    public float attackTimer, attackTimerLimit;
    public float m_speed;
    public int m_health, m_healthMax, m_damage, m_living_creep_count, m_bossPhase = 0;
    public int healthFractionToDetermineAddingCreeps, number_of_creeps_deployed;
    
    bool directionIsUp, exitLeft = true;
    float distanceAlongCurve;
    Vector2 startPoint, middlePoint, endPoint;


    public Transform leftWaitPt, rightWaitPt, m_TransformToStartDeathAnim;
    public Slider health_bar;

    float damageTimer, damageTimerLimit;
    int creep_target_idx;
    
    private float m_MoveToDeathAnimStart_CurrTime = 0, m_MoveToDeathAnimStart_Time = 2;
    private Vector3 m_PosWhenKilled = Vector3.zero;

    // Use this for initialization
    void Start () {
        state = BossState.STARTING;
        creepDataList[0].boss = gameObject;
        m_living_creep_count = 1;
        number_of_creeps_deployed = 1;
        healthFractionToDetermineAddingCreeps = (int)(m_health / 5.0f);
    }
	
	// Update is called once per frame
	void Update () {

        switch(state)
        {
            case BossState.STARTING:
                decrementCreepAndCheckIfAnyAlive();
                break;
            case BossState.WAITING:
                break;
            case BossState.REVIVING_CREEPS:
                Travel();
                break;
            case BossState.EXITING_LEFT:
                Travel();
                break;
            case BossState.EXITING_RIGHT:
                Travel();
                break;
            case BossState.DYING:
                if (MoveToDeathAnimStart())
                {
                    m_BossCinematic.SetActive(true);
                    m_BossCinematic.GetComponent<Animator>().Play("Boss_Death Animation");
                    gameObject.SetActive(false);
                }
                break;
        }

        if (isBeingDamaged)
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
                if (damageTimer < damageTimerLimit * 0.5)
                    showDamageSprite = true;
                else if (damageTimer >= damageTimerLimit * 0.5 && damageTimer < damageTimerLimit * 0.75)
                    showDamageSprite = false;
                else
                    showDamageSprite = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon" && !isBeingDamaged)
        {
            takeDamage(collision.GetComponent<WeaponScript>().getDamage());
        }
    }

    void Travel()
    {
        distanceAlongCurve += m_speed * Time.deltaTime;

        // Completed current curve
        if (distanceAlongCurve > 1)
        {
            if (state == BossState.REVIVING_CREEPS && m_living_creep_count < number_of_creeps_deployed)
            {
                ++m_living_creep_count;
                int randBehavior = UnityEngine.Random.Range(0, 3);

                creepDataList[creep_target_idx].GetComponent<CreepDataObject>().creepBehaviour = creepBehaviorList[randBehavior];
                creepDataList[creep_target_idx].GetComponent<CreepDataObject>().preppedForRevive = true;
            }

            distanceAlongCurve = 0;
            directionIsUp = !directionIsUp;
            setNextPoints();
        }
        else // Still moving along curve
        {
            Vector2 L0 = Vector2.Lerp(startPoint, middlePoint, distanceAlongCurve);
            Vector2 L1 = Vector2.Lerp(middlePoint, endPoint, distanceAlongCurve);

            transform.position = Vector2.Lerp(L0, L1, distanceAlongCurve);
        }
    }

    void setNextPoints()
    {
        if (state == BossState.EXITING_LEFT || state == BossState.EXITING_RIGHT)
        {
            state = BossState.WAITING;
            if (m_living_creep_count == number_of_creeps_deployed)
                for (int ii = 0; ii < number_of_creeps_deployed; ++ii)
                {
                    creepDataList[ii].GetComponent<CreepDataObject>().reviveCreep();
                }
            return;
        }

        startPoint = transform.position;
        endPoint = closestDeadCreep();
        middlePoint = new Vector2(Random.Range(Mathf.Min(startPoint.x, endPoint.x), Mathf.Max(startPoint.x, endPoint.x)), 
                                  Random.Range(Mathf.Min(startPoint.y, endPoint.y), Mathf.Max(startPoint.y, endPoint.y)));
        if (!directionIsUp)
            middlePoint.y -= (middlePoint.y + startPoint.y)/2.0f;
        else
            middlePoint.y += (middlePoint.y + startPoint.y)/2.0f;

        //if()
        //    state = BossState.TRAVELING;

        C0.transform.position = startPoint;
        C1.transform.position = middlePoint;
        C2.transform.position = endPoint;
    }

    public void decrementCreepAndCheckIfAnyAlive()
    {
        --m_living_creep_count;

        if (m_living_creep_count <= 0)
        {
            state = BossState.REVIVING_CREEPS;
            startPoint = transform.position;
            setNextPoints();
        }
    }

    private Vector2 closestDeadCreep()
    {
        float shortest_distance = float.PositiveInfinity;
        Vector2 retVec = new Vector2(0, 0);

        for(int ii=0; ii<number_of_creeps_deployed; ++ii)
        {
            if(!creepDataList[ii].GetComponent<CreepDataObject>().preppedForRevive && 
                shortest_distance > Vector2.Distance(transform.position, creepDataList[ii].transform.position))
            {
                shortest_distance = Vector2.Distance(transform.position, creepDataList[ii].transform.position);
                retVec = creepDataList[ii].transform.position;
                creep_target_idx = ii;
            }
        }

        // If no creeps are dead go to one of the waiting points
        if (float.IsInfinity(shortest_distance))
        {
            if (exitLeft)
            {
                retVec = leftWaitPt.position;
                state = BossState.EXITING_LEFT;
            }
            else
            {
                retVec = rightWaitPt.position;
                state = BossState.EXITING_RIGHT;
            }
            exitLeft = !exitLeft;
        }
        return retVec;
    }

    bool AllCreepsAreDead()
    {
        for(int ii=0; ii< creepDataList.Count; ++ii)
        {
            if (creepDataList[ii].GetComponent<CreepDataObject>().isAlive)
                return false;
        }
        return true;
    }

    bool AllCreepsAreAlive()
    {
        for (int ii = 0; ii < number_of_creeps_deployed; ++ii)
        {
            if (!creepDataList[ii].GetComponent<CreepScript>().isAlive)
                return false;
        }
        return true;
    }

    public void takeDamage(int damage)
    {
        if (!isBeingDamaged)
        {
            m_health -= damage;
            isBeingDamaged = true;
            UpdateHealthBar();

            if (m_health <= 0)
            {
                Die();
                return;
            }

            if (m_bossPhase == 0 && m_health < m_healthMax - healthFractionToDetermineAddingCreeps)
            {
                ++number_of_creeps_deployed;
                m_bossPhase = 1;
            }
            else if (m_bossPhase == 1 && m_health < m_healthMax - healthFractionToDetermineAddingCreeps*2)
            {
                ++number_of_creeps_deployed;
                m_bossPhase = 2;
            }
            else if (m_bossPhase == 2 && m_health < m_healthMax - healthFractionToDetermineAddingCreeps*4)
            {
                ++number_of_creeps_deployed;
                ++number_of_creeps_deployed;
                m_bossPhase = 3;
            }
        }
    }

    void Die()
    {
        state = BossState.DYING;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        isAlive = false;
        isBeingDamaged = false;
        isAttacking = false;
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        m_PosWhenKilled = transform.position;
    }

    private bool MoveToDeathAnimStart()
    {
        if (m_MoveToDeathAnimStart_CurrTime < m_MoveToDeathAnimStart_Time)
        {
            m_MoveToDeathAnimStart_CurrTime += Time.deltaTime;

            if (m_MoveToDeathAnimStart_CurrTime >= m_MoveToDeathAnimStart_Time)
                m_MoveToDeathAnimStart_CurrTime = m_MoveToDeathAnimStart_Time;
            transform.position = Vector3.Lerp(m_PosWhenKilled, m_TransformToStartDeathAnim.position, m_MoveToDeathAnimStart_CurrTime / m_MoveToDeathAnimStart_Time);
        }
        return m_MoveToDeathAnimStart_CurrTime >= m_MoveToDeathAnimStart_Time;
    }

    void UpdateHealthBar()
    {
        health_bar.value = m_health / m_healthMax;
    }
}
