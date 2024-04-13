using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentMouseDraw : MonoBehaviour
{
    private void Awake()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(-1, +1);
        vertices[1] = new Vector3(-1, -1);
        vertices[2] = new Vector3(+1, -1);
        vertices[3] = new Vector3(+1, +1);

        uv[0] = Vector2.zero;
        uv[1] = Vector2.zero;
        uv[2] = Vector2.zero;
        uv[3] = Vector2.zero;

        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
