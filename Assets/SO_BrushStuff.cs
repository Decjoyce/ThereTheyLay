using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Brush Stuff", menuName = "Brush Colours")]
public class SO_BrushStuff : ScriptableObject
{
    public Material pen_material;
    public Material oil_material;
    public Material airbrush_material;

    public Texture2D b_p_f;
    public Texture2D b_p_b;
    public Texture2D b_p_j;

    public Texture2D b_o_f;
    public Texture2D b_o_b;
    public Texture2D b_o_j;
}
