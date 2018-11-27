using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border_Control : MonoBehaviour {
    public Shader shader1 ;
    public Shader shader2 ;
    public GameObject target;
    // Use this for initialization
    void Start () {
        shader1 = Shader.Find("Mobile/Diffuse");
        target.GetComponent<Renderer>().material.shader = shader1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag.Equals("police")|| other.tag.Equals("burglar"))
        {
            Debug.Log("enter");
            target.GetComponent<Renderer>().material.shader = shader2;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag.Equals("police") || other.tag.Equals("burglar"))
        {
            Debug.Log("exit");
            target.GetComponent<Renderer>().material.shader = shader1;
        }
    }
}
