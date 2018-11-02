using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class policeLogic : MonoBehaviour {

    private int playerNum;

	// Use this for initialization
	void Start () {
        playerNum = int.Parse(name.Substring(6, 1));
        GetComponent<movement>().speed = Camera.main.GetComponent<gameController>().policeSpeed;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "burglar")
        {
            other.gameObject.GetComponent<burglarLogic>().Die();
        }
    }
}
