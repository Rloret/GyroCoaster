using UnityEngine;
using System.Collections;

public class wheelsAnimation : MonoBehaviour {

    public float speed;
    public GameObject WheelI;
    public GameObject WheelD;
    public GameObject WheelFront;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        WheelD.transform.Rotate(Vector3.forward * Time.deltaTime * speed * HoverCarControl.dist);
        WheelI.transform.Rotate(Vector3.forward * Time.deltaTime * speed * HoverCarControl.dist);
        WheelFront.transform.Rotate(Vector3.forward * Time.deltaTime * speed * HoverCarControl.dist);
    }
}
