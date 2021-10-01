using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject GoalCanvas;

    void Awake()
    {
        GoalCanvas.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GoalCanvas.SetActive(true);
        }
    }
}
