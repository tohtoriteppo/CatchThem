using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class truckMove : MonoBehaviour {

    public GameObject checkpointParent;
    public int counterLimit = 120;

    private GameObject dumpster;
    private NavMeshAgent agent;
    private int childCount;
    private int counter;
    private int currentTarget = 0;
    private int coinsCarried;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = checkpointParent.transform.GetChild(currentTarget).transform.position;
        childCount = checkpointParent.transform.childCount;
        counter = counterLimit;
        dumpster = null;
    }

    private void Update()
    {
        
        if (transform.position.x == agent.destination.x && transform.position.z == agent.destination.z)
        {
            counter--;
            if(counter <= 0)
            {
                emptyDumpster();
                changeTarget();
                counter = counterLimit;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "dumpster")
        {
            dumpster = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "dumpster")
        {
            dumpster = null;
        }
    }

    private void emptyDumpster()
    {
        
        coinsCarried += dumpster.GetComponent<dumpsterLogic>().coinsInStash;
        dumpster.GetComponent<dumpsterLogic>().coinsInStash = 0;
        dumpster.GetComponent<dumpsterLogic>().updateValue();
        Debug.Log("ONNN ny " + coinsCarried);
    }
    private void changeTarget()
    {
        currentTarget++;
        if(currentTarget>=childCount)
        {
            currentTarget = 0;
        }
        agent.destination = checkpointParent.transform.GetChild(currentTarget).transform.position;
    }


}
