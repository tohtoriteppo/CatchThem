using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponLogic : MonoBehaviour {

    public GameObject bulletPartition;
    public GameObject bulletUI;
    public GameObject bulletBar;
    public GameObject bullet;
    public GameObject rechargeSliderPrefab;
    public List<GameObject> partitions;

    private float bulletSpeed;
    private float width;
    private int rechargeCounter;
    private int automaticRechargeTimePerBullet;
    private int automaticRechargeTimeTotal;
    private int bulletCounter;
    private int bulletCD;
    private int bullets;
    private int maxBullets;
    private int chargeCounter = 0;
    private int chargeTimePerBullet;
    private int playerNum;
    private Movement movement;
    private GameController controller;
    private GameObject rechargeStation;
    private GameObject rechargeSlider;
    // Use this for initialization
    void Start()
    {
        playerNum = int.Parse(name.Substring(6, 1));
        controller = Camera.main.GetComponent<GameController>();
        movement = GetComponent<Movement>();
        SetWeapon();
        SetWeaponUI();
    }

    // Update is called once per frame
    void Update()
    {
        
        FillBulletBar();
        bulletCounter--;
        if (bulletCounter <= 0 && bullets > 0 && Input.GetButtonDown("p" + playerNum.ToString() + "_button_b"))
        {
            Shoot();
        }
        if (rechargeStation != null /*&& Input.GetButton("p" + playerNum.ToString() + "_button_a")*/)
        {
            Recharge();
        }
    }
    public void FillBulletBar()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y+GetComponent<BoxCollider>().size.y, transform.position.z);
        bulletUI.transform.position = Camera.main.WorldToScreenPoint(pos);
        bulletUI.transform.position = new Vector3(bulletUI.transform.position.x, bulletUI.transform.position.y + 10);
        rechargeCounter = Mathf.Min(rechargeCounter+1, automaticRechargeTimeTotal);
        bulletUI.GetComponent<Slider>().value = (float)rechargeCounter / (float)automaticRechargeTimeTotal;
        if(rechargeCounter == automaticRechargeTimePerBullet*(bullets+1))
        {
            AddBullet();
        }
    }
    public void AddBullet()
    {
        bullets++;
    }
    public void RemoveBullet()
    {
        bullets--;
        rechargeCounter -= automaticRechargeTimePerBullet;
        //transform.GetChild(whichBullet).gameObject.SetActive(false);
    }
    private void Shoot()
    {
        GameObject bul = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
        bul.GetComponent<Rigidbody>().velocity = movement.GetDirection() * bulletSpeed;
        bul.transform.position = bul.transform.position + movement.GetDirection();
        bulletCounter = bulletCD;
        RemoveBullet();
    }

    private void Recharge()
    {
        if(bullets<maxBullets)
        {
            Debug.Log("Charging... " + chargeCounter);
            if (chargeCounter > chargeTimePerBullet && bullets < maxBullets)
            {
                chargeCounter = 0;
                AddBullet();
                rechargeCounter = Mathf.Min(rechargeCounter + automaticRechargeTimePerBullet, automaticRechargeTimeTotal);
            }
            rechargeSlider.GetComponent<Slider>().value = ((float)chargeCounter / (float)chargeTimePerBullet);
            chargeCounter++;
        }
        else
        {
            Destroy(rechargeSlider);
        }

    }
    private void SetWeapon()
    {
        bulletSpeed = controller.bulletSpeed;
        bulletCD = controller.bulletCD;
        maxBullets = controller.bulletAmount;
        automaticRechargeTimePerBullet = (int)(controller.automaticRechargeTimePerBullet*60f);
        Debug.Log("AUTO "+automaticRechargeTimePerBullet);
        automaticRechargeTimeTotal = maxBullets * automaticRechargeTimePerBullet;
        chargeTimePerBullet = (int)controller.chargeTimePerBullet * 60;
        bullets = maxBullets;
        rechargeStation = null;
    }
    private void SetWeaponUI()
    {
        rechargeCounter = automaticRechargeTimeTotal;
        bulletUI = Instantiate(bulletUI, GameObject.FindGameObjectWithTag("canvas").transform);
        bulletUI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        //bulletBar = weaponUI.transform.GetChild(0).gameObject;
        width = bulletUI.GetComponent<RectTransform>().sizeDelta.x;
        float widthPerOne = width / maxBullets;
       
        for (int i = 0; i < maxBullets; i++)
        {
            GameObject obj = Instantiate(bulletPartition, bulletUI.transform) as GameObject;
            //PanelSpacerRectTransform.offsetMin = new Vector2(0, 0); new Vector2(left, bottom);
            //PanelSpacerRectTransform.offsetMax = new Vector2(-360, -0); new Vector2(-right, -top);
            obj.GetComponent<RectTransform>().offsetMin = new Vector2(i * widthPerOne, 0); // left,bottom
            obj.GetComponent<RectTransform>().offsetMax = new Vector2(-(maxBullets - i - 1) * widthPerOne, 0); // right,top
            partitions.Add(obj);
        }
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "recharge")
        {
            rechargeStation = other.gameObject;
            if(bullets<maxBullets)
            {
                rechargeSlider = Instantiate(rechargeSliderPrefab, GameObject.FindGameObjectWithTag("canvas").transform);
                rechargeSlider.transform.position = Camera.main.WorldToScreenPoint(other.transform.position);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "recharge")
        {
            rechargeStation = null;
            if (rechargeSlider != null)
            {
                Destroy(rechargeSlider);
            }
            chargeCounter = 0;
        }
    }
}
