using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//HANDLES PLAYER LIFE AND STAMINA
public class statusSystem : MonoBehaviour
{

    [Header("Player 1 Stats")]
    public int player1Lives;
    public int player1Stamina = 100;

    [Header("Player 2 Stats")]
    public int player2Lives;
    public int player2Stamina = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

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
                print("PLaYER 1 LOSE"); //player 1 LOSE BY LOSS OF HEaRTS =============================================
                turnManager.instance.turn = turnManager.turns.end;
                player1Dead = true;
            }
            else if (player2Lives == 0)
            {
                print("PLAYER 2 LOSE"); //player 2 LOSE BY LOSS OF HEaRTS =============================================
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
