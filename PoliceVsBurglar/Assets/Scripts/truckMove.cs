using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class truckMove : MonoBehaviour {

    public GameObject checkpointParent;
    public float collectTime;
    public AudioClip emptyClip;
    public AudioClip coinClip;
    private Animator animator;
    private GameObject dumpster;
    private NavMeshAgent agent;
    private int childCount;
    private int counter;
    public int currentTarget = 0;
    private int coinsCarried;
    private Quaternion lidStartRotation;
    private Quaternion lidEndRotation;
    private AudioSource audioSource;
    private GameController controller;
    public bool atDestination = false;

    void Start()
    {
        lidStartRotation = transform.GetChild(0).rotation;
        lidEndRotation = lidStartRotation;
        lidEndRotation *= Quaternion.Euler(90, 0, 0);
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        childCount = checkpointParent.transform.childCount;
        counter = (int)collectTime*60;
        dumpster = null;
        audioSource = GetComponent<AudioSource>();
        controller = Camera.main.GetComponent<GameController>();
        //agent.updateUpAxis = false;
    }

    private void Update()
    {
        if(controller.gameStarted)
        {
            if (atDestination)
            {
                counter--;
                if (counter <= 0)
                {
                   
                    emptyDumpster();
                    changeTarget();
                    counter = (int)collectTime * 60;
                }
            }
        }
        
        
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "dumpster" && controller.gameStarted)
        {
            dumpster = other.gameObject;
            if(Vector3.Distance(dumpster.transform.parent.transform.position,agent.destination)<2.0f)
            {
                if(dumpster.GetComponent<DumpsterLogic>().coinsInStash > 0)
                {
                    animator.Play("Trunk_Open");
                    dumpster.GetComponent<DumpsterLogic>().OpenLid();
                }
                
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
        if(dumpster.GetComponent<DumpsterLogic>().coinsInStash > 0)
        {
            animator.Play("Trunk_close");
            coinsCarried += dumpster.GetComponent<DumpsterLogic>().coinsInStash;
            controller.CoinGathered(dumpster.GetComponent<DumpsterLogic>().coinsInStash);
            Debug.Log("Coins carried: " + coinsCarried.ToString());
            if(controller.GetCoinsGathered() >= controller.goalLimit)
            {
                dumpster.transform.parent.GetChild(0).gameObject.SetActive(true);
            }
            dumpster.GetComponent<DumpsterLogic>().coinsInStash = 0;
            dumpster.GetComponent<DumpsterLogic>().UpdateValue();
            dumpster.GetComponent<DumpsterLogic>().CloseLid();
            audioSource.clip = coinClip;
            audioSource.Play();
            controller.GetComponent<AudioSource>().clip = emptyClip;
            controller.GetComponent<AudioSource>().Play();
        }
        
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
    public void StartMoving()
    {
        agent.enabled = true;
        agent.destination = checkpointParent.transform.GetChild(currentTarget).transform.position;
    }
}
