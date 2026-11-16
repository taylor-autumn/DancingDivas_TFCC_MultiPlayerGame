using UnityEngine;

//REQUIRE A COMPONENT FOR THIS CLASS TO WORK, GO HAS TO HAVE IT ON IT,
//it'll automatically add it onto it if it doesn't already have the component
[RequireComponent (typeof(playerInput))]
public class playerMovement : MonoBehaviour
{

    private playerInput input;
    public float moveSpeed = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        input = GetComponent<playerInput>();
    }

    private void Update()
    {
        transform.position += new Vector3(input.move.x,input.move.y,0)*moveSpeed*Time.deltaTime;
    }
}
