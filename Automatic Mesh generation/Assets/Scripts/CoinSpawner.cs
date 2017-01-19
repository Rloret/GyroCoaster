using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour {

    public GameObject CoinPrefab;

    public Transform PlayerTransform;

    public pathGenerator path;

    public float timeBetweenCoins=2f;

    public float spawnZ = 6000f;
    private float spawnX;
    private float timeEllapsed=5f;
    private float height;
    private float timeBetweenCoinsModifier=0f;
    private float zDispl;
	// Use this for initialization
	void Start () {
        zDispl = path.ZDisplacement;
	}
	
	// Update is called once per frame
	void Update () {
        if ((Time.time - timeEllapsed) > (timeBetweenCoins + timeBetweenCoinsModifier)) {
            timeEllapsed = Time.time;

            CreateCoin();
        }
	}
    void CreateCoin()
    {
        height = Random.Range(-134, 290);
        timeBetweenCoinsModifier = Random.Range(-2, 2);
        float aux = path.factor * zDispl;
        spawnX = PlayerTransform.position.x;
        GameObject coin = Instantiate(CoinPrefab, new Vector3(spawnX, height,aux + spawnZ),new Quaternion(0f,0f,0f,0f)) as GameObject;
    }

}
