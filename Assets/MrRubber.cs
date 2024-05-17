using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrRubber : MonoBehaviour
{
    Rigidbody2D rb;

    LineRenderer lr;
    bool followPath;

    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        Mouse_Draw_Script.onUnPause += UnFreeze;
    }

    private void OnDisable()
    {
        Mouse_Draw_Script.onUnPause -= UnFreeze;
    }

    public void UnFreeze()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
