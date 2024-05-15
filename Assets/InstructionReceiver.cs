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
    [SerializeField] float airbrush_slowness;
    [SerializeField] float oil_slippiness;
    [SerializeField] float maxVelocity = 5;

    float current_airbrush_slowness = 1;

    bool isActive;
    public bool inst_MoveForward;
    public bool inst_MoveBackwards;
    public bool inst_MoveJump;

    public bool affect_oil;
    public bool affect_airbrush;

    float startingDrag;
    float currentMaxVelocity;
    float currentMoveSpeed;

    [SerializeField] bool player;

    [SerializeField] Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingDrag = rb.drag;
        currentMaxVelocity = maxVelocity;
        currentMoveSpeed = moveSpeed;
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

        if (player)
        {
            //anim.SetFloat("x_vel", rb.velocity.normalized.x);
            anim.SetBool("instructed_forward", inst_MoveForward);
            anim.SetBool("instructed_backward", inst_MoveBackwards);
        }
    }

    private void FixedUpdate()
    {
        if (isActive && inst_MoveForward)
        {
            rb.AddForce(Vector2.right * currentMoveSpeed * Time.fixedDeltaTime * current_airbrush_slowness, ForceMode2D.Impulse);
            if (rb.velocity.x >= currentMaxVelocity * current_airbrush_slowness)
                rb.velocity = new(currentMaxVelocity * current_airbrush_slowness, rb.velocity.y);
        }
        if (isActive && inst_MoveBackwards)
        {
            rb.AddForce(-Vector2.right * currentMoveSpeed * Time.fixedDeltaTime * current_airbrush_slowness, ForceMode2D.Impulse);
            if (rb.velocity.x <= -currentMaxVelocity * current_airbrush_slowness)
                rb.velocity = new(-currentMaxVelocity * current_airbrush_slowness, rb.velocity.y);
        }
        if(isActive && inst_MoveJump)
        {
            rb.AddForce(Vector2.up * jumpForce * current_airbrush_slowness, ForceMode2D.Impulse);
        }


        if (affect_airbrush)
            current_airbrush_slowness = airbrush_slowness;
        else
            current_airbrush_slowness = 1;

        if (affect_oil)
        {
            currentMaxVelocity = Mathf.Infinity;
            currentMoveSpeed = moveSpeed / 2;
        }
        else
        {
            currentMaxVelocity = maxVelocity;
            currentMoveSpeed = moveSpeed;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isActive && collision.gameObject.CompareTag("Instruction"))
        {
            InstructionGiver giver = collision.gameObject.GetComponent<InstructionGiver>();
            ReceiveInstruction(giver, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Instruction"))
        {
            InstructionGiver giver = collision.gameObject.GetComponent<InstructionGiver>();
            ReceiveInstruction(giver, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Instruction"))
        {
            InstructionGiver giver = collision.gameObject.GetComponent<InstructionGiver>();
            ReceiveInstruction(giver, false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Instruction"))
        {
            InstructionGiver giver = collision.gameObject.GetComponent<InstructionGiver>();
            ReceiveInstruction(giver, false);
        }
    }

    void ReceiveInstruction(InstructionGiver _giver, bool receive)
    {
        switch (_giver.brushType)
        {
            case BrushType.airbrush:
                affect_airbrush = receive;
                break;
            case BrushType.oil:
                affect_oil = receive;
                break;
        }

        switch (_giver.instructionType)
        {
            case InstructionType.move_forward:
                inst_MoveForward = receive;
                break;
            case InstructionType.move_backward:
                inst_MoveBackwards = receive;
                break;
            case InstructionType.move_jump:
                inst_MoveJump = receive;
                break;
        }
    }

    void PlayerRotate()
    {

    }

}