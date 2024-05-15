using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private GameObject PopupWindow;
    // Start is called before the first frame update
    void Start()
    {
        PopupWindow.SetActive(false);
    }

   public void ShutDown()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void BobsLife()
    {
        Application.OpenURL("https://yourbuddyaaron.itch.io/bobs-life");
        Debug.Log("Bobs life");
    }
}
