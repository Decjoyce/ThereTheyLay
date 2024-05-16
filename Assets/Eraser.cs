using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MonoBehaviour
{
    [SerializeField] GameObject brushPrefab;

    Mouse_Draw_Script papa;

    List<Vector2> shit = new List<Vector2>();

    GameObject newLine;

    Collider2D col;

    private void Start()
    {
        papa = transform.parent.GetComponent<Mouse_Draw_Script>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Instruction"))
        {
            Debug.Log("HIT");
            LineRenderer[] renderers = collision.GetComponentsInChildren<LineRenderer>();
            LineRenderer lineRenderer = renderers[0];
            LineRenderer fakeLineRenderer = renderers[1];
            EdgeCollider2D edgeCollider = collision.GetComponent<EdgeCollider2D>();
            InstructionGiver inst_giver = collision.GetComponent<InstructionGiver>();

            float closestDist1 = 10000f;
            float closestDist2 = 15000f;
            int closestIndex1 = 0;
            int closestIndex2 = 0;
            for(int i = 0; i < lineRenderer.positionCount; i++)
            {
                float newDist1 = Vector3.Distance(transform.position, lineRenderer.GetPosition(i));
                float newDist2 = Vector3.Distance(transform.position, lineRenderer.GetPosition(i));
                if(newDist1 < closestDist1)
                {
                    closestDist1 = newDist1;
                    closestIndex1 = i;
                }
                if(newDist2 < closestDist2)
                {
                    closestDist2 = newDist2;
                    closestIndex2 = i;
                }
            }

            Vector3[] newLRPoints = new Vector3[lineRenderer.positionCount - closestIndex2 - 1];
            int fakeInt = 0;
            for(int i = closestIndex2 + 1; i < lineRenderer.positionCount; i++)
            {
                newLRPoints[fakeInt] = lineRenderer.GetPosition(i);


                fakeInt++;
            }

            if(closestIndex1 > 0)
            {
                lineRenderer.positionCount = closestIndex1 - 1;
                fakeLineRenderer.positionCount = closestIndex1 - 1;
            }
            else
            {
                lineRenderer.positionCount = closestIndex1;
                fakeLineRenderer.positionCount = closestIndex1;
            }

            edgeCollider.edgeRadius = papa.width / 2f;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                Vector3 convertedPos = lineRenderer.GetPosition(i);
                shit.Add(new Vector2(convertedPos.x, convertedPos.y));
            }
            edgeCollider.SetPoints(shit);
            shit.Clear();
            newLine = Instantiate(brushPrefab, Vector3.zero, Quaternion.identity, papa.GetDrawHolder(inst_giver.brushType, inst_giver.instructionType));

            InstructionGiver giver = newLine.GetComponent<InstructionGiver>();
            giver.brushType = inst_giver.brushType;
            giver.instructionType = inst_giver.instructionType;

            LineRenderer[] newRenderers = newLine.GetComponentsInChildren<LineRenderer>();
            renderers[0].colorGradient = lineRenderer.colorGradient;
            LineRenderer newlineRenderer = newRenderers[0];
            LineRenderer newfakeLineRenderer = newRenderers[1];

            EdgeCollider2D newEdgeCollider = newLine.GetComponent<EdgeCollider2D>();

            newlineRenderer.positionCount = newLRPoints.Length;
            newfakeLineRenderer.positionCount = newLRPoints.Length;
            newlineRenderer.SetPositions(newLRPoints);
            newfakeLineRenderer.SetPositions(newLRPoints);
            newlineRenderer.startWidth = newlineRenderer.endWidth = lineRenderer.startWidth;
            newfakeLineRenderer.startWidth = newfakeLineRenderer.endWidth = lineRenderer.startWidth;

            newfakeLineRenderer.Simplify(0.1f);

            newEdgeCollider.edgeRadius = papa.width / 2f;
            for (int i = 0; i < newfakeLineRenderer.positionCount; i++)
            {
                Vector3 convertedPos = newfakeLineRenderer.GetPosition(i);
                shit.Add(new Vector2(convertedPos.x, convertedPos.y));
            }
            newEdgeCollider.SetPoints(shit);
            shit.Clear();

            if (lineRenderer.positionCount <= 1)
                Destroy(lineRenderer.gameObject);
            if (newlineRenderer.positionCount <= 1)
                Destroy(newlineRenderer.gameObject);
        }
    }

    public void ClearThePreviousLine()
    {
        Destroy(newLine);
    }
}
