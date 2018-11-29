using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public bool isPolice;
    public GameObject directionLight;

    private float speed = 0.1f;
    private int playerNum;
    private Vector3 lastPos;
    private Vector3 direction;
    //for Animator
    private Animator characterAnimator;
    // Use this for initialization
    void Start () {
        playerNum = int.Parse(name.Substring(6, 1));
        characterAnimator = GetComponent<Animator>();
        //directionLight = Instantiate(directionLight, transform);
       // directionLight.transform.position = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Vector3 newDir = (new Vector3(transform.position.x,0, transform.position.z) - new Vector3(lastPos.x,0,lastPos.z)).normalized;
        if (newDir != Vector3.zero)
        {
            direction = newDir;
        }
        transform.rotation = Quaternion.LookRotation(direction);
        lastPos = transform.position;
        transform.position = 
            new Vector3(transform.position.x - Input.GetAxis("p"+playerNum.ToString()+"_joystick_horizontal") * speed, 
            transform.position.y,
            transform.position.z - Input.GetAxis("p" + playerNum.ToString() + "_joystick_vertical") * speed);
        
        //For animator
        //idle
        if ((Input.GetAxis("p" + playerNum.ToString() + "_joystick_horizontal"))==0 && (Input.GetAxis("p" + playerNum.ToString() + "_joystick_vertical")) == 0){
           characterAnimator.Play("IDLE");
        }
        else{
           characterAnimator.Play("RUN");
        }
        
    }


    public void SetSpeed(float speedToSet)
    {
        speed = speedToSet;
    }
    public float GetSpeed()
    {
        return speed;
    }
    public Vector3 GetDirection()
    {
        return direction;
    }

}

