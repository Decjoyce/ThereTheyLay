using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Main_Menu : MonoBehaviour
{
    [SerializeField] private string PasswordText;

    [SerializeField] private string[] PasswordOptions;
    [SerializeField] private TextMeshProUGUI ButtonText;

    [SerializeField] private GameObject SettingsMenuButton;
    [SerializeField] private GameObject PlayMenuButton;
    [SerializeField] private GameObject NoResButton;
    [SerializeField] private GameObject EndGame;
    [SerializeField] private GameObject EndGamePicture;

    private void Start()
    {
        PasswordOptions[0] = "Settings";
        PasswordOptions[1] = "Play";
        PasswordOptions[2] = "End";
    }
    private void Update()
    {
        if(PasswordText == PasswordOptions[0])
        {
            SettingsMenuButton.SetActive(true);
            PlayMenuButton.SetActive(false);
            NoResButton.SetActive(false);
        }

        else if(PasswordText == PasswordOptions[1]) 
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

        if(PasswordText == PasswordOptions[2])
        {
            EndGame.SetActive(true);
        }


    }
    public void InsertInput(string input)
    {
        PasswordText = input;
        DisplayReactionText();
    }

    private void DisplayReactionText()
    {
        ButtonText.text = PasswordText;
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
