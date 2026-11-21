using UnityEngine;


[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5;
    private PlayerInput Input;

    
    private void Start()
    {
        Input = GetComponent<PlayerInput>();

    }
    public void Update()
    {
    transform.position+= 
    new UnityEngine.Vector3(Input.Move.x, Input.Move.y,0)*
    Time.deltaTime * moveSpeed;
    }

    private void FixedUpdate()
    {
    
    }

}
