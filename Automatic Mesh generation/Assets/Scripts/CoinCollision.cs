using UnityEngine;
using System.Collections;

public class CoinCollision : MonoBehaviour {

    private ScoreScript scoreScript;
    private Controller controllerscript;
	// Use this for initialization
	void Start () {
        scoreScript = GameObject.FindGameObjectWithTag("UI").GetComponent<ScoreScript>();
        controllerscript = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider c) {
        if (c.tag == "Player") {
           // Debug.Log("he chocau");
            scoreScript.UpdateScore();
            controllerscript.IncreaseTime();
            Destroy(this.gameObject);

        }
    }
}
