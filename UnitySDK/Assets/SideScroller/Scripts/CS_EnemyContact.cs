using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6007 - Machine Learning SideScrolling game
//Script Purpose: Simple trigger enter to tell the agent that they have collided with an enemy
//////////////////////////////////////////////////////////////////
public class CS_EnemyContact : MonoBehaviour
{
    [SerializeField]
    private GameObject m_goParentObject;//The parent enemy gameobject (Specified in inspector)

    private bool m_bSideCheck = true;//If the enemy has touched an edge or a side

    /// <summary>
    /// Called when [trigger enter].
    /// </summary>
    /// <param name="other">The other collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//If the collider is the player
        {
            if (other.transform.position.y - (other.transform.localScale.y / 2) > (m_goParentObject.transform.position.y + m_goParentObject.transform.localScale.y / 2))//If the player is above the enemy
            {
                other.GetComponent<SideScrollerAgent>().EnemyStomped();//Stomp the enemy
                Destroy(m_goParentObject);//Destroy the enemy
            }
            else
            {
                other.GetComponent<SideScrollerAgent>().Killed();//Enemy killed the player
            }
        }
        if (other.CompareTag("SideScrollerWall") && m_bSideCheck)//If the enemy bumps into a wall
        {
            m_goParentObject.GetComponent<CS_EnemyController>().SwitchDirection();//Change direction
            m_bSideCheck = false;
        }
        if (other.CompareTag("Hole") && m_bSideCheck)//If the enemy walks next to an edge
        {
            m_goParentObject.GetComponent<CS_EnemyController>().SwitchDirection();//Change direction
            m_bSideCheck = false;
        }
        if (other.CompareTag("Enemy") && m_bSideCheck)//If the enemy bumps into an enemy
        {
            m_goParentObject.GetComponent<CS_EnemyController>().SwitchDirection();//Change direction
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