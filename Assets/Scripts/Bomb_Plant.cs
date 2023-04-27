using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Plant : Interactable, I_RoomItems
{
    public Bomb bombSeedPrefab;
    public Bomb bombSeedInstatiated;
    public bool m_hasBombseed = false, m_bombseedIsActive = false;
    public GameObject m_carrier;
    public new void Update()
    {
        if (m_hasBombseed && Input.GetKeyUp(KeyCode.Space) && playerIsInRange)
        {
            m_hasBombseed = false;
            bombSeedInstatiated.PickedUp();
            m_bombseedIsActive = true;
            bombSeedInstatiated.reportDestroyed = BombseedIsDestroyed;
        }
    }

    public void RoomReset()
    {
        if (!m_hasBombseed && !m_bombseedIsActive)
        {
            bombSeedInstatiated = Instantiate(bombSeedPrefab, transform.position, Quaternion.identity);
            bombSeedInstatiated.m_player = m_carrier;
            m_hasBombseed = true;
        }
    }

    public void BombseedIsDestroyed()
    {
        m_bombseedIsActive = false;
    }
}
