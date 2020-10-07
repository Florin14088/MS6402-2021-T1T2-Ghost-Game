using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EH_HighScore : MonoBehaviour
{

    public int highScore;



    // Start is called before the first frame update
    void Start()
    {
        int savedScore = PlayerPrefs.GetInt("savedScore", 0);

        if (highScore > savedScore)
        {
            PlayerPrefs.SetInt("savedScore", highScore);
        }
        else
        {
            highScore = savedScore;
        }
    }
}
