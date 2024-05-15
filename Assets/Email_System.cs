using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Email_System : MonoBehaviour
{
    [SerializeField] private GameObject EmailScreen;
    [SerializeField] private GameObject EmailButton;
    [SerializeField] private string Email;
    [SerializeField] private string Subject;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI subjecttext;
    private float PopupTime;
    [SerializeField] private AudioSource NotificationSound;
    [SerializeField] private AudioClip Sound;   
    void Start()
    {
        EmailScreen.SetActive(false);
        EmailButton.SetActive(false);
        PopupTime = Random.Range(3f, 12f);
        StartCoroutine(Mail());
    }

    // Update is called once per frame


    public void EmailPopup()
    {
        EmailScreen.SetActive(true);
        text.text = Email;
        subjecttext.text = Subject;
    }
    public void OK()
    {
        EmailScreen.SetActive(false);
    }

    IEnumerator Mail()
    {
        yield return new WaitForSeconds(PopupTime);
        NotificationSound.PlayOneShot(Sound, 1f);
        EmailButton.SetActive(true);
    }
}
