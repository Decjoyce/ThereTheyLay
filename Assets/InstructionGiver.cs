using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionGiver : MonoBehaviour
{
    public BrushType brushType;
    public InstructionType instructionType;
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