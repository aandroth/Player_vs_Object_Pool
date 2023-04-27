using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;


public class Boulder : MonoBehaviour, IDamageable
{
    public GameObject rubbleSprite;
    public int m_EditorHealth = 100;
    public int m_Health { get; set; }

    public void Start()
    {
        m_Health = m_EditorHealth;
    }

    void IDamageable.TakeDamage(int damage)
    {
        m_Health -= damage;

        if(m_Health <= 0)
        {
            Destroyed();
        }
    }

    public void Destroyed()
    {
        var colliderArray = GetComponents<BoxCollider2D>();
        foreach (var boxCollider2d in colliderArray)
            boxCollider2d.enabled = false;
        rubbleSprite.SetActive(true);
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
