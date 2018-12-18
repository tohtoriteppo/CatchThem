using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survive : MonoBehaviour {

    private bool run = false;
	// Use this for initialization
	void Awake () {
        if(!run)
        {
            if (GameObject.FindGameObjectsWithTag("ambience").Length > 1)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(transform.gameObject);
            run = true;
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
