using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Email_System : MonoBehaviour
{
    #region singleton
    public static Email_System instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of <b>Email_System</b>");
            return;
        }
        instance = this;
    }

    #endregion

    [SerializeField] private GameObject EmailScreen;
    [SerializeField] private GameObject EmailButton;
    [TextArea(3, 10)]
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
        //StartCoroutine(Mail());
    }

    // Update is called once per frame
    public void EmailNotification()
    {
        NotificationSound.PlayOneShot(Sound, 1f);
        EmailButton.SetActive(true);
    }

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

    }
}
