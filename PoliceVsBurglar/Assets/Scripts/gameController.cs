using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameController : MonoBehaviour {

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
    public int bulletAmount;
    public float bankRefillTime;
    public float coinBlockTime;
    public int startEnergy;
    public int energyPerDonut;

    public GameObject winText;
    public GameObject timeLeftText;
    public GameObject timeSlider;
    public GameObject coinsText;

    private int coinsLeft;
    private float timer = 0;
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
            obj.GetComponent<movement>().setSpeed(burglarSpeed);
        }
        GameObject[] police = GameObject.FindGameObjectsWithTag("police");
        Debug.Log("police " + police.Length);
        foreach (GameObject obj in police)
        {
            obj.GetComponent<movement>().setSpeed(policeSpeed);
        }
        coinsLeft = goalLimit;
        coinsText.GetComponent<Text>().text = coinsLeft.ToString();
        spawnPoints = GameObject.FindGameObjectsWithTag("spawnLocation");
        GameObject[] banks = GameObject.FindGameObjectsWithTag("robbable");
        int perBank = bankCoinsTotal / banks.Length;
        int leftovers = bankCoinsTotal % banks.Length;
        for (int i = 0; i < banks.Length; i++)
        {

            banks[i].GetComponent<robbable>().robAmount = perBank;
            if(leftovers > 0)
            {
                banks[i].GetComponent<robbable>().produceMoney();
                leftovers--;
            }
            banks[i].GetComponent<robbable>().updateValue();
        }
    }
	
	// Update is called once per frame
	void Update () {
        timer = Time.timeSinceLevelLoad;
        timeSlider.GetComponent<Slider>().value = timer / timeLimit;
        timeLeftText.GetComponent<Text>().text = ((int)(60 - timer)).ToString();
        if (timer > timeLimit)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            timer = 0;
        }
        
	}

    public void coinGathered(int howMany)
    {
        coinsLeft -= howMany;
        if (coinsLeft <= 0)
        {
            winText.SetActive(true);
        }
        coinsText.GetComponent<Text>().text = coinsLeft.ToString();
    }

    public void coinDropped()
    {
        coinsLeft++;
        coinsText.GetComponent<Text>().text = coinsLeft.ToString();
    }
    public Vector3 getSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
    }
}
