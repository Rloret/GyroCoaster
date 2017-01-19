using UnityEngine;
using System.Collections;

public class coinAnimaiton : MonoBehaviour {
    private float maxVel = 10000;
    private float turnVel =360;
    private float currentvel=0;
	
	// Update is called once per frame
	void Update () {
        float sound = 1-SpectrumData.spectrumValueClamped;

        if (Time.time > 2f)
        {
            if (sound > 0.85 )
            {
                currentvel = maxVel;
            }
            else
            {
                currentvel = Mathf.Lerp(currentvel, turnVel, 0.5f);
            }

            this.transform.Rotate(Vector3.up * currentvel * Time.deltaTime);
        }
            
        
	}
}
