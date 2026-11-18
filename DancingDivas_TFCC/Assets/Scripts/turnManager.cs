using UnityEngine;

//HANDLES THE SEQUENCE OF TURNS
public class turnManager : MonoBehaviour
{
    public static turnManager instance;
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
            Destroy(this);
        }

        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //start in waiting
        turn = turns.waiting; //WE WILL WAIT A FEW SECONDS IN THE BEGINNING BEFORE STARTING

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
