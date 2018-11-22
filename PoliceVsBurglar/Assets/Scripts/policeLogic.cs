using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class policeLogic : MonoBehaviour {

    private int playerNum;
    private Vector3 lastPos;
    private Vector3 direction;
    private Vector3 originalScale;
    private float bulletSpeed;
    private float slowAmount;
    private float originalSpeed;
    private float minSpeed = 0.01f;
    private float fatnessFactor;
    private int bulletCounter;
    private int bulletCD;
    private int chargeCounter = 0;
    private int bullets;
    private int maxBullets;
    private int chargeTimePerBullet;
    private int energyPerDonut;
    private int energy;
    private int minEnergy;
    private float maxScale = 0.5f;
    private gameController controller;
    private GameObject rechargeStation;
    private GameObject rechargeSlider;
    private GameObject cafeteria;


    public GameObject bullet;
    public GameObject rechargeSliderPrefab;


    private bool isFatness = false;
    


    // Use this for initialization
    void Start () {
        playerNum = int.Parse(name.Substring(6, 1));
        controller = Camera.main.GetComponent<gameController>();
        bulletSpeed = controller.bulletSpeed;
        bulletCD = controller.bulletCD;
        maxBullets = controller.bulletAmount;
        chargeTimePerBullet = (int)controller.chargeTimePerBullet*60;
        bullets = maxBullets;
        energyPerDonut = controller.energyPerDonut;
        energy = controller.startEnergy;
        
        minEnergy = -energy;
        rechargeStation = null;
        cafeteria = null;
        originalSpeed = GetComponent<movement>().getSpeed();
        slowAmount = originalSpeed / energy;
        originalScale = transform.localScale;
        fatnessFactor = originalScale.x / energy;
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
        if(isFatness)
        {
            if (cafeteria != null && Input.GetButtonDown("p" + playerNum.ToString() + "_button_x"))
            {
                buyDonut();
            }
            if (name == "player4")
            {
                //Debug.Log("ENERGY " + energy);
            }
            if (energy > minEnergy)
            {
                energy--;
            }
            if (energy > 0)
            {
                changeShape(true);
            }
            else
            {
                changeShape(false);
            }
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
    private void changeShape(bool getFaster)
    {
        if(getFaster)
        {
            //GetComponent<movement>().setSpeed(Mathf.Max(minSpeed, originalSpeed - energy * slowAmount));
            
        }
        else
        {
            GetComponent<movement>().setSpeed(Mathf.Max(minSpeed, originalSpeed + energy * slowAmount));
        }
        transform.localScale = new Vector3(
                originalScale.x + fatnessFactor * energy * 0.5f,
                originalScale.y + fatnessFactor * energy * 0.5f,
                originalScale.z + fatnessFactor * energy * 0.5f);


    }
    private void buyDonut()
    {
        //energy = Mathf.Max(energy, 0);
        energy += energyPerDonut;

        //GetComponent<movement>().setSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
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
        if(other.tag == "cafeteria")
        {
            cafeteria = other.gameObject;
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
        if (other.tag == "cafeteria")
        {
            cafeteria = null;
        }

    }
    

}
