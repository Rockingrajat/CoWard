using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void LevelSceneLoader()
    {
        SceneManager.LoadScene("Level");
        MainScript.resumeGame = false;
        // SceneManager.LoadScene("GameScene");
        
    }

    public void ResumeGame()
    {
        SceneManager.LoadScene("GameScene");
        MainScript.resumeGame = true;
        MainScript.resumeFile = "save1";
        //Debug.Log("Resume");
    }
    public void Help()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
