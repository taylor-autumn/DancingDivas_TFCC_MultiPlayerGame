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
    private bool[] targetBeats = new bool[8];

    [SerializeField] private bool[] player1BeatsMap = new bool[8];
    [SerializeField] private bool[] player2BeatsMap = new bool[8];



    private float beatTimer = 0f;
    private bool pendingFirstBeat;


    //Audio
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip beatTic;
    [SerializeField] private AudioClip beatTac;
    [SerializeField] private AudioClip beatReplay;



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
                StartCurrentTurn();
            }

            var currentTurn = turnManager.instance.turn;
            if(currentTurn == turnManager.turns.player1Respond || currentTurn == turnManager.turns.player2Respond)
            {
                ReplayBeatsMap(targetBeats);
            }


        }

    }

    void StartCurrentTurn()
    {
        switch(turnManager.instance.turn)
        {
            case turnManager.turns.player1Record:
                StartRecording(player1BeatsMap);
                break;
            case turnManager.turns.player1Respond:
                StartResponding(player2BeatsMap);
                break;
            case turnManager.turns.player2Record:
                StartRecording(player2BeatsMap);
                break;
            case turnManager.turns.player2Respond:
                StartResponding(player1BeatsMap);
                break;
        }
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

    public void OnInteract(InputValue input)
    {
        if (!input.isPressed)
        {
            return;
        }

        if (turnManager.instance.turn == turnManager.turns.player1Record)
        {
            RecordBeat(player1BeatsMap);
            audioSource.PlayOneShot(beatReplay);
        }
        else if (turnManager.instance.turn == turnManager.turns.player2Record)
        {
            audioSource.PlayOneShot(beatReplay);
            RecordBeat(player2BeatsMap);
        }
        else
        {
            
        }


    }


    void RecordBeat(bool[] playerBeatMap)
    {


        if (pendingFirstBeat)
        {
            pendingFirstBeat = false;
        }
        if (beatTimer < beatInterval / 2)
        {
            playerBeatMap[currentBeatIndex] = true;
        }
        else if (beatTimer >= beatInterval / 2)
        {
            if (currentBeatIndex + 1 < playerBeatMap.Length)
            {
                playerBeatMap[currentBeatIndex + 1] = true;
            }
            else
            {
                pendingFirstBeat = true;
            }
                
        }

    }

    void ReplayBeatsMap(bool[] playerBeatMap)
    {
        //print(string.Join(", ", player1BeatsMap));
        if (playerBeatMap[currentBeatIndex])
        {
            audioSource.PlayOneShot(beatReplay);
        }


    }

    void StartRecording(bool[] playerBeatMap)
    {
        ClearBeatsMap(playerBeatMap);
    }

    void StartResponding(bool[] playerBeatMap)
    {
        targetBeats = playerBeatMap;

    }



    void ClearBeatsMap(bool[] playerBeatMap)
    {
        Array.Clear(playerBeatMap, 0, playerBeatMap.Length);
    }


}
