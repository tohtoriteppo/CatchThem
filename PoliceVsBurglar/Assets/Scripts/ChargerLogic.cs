using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerLogic : MonoBehaviour {

    public GameObject chargerUI;
	// Use this for initialization
	void Start () {
        chargerUI = Instantiate(chargerUI, GameObject.FindGameObjectWithTag("UIContainer").transform);
        chargerUI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 pos = new Vector2(chargerUI.transform.position.x, chargerUI.transform.position.y+chargerUI.GetComponent<RectTransform>().sizeDelta.y/2);
        chargerUI.transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
