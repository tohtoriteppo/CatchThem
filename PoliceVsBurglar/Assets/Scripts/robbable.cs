using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class robbable : MonoBehaviour {

    public int robAmount = 10;
    public GameObject coinsLeft;

    private Vector3 screenPos;
    // Use this for initialization
    void Start () {
        screenPos = Camera.main.WorldToScreenPoint(transform.position);
        coinsLeft = Instantiate(coinsLeft, GameObject.FindGameObjectWithTag("canvas").transform) as GameObject;
        coinsLeft.GetComponent<Text>().text = robAmount.ToString();
        coinsLeft.transform.position = screenPos;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void updateValue()
    {
        coinsLeft.GetComponent<Text>().text = robAmount.ToString();
    }
}
