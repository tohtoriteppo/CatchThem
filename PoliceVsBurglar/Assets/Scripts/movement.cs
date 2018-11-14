using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {

    public bool isPolice;
    private float speed = 0.1f;
    private int playerNum;
    //for Animator
    private Animator characterAnimator;
    // Use this for initialization
    void Start () {
        playerNum = int.Parse(name.Substring(6, 1));
        characterAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        transform.position = 
            new Vector3(transform.position.x - Input.GetAxis("p"+playerNum.ToString()+"_joystick_horizontal") * speed, 
            transform.position.y,
            transform.position.z - Input.GetAxis("p" + playerNum.ToString() + "_joystick_vertical") * speed);

        //For animator
        //idle
        if((Input.GetAxis("p" + playerNum.ToString() + "_joystick_horizontal"))==0 && (Input.GetAxis("p" + playerNum.ToString() + "_joystick_vertical")) == 0){
                //characterAnimator.SetBool("StartRun", false);
        }
        else{
           // characterAnimator.SetBool("StartRun", true);
        }

    }


public void setSpeed(float speedToSet)
    {
        speed = speedToSet;
    }
    public float getSpeed()
    {
        return speed;
    }
}
