using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction_End : MonoBehaviour {
    public GameController gamecontroller;
	// Use this for initialization
	void Start () {
        gamecontroller = Camera.main.GetComponent<GameController>();
        gamecontroller.StartGame();
        Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	}
}
