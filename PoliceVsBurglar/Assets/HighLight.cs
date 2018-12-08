using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLight : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Animator>().Play("Highlighted");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
