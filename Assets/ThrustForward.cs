using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustForward : MonoBehaviour
{

    Rigidbody2D rb;

    [SerializeField] Vector2 force;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GameManager.instance.gameUnFrozen)
        {            
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.AddForce(force, ForceMode2D.Impulse);
        }

    }

    private void OnEnable()
    {
        Mouse_Draw_Script.onUnPause += Go;
    }

    private void OnDisable()
    {
        Mouse_Draw_Script.onUnPause -= Go;
    }

    public void Go()
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
