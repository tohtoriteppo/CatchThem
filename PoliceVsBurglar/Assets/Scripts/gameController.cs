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
    }
	
	// Update is called once per frame
	void Update () {
        timer = Time.timeSinceLevelLoad;
        timeSlider.GetComponent<Slider>().value = timer / timeLimit;
        timeLeftText.GetComponent<Text>().text = ((int)(60 - timer)+1).ToString();
        if (timer > timeLimit)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            timer = 0;
        }
        
	}

    public void CoinGathered(int howMany)
    {
        coinsLeft += howMany;
        if (coinsLeft >= goalLimit)
        {
            winText.SetActive(true);
        }
        coinsText.GetComponent<Text>().text = coinsLeft.ToString()+"/"+goalLimit.ToString();
    }

    public void CoinDropped()
    {
        coinsLeft++;
        coinsText.GetComponent<Text>().text = coinsLeft.ToString() + "/" + goalLimit.ToString();
    }
    public Vector3 getSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
    }
}
