using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinGameScene : MonoBehaviour {

    public Text scorerext;
    public static int score;

	// Use this for initialization
	void Start () {
        UpdateText();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateText() {
        scorerext.text = "SCORE: " + score;
    }
}
