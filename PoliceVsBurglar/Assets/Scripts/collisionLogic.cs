﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "unpassable")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }

}
