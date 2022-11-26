using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultScript : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PushReStart()
    {
        SceneManager.LoadScene("GameMainScene");
    }

    public void PushTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

}
