using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5;
    private PlayerInput input;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        Vector3 direction = new Vector3(input.Move.x, input.Move.y, 0);
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

}