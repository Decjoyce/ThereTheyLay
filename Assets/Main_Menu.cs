using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Main_Menu : MonoBehaviour
{
    [SerializeField] private string UsernameText;

    [SerializeField] private string[] PasswordOptions;
    [SerializeField] private TextMeshProUGUI ButtonText;

    [SerializeField] private GameObject SettingsMenuButton;
    [SerializeField] private GameObject PlayMenuButton;
    [SerializeField] private GameObject NoResButton;
    [SerializeField] private GameObject EndGame;
    [SerializeField] private GameObject EndGamePicture;

    [SerializeField] GameObject secreteStuff;

    private void Start()
    {
        PasswordOptions[0] = "Settings";
        PasswordOptions[1] = "Play";
        PasswordOptions[2] = "End";
        PasswordOptions[3] = "Dick Richardson";
    }
    private void Update()
    {
        if(UsernameText == PasswordOptions[0])
        {
            SettingsMenuButton.SetActive(true);
            PlayMenuButton.SetActive(false);
            NoResButton.SetActive(false);
        }

        else if(UsernameText == PasswordOptions[1]) 
        {
            SettingsMenuButton.SetActive(false);
            PlayMenuButton.SetActive(true);
            NoResButton.SetActive(false);
        }

        else
        {
            SettingsMenuButton.SetActive(false);
            PlayMenuButton.SetActive(false);
            NoResButton.SetActive(true);
        }

        if(UsernameText == PasswordOptions[2])
        {
            EndGame.SetActive(true);
        }

        if(UsernameText == PasswordOptions[3])
        {
            secreteStuff.SetActive(true);
        }

    }
    public void InsertInput(string input)
    {
        UsernameText = input;
        DisplayReactionText();
    }

    private void DisplayReactionText()
    {
        ButtonText.text = UsernameText;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Tutorial Level");
    }

    public void End()
    {
        StartCoroutine(OhFuck());
    }

     IEnumerator OhFuck()
    {
        EndGamePicture.SetActive(true);
        yield return new WaitForSeconds(10);
        Application.Quit();
        Debug.Log("Quit");
    }
}
