using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoliceLogic : MonoBehaviour {

    public GameObject dumpsterSliderPrefab;


    private bool isFatness = false;
    private Vector3 originalScale;
    private float slowAmount;
    private float originalSpeed;
    private float minSpeed = 0.01f;
    private float fatnessFactor;
    private float maxScale = 0.5f;
    private int playerNum;
    private int energyPerDonut;
    private int energy;
    private int minEnergy;
    private int dumpsterCounter;
    private int emptyLimit = 150;
    private GameObject cafeteria;
    private GameObject dumpster;
    private GameObject dumpsterSlider;
    private Movement movement;
    private GameController controller;

    // Use this for initialization
    void Start () {
        //playerNum = int.Parse(name.Substring(6, 1));
        controller = Camera.main.GetComponent<GameController>();
        movement = GetComponent<Movement>();
        energyPerDonut = controller.energyPerDonut;
        energy = controller.startEnergy;
        dumpsterCounter = 0;
        minEnergy = -energy;
        dumpsterSlider = null;
        cafeteria = null;
        originalSpeed = movement.GetSpeed();
        slowAmount = originalSpeed / energy;
        originalScale = transform.localScale;
        fatnessFactor = originalScale.x / energy;
    }
	
	// Update is called once per frame
	void Update () {

        if(dumpster!=null /*&& Input.GetButton("p" + playerNum.ToString() + "_button_a")*/)
        {
            EmptyDumpster();
        }
        if(isFatness)
        {
            if (cafeteria != null && Input.GetButtonDown("p" + playerNum.ToString() + "_button_x"))
            {
                BuyDonut();
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
                ChangeShape(true);
            }
            else
            {
                ChangeShape(false);
            }
        }
        

    }

    
    private void ChangeShape(bool getFaster)
    {
        if(getFaster)
        {
            //GetComponent<movement>().setSpeed(Mathf.Max(minSpeed, originalSpeed - energy * slowAmount));
            
        }
        else
        {
            movement.SetSpeed(Mathf.Max(minSpeed, originalSpeed + energy * slowAmount));
        }
        transform.localScale = new Vector3(
                originalScale.x + fatnessFactor * energy * 0.5f,
                originalScale.y + fatnessFactor * energy * 0.5f,
                originalScale.z + fatnessFactor * energy * 0.5f);


    }
    private void BuyDonut()
    {
        //energy = Mathf.Max(energy, 0);
        energy += energyPerDonut;

        //GetComponent<movement>().setSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
    }
    private void EmptyDumpster()
    {
        if (dumpster.GetComponent<DumpsterLogic>().coinsInStash > 0)
        {
            dumpsterCounter++;
            dumpsterSlider.GetComponent<Slider>().value = ((float)dumpsterCounter / (float)emptyLimit);
            if (dumpsterCounter > emptyLimit)
            {
                dumpster.GetComponent<DumpsterLogic>().EmptyCoin();
                dumpsterCounter = 0;
                //ANIM emptycoin
            }
        }
        else
        {
            Destroy(dumpsterSlider);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
       if(other.tag == "dumpster")
        {
            dumpster = other.gameObject;
            if(dumpster.GetComponent<DumpsterLogic>().coinsInStash > 0)
            {
                dumpsterSlider = Instantiate(dumpsterSliderPrefab, GameObject.FindGameObjectWithTag("UIContainer").transform);
                dumpsterSlider.transform.position = Camera.main.WorldToScreenPoint(other.transform.position);
            }
            
        }
        if(other.tag == "cafeteria")
        {
            cafeteria = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "dumpster")
        {
            dumpster = null;
            dumpsterCounter = 0;
            if(dumpsterSlider!= null)
            {
                Destroy(dumpsterSlider);
            }
        }
        if (other.tag == "cafeteria")
        {
            cafeteria = null;
        }

    }
    public void SetPlayerNum(int number)
    {
        playerNum = number;
    }
    

}
