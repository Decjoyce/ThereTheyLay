using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] GameObject shotObject;

    bool active;

    // Start is called before the first frame update
    public void Johnert()
    {
        StartCoroutine(Shoot());
        Instantiate(shotObject, transform);
    }

    private void OnEnable()
    {
        Mouse_Draw_Script.onUnPause += Johnert;
    }

    private void OnDisable()
    {
        Mouse_Draw_Script.onUnPause -= Johnert;
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            Instantiate(shotObject, transform);
        }
    }
}
