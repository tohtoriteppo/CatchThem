using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class truckMove : MonoBehaviour {

    public GameObject checkpointParent;
    public float collectTime;

    private Animator animator;
    private GameObject dumpster;
    private NavMeshAgent agent;
    private int childCount;
    private int counter;
    private int currentTarget = 0;
    private int coinsCarried;
    private Quaternion lidStartRotation;
    private Quaternion lidEndRotation;
    private AudioSource audioSource;
    public bool atDestination = false;

    void Start()
    {
        lidStartRotation = transform.GetChild(0).rotation;
        lidEndRotation = lidStartRotation;
        lidEndRotation *= Quaternion.Euler(90, 0, 0);
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = checkpointParent.transform.GetChild(currentTarget).transform.position;
        childCount = checkpointParent.transform.childCount;
        counter = (int)collectTime*60;
        dumpster = null;
        audioSource = GetComponent<AudioSource>();
        //agent.updateUpAxis = false;
    }

    private void Update()
    {
        
        if(atDestination){
            counter--;
            if(counter <= 0)
            {
                animator.Play("Trunk_close");
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
            if(Vector3.Distance(dumpster.transform.parent.transform.position,agent.destination)<2.0f)
            {
                animator.Play("Trunk_Open");
                dumpster.GetComponent<DumpsterLogic>().OpenLid();
                //OpenLid();
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
        
        coinsCarried += dumpster.GetComponent<DumpsterLogic>().coinsInStash;
        Camera.main.GetComponent<GameController>().CoinGathered(dumpster.GetComponent<DumpsterLogic>().coinsInStash);
        dumpster.GetComponent<DumpsterLogic>().coinsInStash = 0;
        dumpster.GetComponent<DumpsterLogic>().UpdateValue();
        dumpster.GetComponent<DumpsterLogic>().CloseLid();
        audioSource.Play();
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
    private void OpenLid()
    {
        transform.GetChild(0).rotation = lidStartRotation * Quaternion.Euler(90 / (counter + 1), 0, 0);
    }
    private void CloseLid()
    {
        transform.GetChild(0).rotation = lidStartRotation * Quaternion.Euler(90 / (counter + 1), 0, 0);
    }
}
