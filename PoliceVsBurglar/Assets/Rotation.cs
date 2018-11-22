using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {
    public int speed;
    public bool rotate_start;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (rotate_start)
        {
            gameObject.transform.Rotate(Vector3.forward, speed);
        }
	}
}
