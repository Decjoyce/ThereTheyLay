using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    [SerializeField] SO_BrushStuff b;

    private void Start()
    {
        Cursor.SetCursor(b.defaultCursor, new(0, 0), CursorMode.Auto);
    }

    public void ResetDaGame()
    {
        SceneManager.LoadScene(0); 
    }
}
