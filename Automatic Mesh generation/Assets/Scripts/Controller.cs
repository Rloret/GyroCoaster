using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {

    public float Finaltime=10f;
    public Text TimeText;

    private float bonusTime = 10f;
    private float songduration;
    private float timeStart;
    AudioSource song;



	// Use this for initialization
	void Start () {
        song = GetComponentInChildren<AudioSource>();
        songduration = song.clip.length;
        timeStart = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (Finaltime<=0f) {

            SceneManager.LoadScene("FinScene");
            FinGameScene.score = ScoreScript.score;
        }
        Finaltime -= Time.deltaTime;
        TimeText.text = "Time: " + (int)Finaltime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene("FinScene");
            FinGameScene.score = ScoreScript.score;
        }
        if (Time.time - timeStart > songduration)
        {
            SceneManager.LoadScene("FinScene");
            FinGameScene.score = ScoreScript.score;
        }
	}

    public void IncreaseTime() {
        Finaltime += bonusTime;
    }
}
