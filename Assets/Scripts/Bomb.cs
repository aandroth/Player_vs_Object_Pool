using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    const int MILLI_TO_SECONDS_MULTIPLIER = 100;

    public float m_BlastRadius = 5.0f, m_BlastForce = 5.0f, m_BlastDamage = 100.0f;
    public float m_TimeTillDetonate = 5.0f, timeTillThrowable = 0.5f;
    public float m_TimeFromExplosionStartToDamageDealt = 2.5f;
    public float m_TimeFromDamageDealtToExplosionOver = 2.5f;


    public GameObject dialogueBox;
    public Text dialogueText;
    public string textString;
    public GameObject m_bombPlant;
    public GameObject m_redBomb;
    public GameObject m_explosion;
    public float m_heldOffsetX = 0;
    public float m_heldOffsetY = 0;

    public GameObject m_player;

    public delegate void ReportDestroyed();
    public ReportDestroyed reportDestroyed = null;

    public enum BOMB_STATE { IN_PLANT, HELD, READYING_EXPLODING, EXPLODING, FINISHING }
    public BOMB_STATE m_state;

    public void Start()
    {
        m_state = BOMB_STATE.IN_PLANT;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case BOMB_STATE.IN_PLANT:
                // Bomb does nothing
                break;
            case BOMB_STATE.HELD:
                timeTillThrowable -= Time.deltaTime;
                if (timeTillThrowable <= 0)
                {
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        transform.SetParent(null);
                        Rigidbody2D rigid = gameObject.AddComponent<Rigidbody2D>();
                        GetComponent<Rigidbody2D>().freezeRotation = enabled;
                        rigid.gravityScale = 0;
                        Vector3 v = m_player.transform.right;
                        GetComponent<BoxCollider2D>().enabled = true;
                        GetComponent<Rigidbody2D>().AddForce(v * 500);
                        StartCoroutine(Thrown());
                        m_state = BOMB_STATE.READYING_EXPLODING;
                        GetComponent<Animator>().Play("Bomb");
                    }
                }
                break;
            case BOMB_STATE.READYING_EXPLODING:
                m_TimeTillDetonate -= Time.deltaTime;
                if (m_TimeTillDetonate <= 0)
                {
                    m_state = BOMB_STATE.EXPLODING;
                    gameObject.GetComponent<Animator>().enabled = false;
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    m_redBomb.SetActive(false);
                    m_explosion.SetActive(true);
                }
                break;
            case BOMB_STATE.EXPLODING:
                m_TimeFromExplosionStartToDamageDealt -= Time.deltaTime;
                if (m_TimeFromExplosionStartToDamageDealt <= 0)
                {
                    m_state = BOMB_STATE.FINISHING;
                    ExplodeWithForceAndDamage();
                }
                break;
            case BOMB_STATE.FINISHING:
                m_TimeFromDamageDealtToExplosionOver -= Time.deltaTime;
                if (m_TimeFromDamageDealtToExplosionOver <= 0)
                {
                    if (reportDestroyed != null)
                        reportDestroyed.Invoke();
                    Destroy(this);
                }
                break;
        }
    }

    public void PickedUp()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        transform.position = m_player.transform.position + new Vector3(m_heldOffsetX, m_heldOffsetY, 0);
        this.transform.SetParent(m_player.transform);
        m_state = BOMB_STATE.HELD;
    }

    public IEnumerator Thrown()
    {
        yield return new WaitForSeconds(0.25f);
        m_player.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ExplodeWithForceAndDamage()
    {
        Collider2D[] objectsHit = Physics2D.OverlapCircleAll(transform.position, m_BlastRadius);

        for (int ii = 0; ii < objectsHit.Length; ++ii)
        {
            if (objectsHit[ii].GetComponent<IDamageable>() == null)
                continue;

            Vector2 forceDirection = gameObject.transform.position - objectsHit[ii].transform.position;

            // Give each go force and damage{
            objectsHit[ii].GetComponent<Rigidbody2D>().AddForce(m_BlastForce * forceDirection);
            objectsHit[ii].GetComponent<IDamageable>().TakeDamage((int)m_BlastDamage);
        }

    }
}
