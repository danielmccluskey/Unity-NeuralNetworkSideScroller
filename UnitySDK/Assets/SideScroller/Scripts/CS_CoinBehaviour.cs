using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
//Created by: Daniel McCluskey
//Project: CT6007 - Machine Learning SideScrolling game
//Script Purpose: Simple trigger enter to tell the agent that they have collected a coin
//////////////////////////////////////////////////////////////////
public class CS_CoinBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject m_goParentRef;//The parent object of this object (Specified in Inspector)

    /// <summary>
    /// Called when [trigger enter].
    /// </summary>
    /// <param name="other">The other collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<SideScrollerAgent>().CoinCollected();//Alert that coin has been collected
            Debug.Log("Coin Collected!");//Debug message
            Destroy(m_goParentRef);//Destroy self
            Destroy(gameObject);//Destroy self
        }
    }
}