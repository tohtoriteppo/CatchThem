using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinLogic : MonoBehaviour {

    private List<GameObject> otherCoins;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "coin")
        {
            //robbableObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "coin")
        {
            //robbableObjects.Remove(other.gameObject);
        }
    }

}
