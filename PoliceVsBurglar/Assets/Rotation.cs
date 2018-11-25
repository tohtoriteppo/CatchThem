using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {
    public int speed;
    public bool rotate_start;
    public int angle;
    public int maxangle;
    public bool simple;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //for items only rotating 
        if (rotate_start)
        {
            if (angle == 0)
            gameObject.transform.Rotate(Vector3.forward, speed);
            else if (angle == 1)
                gameObject.transform.Rotate(Vector3.right, speed);
            else if (angle == 2)
                gameObject.transform.Rotate(Vector3.up, speed);
        }


	}

}
