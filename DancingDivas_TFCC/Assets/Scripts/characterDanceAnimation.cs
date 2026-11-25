using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

//HANDLES CHARACTER DANCE ANIMATION
public class characterDanceAnimation : MonoBehaviour
{
    [Header("Player 1")]
    public List<Sprite> player1DanceMoves;
    public Image player1Sprite;
    public bool player1InCoroutine = false;
    public Vector3 player1OriginalPosition;
    public Vector3 player1OriginalScale;
    public RectTransform play1RT;

    [Header("Player 2")]
    public List<Sprite> player2DanceMoves;
    public Image player2Sprite;
    public bool player2InCoroutine = false;
    public Vector3 player2OriginalPosition;
    public Vector3 player2OriginalScale;
    public RectTransform play2RT;

    [Header("Bounce/Dance Stuff")]
    public float squashDuration = 0.1f;
    public float bounceDuration = 0.2f;
    public float returnDuration = 0.3f;
    public float widenAmount = 1.1f;
    public float shrinkAmount = 0.85f;
    public float bounceAmount = 30f;

    private void Start()
    {
        //start off in idle bounce on beat but for NOW get a random position-========================

        //initialize not in coroutine
        player1InCoroutine = false;
        player2InCoroutine = false;

        //get the original scale and position for each of the players
        //player 1
        play1RT = player1Sprite.GetComponent<RectTransform>();
        player1OriginalPosition = play1RT.anchoredPosition;
        player1OriginalScale=play1RT.localScale;

        //player 2
        play2RT = player2Sprite.GetComponent<RectTransform>();
        player2OriginalPosition = play2RT.anchoredPosition;
        player2OriginalScale = play2RT.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //temp input for wherever the input is actually being called

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //player 1 going
            dance(1);
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            //playerr 2 going
            dance(2);
        }

    }

    public void dance(int playerNum)
    {
        print("player " +  playerNum + " is dancing now");
        if (playerNum == 1)
        {
            if (player1InCoroutine)
            {
                play1RT.anchoredPosition = player1OriginalPosition;
                play1RT.localScale = player1OriginalScale;
                StopCoroutine(danceMotion(1, player1Sprite)); //stop if in it already
            }
            StartCoroutine(danceMotion(1, player1Sprite));
            Sprite currentSprite = player1Sprite.sprite;
            Sprite newMove = getDanceMove(player1DanceMoves, currentSprite);
            player1Sprite.sprite = newMove;
        }
        else if (playerNum == 2)
        {
            if (player2InCoroutine)
            {
                play2RT.anchoredPosition = player2OriginalPosition;
                play2RT.localScale = player2OriginalScale;
                StopCoroutine(danceMotion(2, player2Sprite)); //stop if in it already
            }
            StartCoroutine(danceMotion(2, player2Sprite));
            Sprite currentSprite = player2Sprite.sprite;
            Sprite newMove = getDanceMove(player2DanceMoves, currentSprite);
            player2Sprite.sprite = newMove;
        }
        else
        {
            Debug.LogError("Misinput in dance function");
        }
    }

    public Sprite getDanceMove(List<Sprite> danceOptions, Sprite currentSprite)
    {
        Sprite newMove = null;

        //return if list is empty
        if (danceOptions == null)
        {
            Debug.LogWarning("Returning cause dance options is empty");
            return null;
        }

        while (newMove == null || newMove == currentSprite)
        {
            int randDanceOption = Random.Range(0,danceOptions.Count);
            newMove = danceOptions[randDanceOption];
        }
        return newMove;

    }

    IEnumerator danceMotion(int playerNum, Image player)
    {
        RectTransform playerTransform = null;
        Vector3 originalPosition = Vector3.zero;
        Vector3 originalScale = Vector3.zero;

        if (playerNum == 1)
        {
            player1InCoroutine = true;
            playerTransform = play1RT;
            originalPosition = player1OriginalPosition;
            originalScale = player1OriginalScale;
            
        }
        else if (playerNum == 2)
        {
            player2InCoroutine = true;
            playerTransform = play2RT;
            originalPosition = player2OriginalPosition;
            originalScale = player2OriginalScale;
        }
        else
        {
            Debug.LogError("Misinput in dance motion coroutine");
            yield return null;
        }

        Vector3 squashScale = new Vector3(originalScale.x * widenAmount,
        originalScale.y * shrinkAmount, originalScale.z);

        //initialize timer
        float timer = 0f;
        //shrink down
        while (timer < squashDuration)
        {
            timer += Time.deltaTime;
            playerTransform.localScale = Vector3.Lerp(originalScale, squashScale, timer/squashDuration);
            yield return null;
        }

        //or lerp back up, smoother
        Vector3 bouncePosition = new Vector3(originalPosition.x,originalPosition.y+bounceAmount, originalPosition.z);
        
        //reset timer
        timer = 0f;
        //now we bounce up
        while (timer < bounceDuration)
        {
            timer += Time.deltaTime;
            playerTransform.anchoredPosition = Vector3.Lerp(originalPosition, bouncePosition, timer / bounceDuration);
            yield return null;
        }

        //reset timer
        timer = 0f;
        //now we return back to og position
        while (timer < returnDuration)
        {
            timer += Time.deltaTime;
            playerTransform.anchoredPosition = Vector3.Lerp(bouncePosition, originalPosition, timer / returnDuration);
            playerTransform.localScale = Vector3.Lerp(squashScale, originalScale, timer / returnDuration);
            yield return null;
        }

        //no longer in bool
        if (playerNum == 1) player1InCoroutine = false;
        else player2InCoroutine = false;

    }
}
