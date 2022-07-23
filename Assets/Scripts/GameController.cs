using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    delegate void creepDied(uint index);

    enum GAME_STATES {INTRO, PLAYING, ENDING_DEFEAT, ENDING_VICTORY, RESTART_GAME};
    GAME_STATES state;

    public BossScript boss;
    public GameObject m_BossCinematic;
    public PlayerScript player;
    public CameraScript cameraScript;
    public List<GameObject> creepList;

    public RoomScript currRoom;

    public float imageFadeRate;
    int currentFade = 255;

    public UnityEngine.UI.Image imageIntro, imageVictory, imageDefeat;

    public float introHangTime, endingHangTime;
    private float hungTime;

    // Start is called before the first frame update
    void Start()
    {
        state = GAME_STATES.INTRO;
        hungTime = introHangTime;
        //cameraScript.setNewLimitsUppDwnLftRht(currRoom.topMostPos, currRoom.botMostPos, currRoom.leftMostPos, currRoom.rightMostPos);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GAME_STATES.INTRO:
                hungTime -= Time.deltaTime;
                if (hungTime <= 0)
                {
                    fadeOutImage(imageIntro);

                    if (imageIntro.color.a <= 0)
                    {
                        //boss.gameObject.SetActive(true);
                        state = GAME_STATES.PLAYING;
                        player.freezeControls = false;
                        cameraScript.setNewLimitsUppDwnLftRht(currRoom.topMostPos, currRoom.botMostPos, currRoom.leftMostPos, currRoom.rightMostPos);
                    }
                }
                break;
            case GAME_STATES.PLAYING:

                if (player.m_health <= 0)
                {
                    state = GAME_STATES.ENDING_DEFEAT;
                    player.freezeControls = true;
                }
                else if (boss.m_health <= 0)
                {
                    state = GAME_STATES.ENDING_VICTORY;
                    m_BossCinematic.GetComponent<BossCinematic>().PlayDeathCinematic();
                    player.freezeControls = true;
                }
                break;
            case GAME_STATES.ENDING_DEFEAT:
                if (m_BossCinematic.GetComponent<BossCinematic>().AnimationFinished())
                    state = GAME_STATES.RESTART_GAME;

                //fadeInImage(imageDefeat);

                //if (imageDefeat.color.a == 255)
                //    state = GAME_STATES.PLAYING;
                break;
            case GAME_STATES.ENDING_VICTORY:

                //fadeInImage(imageVictory);

                if (m_BossCinematic.GetComponent<BossCinematic>().AnimationFinished())
                    state = GAME_STATES.RESTART_GAME;

                //if (imageVictory.color.a == 255)
                //    state = GAME_STATES.PLAYING;
                break;
            case GAME_STATES.RESTART_GAME:
                // Fade to black while putting everything back
                fadeInImage(imageVictory);
                break;
        }
    }

    private void fadeInImage(UnityEngine.UI.Image img)
    {
        float fade = imageFadeRate * Time.deltaTime;
        currentFade += (int) (fade);
        if (currentFade >= 255)
            currentFade = 255;
        img.color = new Color32(255, 255, 255, (byte)currentFade);
    }

    private void fadeOutImage(UnityEngine.UI.Image img)
    {
        float fade = imageFadeRate * Time.deltaTime;
        currentFade -= (int) (fade);
        if (currentFade <= 0)
            currentFade = 0;
        img.color = new Color32(255, 255, 255, (byte)currentFade);
    }

    public void CutSceneBegins()
    {

    }
}
