using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dumpsterLogic : MonoBehaviour {

    public int coinsInStash = 10;
    public GameObject coinText;
    private Vector3 screenPos;
    // Use this for initialization
    void Start () {
        screenPos = Camera.main.WorldToScreenPoint(transform.position);
        coinText = Instantiate(coinText, GameObject.FindGameObjectWithTag("canvas").transform) as GameObject;
        coinText.GetComponent<Text>().text = coinsInStash.ToString();
        coinText.transform.position = screenPos;

    }

    // Update is called once per frame
    void Update () {
		
	}
    public void updateValue()
    {
        coinText.GetComponent<Text>().text = coinsInStash.ToString();
    }
}
