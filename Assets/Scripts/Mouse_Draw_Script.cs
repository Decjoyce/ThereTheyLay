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

    [SerializeField] GameObject[] pens;
    int currentPenID;

    [SerializeField] private float minDistance = 0.1f;
    [SerializeField, Range(1,6)] private float width;
    [SerializeField] Transform drawWindow;
    [SerializeField] Vector2 drawWindowPos, drawWindowScale;

    bool isDrawing;

    List<Vector2> shit = new List<Vector2>();

    private void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.positionCount = 1;
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
                GameObject newLine = Instantiate(pens[currentPenID], transform.position, Quaternion.identity, drawHolders[currentPenID]);
                LineRenderer[] renderers = newLine.GetComponentsInChildren<LineRenderer>();
                lineRenderer = renderers[0];
                fakeLineRenderer = renderers[1];
                edgeCollider = newLine.GetComponent<EdgeCollider2D>();
                lineRenderer.positionCount = 1;
                fakeLineRenderer.positionCount = 1;
                previousPos = transform.position;
                lineRenderer.startWidth = lineRenderer.endWidth = width;
                fakeLineRenderer.startWidth = lineRenderer.endWidth = width;
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
