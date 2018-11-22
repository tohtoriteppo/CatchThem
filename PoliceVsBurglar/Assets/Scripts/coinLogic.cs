using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinLogic : MonoBehaviour {

    private int hitBoxCounter;
	// Use this for initialization
	void Start () {
        hitBoxCounter = (int)Camera.main.GetComponent<gameController>().coinBlockTime*60;
        GameObject[] burglars = GameObject.FindGameObjectsWithTag("burglar");
        GameObject[] police = GameObject.FindGameObjectsWithTag("police");
        foreach(GameObject obj in burglars)
        {
            Physics.IgnoreCollision(obj.GetComponent<Collider>(), GetComponent<Collider>());
        }
        foreach (GameObject obj in police)
        {
            Physics.IgnoreCollision(obj.GetComponent<Collider>(), GetComponent<Collider>());
        }

    }
	
	// Update is called once per frame
	void Update () {
        hitBoxCounter--;
        if(hitBoxCounter <= 0)
        {
            Destroy(gameObject);
        }
	}

}
