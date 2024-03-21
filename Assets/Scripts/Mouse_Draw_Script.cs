using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Mouse_Draw_Script : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 previousPos;
    [SerializeField] private float minDistance = 0.1f;
    [SerializeField, Range(1,6)] private float width;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 1;
        previousPos = transform.position;
        lineRenderer.startWidth = lineRenderer.endWidth = width;
    }

    private void Update()
    {

        if(Input.GetMouseButton(0))
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPos.z = 0f;

            if(Vector3.Distance(previousPos, currentPos) > minDistance)
            {
                if(previousPos == transform.position)
                {
                    lineRenderer.SetPosition(0, currentPos);
                }

                else
                {
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPos);
                }

                previousPos = currentPos;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            lineRenderer.positionCount = 0;
        }
    }

}
