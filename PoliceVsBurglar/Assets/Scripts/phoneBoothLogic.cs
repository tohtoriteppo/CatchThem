using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class phoneBoothLogic : MonoBehaviour {

    public GameObject targetPhoneBooth;
    private Transform coin;
    private Vector3 coinStartPos;
    private Vector3 coinEndPos;
    private float distance;
    private float step;
    private float endZ = 0.010f;
	// Use this for initialization
	void Start () {
        coin = transform.GetChild(0);
        coinStartPos = coin.transform.localPosition;
        coinEndPos = new Vector3(coinStartPos.x, coinStartPos.y, endZ);
        distance = coinStartPos.z - endZ;
        step = distance / 120f;
        Debug.Log("ASETP "+step);
    }
	
	// Update is called once per frame
	void Update () {
        coin.transform.localPosition = Vector3.MoveTowards(coin.transform.localPosition, coinEndPos, step);
        if(coin.transform.localPosition == coinEndPos)
        {
            Debug.Log("HEEP");
            coin.transform.localPosition = coinStartPos;
        }
    }
    

}
