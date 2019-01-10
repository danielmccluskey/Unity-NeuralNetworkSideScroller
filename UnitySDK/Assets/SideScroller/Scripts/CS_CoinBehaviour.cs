using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CoinBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject m_goParentRef;

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
            other.GetComponent<SideScrollerAgent>().CoinCollected();
            Debug.Log("Coin Collected!");
            Destroy(m_goParentRef);
            Destroy(gameObject);
        }
    }
}