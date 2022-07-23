using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepDataObject : MonoBehaviour
{
    public Creep_Behaviour creepBehaviour;
    public float attackDistance;
    public bool isInHorizDirection = true;
    public bool preppedForRevive = false;

    bool isWeapon;
    int objectPoolIndex;
    public bool isAlive;
    public float m_speed, 
                attackTimer, attackTimerLimit, 
                takeDamageTimer, takeDamageTimerLimit;
    public int m_health, m_healthMax, m_damage;
    public GameObject boss = null;

    private Vector3 originalPos;
    private Quaternion originalRot;

    public void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    public void ActivateCreep()
    {
        getCreepGameObjectFromPool();
    }

    public void getCreepGameObjectFromPool()
    {
        if (ObjectPool.SharedInstance)
        {
            GameObject gO = ObjectPool.SharedInstance.GetPooledObject();
            if (gO != null)
            {
                gO.SetActive(true);
                transferDataToCreepObject(gO.GetComponent<CreepScript>());
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else
        {
            Debug.Log("There's no Object Pool!");
        }
    }

    public void releaseCreepGameObjectFromPool()
    {
        if (ObjectPool.SharedInstance)
        {
            ObjectPool.SharedInstance.ReleasePooledObjectAtIndex(objectPoolIndex);
            isAlive = false;
        }
        else
        {
            Debug.Log("There's no Object Pool!");
        }
    }

    public void refreshCreepData()
    {
        transform.position = originalPos;
        transform.rotation = originalRot;

        if (!isAlive)
        {
            isAlive = true;
            m_health = m_healthMax;
        }
        attackTimer = 0;
        takeDamageTimer = 0;
    }

    public void reviveCreep()
    {
        if (!isAlive)
        {
            isAlive = true;
            m_health = m_healthMax;
        }
        attackTimer = 0;
        takeDamageTimer = 0;
        getCreepGameObjectFromPool();
    }

    public void transferDataToCreepObject(CreepScript cs)
    {
        Debug.Log("transferDataToCreepObject");
        if (isAlive)
        {
            objectPoolIndex = cs.objectPoolIndex;

            cs.attackDistance = attackDistance;
            cs.isInHorizDirection = true;
            cs.gameObject.transform.position = transform.position;
            cs.gameObject.transform.rotation = transform.rotation;

            cs.isAlive = true;
            cs.m_speed = m_speed;
            cs.attackTimer = attackTimer;
            cs.attackTimerLimit = attackTimerLimit;
            cs.takeDamageTimer = takeDamageTimer;
            cs.takeDamageTimerLimit = takeDamageTimerLimit;
            cs.m_health = m_health;
            cs.m_healthMax = m_healthMax;
            cs.m_damage = m_damage;
            cs.ChangeBehavior(creepBehaviour);
            cs.sendBackDataOnRelease = transferDataFromCreepObject;
            cs.state = CreepScript.ENEMY_STATE.CHASING;
            if (boss != null)
                cs.Boss = boss;
            else
                cs.Boss = null;
        }
    }

    public void transferDataFromCreepObject(CreepScript cs, int h)
    {
        Debug.Log("transferDataFromCreepObject");
        attackDistance = cs.attackDistance;
        transform.position = cs.gameObject.transform.position;
        transform.rotation = cs.gameObject.transform.rotation;

        isAlive = cs.isAlive;
        if (isAlive)
        {
            m_speed = cs.m_speed;
            attackTimer = 0;
            takeDamageTimer = 0;
            m_health = h;
        }
        else
        {
            preppedForRevive = false;
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
