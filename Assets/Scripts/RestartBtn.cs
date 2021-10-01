using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartBtn : MonoBehaviour
{
    Button restartBtn;

    // Start is called before the first frame update
    void Start()
    {
        restartBtn = GetComponent<Button>();
        restartBtn.onClick.AddListener(Restart);
    }
    void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
