﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletLogic : MonoBehaviour {

    private int lifeCounter;
    private AudioSource audioSource;
    private float speedMag;
    // Use this for initialization
    void Awake () {
        //GetComponent<Rigidbody>().velocity = new Vector3(10, 0, 10);
        lifeCounter = (int)Camera.main.GetComponent<GameController>().bulletLifeTime*60;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("river");
        foreach(GameObject obj in objs)
        {
            Physics.IgnoreCollision(obj.GetComponent<Collider>(), GetComponent<Collider>());
        }
        
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        speedMag = GetComponent<Rigidbody>().velocity.magnitude;
    }

    // Update is called once per frame
    void Update () {
        GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * speedMag;
        speedMag = GetComponent<Rigidbody>().velocity.magnitude;
        lifeCounter--;
        if(lifeCounter <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        audioSource.Play();

    }
}
