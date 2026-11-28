using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

//CHARS WHOLE RHYTHM SCRIPT
public class rhythmSystem : MonoBehaviour
{




    //Beat and Meter Settings
    [Header("BPM Setting")]

    public float bpm = 120f;
    private float beatInterval;
    [SerializeField] private int meterNumerator = 4; // Number of beats in a measure
    [SerializeField] private int meterDenominator = 4; // Note value that represents one beat

    [SerializeField] private float recordInputWindow = 0.2f; // Time window for record beat

    //private List<bool> beatsMap;
    [SerializeField] private int currentBeatIndex = 0;

    //private bool[] recordingBeats = new bool[8];
    private bool[] targetBeatsMap = new bool[8];
    private bool[] replayBeatsMap = new bool[8];

    [SerializeField] private bool[] player1BeatsMap = new bool[8];
    [SerializeField] private bool[] player2BeatsMap = new bool[8];

    [SerializeField] private SpriteRenderer[] player1BeatSprites;
    [SerializeField] private SpriteRenderer[] player2BeatSprites;



    private float beatTimer = 0f;
    private bool pendingFirstBeat;
    [SerializeField] private float successRecordTimeRange = 0.15f; // Time range for successful beat recording
    [SerializeField] private bool[] recordAttempted = new bool[8];

    [SerializeField] private float successRespondTimeRange = 0.15f; // Time range for successful beat replay
    [SerializeField] private bool[] respondAttempted = new bool[8];



    [Header("Beat Indicator")]
    [SerializeField] private RectTransform beatBar;
    [SerializeField] private RectTransform beatIndicator;
    



    //Audio
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip beatTic;
    [SerializeField] private AudioClip beatTac;
    [SerializeField] private AudioClip beatReplay;
    [SerializeField] private AudioClip successRespondSound;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //beatsMap = new List<bool>();
        beatInterval = (60f / bpm)/2;

        ClearBeatsMap(player1BeatsMap);

        currentBeatIndex = -1; // Start at -1 so that the first beat is 0 after the first increment
        BeatsCounting(); // Increment to the first beat


    }

    // Update is called once per frame
    void Update()
    {
        ShowBeatsMap(player1BeatsMap, player1BeatSprites);
        ShowBeatsMap(player2BeatsMap, player2BeatSprites);

        //Timer every beat
        beatTimer += Time.deltaTime;

        //Timer's times up to next beat
        while (beatTimer >= beatInterval)
        {
            //reset timer
            beatTimer -= beatInterval;

            //update the burrent beat index (+1 everytime, back to 0 if end)
            BeatsCounting();


            //if is the first beat, clear the map for the next round
            if (currentBeatIndex == 0)
            {

                turnManager.instance.NextTurn();
            }

            var currentTurn = turnManager.instance.turn;
            if(currentTurn == turnManager.turns.player1Respond || currentTurn == turnManager.turns.player2Respond)
            {
                ReplayBeatsMap(targetBeatsMap);
            }


        }
        UpdateBeatIndicator();



    }





    private void BeatsCounting()
    {
        currentBeatIndex = (currentBeatIndex + 1) % (meterNumerator * 2);

        if (currentBeatIndex == 0)
        {
            audioSource.PlayOneShot(beatTic);
        }
        else if (currentBeatIndex % 2 == 0)
        {
            audioSource.PlayOneShot(beatTac);
        }

    }

    private void UpdateBeatIndicator()
    {
        //betTimer / beatInterval gives us the progress within the current beat (0 to 1)

        float progress = (currentBeatIndex + (beatTimer / beatInterval)) / (meterNumerator * 2);

        float halfWidth = beatBar.rect.width / 2f;
        Vector2 IndicatorPosition = beatIndicator.anchoredPosition;

        switch (turnManager.instance.turn)
        {
            case turnManager.turns.player1Record:
                IndicatorPosition.x = Mathf.Lerp(-halfWidth, 0, progress);
                break;
            case turnManager.turns.player2Respond:
                IndicatorPosition.x = Mathf.Lerp(0, halfWidth, progress);
                break;
            case turnManager.turns.player2Record:
                IndicatorPosition.x = Mathf.Lerp(halfWidth, 0, progress);
                break;
            case turnManager.turns.player1Respond:
                IndicatorPosition.x = Mathf.Lerp(0, -halfWidth, progress);
                break;

            default:
                return;
        }

        beatIndicator.anchoredPosition = IndicatorPosition;

    }



    //When player press
    public void OnInteract(InputValue input)
    {
        if (!input.isPressed)
        {
            return;
        }

        if (turnManager.instance.turn == turnManager.turns.player1Record)
        {
            RecordBeat(player1BeatsMap);
            //audioSource.PlayOneShot(beatReplay);
        }
        else if (turnManager.instance.turn == turnManager.turns.player2Record)
        {
            //audioSource.PlayOneShot(beatReplay);
            RecordBeat(player2BeatsMap);
        }
        else
        {
            
        }

        if (turnManager.instance.turn == turnManager.turns.player1Respond)
        {
            RespondBeatsMap(player1BeatsMap);

        }
        if (turnManager.instance.turn == turnManager.turns.player2Respond)
        {
            RespondBeatsMap(player2BeatsMap);
        }

    }


    void RecordBeat(bool[] playerBeatMap)
    {
        int index = currentBeatIndex;
        float timeError = beatTimer;

        if (beatTimer >= beatInterval / 2)
        {
            index++;
            timeError = beatInterval - beatTimer;

        }

        if (index >= playerBeatMap.Length)
            return;

        if (recordAttempted[index])
            return;

        recordAttempted[index] = true;

        if (timeError > successRecordTimeRange)
            return;

        playerBeatMap[index] = true;
        audioSource.PlayOneShot(beatReplay);


    }

    void ReplayBeatsMap(bool[] playerBeatMap)
    {
        //print(string.Join(", ", player1BeatsMap));
        if (playerBeatMap[currentBeatIndex])
        {
            audioSource.PlayOneShot(beatReplay);
        }


    }

    void RespondBeatsMap(bool[] playerBeatMap)
    {
        int index = currentBeatIndex;
        float timeError = beatTimer;

        if (beatTimer >= beatInterval / 2)
        {
            index++;
            timeError = beatInterval - beatTimer;

        }

        if (index >= playerBeatMap.Length)
            return;

        if (respondAttempted[index])
            return;

        respondAttempted[index] = true;

        if (timeError > successRespondTimeRange)
            return;


        //Successful response
        audioSource.PlayOneShot(successRespondSound);

    }


    public void StartRecording(turnManager.turns turn)
    {
        if (turn == turnManager.turns.player1Record)
        {
            ClearBeatsMap(player1BeatsMap);
            Array.Clear(recordAttempted, 0, recordAttempted.Length);
        }
        else if (turn == turnManager.turns.player2Record)
        {
            ClearBeatsMap(player2BeatsMap);
            Array.Clear(recordAttempted, 0, recordAttempted.Length);
        }

    }

    public void StartResponding(turnManager.turns turn)
    {
        if (turn == turnManager.turns.player1Respond)
        {
            targetBeatsMap = player2BeatsMap;
        }
        else if (turn == turnManager.turns.player2Respond)
        {
            targetBeatsMap = player1BeatsMap;
        }

    }



    void ClearBeatsMap(bool[] playerBeatMap)
    {
        Array.Clear(playerBeatMap, 0, playerBeatMap.Length);
    }


    void ShowBeatsMap(bool[] playerBeatMap, SpriteRenderer[] sprites)
    {
        for (int i = 0; i < playerBeatMap.Length; i++)
        {
            sprites[i].enabled = playerBeatMap[i];
        }
    }


}
