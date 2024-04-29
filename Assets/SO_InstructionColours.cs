using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Instruction Colours", menuName = "Instrction Colours")]
public class SO_InstructionColours : ScriptableObject
{
    public Gradient move_forward;
    public Gradient move_backward;
    public Gradient move_jump;
}
