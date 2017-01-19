using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class endGame : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SceneManager.LoadScene("FinScene");
            FinGameScene.score = ScoreScript.score;
        }
    }
}
