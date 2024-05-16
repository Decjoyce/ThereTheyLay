using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClippyByeBye : MonoBehaviour
{
    [SerializeField] GameObject text;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            text.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
