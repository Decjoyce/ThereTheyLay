using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField] GameObject victoryText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("VICTORY");
            victoryText.SetActive(true);
            collision.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GameManager.instance.BeginWin();
        }
    }
}
