using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6007 - Machine Learning SideScrolling game
//Script Purpose: Simple script to move the enemies in the game
//////////////////////////////////////////////////////////////////
public class CS_EnemyController : MonoBehaviour
{
    [SerializeField]
    private float m_fSpeed = 1;//The speed of the enemy

    private int m_iDirection = 1;//The current direction the enemy is moving in

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += new Vector3((m_iDirection * m_fSpeed) * Time.deltaTime, 0.0f, 0.0f);
    }

    /// <summary>
    /// Used to draw gizmo on all enemies
    /// </summary>
    private void OnDrawGizmos()
    {
    }

    /// <summary>
    /// Switches the direction of the enemy.
    /// </summary>
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