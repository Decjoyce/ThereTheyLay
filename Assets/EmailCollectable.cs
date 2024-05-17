using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailCollectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Email_System.instance.EmailNotification();
            Destroy(gameObject);
        }
    }
}
