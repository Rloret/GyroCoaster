using UnityEngine;
using System.Collections;

public class AvoidFlying : MonoBehaviour {
    public float velocity = 100;
    private Rigidbody rb;

    // Use this for initialization
    void Start () {
   //     rb = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update () {
   //     rb.AddForce(Vector3.down * velocity * Time.deltaTime);


    }
    /* void OnTriggerExit(Collider c) {
         if (c.gameObject.tag == "path") {
             this.gameObject.transform.Translate( Vector3.down *velocity * Time.deltaTime );

         }
     }*/
}
