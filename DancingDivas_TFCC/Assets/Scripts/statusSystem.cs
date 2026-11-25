using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//HANDLES PLAYER LIFE AND STAMINA
public class statusSystem : MonoBehaviour
{

    [Header("Player 1 Stats")]
    public int player1Lives = 3;
    public int player1Stamina = 50;
    public List<GameObject> player1Hearts;
    public int player1HeartsIndex=0;
    public bool player1Dead = false;

    [Header("Player 2 Stats")]
    public int player2Lives = 3;
    public int player2Stamina = 50;
    public List<GameObject> player2Hearts;
    public int player2HeartsIndex=0;
    public bool player2Dead = false;

    [Header("Heart Sprites/Images")]
    public Sprite fullHeart;
    public Sprite brokenHeart;

    [Header("Stamina Slider Stuff")]
    public Slider player1StaminaSlider;
    public Slider player2StaminaSlider;

    [Header("Sounds")]
    public AudioClip loseHeartSound;

    public void loseHeart(string player)
    {
        if (!player1Dead && !player2Dead)
        {
            //play the sound
            audioManager.instance.fx.PlayOneShot(loseHeartSound);

            if (player == "player1")
            {
                player1Lives--; //subtract the total num of lives
                updateHeartImg(player1Hearts, player1HeartsIndex);
                //update index now, if not already at count
                if (player1HeartsIndex != player1Hearts.Count - 1)
                {
                    player1HeartsIndex++;
                }
            }
            else if (player == "player2")
            {
                player2Lives--; //subtract the total num of lives
                updateHeartImg(player2Hearts, player2HeartsIndex);
                //update index now, if not already at count
                if (player2HeartsIndex != player2Hearts.Count - 1)
                {
                    player2HeartsIndex++;
                }
            }
            else
            {
                Debug.LogError("You put in something wrong in the loseHeart function");
            }

            if (player1Lives == 0)
            {
                print("PLaYER 1 LOSE BY LIVES"); //player 1 LOSE BY LOSS OF HEaRTS =============================================
                turnManager.instance.turn = turnManager.turns.end;
                player1Dead = true;
            }
            else if (player2Lives == 0)
            {
                print("PLAYER 2 LOSE BY STAMINA"); //player 2 LOSE BY LOSS OF HEaRTS =============================================
                turnManager.instance.turn = turnManager.turns.end;
                player2Dead = true;
            }
        }
        else
        {
            if (player1Lives == 0)
            {
                Debug.LogWarning("Not going bc player 1 is dead");
            }
            else if (player2Lives == 0)
            {
                Debug.LogWarning("Not going bc player 2 is dead");
            }
        }
    }

    public void loseStamina(string player)
    {
        if (!player1Dead && !player2Dead)
        {
            //CHAR THIS IS WHERE SOUND WOULD PLAY IF WE ARE PLAYING ONE WHEN WE DANCE================

            if (player == "player1")
            {
                player1StaminaSlider.value -= 1;
                player1Stamina -= 1;
                //determine if slider is visible ** NOT USING SINCE IT WORKS WITHOUT THIS
                //player1SliderFill.enabled = player1Stamina > 0;

            }
            else if (player == "player2")
            {
                player2StaminaSlider.value -= 1;
                player2Stamina -= 1;
                //determine if slider is visible ** NOT USING SINCE IT WORKS WITHOUT THIS
                //player2SliderFill.enabled = player2Stamina > 0;
            }
            else
            {
                Debug.LogError("You put in something wrong in the loseStamina function");
            }

            if (player1Stamina == 0)
            {
                print("PLAYER 1 LOSE BY STAMINA"); //player 1 LOSE BY LOSS OF HEaRTS =============================================
                turnManager.instance.turn = turnManager.turns.end;
                player1Dead = true;
            }
            else if (player2Stamina == 0)
            {
                print("PLAYER 2 LOSE BY STAMINA"); //player 2 LOSE BY LOSS OF HEaRTS =============================================
                turnManager.instance.turn = turnManager.turns.end;
                player2Dead = true;
            }
        }
        else
        {
            if (player1Stamina == 0)
            {
                Debug.LogWarning("Not going bc player 1 is dead");
            }
            else if (player2Stamina == 0)
            {
                Debug.LogWarning("Not going bc player 2 is dead");
            }
        }
    }

    public void updateHeartImg(List<GameObject> heartList, int heartListIndex)
    {

        //basically just grab whatever heart the index is on and change it, then update index
        Image heartImg = heartList[heartListIndex].GetComponent<Image>();
        Animator heartAnim = heartList[heartListIndex].GetComponent<Animator>();

        //change the img
        heartImg.sprite = brokenHeart;

        //go break animation
        heartAnim.SetTrigger("break");
    }
}
