using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 Move { get; private set; }
    public bool Jump { get; private set; }

    private void Start ()
    {
        Move = Vector2.zero;
        Jump = false;
    }

    
    public void OnMovement(InputValue input)
    {
        Move = input.Get<Vector2>();
        Debug.Log(Move);
    }

}
