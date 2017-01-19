using UnityEngine;
using System.Collections;

public class skullAnimation : MonoBehaviour {
    public GameObject low;
    public GameObject high;
    public GameObject lowlimit;
    public GameObject spring;
    public GameObject Eyei;
    public GameObject Eyed;
    public AnimationCurve curve;

    private float spectrumValue=0;
    private float awaketime ;
    // Use this for initialization
    void Start () {

        awaketime = Time.time;
	}

    // Update is called once per frame
    void Update()
    {
        if (Time.time-awaketime > 2f)
        {

            Vector3 pos = spring.transform.position;
            spectrumValue = curve.Evaluate(SpectrumData.fragmentMeanClamped);

            Vector3 lerp = Vector3.Lerp(low.transform.position, high.transform.position, spectrumValue);

            Vector3 localscale = spring.transform.localScale;
            localscale.z = 2 * (1 - SpectrumData.spectrumValueClamped);
            spring.transform.localScale = localscale;


            if (lerp.y < lowlimit.transform.position.y)
            {
                lerp.y = lowlimit.transform.position.y;
            }

            this.transform.position = lerp;

            spectrumValue = 0.8f - SpectrumData.spectrumValueClamped;
            Renderer renderer = Eyed.GetComponent<Renderer>();
            Material mat = renderer.material;
            Color baseColor = Color.red; //Replace this with whatever you want for your base color at emission level '1'
            Color finalColor = baseColor * Mathf.LinearToGammaSpace(spectrumValue);

            mat.SetColor("_EmissionColor", finalColor);

            renderer = Eyei.GetComponent<Renderer>();
            mat = renderer.material;
            baseColor = Color.red; //Replace this with whatever you want for your base color at emission level '1'
            finalColor = baseColor * Mathf.LinearToGammaSpace(spectrumValue);

            mat.SetColor("_EmissionColor", finalColor);

        }
    }
}
