using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_EnemyController : MonoBehaviour
{
    [SerializeField]
    private float m_fSpeed = 1;

    private int m_iDirection = 1;

    [SerializeField]
    private Transform m_tLeftEdge;

    [SerializeField]
    private Transform m_tRightEdge;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += new Vector3((m_iDirection * m_fSpeed) * Time.deltaTime, 0.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
    }

    private void CheckForHoles()
    {
    }

    public void SwitchDirection()
    {
        if (m_iDirection == 1)
        {
            m_iDirection = -1;
        }
        else
        {
            m_iDirection = 1;
        }
    }
}