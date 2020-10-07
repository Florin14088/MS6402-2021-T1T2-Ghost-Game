using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EH_MenuFunctions : MonoBehaviour
{

    public int mainSceneIndex = 1;
    public int creditsIndex = 2;
    public int optionsIndex = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     

    void StartGame()
    {
        SceneManager.LoadScene(mainSceneIndex);
    }

    void Options()
    {
        SceneManager.LoadScene(optionsIndex);

    }

    void Credits()
    {
        SceneManager.LoadScene(creditsIndex);

    }

    void QuitGame()
    {
        Application.Quit();

    }

}
