using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(AudioSource))]
public class SpectrumData : MonoBehaviour
{
    public AudioSource carmina;
    public static float spectrumValueClamped=0;
    public static float fragmentMeanClamped = 0;


    List<float> SpectrumMeans;
    List<float> spectrumList;
    float treshold;
    float max=float.MinValue;
    float min=float.MaxValue;
    float valOfMean = 0;
    float fragmentMean = 0;
    float timeOfStart = 0;
    void Start()
    {
        SpectrumMeans = new List<float>();
        spectrumList = new List<float>();
        carmina.Stop();
        carmina.Play();
        spectrumValueClamped = 0;
        fragmentMeanClamped = 0;
        max = float.MinValue;
        min = float.MaxValue;
        timeOfStart = Time.time;

    }

    void Update()
    {
        float[] spectrum = new float[256];
        float spectrumMean = 0;


        carmina.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        spectrumList.AddRange(spectrum);
        for (int i = 1; i < spectrumList.Count - 1; i++)
        {
           // Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
           // Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
            spectrumMean += Mathf.Abs(Mathf.Log(spectrum[i]));
            //Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
           // Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
        }
        
        valOfMean = (spectrumMean / spectrum.Length);

        SpectrumMeans.Add(valOfMean);
        spectrumList.Clear();

        for (int i = 0; i < SpectrumMeans.Count; i++)
        {
            //float drawValue0 = SpectrumMeans[i - 1] ;
            float drawValue1 = SpectrumMeans[i];
            fragmentMean += drawValue1;
            min = drawValue1 < min ? drawValue1 : min;
            max = drawValue1 < max ? max : drawValue1; 
            //Debug.DrawLine(new Vector3(i - 1,drawValue0*100,0),new Vector3(i,drawValue1*100, 0), Color.black);
        }
        fragmentMean /= SpectrumMeans.Count;
        spectrumValueClamped = ((valOfMean - min) / (max - min));
        fragmentMeanClamped = ((fragmentMean - min) / (max - min));
        Debug.Log(valOfMean + "\t"+ spectrumValueClamped + "\t"+ fragmentMeanClamped);
        /*Debug.DrawLine(new Vector3(0, fragmentMean*100, 0), new Vector3(100, fragmentMean*100, 0), Color.green);
        Debug.DrawLine(new Vector3(0, min*100 , 0), new Vector3(100, min*100 , 0), Color.cyan);
        Debug.DrawLine(new Vector3(0, max *100, 0), new Vector3(100, max *100, 0), Color.magenta);*/
        if (Time.time-timeOfStart >7f)
        {
            SpectrumMeans.RemoveAt(0);
        }
        //  Debug.Log(valOfMean +"\t"+min + "\t" +max + "\t" + spectrumValueClamped + "\t"+ (meanPercent < clapRange));

        max = float.MinValue;
         min =float.MaxValue;
    }
}

