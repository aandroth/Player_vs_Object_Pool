﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCinematic : MonoBehaviour
{
    public GameObject boss = null;

    private enum BOSS_CINEMATIC_STATE { ENTERING, DYING}
    private BOSS_CINEMATIC_STATE state = BOSS_CINEMATIC_STATE.ENTERING;

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BOSS_CINEMATIC_STATE.ENTERING:
                if(AnimationFinished())
                {
                    boss.transform.position = transform.position;
                    boss.SetActive(true);
                    gameObject.SetActive(false);
                }
                break;
            case BOSS_CINEMATIC_STATE.DYING:
                if(AnimationFinished())
                {
                    gameObject.SetActive(false);
                }
                break;
        }
    }

    public bool AnimationFinished()
    {
        return (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !GetComponent<Animator>().IsInTransition(0));
    }

    public void PlayDeathCinematic()
    {
        gameObject.GetComponent<Animator>().Play("Boss_Death Animation");
        transform.position = boss.transform.position;
        state = BOSS_CINEMATIC_STATE.DYING;
    }
}
