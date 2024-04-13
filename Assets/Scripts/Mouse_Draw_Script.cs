using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Draw_Script : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private Vector3 previousPos;

    [SerializeField] Transform mainDrawHolder;
    [SerializeField] Transform[] drawHolders;

    [SerializeField] GameObject[] pens;
    int currentPenID;

    [SerializeField] private float minDistance = 0.1f;
    [SerializeField, Range(1,6)] private float width;

    [SerializeField] Vector2 drawWindowPos, drawWindowScale;

    bool isDrawing;

    List<Vector2> shit = new List<Vector2>();

    private void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.positionCount = 1;
        previousPos = transform.position;
        //lineRenderer.startWidth = lineRenderer.endWidth = width;
    }

    private void Update()
    {

        if(Input.GetMouseButton(0) && CheckIfInDrawArea(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            if (!isDrawing)
            {
                GameObject newLine = Instantiate(pens[currentPenID], transform.position, Quaternion.identity, drawHolders[currentPenID]);
                lineRenderer = newLine.GetComponent<LineRenderer>();
                edgeCollider = newLine.GetComponent<EdgeCollider2D>();
                lineRenderer.positionCount = 1;
                previousPos = transform.position;
                lineRenderer.startWidth = lineRenderer.endWidth = width;
            }
                

            isDrawing = true;

            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPos.z = 0f;

            if(Vector3.Distance(previousPos, currentPos) > minDistance)
            {
                if(previousPos == transform.position)
                {
                    lineRenderer.SetPosition(0, currentPos);
                    edgeCollider.points[0] = currentPos;
                }

                else
                {
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPos);
                    edgeCollider.points[edgeCollider.pointCount - 1] = currentPos;
                }
                previousPos = currentPos;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if (isDrawing)
            {
                SetEdgeCollider();
                isDrawing = false;
                lineRenderer = null;
                edgeCollider = null;
            }
        }
    }

    bool CheckIfInDrawArea(Vector2 mousePosition)
    {
        //if((mousePosition.x > )
        return mousePosition.x > -drawWindowScale.x / 2 && mousePosition.x < drawWindowScale.x / 2 && mousePosition.y > -drawWindowScale.y / 2 && mousePosition.y < drawWindowScale.y / 2;
    }

    void SetEdgeCollider()
    {
        //lineRenderer.Simplify(0.5f);
        edgeCollider.edgeRadius = width/2;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 convertedPos = lineRenderer.GetPosition(i);
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
        Destroy(drawHolders[IDCHECKER(id)]);
    }

    public void ClearDrawingUsingID(int id)
    {

    }

    public void SwitchColour(string id)
    {
        currentPenID = IDCHECKER(id);
    }

    public void SwitchColourUsingID(int id)
    {
        if(id >= pens.Length)
        {
            Debug.LogError("YOU STUPID IDIOT!!!! THIS PEN DOES NOT EXIST!!!!!");
            return;
        }

        currentPenID = id;
    }

    int IDCHECKER(string id)
    {
        switch (id)
        {
            case "MOVE_FOWARD":
                return 0;
            case "MOVE_BACKWARD":
                return 1;
            default:
                Debug.LogError("INVALID PEN ID");
                return 0;
        }
    }

}
