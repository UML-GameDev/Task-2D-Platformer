using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashIndicator : MonoBehaviour
{
    private Text text;
    void Start()
    {
        text = GetComponent<Text>();
    }
    public void DashEnable()
    {
        text.color = Color.black;
    }

    public void DashDisable()
    {
        text.color = Color.gray;
    }
}
