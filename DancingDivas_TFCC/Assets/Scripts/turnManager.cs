using UnityEngine;

//HANDLES THE SEQUENCE OF TURNS
public class turnManager : MonoBehaviour
{
    public static turnManager instance;
    [SerializeField] private rhythmSystem rhythmSystemInstance;

    public enum turns
    {
        waiting,
        player1Respond,
        player1Record,
        player2Respond,
        player2Record,
        end
    }
    [Header("Current Turn")]
    public turns turn;

    private void Awake()
    {
        //singleton reference
        if (instance != null)
        {
            Debug.LogError("More than one instance of turnManager found!");
            Destroy(this);
        }

        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //start in waiting
        //turn = turns.waiting; //WE WILL WAIT A FEW SECONDS IN THE BEGINNING BEFORE STARTING

        turn = turns.player1Record; //FOR TESTING PURPOSES, START DIRECTLY WITH PLAYER 1 RESPOND
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTurn()
    {
        switch (turn)
        {

            case turns.player1Record:
                turn = turns.player2Respond;
                StartCurrentTurn();
                break;
            case turns.player1Respond:
                turn = turns.player1Record;
                StartCurrentTurn();
                break;
            case turns.player2Record:
                turn = turns.player1Respond;
                StartCurrentTurn();
                break;
            case turns.player2Respond:
                turn = turns.player2Record;
                StartCurrentTurn();
                break;


        }
    }

    void StartCurrentTurn()
    {
        switch (turn)
        {
            case turns.player1Record:
                rhythmSystemInstance.StartRecording(turn);
                break;
            case turns.player1Respond:
                rhythmSystemInstance.StartResponding(turn);
                break;
            case turns.player2Record:
                rhythmSystemInstance.StartRecording(turn);
                break;
            case turns.player2Respond:
                rhythmSystemInstance.StartResponding(turn);
                break;
        }
    }


}
