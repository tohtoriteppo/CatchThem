using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("p1_button_b"))
        {
            SceneManager.LoadScene("TeoTruck", LoadSceneMode.Single);
        }
    }
}
