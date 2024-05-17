using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionGiver : MonoBehaviour
{
    public BrushType brushType;
    public InstructionType instructionType;

    Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
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
        col.enabled = false;
        Invoke(nameof(plsjustwork), 0f);
    }

    void plsjustwork()
    {
        col.enabled = true;
    }
}

public enum BrushType
{
    pen,
    airbrush,
    oil,
    eraser,
}

public enum InstructionType
{
    move_forward,
    move_backward,
    move_jump,
}