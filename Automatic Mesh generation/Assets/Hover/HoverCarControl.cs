using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class HoverCarControl : MonoBehaviour
{
    Rigidbody m_body;
    float m_deadZone = 0.1f;
    public GameObject end;
    public float m_hoverForce = 9.0f;
    public float m_ReturnForce = 100f;
    public float m_hoverHeight = 2.0f;
    public GameObject[] m_hoverPoints;
    float maxDist = 1530;

    public float m_forwardAcl = 100.0f;
    public float maxVel;
    public static float dist;
    float m_currThrust = 0.0f;

    public ParticleSystem pM,pI,pD;

    int m_layerMask;

    void Start()
    {
        m_body = GetComponent<Rigidbody>();

        m_layerMask = 1 << LayerMask.NameToLayer("Characters");
        m_layerMask = ~m_layerMask;
    }

    void OnDrawGizmos()
    {

        //  Hover Force
        RaycastHit hit;
        for (int i = 0; i < m_hoverPoints.Length; i++)
        {
            var hoverPoint = m_hoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position,
                                -Vector3.up, out hit,
                                m_hoverHeight,
                                m_layerMask))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(hoverPoint.transform.position, hit.point);
                Gizmos.DrawSphere(hit.point, 0.5f);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(hoverPoint.transform.position,
                               hoverPoint.transform.position - Vector3.up * m_hoverHeight);
            }
        }
    }

    void FixedUpdate()
    {
        move();
        //  Hover Force
        RaycastHit hit;
        for (int i = 0; i < m_hoverPoints.Length; i++)
        {
            var hoverPoint = m_hoverPoints[i];


            if (Physics.Raycast(hoverPoint.transform.position, -Vector3.up, out hit, m_hoverHeight, m_layerMask))
            {
                m_body.AddForceAtPosition(Vector3.up * m_hoverForce * (1.0f - (hit.distance / m_hoverHeight)), hoverPoint.transform.position);
                if (hoverPoint.name == "hoverPoint1" || hoverPoint.name == "hoverPoint2")
                {
                    activateParticle(pM);
                }

                else if (hoverPoint.name == "hoverPoint3") // I
                {
                    activateParticle(pI);
                }
                else //D
                {
                    activateParticle(pD);
                }

            }
            else
            {
                if (hoverPoint.name == "hoverPoint1" || hoverPoint.name == "hoverPoint2")
                {
                    deactivateParticle(pM);
                }

                else if (hoverPoint.name == "hoverPoint3") // I
                {
                    deactivateParticle(pI);
                }
                else //D
                {
                    deactivateParticle(pD);
                }
                if (transform.position.y > hoverPoint.transform.position.y)
                {
                    m_body.AddForceAtPosition(hoverPoint.transform.up * m_hoverForce, hoverPoint.transform.position);


                }
                else
                {
                    m_body.AddForceAtPosition(hoverPoint.transform.up * -m_ReturnForce, hoverPoint.transform.position);


                }
            }


          
        }
    }

    public void move()
    {
        dist = Mathf.Abs(end.transform.position.z - this.transform.position.z);

        dist = dist / maxDist;
        Vector3 currV = m_body.velocity;
        currV.z = (30f / Time.deltaTime) * dist;
        m_body.velocity = currV;
    }

    private void activateParticle(ParticleSystem p)
    {
        if (!p.isPlaying) p.Play();
    
    }
    private void deactivateParticle(ParticleSystem p)
    {
        if (p.isPlaying) p.Stop();

    }
}
