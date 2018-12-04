using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border_Control : MonoBehaviour {
    public Shader shader1 ;
    public Shader shader2 ;
    public GameObject target;
    private List<GameObject> objects;
    // Use this for initialization
    void Start () {
        shader1 = Shader.Find("Mobile/Diffuse");
        target.GetComponent<Renderer>().material.shader = shader1;
        objects = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if(tag=="recharge")
        {
            if(other.tag.Equals("police"))
            {
                target.GetComponent<Renderer>().material.shader = shader2;
                objects.Add(other.gameObject);
            }
        }
        else if (other.tag.Equals("police") || other.tag.Equals("burglar"))
        {
           // Debug.Log("enter");
            target.GetComponent<Renderer>().material.shader = shader2;
            objects.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag.Equals("police") || other.tag.Equals("burglar"))
        {
            //Debug.Log("exit");
            
            objects.Remove(other.gameObject);
            if(objects.Count == 0)
            {
                target.GetComponent<Renderer>().material.shader = shader1;
            }
        }
    }
}
