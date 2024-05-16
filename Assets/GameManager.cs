using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region singleton
    public static GameManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("More than one instance of <b>GameManager</b>");
            return;
        }
        instance = this;
    }

    #endregion

    public TextMeshProUGUI texty;

    public int currentLevel;

    // Start is called before the first frame update
    void Start()
    {
            currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            texty.text = "Press R To Reset";
        }
    }

    public void BeginWin()
    {
        StartCoroutine(Win());
    }

    IEnumerator Win()
    {
        yield return new WaitForSecondsRealtime(3f);
        currentLevel++;
        SceneManager.LoadScene(currentLevel);
    }
}
