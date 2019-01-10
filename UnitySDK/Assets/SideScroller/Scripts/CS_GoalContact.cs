using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GoalContact : MonoBehaviour
{
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
            other.GetComponent<SideScrollerAgent>().GotToEnd();
        }
    }
}