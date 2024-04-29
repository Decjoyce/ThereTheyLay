using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class InstructionReceiver : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce = 5f;

    bool isActive;
    public bool inst_MoveForward;
    public bool inst_MoveBackwards;
    public bool inst_MoveJump;

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
        if (isActive && inst_MoveForward)
        {
            //rb.AddForce(Vector2.right * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
            rb.velocity = new(moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        if (isActive && inst_MoveBackwards)
        {
            //rb.AddForce(-Vector2.right * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
            rb.velocity = new(-moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        if(isActive && inst_MoveJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Hint/MoveForwards"))
        {
            inst_MoveForward = true;
        }
        if (isActive && collision.gameObject.CompareTag("Hint/MoveBackwards"))
        {
            inst_MoveBackwards = true;
        }
        if (isActive && collision.gameObject.CompareTag("Hint/MoveJump"))
        {
            inst_MoveJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Hint/MoveForwards"))
        {
            inst_MoveForward = false;
        }
        if (isActive && collision.gameObject.CompareTag("Hint/MoveBackwards"))
        {
            inst_MoveBackwards = false;
        }
        if (isActive && collision.gameObject.CompareTag("Hint/MoveJump"))
        {
            inst_MoveJump = false;
        }
    }

    void PlayerRotate()
    {

    }

}