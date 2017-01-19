using UnityEngine;
using System.Collections;

public class lightAndFxAnimation : MonoBehaviour {

    public GameObject lightning;

    private float Radius = 1000;
    public Material skyboxMat;
    public AnimationCurve curve;

    private DigitalRuby.LightningBolt.LightningBoltScript bolt;
    private float awaketime = 0;

    // Use this for initialization
    void Start () {
        bolt = lightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>();
        lightning.SetActive(false);
        awaketime = Time.time;

    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time-awaketime > 2f)
        {
            float val = curve.Evaluate(1 - SpectrumData.spectrumValueClamped);
            skyboxMat.SetFloat("_Exposure",val );
            if (val >= 0.5f)
            {
                GameObject parent = bolt.StartObject.transform.parent.gameObject;
                bolt.transform.parent = this.transform.parent;
                bolt.StartObject.transform.position = new Vector3(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f)*Radius );
                bolt.transform.position = new Vector3(bolt.transform.position.x, bolt.transform.position.y, parent.transform.position.z);

                bolt.transform.parent = parent.transform;

                lightning.SetActive(true);
                Invoke("deactiveBolt", 0.5f);
            }

        }
    }

    private void deactiveBolt()
    {
        lightning.SetActive(false);
    }

    public void spawnRay()
    {
        GameObject parent = bolt.StartObject.transform.parent.gameObject;
        bolt.transform.parent = this.transform.parent;
        bolt.StartObject.transform.position = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f) * Radius);
        bolt.transform.position = new Vector3(bolt.transform.position.x, bolt.transform.position.y, parent.transform.position.z);

        bolt.transform.parent = parent.transform;

        lightning.SetActive(true);
        Invoke("deactiveBolt", 0.5f);
    }
}
