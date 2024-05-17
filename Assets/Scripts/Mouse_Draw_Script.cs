using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Mouse_Draw_Script : MonoBehaviour
{
    public delegate void OffClick();
    public static event OffClick OnOffClick;

    public delegate void OnUnPause();
    public static event OnUnPause onUnPause;

    #region singleton
    public static Mouse_Draw_Script instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of <b>Mouse_Draw_Script</b>");
            return;
        }
        instance = this;
    }

    #endregion


    private LineRenderer lineRenderer;
    private LineRenderer fakeLineRenderer;
    private EdgeCollider2D edgeCollider;
    private Vector3 previousPos;

    [SerializeField] Transform mainDrawHolder;
    public Transform[] drawHolders;

    public int currentPenID;

    [SerializeField] GameObject brush;
    [SerializeField] GameObject eraser;
    private GameObject activeEraser;

    [SerializeField] private float minDistance = 0.1f;
    [Range(0.25f,6)] public float width;
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

    Camera cam;

    public float inkLeft, inkUsed;

    [SerializeField] Image playButton;

    private void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.positionCount = 1;
        cam = Camera.main;
        SwitchColourUsingID(0);
        SwitchBrushUsingID(0);
        previousPos = transform.position;
        drawWindow.localScale = drawWindowScale;
        //lineRenderer.startWidth = lineRenderer.endWidth = width;
    }

    private void OnEnable()
    {
        OnOffClick += Shush;
        onUnPause += Shush;
    }

    private void OnDisable()
    {
        OnOffClick -= Shush;
        onUnPause -= Shush;
    }

    private void Update()
    {

        if(Input.GetMouseButton(0) && CheckIfInDrawArea(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("NoDrawBox"))
                {
                    if (isDrawing)
                    {
                        OnOffClick.Invoke();
                        if (currentBrush != BrushType.eraser)
                        {
                            fakeLineRenderer.Simplify(0.1f);
                            SetEdgeCollider();
                            isDrawing = false;
                            lineRenderer = null;
                            fakeLineRenderer = null;
                            edgeCollider = null;
                        }
                        else
                        {
                            if (activeEraser)
                                Destroy(activeEraser);
                            isDrawing = false;
                        }
                    }
                    return;
                }
            }

            if (currentBrush != BrushType.eraser)
            {
                if (!isDrawing)
                {
                    BeginDraw();
                }


                isDrawing = true;

                Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentPos.z = 0f;

                float distanceFromPoints = Vector3.Distance(previousPos, currentPos);

                if (distanceFromPoints > minDistance)
                {
                    if (previousPos == transform.position)
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
                    inkUsed += distanceFromPoints;
                }
            }
            else
            {

                Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentPos.z = 0f;


                if (!isDrawing)
                {
                    activeEraser = Instantiate(eraser, currentPos, Quaternion.identity, transform);
                }

                isDrawing = true;

                activeEraser.transform.position = currentPos;

            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if (isDrawing)
            {
                OnOffClick.Invoke();
                if (currentBrush != BrushType.eraser)
                {
                    fakeLineRenderer.Simplify(0.1f);
                    SetEdgeCollider();
                    isDrawing = false;
                    lineRenderer = null;
                    fakeLineRenderer = null;
                    edgeCollider = null;
                }
                else
                {
                    if (activeEraser)
                        Destroy(activeEraser);
                    isDrawing = false;
                }
            }
        }

        if (CheckIfInDrawArea(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            CheckCursor();
        }
        else
            Cursor.SetCursor(brushStuff.defaultCursor, new(0, 0), CursorMode.Auto);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnPauseGame();
        }

    }

    public void UnPauseGame()
    {
        onUnPause.Invoke();
        playButton.color = new Color(0, 0.65f, 1f);
    }

    void BeginDraw()
    {
        GameObject newLine = Instantiate(brush, transform.position, Quaternion.identity, GetDrawHolder(currentBrush, currentInstruction));
        InstructionGiver giver = newLine.GetComponent<InstructionGiver>();
        giver.brushType = currentBrush;
        giver.instructionType = currentInstruction;
        LineRenderer[] renderers = newLine.GetComponentsInChildren<LineRenderer>();
        renderers[0].colorGradient = currentColour;
        renderers[0].material = currentMaterial;
        lineRenderer = renderers[0];
        fakeLineRenderer = renderers[1];
        edgeCollider = newLine.GetComponent<EdgeCollider2D>();
        if (currentBrush == BrushType.airbrush)
            edgeCollider.isTrigger = true;
        lineRenderer.positionCount = 1;
        fakeLineRenderer.positionCount = 1;
        previousPos = transform.position;
        lineRenderer.startWidth = lineRenderer.endWidth = width;
        fakeLineRenderer.startWidth = fakeLineRenderer.endWidth = width;
    }

    bool CheckIfInDrawArea(Vector2 mousePosition)
    {
        return mousePosition.x > drawWindowPos.x + -drawWindowScale.x / 2 && mousePosition.x < drawWindowPos.x + drawWindowScale.x / 2 && mousePosition.y > drawWindowPos.y + -drawWindowScale.y / 2 && mousePosition.y < drawWindowPos.y + drawWindowScale.y / 2;
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

    public void ChangeBrushSize(float size)
    {
        float newSize = Mathf.Clamp(size, 0.25f, 1);
        width = size;
    }

    public void ClearAllDrawings()
    {
        foreach(Transform holder in drawHolders)
        {
            foreach(Transform drawing in holder)
            {
                foreach (Transform drawing1 in drawing)
                {
                    Destroy(drawing1.gameObject);
                }
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
        switch (id)
        {
            case 0:
                foreach (Transform drawing in drawHolders[1])
                {
                    Destroy(drawing.gameObject);
                }
                foreach (Transform drawing in drawHolders[5])
                {
                    Destroy(drawing.gameObject);
                }
                foreach (Transform drawing in drawHolders[9])
                {
                    Destroy(drawing.gameObject);
                }
                break;
            case 1:
                foreach (Transform drawing in drawHolders[2])
                {
                    Destroy(drawing.gameObject);
                }
                foreach (Transform drawing in drawHolders[6])
                {
                    Destroy(drawing.gameObject);
                }
                foreach (Transform drawing in drawHolders[10])
                {
                    Destroy(drawing.gameObject);
                }
                break;
            case 2:
                foreach (Transform drawing in drawHolders[3])
                {
                    Destroy(drawing.gameObject);
                }
                foreach (Transform drawing in drawHolders[7])
                {
                    Destroy(drawing.gameObject);
                }
                foreach (Transform drawing in drawHolders[11])
                {
                    Destroy(drawing.gameObject);
                }
                break;
        }

    }

    public Transform GetDrawHolder(BrushType bt, InstructionType inst)
    {
        switch (bt, inst)
        {
            case (BrushType.pen, InstructionType.move_forward):
                return drawHolders[1];                
            case (BrushType.pen, InstructionType.move_backward):
                return drawHolders[2];
            case (BrushType.pen, InstructionType.move_jump):
                return drawHolders[3];


            case (BrushType.oil, InstructionType.move_forward):
                return drawHolders[5];
            case (BrushType.oil, InstructionType.move_backward):
                return drawHolders[6];
            case (BrushType.oil, InstructionType.move_jump):
                return drawHolders[7];


            case (BrushType.airbrush, InstructionType.move_forward):
                return drawHolders[9];
            case (BrushType.airbrush, InstructionType.move_backward):
                return drawHolders[10];
            case (BrushType.airbrush, InstructionType.move_jump):
                return drawHolders[11];
            default:
               // Debug.Log(bt + " " + inst);
                return drawHolders[12];
        }
    }

    public void DestroyDrawHolder(int i)
    {
        switch (i)
        {
            case 0:
                foreach (Transform drawing in drawHolders[0])
                {
                    foreach (Transform drawing1 in drawing)
                    {
                        Destroy(drawing1.gameObject);
                    }
                }
                break;
            case 1:
                foreach (Transform drawing in drawHolders[4])
                {
                    foreach (Transform drawing1 in drawing)
                    {
                        Destroy(drawing1.gameObject);
                    }
                }
                break;
            case 2:
                foreach (Transform drawing in drawHolders[8])
                {
                    foreach (Transform drawing1 in drawing)
                    {
                        Destroy(drawing1.gameObject);
                    }
                }
                break;
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
            case 3:
                currentBrush = BrushType.eraser;
                break;
            default:
                Debug.LogError("INVALID PEN ID");
                break;
        }
        CheckCursor();
    }

    public void GetMaterial(int id)
    {
        switch (id)
        {
            case 0:
                currentMaterial = brushStuff.pen_material;
                break;
            case 1:
                currentMaterial = brushStuff.oil_material;
                break;
            case 2:
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
            case (BrushType.eraser, InstructionType.move_forward):
                Cursor.SetCursor(brushStuff.Eraser, new(brushStuff.Eraser.width, brushStuff.Eraser.height), CursorMode.Auto);
                break;
            case (BrushType.eraser, InstructionType.move_backward):
                Cursor.SetCursor(brushStuff.Eraser, new(0, brushStuff.Eraser.height), CursorMode.Auto);
                break;
            case (BrushType.eraser, InstructionType.move_jump):
                Cursor.SetCursor(brushStuff.Eraser, new(brushStuff.Eraser.width, brushStuff.Eraser.height), CursorMode.Auto);
                break;


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
                Cursor.SetCursor(brushStuff.b_a_f, new(brushStuff.b_a_f.width, brushStuff.b_a_f.height), CursorMode.Auto);
                break;
            case (BrushType.airbrush, InstructionType.move_backward):
                Cursor.SetCursor(brushStuff.b_a_b, new(brushStuff.b_a_b.width, brushStuff.b_a_b.height), CursorMode.Auto);
                break;
            case (BrushType.airbrush, InstructionType.move_jump):
                Cursor.SetCursor(brushStuff.b_a_j, new(brushStuff.b_a_j.width, brushStuff.b_a_j.height), CursorMode.Auto);
                break;
            default:
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

    public void Shush()
    {
        
    }

}
