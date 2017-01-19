using UnityEngine;
using System.Collections;

public class LapidaController : MonoBehaviour {
    public GameObject[] lapidasPrefab;
    public GameObject EndOfRoad;
    public GameObject particles;

    public lightAndFxAnimation FX;

    private GameObject lapida;
    private GameObject particleAux;

    private int lapidaIndex;
    private float timeBetweenLapidas = 8f;
    private float fixedtimeBetweenLapidas =8f;
    private float ellapsedTime=0f;

    private float timeMovingLapida = 3f;
    private float timeStartLapida = 0f;
    private float timeStart = 0;

	// Use this for initialization
	void Start () {
        timeStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if ((Time.time-timeStart) - ellapsedTime >= timeBetweenLapidas)
        {
            ellapsedTime = (Time.time - timeStart);
            lapidaIndex = Random.Range(0, 2);
            lapida = Instantiate(lapidasPrefab[lapidaIndex], new Vector3(0f, EndOfRoad.transform.position.y, EndOfRoad.transform.position.z), Quaternion.Euler(-90, 180, 0)) as GameObject;
            lapida.transform.localScale = lapida.transform.localScale * 2;
            particleAux = Instantiate(particles,lapida.transform.position , Quaternion.identity) as GameObject;
            particleAux.transform.position =new Vector3(lapida.transform.position.x, lapida.transform.position.y+50, lapida.transform.position.z);
            particleAux.transform.parent = lapida.transform;
            timeStartLapida = Time.time;
            timeBetweenLapidas = fixedtimeBetweenLapidas + Random.Range(1, 3);
            FX.spawnRay();
        }

        if (lapida != null) {
            if (Time.time - timeStartLapida <= timeMovingLapida) {
                lapida.transform.position =EndOfRoad.transform.position;
            }
        }
	}
}
