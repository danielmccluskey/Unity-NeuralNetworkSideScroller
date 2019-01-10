using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_EnemyContact : MonoBehaviour
{
    [SerializeField]
    private GameObject m_goParentObject;

    private bool m_bSideCheck = true;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.y - (other.transform.localScale.y / 2) > (transform.position.y + transform.localScale.y / 2))
            {
                other.GetComponent<SideScrollerAgent>().EnemyStomped();
                Destroy(m_goParentObject);
            }
            else
            {
                other.GetComponent<SideScrollerAgent>().Killed();
            }
        }
        if (other.CompareTag("SideScrollerWall") && m_bSideCheck)
        {
            m_goParentObject.GetComponent<CS_EnemyController>().SwitchDirection();
            m_bSideCheck = false;
        }
        if (other.CompareTag("Hole") && m_bSideCheck)
        {
            m_goParentObject.GetComponent<CS_EnemyController>().SwitchDirection();
            m_bSideCheck = false;
        }
        if (other.CompareTag("Enemy") && m_bSideCheck)
        {
            m_goParentObject.GetComponent<CS_EnemyController>().SwitchDirection();
            m_bSideCheck = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SideScrollerWall"))
        {
            m_bSideCheck = true;
        }
        if (other.CompareTag("Hole"))
        {
            m_bSideCheck = true;
        }
        if (other.CompareTag("Enemy"))
        {
            m_bSideCheck = true;
        }
    }
}