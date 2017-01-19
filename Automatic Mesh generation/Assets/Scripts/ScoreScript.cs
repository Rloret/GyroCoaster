using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScript:MonoBehaviour  {

    public static int score =0;
    public Text ScoreText;

    public void UpdateScore() {
        score++;
        ScoreText.text = "COINS: " + score;
    }
}
