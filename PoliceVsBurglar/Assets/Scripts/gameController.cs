using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public int goalLimit;
    public int bankCoinsTotal;
    public int maxCoinsInBank;
    public float burglarSpeed;
    public float slowPerCoin;
    public float policeSpeed;
    public float bulletSpeed;
    public float fatSlowFactor;
    public int bulletCD;
    public float bulletLifeTime;
    public float chargeTimePerBullet;
    public float automaticRechargeTimePerBullet;
    public int bulletAmount;
    public float bankRefillTime;
    public float coinBlockTime;
    public int startEnergy;
    public int energyPerDonut;

    public GameObject winText;
    public GameObject timeLeftText;
    public GameObject timeSlider;
    public GameObject coinsText;
    public GameObject goalUI;
    public GameObject goalPartition;
    public GameObject startMenu;
    public GameObject selectionArrow;
    public List<GameObject> startMenuButtons;

    private int menuSelection;
    public bool inStartMenu;
    private int coinsLeft;
    private float timer = 0;
    //private 
    private float timeLimit = 60.0f;
    private GameObject[] spawnPoints;
	// Use this for initialization
	void Start () {
        foreach (string t in Input.GetJoystickNames())
        {
            Debug.Log(t);
        }
        GameObject[] burglars = GameObject.FindGameObjectsWithTag("burglar");
        foreach(GameObject obj in burglars)
        {
            obj.GetComponent<Movement>().SetSpeed(burglarSpeed);
        }
        GameObject[] police = GameObject.FindGameObjectsWithTag("police");
        Debug.Log("police " + police.Length);
        foreach (GameObject obj in police)
        {
            obj.GetComponent<Movement>().SetSpeed(policeSpeed);
        }
        coinsLeft = 0;
        coinsText.GetComponent<Text>().text = coinsLeft.ToString() + "/" + goalLimit.ToString();
        spawnPoints = GameObject.FindGameObjectsWithTag("spawnLocation");
        GameObject[] banks = GameObject.FindGameObjectsWithTag("robbable");
        int perBank = bankCoinsTotal / banks.Length;
        int leftovers = bankCoinsTotal % banks.Length;
        for (int i = 0; i < banks.Length; i++)
        {

            banks[i].GetComponent<Robbable>().robAmount = perBank;
            if(leftovers > 0)
            {
                banks[i].GetComponent<Robbable>().ProduceMoney();
                leftovers--;
            }
            banks[i].GetComponent<Robbable>().UpdateValue();
        }
        //startMenuButtons = new List<GameObject>();
        //startMenu.SetActive(true);
        SetGoalUI();
    }
	
	// Update is called once per frame
	void Update () {
        if(inStartMenu)
        {
            if (Input.GetButtonDown("p1_button_a"))
            {
                inStartMenu = false;
                startMenu.SetActive(false);
            }
            float dir = Input.GetAxis("p1_joystick_vertical");
            //move to play button
            if (dir>0)
            {
                menuSelection = 0;
                selectionArrow.transform.position = new Vector2(selectionArrow.transform.position.x, startMenuButtons[menuSelection].transform.position.y);
            }
            //move to exit button
            else if(dir<0)
            {
                menuSelection = 1;
                selectionArrow.transform.position = new Vector2(selectionArrow.transform.position.x, startMenuButtons[menuSelection].transform.position.y);
            }
        }
        
        else
        {
            
            timer = Time.timeSinceLevelLoad;
            timeSlider.GetComponent<Slider>().value = timer / timeLimit;
            timeLeftText.GetComponent<Text>().text = ((int)(60 - timer) + 1).ToString();
            if (timer > timeLimit)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                timer = 0;
            }
        }
        
        
	}

    public void CoinGathered(int howMany)
    {
        for(int i = coinsLeft; i<Mathf.Min(coinsLeft+howMany, goalLimit); i++)
        {
            goalUI.transform.GetChild(i).gameObject.SetActive(true);
        }
        coinsLeft += howMany;
        if (coinsLeft >= goalLimit)
        {
            winText.SetActive(true);
        }
        coinsText.GetComponent<Text>().text = coinsLeft.ToString()+"/"+goalLimit.ToString();
    }

    public void CoinDropped()
    {
        coinsLeft--;
        coinsText.GetComponent<Text>().text = coinsLeft.ToString() + "/" + goalLimit.ToString();
        goalUI.transform.GetChild(coinsLeft).gameObject.SetActive(true);
    }
    public Vector3 getSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
    }

    private void SetGoalUI()
    {
        //goalUI = Instantiate(goalUI, GameObject.FindGameObjectWithTag("canvas").transform);
        //Vector3 pos = new Vector3(transform.position.x, transform.position.y + GetComponent<BoxCollider>().size.y, transform.position.z);
        //goalUI.transform.position = Camera.main.WorldToScreenPoint(pos);
        //bulletBar = weaponUI.transform.GetChild(0).gameObject;
        float width = goalUI.GetComponent<RectTransform>().sizeDelta.x;
        float widthPerOne = width / goalLimit;

        for (int i = 0; i < goalLimit; i++)
        {
            GameObject obj = Instantiate(goalPartition, goalUI.transform) as GameObject;
            //PanelSpacerRectTransform.offsetMin = new Vector2(0, 0); new Vector2(left, bottom);
            //PanelSpacerRectTransform.offsetMax = new Vector2(-360, -0); new Vector2(-right, -top);
            int space = 2;
            obj.GetComponent<RectTransform>().offsetMin = new Vector2(i * widthPerOne+space, 0); // left,bottom
            obj.GetComponent<RectTransform>().offsetMax = new Vector2(-(goalLimit - i - 1) * widthPerOne, 0); // right,top
            if (i >= coinsLeft)
            {
                obj.SetActive(false);
            }
        }
    }
    public void StartGame()
    {

    }
    public void EndGame()
    {

    }
}
