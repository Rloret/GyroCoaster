using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class pathGenerator : MonoBehaviour {

    public List<GameObject> basicPoints;
    public GameObject pivot;
    public GameObject collider;
    public GameObject farolaGO;
    public GameObject farola1;
    public float ZDisplacement;
    public float maxYSlope;
    public float timeBetweenChunks;
    public float timeToBeginErasing;
    public float timeBetweenCuts;
    public static float[] InputData;
    [Range(0f,1f)]
    public float meshCutTreshold;
    public enum PortAction
    {
       OPEN,CLOSE 
    };
    public PortAction ArduinoConectedTo;

    public AnimationCurve curve;

    public Material meshMat;
    public Material fixedMat;


    private Mesh thisMesh;
    private MeshCollider mCollider;

    private GameObject player;
    private List<GameObject> farolas;
    private List<Vector3>listOfVertices;
    private List<Vector3> basicShape;
    private List<Vector2> listOfUV;
    private List<float> listOfYValues;
    private List<int> listOfTriangles;

    private Vector3[] Vertices;
    private Vector2[] UV;

    private int[] newTriangles;
    public int factor = 1;


    private float timesinceLastChunk=0,timesinceLastErasedChunk,timeBetweenChunksErase;

    private float yAnt = 0f;
    private float yLerpAnt;
    private float pendiente = 0f;

    private float timeGame = 0f;
    private float maxAngularPeak = 0f;
    private float timeSinceLastCut = 2f;

    private bool evaluate = true;
    private bool left=false;

    public Color fixedColor;



    void OnValidate()
    {
        switch (ArduinoConectedTo)
        {
            case PortAction.OPEN:
                arduinoCommunication.createAndOpenPort(38400);
                break;
            case PortAction.CLOSE:
                arduinoCommunication.closePort();
                break;
            default:
                break;
        }

    }
    void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        for (int i = 0; i < basicPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(basicPoints[i].transform.position, basicPoints[i + 1].transform.position);
        }
        Gizmos.DrawLine(basicPoints[basicPoints.Count - 1].transform.position, basicPoints[0].transform.position);
        if (listOfTriangles != null)
        {
            for (int i = 0; i < listOfVertices.Count; i++)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(listOfVertices[i], 0.1f);
            }
        }
    }

    // Use this for initialization
    void Start () {
        listOfYValues = new List<float>();
        float[] initialList = { 0, 0, 0,0,0,0 };
        listOfYValues.AddRange(initialList);
        GetComponent<MeshFilter>().mesh = thisMesh = new Mesh();
        mCollider = collider.GetComponent<MeshCollider>();
        Vector3[] auxpoints = new Vector3[basicPoints.Count];
        basicShape = new List<Vector3>();
        listOfVertices = new List<Vector3>();
        farolas = new List<GameObject>();
        timeBetweenChunksErase = timeBetweenChunks ;

        pendiente = 100 / maxYSlope;

        listOfUV = new List<Vector2>();
        listOfTriangles = new List<int>();
        listOfUV.Capacity = listOfTriangles.Capacity =listOfVertices.Capacity= 10000;
        

        for (int i = 0; i < basicPoints.Count; i++)
        {
            auxpoints[i] = basicPoints[i].transform.position;
            
        }
       
        listOfVertices.AddRange(auxpoints);
      //  Debug.Log("Inicialmente hay " + listOfVertices.Count + "vertices ");
        basicShape.AddRange(auxpoints);
        timesinceLastErasedChunk = timeToBeginErasing;
      //  Debug.Log("between chunks = " + timeBetweenChunks + " betweenErases" + timeBetweenChunksErase +" initial erase should be at "+( -timesinceLastErasedChunk));

        arduinoCommunication.closePort();
        arduinoCommunication.createAndOpenPort(38400);
        InputData = new float[6] { 0, 0, 0, 0, 0, 0 };
        player = GameObject.FindGameObjectWithTag("Player");
        maxAngularPeak = 2*360;
        fixedColor = fixedMat.color;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float percent = (player.transform.position.y+200) / 400;
        fixedMat.color = Color.Lerp(fixedColor , Color.red*(1-percent),0.5f);
        #region mesh_region
        timeGame += Time.deltaTime;
        float[] angularVals = { InputData[3], InputData[5] };
        float maxCurrentAngularPeak = Mathf.Max(angularVals);
        if (timeGame - timesinceLastChunk > timeBetweenChunks)
        {
            //Debug.Log(timeGame+" - "+timesinceLastChunk +"("+( timeGame - timesinceLastChunk)  +") + >"+ timeBetweenChunks);
            float v = InputData[0];
            if ((Time.time-timeSinceLastCut>timeBetweenCuts)&& ((Input.GetKeyDown(KeyCode.Space) || (maxCurrentAngularPeak>maxAngularPeak*meshCutTreshold)) && player.transform.position.y>-160))
            {
                timeSinceLastCut = Time.time;
                yAnt = yLerpAnt-3;
                listOfYValues.Clear();
                float[] auxY =  {yAnt, yAnt, yAnt, yAnt, yAnt , yAnt  };
    
                listOfYValues.AddRange(auxY);
                SaveOldMesh();
                emptyMeshes();


            }
            else
            {
                for (int i = 0; i < listOfYValues.Count; i++)
                {
                    yLerpAnt += listOfYValues[i];
                }

                if (!evaluate)
                {
                    yLerpAnt += InputData[0];
                    yLerpAnt /= listOfYValues.Count + 1;
                    yLerpAnt = Mathf.Clamp(yLerpAnt, -3.0f, 1.0f);

                    Invoke("changeEvaluate", 2f);
                }
                else
                {
                    yLerpAnt += InputData[0] ;
                    yLerpAnt /= listOfYValues.Count + 1;
                    yLerpAnt = Mathf.Clamp(yLerpAnt, -1.0f, 1.0f);
                   
                }
               
                if (listOfVertices.Count == 0)
                {
                    createBeginingOfMesh(yLerpAnt*maxYSlope);
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    addNewVertices(new Vector3(Input.GetAxis("Vertical"), 0f, InputData[2]) * maxYSlope);
                }
                else
                {
                    addNewVertices(new Vector3(yLerpAnt, 0f, 0) * maxYSlope);
                }

                yAnt = yLerpAnt;

                //Debug.Log(yLerpAnt);
                listOfYValues.Add(yLerpAnt);
                if (listOfYValues.Count > 3)
                {
                    listOfYValues.RemoveAt(0);
                }
            }

            arduinoCommunication.requestInputData();


            timesinceLastChunk = timeGame;
            applyVertexAndTriangleData();

            maxAngularPeak = maxCurrentAngularPeak > maxAngularPeak ? maxCurrentAngularPeak : maxAngularPeak;

        }


        if (timeGame - timesinceLastErasedChunk > timeBetweenChunksErase)
        {
            //Debug.Log("Erasing at " + Time.time + "last time was " + timesinceLastErasedChunk);
            erasechunk();
            timesinceLastErasedChunk = timeGame;
        }

        #endregion

       
        //thisMesh.RecalculateNormals();

    }

    public void addNewVertices(Vector3 speed)
    {
        int shapeVerticesCount = basicShape.Count;
        int[] newTriangle = new int[6];
        //  Debug.Log(speed);
        Vector3 currentVertex = basicShape[0];
        Vector3 auxv = currentVertex + new Vector3(/*speed.y*/ 0f, speed.x, ZDisplacement * factor);
        listOfVertices.Add(auxv);

        float angle =getAngle( listOfVertices[listOfVertices.Count - shapeVerticesCount], auxv);
        //spawnfarolas(angle, speed.x, ZDisplacement * factor);

        for (int i = 1; i < shapeVerticesCount; i++)
        {
           // vi_R = listOfVertices.Count - shapeVerticesCount;
            currentVertex = basicShape[i];
            listOfVertices.Add(currentVertex + new Vector3(/*speed.y*/ 0f, speed.x, ZDisplacement * factor));
            int lastVerticesIndex = listOfVertices.Count - 1;
            newTriangle[0] = lastVerticesIndex - shapeVerticesCount - 1;
            newTriangle[3] = newTriangle[2] = lastVerticesIndex - shapeVerticesCount;
            newTriangle[4] = newTriangle[1] = lastVerticesIndex - 1;
            newTriangle[5] = lastVerticesIndex;

            listOfTriangles.AddRange(newTriangle);
        }
        Vector3 aux = this.transform.position;
        pivot.transform.position = new Vector3(aux.x + /*speed.y*/ 0f, aux.y + speed.x, factor * ZDisplacement);

        float vel = (factor * ZDisplacement - (factor - 1) * ZDisplacement) / Time.deltaTime;
       // Debug.Log(vel);
        factor++;

    }

    private void erasechunk()
    {
        for (int i = 0; i < basicShape.Count; i++)
        {
            listOfVertices.RemoveAt(0);        

        }
        int numberoftriangles = (basicShape.Count - 1) * 2;

        for (int i = 0; i <numberoftriangles ; i++)
        {
            listOfTriangles.RemoveAt(listOfTriangles.Count-1);
            listOfTriangles.RemoveAt(listOfTriangles.Count - 1);
            listOfTriangles.RemoveAt(listOfTriangles.Count - 1);
        }
        // listOfVertices.RemoveAt(0);
    }

    private void applyVertexAndTriangleData()
    {

        thisMesh.Clear();
        newTriangles = new int[listOfTriangles.Count];
        Vertices = new Vector3[listOfVertices.Count];
        for (int i = 0; i < listOfTriangles.Count; i ++)
        {
            newTriangles[i] = listOfTriangles[i];
         /*   if (i == 0 || i % 6 == 0)
            {
                  Debug.DrawLine(listOfVertices[listOfTriangles[i]], listOfVertices[listOfTriangles[i + 1]], Color.red);
                  Debug.DrawLine(listOfVertices[listOfTriangles[i + 1]], listOfVertices[listOfTriangles[i + 2]],Color.green);
                  Debug.DrawLine(listOfVertices[listOfTriangles[i + 2]], listOfVertices[listOfTriangles[i]], Color.blue);

                  Debug.DrawLine(listOfVertices[listOfTriangles[i + 3]], listOfVertices[listOfTriangles[i + 4]], Color.cyan);
                  Debug.DrawLine(listOfVertices[listOfTriangles[i + 4]], listOfVertices[listOfTriangles[i + 5]], Color.magenta);
                  Debug.DrawLine(listOfVertices[listOfTriangles[i + 5]], listOfVertices[listOfTriangles[i + 3]], Color.yellow);
            }*/
        }
        for (int i = 0; i < listOfVertices.Count; i ++)
        {
            Vertices[i] = listOfVertices[i];
        }

        thisMesh.vertices = Vertices;
        // thisMesh.uv = UV;
        thisMesh.triangles = newTriangles;
        thisMesh.RecalculateNormals();
        thisMesh.RecalculateBounds();

        if(mCollider.sharedMesh != null)
        {
            mCollider.sharedMesh=null;
            
        }
        mCollider.sharedMesh = thisMesh;


    }

    private void emptyMeshes()
    {
        listOfTriangles.Clear();
        listOfVertices.Clear();
        listOfUV.Clear();

        factor = (int)(player.transform.position.z / ZDisplacement);




    }

    private void createBeginingOfMesh(float Ydisplacement) {

      //  Debug.Log("creando malla");
        timeGame = 0f;
        timesinceLastChunk = 0f;
        timesinceLastErasedChunk = timeToBeginErasing;
        for (int i = 0; i < basicShape.Count; i++)
        {
            listOfVertices.Add(basicShape[i] + Vector3.forward * ZDisplacement * factor + Vector3.up*Ydisplacement);

        }


    }

    private void SaveOldMesh() {
        List<Vector3> oldVertices = listOfVertices;
        List<int> oldTriangles = listOfTriangles;

        Vector3[] oldVerts = new Vector3[listOfTriangles.Count];
        int[] oldTris=new int[listOfTriangles.Count];

        GameObject oldMesh = new GameObject();


        for (int i = 0; i < oldVertices.Count; i++)
        {
            oldVerts[i] = oldVertices[i];
        }

        for (int i = 0; i < oldTriangles.Count; i++)
        {
            oldTris[i] = oldTriangles[i];
        }


        Mesh mesh = new Mesh();

        oldMesh.AddComponent<MeshFilter>();

        mesh.Clear();

        mesh.vertices = oldVerts;

        mesh.triangles = oldTris;
        mesh.RecalculateNormals();

        oldMesh.GetComponent<MeshFilter>().mesh = mesh;

        oldMesh.AddComponent<MeshRenderer>();
        oldMesh.GetComponent<MeshRenderer>().sharedMaterial = meshMat;


        Destroy(oldMesh, 9f);


    }

    private void changeEvaluate()
    {
        evaluate = false;
    }

    private float getAngle(Vector3 beg,Vector3 end)
    {
       return Vector2.Angle(new Vector2(beg.y, beg.z), new Vector2(end.y, end.z));

    }

    public void spawnfarolas(float angle,float y,float z )
    {
        if (!left)
        {
            farolas.Add(Instantiate(farolaGO, farola1.transform.position + new Vector3(0, y,ZDisplacement*factor), Quaternion.Euler(angle-90, 0, 0))as GameObject);
        }
       /* else
        {
            farolas.Add(Instantiate(farolaGO, farola2.transform.position + new Vector3(0, y, ZDisplacement * factor), Quaternion.Euler(angle - 90, 180, 0)) as GameObject);
        }*/
        left = !left;

    }
}
