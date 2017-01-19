using UnityEngine;
using System.Collections;

public class wheelParticleAnimationScript : MonoBehaviour {

    public ParticleSystem particles;
	// Use this for initialization

    void Start()
    {
        particles.Stop();
    }
   /* void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "path" && !particles.isPlaying)
        {
            // particles.(true);
            //Debug.Log("activo" +other.name);
            particles.GetComponent<ParticleSystem>().Play();
        }
    }
    void OnTriggerExit(Collider other)
    {
        
      if (other.tag == "path" && particles.isPlaying)
        {
            // particles.(true);
            float val = Mathf.Abs(other.transform.transform.position.y - particles.transform.position.y);
            Debug.Log(val);
            if (val>10)
                particles.GetComponent<ParticleSystem>().Stop();
        }
    }*/

}
