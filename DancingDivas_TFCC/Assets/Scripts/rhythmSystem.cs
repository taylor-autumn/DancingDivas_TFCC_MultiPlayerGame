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
    //private bool[] targetBeats = new bool[8];

    [SerializeField] private bool[] player1BeatsMap = new bool[4];
    [SerializeField] private bool[] player2BeatsMap = new bool[8];



    private float beatTimer = 0f;
    private bool pendingFirstBeat;





    //Audio
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip beatTic;
    [SerializeField] private AudioClip beatTac;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //beatsMap = new List<bool>();
        beatInterval = 60f / bpm;

        ClearBeatsMap(player1BeatsMap);

        currentBeatIndex = -1; // Start at -1 so that the first beat is 0 after the first increment
        BeatsCounting(); // Increment to the first beat


    }

    // Update is called once per frame
    void Update()
    {


        beatTimer += Time.deltaTime;

        while (beatTimer >= beatInterval)
        {
            beatTimer -= beatInterval;
            BeatsCounting();
            if (currentBeatIndex == 0)
            {
                ClearBeatsMap(player1BeatsMap);
                //add the pending first beat after the map is cleared, so it doesn't get cleared
                player1BeatsMap[0] = pendingFirstBeat;
                pendingFirstBeat = false;
            }
        }

    }

    private void BeatsCounting()
    {
        currentBeatIndex = (currentBeatIndex + 1) % meterNumerator;

        if (currentBeatIndex == 0)
        {
            audioSource.PlayOneShot(beatTic);
        }
        else
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

        RecordBeat(player1BeatsMap);


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
        print(string.Join(", ", player1BeatsMap));
    }

    void ClearBeatsMap(bool[] playerBeatMap)
    {
        Array.Clear(playerBeatMap, 0, playerBeatMap.Length);
    }


}
