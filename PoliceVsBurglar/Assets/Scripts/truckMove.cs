using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class truckMove : MonoBehaviour {

    public GameObject checkpointParent;
    public float collectTime;

    private GameObject dumpster;
    private NavMeshAgent agent;
    private int childCount;
    private int counter;
    private int currentTarget = 0;
    private int coinsCarried;
    public bool atDestination = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = checkpointParent.transform.GetChild(currentTarget).transform.position;
        childCount = checkpointParent.transform.childCount;
        counter = (int)collectTime*60;
        dumpster = null;
    }

    private void Update()
    {
        
        if(atDestination){
            counter--;
            if(counter <= 0)
            {
                emptyDumpster();
                changeTarget();
                counter = (int)collectTime * 60;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "dumpster")
        {
            dumpster = other.gameObject;
            Debug.Log("MIIT* " + dumpster.transform.parent.transform.position + "  " + agent.destination);
            if(Vector3.Distance(dumpster.transform.parent.transform.position,agent.destination)<2.0f)
            {
                atDestination = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "dumpster")
        {
            dumpster = null;
            //atDestination = false;
        }
    }

    private void emptyDumpster()
    {
        
        coinsCarried += dumpster.GetComponent<dumpsterLogic>().coinsInStash;
        Camera.main.GetComponent<gameController>().coinGathered(dumpster.GetComponent<dumpsterLogic>().coinsInStash);
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
        atDestination = false;
    }


}
