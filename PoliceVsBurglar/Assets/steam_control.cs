using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class steam_control : MonoBehaviour {

    public GameObject target;
    public ParticleSystem steam;
    // Use this for initialization
    void Start()
    {
        steam.Stop();
     }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag.Equals("police")|| other.tag.Equals("burglar"))
        {
            Debug.Log("enter");
            if (!steam.isPlaying)
                steam.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag.Equals("police") || other.tag.Equals("burglar"))
        {
                steam.Stop();
        }
    }
}
