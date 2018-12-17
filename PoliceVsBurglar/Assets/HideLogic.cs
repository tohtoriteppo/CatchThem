using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLogic : MonoBehaviour {

    public GameObject hideUI;
	// Use this for initialization
	void Start () {
        setUI();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void setUI()
    {
        hideUI = Instantiate(hideUI, GameObject.FindGameObjectWithTag("UIContainer").transform);
        hideUI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        hideUI.transform.position = new Vector2(hideUI.transform.position.x-73, hideUI.transform.position.y+50);
    }
}
