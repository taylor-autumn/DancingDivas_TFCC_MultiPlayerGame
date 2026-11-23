using UnityEngine;
using UnityEngine.InputSystem; //NEED THIS FOR THE INPUT TO WORK

public class playerInput : MonoBehaviour
{
    public Vector2 move { get; private set; } //any script can GET the value, only THIS script can set it
    public bool jump { get; private set; }


    private void Awake() //other things are relying on this data so we want to make sure it is initialized first
    {
        //not moving
        move = Vector2.zero;
        //not jumping
        jump = false;
    }

    //with send message behavior, need to put "On" (CAPS IMPORTANT AND NECESSARY) + whatever
    //the thing is titled in the action tab of the input system

    //must also put input value in the parameter
    public void OnMovement(InputValue input)
    {
        move = input.Get<Vector2>(); //get the vector 2
        
    }
    public void OnJump(InputValue input)
    {
        //not rn
    }

}
