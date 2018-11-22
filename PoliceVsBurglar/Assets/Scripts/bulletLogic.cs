using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletLogic : MonoBehaviour {

    private int lifeCounter;
	// Use this for initialization
	void Awake () {
        //GetComponent<Rigidbody>().velocity = new Vector3(10, 0, 10);
        lifeCounter = (int)Camera.main.GetComponent<gameController>().bulletLifeTime*60;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("river");
        foreach(GameObject obj in objs)
        {
            Physics.IgnoreCollision(obj.GetComponent<Collider>(), GetComponent<Collider>());
        }

    }
	
	// Update is called once per frame
	void Update () {
        lifeCounter--;
        if(lifeCounter <= 0)
        {
            Destroy(gameObject);
        }
    }
}
