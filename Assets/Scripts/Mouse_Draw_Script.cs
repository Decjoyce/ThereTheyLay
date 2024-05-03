using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Draw_Script : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private LineRenderer fakeLineRenderer;
    private EdgeCollider2D edgeCollider;
    private Vector3 previousPos;

    [SerializeField] Transform mainDrawHolder;
    [SerializeField] Transform[] drawHolders;

    int currentPenID;

    [SerializeField] GameObject brush;

    [SerializeField] private float minDistance = 0.1f;
    [SerializeField, Range(1,6)] private float width;
    [SerializeField] Transform drawWindow;
    [SerializeField] Vector2 drawWindowPos, drawWindowScale;

    bool isDrawing;

    List<Vector2> shit = new List<Vector2>();

    [SerializeField] SO_InstructionColours instructionColours;
    [SerializeField] SO_BrushStuff brushStuff;

    public BrushType currentBrush;
    public Material currentMaterial;
    public InstructionType currentInstruction;
    public Gradient currentColour;

    private void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.positionCount = 1;
        SwitchColourUsingID(0);
        SwitchBrushUsingID(0);
        previousPos = transform.position;
        drawWindow.localScale = drawWindowScale;
        //lineRenderer.startWidth = lineRenderer.endWidth = width;
    }

    private void Update()
    {

        if(Input.GetMouseButton(0) && CheckIfInDrawArea(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            if (!isDrawing)
            {
                if(currentBrush != BrushType.eraser)
                    BeginDraw();

            }
                

            isDrawing = true;

            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPos.z = 0f;

            if(Vector3.Distance(previousPos, currentPos) > minDistance)
            {
                if(previousPos == transform.position)
                {
                    lineRenderer.SetPosition(0, currentPos);
                    fakeLineRenderer.SetPosition(0, currentPos);
                    //edgeCollider.points[0] = currentPos;
                }

                else
                {
                    lineRenderer.positionCount++;
                    fakeLineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPos);
                    fakeLineRenderer.SetPosition(fakeLineRenderer.positionCount - 1, currentPos);
                    //edgeCollider.points[edgeCollider.pointCount - 1] = currentPos;
                }
                previousPos = currentPos;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if (isDrawing)
            {
                fakeLineRenderer.Simplify(0.1f);
                SetEdgeCollider();
                isDrawing = false;
                lineRenderer = null;
                fakeLineRenderer = null;
                edgeCollider = null;
            }
        }
    }

    void BeginDraw()
    {
        GameObject newLine = Instantiate(brush, transform.position, Quaternion.identity, drawHolders[currentPenID]);
        InstructionGiver giver = newLine.GetComponent<InstructionGiver>();
        giver.brushType = currentBrush;
        giver.instructionType = currentInstruction;
        LineRenderer[] renderers = newLine.GetComponentsInChildren<LineRenderer>();
        renderers[0].colorGradient = currentColour;
        lineRenderer = renderers[0];
        fakeLineRenderer = renderers[1];
        edgeCollider = newLine.GetComponent<EdgeCollider2D>();
        lineRenderer.positionCount = 1;
        fakeLineRenderer.positionCount = 1;
        previousPos = transform.position;
        lineRenderer.startWidth = lineRenderer.endWidth = width;
        fakeLineRenderer.startWidth = lineRenderer.endWidth = width;
    }

    bool CheckIfInDrawArea(Vector2 mousePosition)
    {
        return mousePosition.x > -drawWindowScale.x / 2 && mousePosition.x < drawWindowScale.x / 2 && mousePosition.y > -drawWindowScale.y / 2 && mousePosition.y < drawWindowScale.y / 2;
    }

    void SetEdgeCollider()
    {
        edgeCollider.edgeRadius = width/2f;
        for (int i = 0; i < fakeLineRenderer.positionCount; i++)
        {
            Vector3 convertedPos = fakeLineRenderer.GetPosition(i);
            shit.Add(new Vector2(convertedPos.x, convertedPos.y));
        }
        edgeCollider.SetPoints(shit);
        shit.Clear();
    }

    public void ClearAllDrawings()
    {
        foreach(Transform holder in drawHolders)
        {
            foreach(Transform drawing in holder)
            {
                Destroy(drawing.gameObject);
            }
        }
    }

    public void ClearDrawing(string id)
    {
        foreach (Transform drawing in drawHolders[IDCHECKER(id)])
        {
            Destroy(drawing.gameObject);
        }
    }

    public void ClearDrawingUsingID(int id)
    {
        foreach (Transform drawing in drawHolders[id])
        {
            Destroy(drawing.gameObject);
        }
    }

    public void SwitchBrush(string id)
    {
        switch (id)
        {
            case "PEN":
                currentBrush = BrushType.pen;
                currentMaterial = brushStuff.pen_material;
                break;
            case "OIL":
                currentBrush = BrushType.oil;
                currentMaterial = brushStuff.oil_material;
                break;
            case "AIRBRUSH":
                currentBrush = BrushType.airbrush;
                currentMaterial = brushStuff.airbrush_material;
                break;
            default:
                Debug.LogError("INVALID PEN ID");
                break;
        }
        CheckCursor();
    }

    public void SwitchBrushUsingID(int id)
    {
        switch (id)
        {
            case 0:
                currentBrush = BrushType.pen;
                currentMaterial = brushStuff.pen_material;
                break;
            case 1:
                currentBrush = BrushType.oil;
                currentMaterial = brushStuff.oil_material;
                break;
            case 2:
                currentBrush = BrushType.airbrush;
                currentMaterial = brushStuff.airbrush_material;
                break;
            default:
                Debug.LogError("INVALID PEN ID");
                break;
        }
        CheckCursor();
    }

    public void SwitchColour(string id)
    {
        switch (id)
        {
            case "MOVE_FOWARD":
                currentInstruction = InstructionType.move_forward;
                currentColour = instructionColours.move_forward;
                break;
            case "MOVE_BACKWARD":
                currentInstruction = InstructionType.move_backward;
                currentColour = instructionColours.move_backward;
                break;
            case "MOVE_JUMP":
                currentInstruction = InstructionType.move_jump;
                currentColour = instructionColours.move_jump;
                break;
            default:
                Debug.LogError("INVALID PEN ID");
                break;
        }
        CheckCursor();
    }

    public void SwitchColourUsingID(int id)
    {

        switch (id)
        {
            case 0:
                currentInstruction = InstructionType.move_forward;
                currentColour = instructionColours.move_forward;
                break;
            case 1:
                currentInstruction = InstructionType.move_backward;
                currentColour = instructionColours.move_backward;
                break;
            case 2:
                currentInstruction = InstructionType.move_jump;
                currentColour = instructionColours.move_jump;
                break;
            default:
                Debug.LogError("INVALID PEN ID");
                break;
        }
        CheckCursor();
    }

    void CheckCursor()
    {
        switch(currentBrush, currentInstruction)
        {
            case (BrushType.pen, InstructionType.move_forward):
                Cursor.SetCursor(brushStuff.b_p_f, new(brushStuff.b_p_f.width, brushStuff.b_p_f.height), CursorMode.Auto);
                break;
            case (BrushType.pen, InstructionType.move_backward):
                Cursor.SetCursor(brushStuff.b_p_b, new(0, brushStuff.b_p_b.height), CursorMode.Auto);
                break;
            case (BrushType.pen, InstructionType.move_jump):
                Cursor.SetCursor(brushStuff.b_p_j, new(brushStuff.b_p_j.width, brushStuff.b_p_j.height), CursorMode.Auto);
                break;


            case (BrushType.oil, InstructionType.move_forward):
                Cursor.SetCursor(brushStuff.b_o_f, new(brushStuff.b_o_f.width, brushStuff.b_o_f.height), CursorMode.Auto);
                break;
            case (BrushType.oil, InstructionType.move_backward):
                Cursor.SetCursor(brushStuff.b_o_b, new(0, brushStuff.b_o_b.height), CursorMode.Auto);
                break;
            case (BrushType.oil, InstructionType.move_jump):
                Cursor.SetCursor(brushStuff.b_o_j, new(brushStuff.b_o_j.width, brushStuff.b_o_j.height), CursorMode.Auto);
                break;


            case (BrushType.airbrush, InstructionType.move_forward):
                //Cursor.SetCursor(brushStuff.b_a_f, new(brushStuff.b_a_f.width, brushStuff.b_a_f.height), CursorMode.Auto);
                break;
            case (BrushType.airbrush, InstructionType.move_backward):
                //Cursor.SetCursor(brushStuff.b_a_b, new(brushStuff.b_a_b.width, brushStuff.b_a_b.height), CursorMode.Auto);
                break;
            case (BrushType.airbrush, InstructionType.move_jump):
                //Cursor.SetCursor(brushStuff.b_a_j, new(brushStuff.b_a_j.width, brushStuff.b_a_j.height), CursorMode.Auto);
                break;
        }
    }

    int IDCHECKER(string id)
    {
        switch (id)
        {
            case "MOVE_FOWARD":
                return 0;
            case "MOVE_BACKWARD":
                return 1;
            case "MOVE_JUMP":
                return 2;
            default:
                Debug.LogError("INVALID PEN ID");
                return 0;
        }
    }

}
