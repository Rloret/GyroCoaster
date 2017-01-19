using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public void StarGame() {
        SceneManager.LoadScene("Scene1");
    }


    public void Instructions() {
      //  SceneManager.LoadScene("InstructionsScene");
    }
}
