﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {

    public float speed = 0.1f;
    private int playerNum;
	// Use this for initialization
	void Start () {
        playerNum = int.Parse(name.Substring(6, 1));
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("p1_button_y"))
        {
            Debug.Log("ENS");
        }
        transform.position = 
            new Vector3(transform.position.x + Input.GetAxis("p"+playerNum.ToString()+"_joystick_horizontal") * speed, 
            transform.position.y,
            transform.position.z + Input.GetAxis("p" + playerNum.ToString() + "_joystick_vertical") * speed);

    }
}
