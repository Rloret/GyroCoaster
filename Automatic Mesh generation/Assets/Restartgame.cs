using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Restartgame : MonoBehaviour {
    public Text text;

    void Update()
    {
        text.fontSize = 20 + Mathf.RoundToInt(Mathf.Sin(Time.time*3)*5)+5;
    }
    public void OnCLick()
    {
        ScoreScript.score = 0;
        SceneManager.LoadScene("Scene1");
       
    }

    public void ToMain()
    {
        ScoreScript.score = 0;
        SceneManager.LoadScene("MenuScene");
      
    }
}
