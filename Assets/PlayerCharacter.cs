using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCharacter : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float moveSpeed;

    bool isActive;
    public PlayerStates state;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isActive = true;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        PlayerRotate();
    }

    private void FixedUpdate()
    {
        if (isActive && state == PlayerStates.move_forwards)
        {
            rb.AddForce(Vector2.right * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        if (isActive && state == PlayerStates.move_backwards)
        {
            rb.AddForce(-Vector2.right * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Hint/MoveForwards"))
        {
            state = PlayerStates.move_forwards;
        }
        if (isActive && collision.gameObject.CompareTag("Hint/MoveBackwards"))
        {
            state = PlayerStates.move_backwards;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Hint/MoveForwards"))
        {
            state = PlayerStates.idle;
        }
        if (isActive && collision.gameObject.CompareTag("Hint/MoveBackwards"))
        {
            state = PlayerStates.idle;
        }
    }

    void PlayerRotate()
    {

    }

}

public enum PlayerStates
{
    idle,
    move_forwards,
    move_backwards,
}