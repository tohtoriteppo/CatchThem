using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class policeLogic : MonoBehaviour {

    private int playerNum;
    private Vector3 lastPos;
    private Vector3 direction;
    private float bulletSpeed;
    private int bulletCounter;
    private int bulletCD;
    private gameController controller;

    public GameObject bullet;
    

	// Use this for initialization
	void Start () {
        playerNum = int.Parse(name.Substring(6, 1));
        controller = Camera.main.GetComponent<gameController>();
        bulletSpeed = controller.bulletSpeed;
        bulletCD = controller.bulletCD;
    }
	
	// Update is called once per frame
	void Update () {
        direction = (transform.position - lastPos).normalized;
        lastPos = transform.position;
        bulletCounter++;
        if(bulletCounter > bulletCD && Input.GetButtonDown("p" + playerNum.ToString() + "_button_b"))
        {
            shoot();
        }
    }

    private void shoot()
    {
        GameObject bul = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
        bul.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
        bul.transform.position = bul.transform.position + direction;
        bulletCounter = 0;
    }

    
}
