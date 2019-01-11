using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6007 - Machine Learning SideScrolling game
//Script Purpose: Simple trigger enter to tell the agent that they have reached their goal
//////////////////////////////////////////////////////////////////
public class CS_GoalContact : MonoBehaviour
{
    /// <summary>
    /// Called when [trigger enter].
    /// </summary>
    /// <param name="other">The other.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//If the collider is the agent
        {
            other.GetComponent<SideScrollerAgent>().GotToEnd();//Alert that they have finished
        }
    }
}