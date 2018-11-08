using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class policeLogic : MonoBehaviour {

    private int playerNum;
    private Vector3 lastPos;
    private Vector3 direction;
    private float bulletSpeed;
    private int bulletCounter;
    private int bulletCD;
    private int chargeCounter = 0;
    private int bullets;
    private int maxBullets;
    private int chargeTimePerBullet;
    private gameController controller;
    private GameObject rechargeStation;
    private GameObject rechargeSlider;

    public GameObject bullet;
    public GameObject rechargeSliderPrefab;
    


    // Use this for initialization
    void Start () {
        playerNum = int.Parse(name.Substring(6, 1));
        controller = Camera.main.GetComponent<gameController>();
        bulletSpeed = controller.bulletSpeed;
        bulletCD = controller.bulletCD;
        maxBullets = controller.bulletAmount;
        chargeTimePerBullet = controller.chargeTimePerBullet;
        bullets = maxBullets;
        rechargeStation = null;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 newDir = (transform.position - lastPos).normalized;
        if (newDir != Vector3.zero)
        {
            direction = newDir;
        }
        lastPos = transform.position;
        bulletCounter--;
        if(bulletCounter <= 0 && bullets > 0 && Input.GetButtonDown("p" + playerNum.ToString() + "_button_b"))
        {
            shoot();
        }
        if (rechargeStation != null && Input.GetButton("p" + playerNum.ToString() + "_button_a"))
        {
            recharge();
        }
    }

    private void shoot()
    {
        GameObject bul = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
        bul.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
        bul.transform.position = bul.transform.position + direction;
        bulletCounter = bulletCD;
        bullets--;     
    }
    private void recharge()
    {
        Debug.Log("Charging... "+chargeCounter);
        if(chargeCounter > chargeTimePerBullet && bullets < maxBullets)
        {
            chargeCounter = 0;
            bullets++;
        }
        rechargeSlider.GetComponent<Slider>().value = ((float)chargeCounter / (float)chargeTimePerBullet);
        chargeCounter++;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "recharge")
        {
            Debug.Log("ENTERED");
            rechargeStation = other.gameObject;
            rechargeSlider = Instantiate(rechargeSliderPrefab, GameObject.FindGameObjectWithTag("canvas").transform);
            rechargeSlider.transform.position = Camera.main.WorldToScreenPoint(other.transform.position);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "recharge")
        {
            Debug.Log("EXited");
            rechargeStation = null;
            Destroy(rechargeSlider);
            chargeCounter = 0;
        }
        
    }
    

}
