using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;

public class GameController : MonoBehaviour {

    public bool playTest;
    public bool randomizedStartPositions;
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
    public bool gameStarted = false;
    public Animator fadeAnimator;
    public Animator fadeAnimatorCharacterSelect;
    public AudioClip mainTheme;
    public AudioClip buttonClip;
    public AudioClip respawnClip;

    public GameObject skipObj;
    public GameObject tutorial;
    private PlayableDirector Timeline;
    public GameObject winText;
    public GameObject timeLeftText;
    public GameObject timeSlider;
    public GameObject coinsText;
    public GameObject goalUI;
    public GameObject goalPartition;
    public GameObject startMenu;
    public GameObject characterSelection;
    public GameObject selectionArrow;
    public GameObject characterSelCamera;
    public GameObject startLocations;
    public GameObject lockIcon;
    private List<GameObject> lockIcons;
    private List<GameObject> players;
    private List<Transform> startMenuButtons;
    private AudioSource audioSource;

    private int menuSelection;
    private bool inTutorial = false;
    public bool inStartMenu;
    private bool inCharacterSelection = false;
    private bool tutorialStarted = false;
    private int coinsLeft;
    private float timer = 0;
    //private 
    private float timeLimit = 60.0f;
    private float startTime = 0;
    private GameObject[] spawnPoints;
    private GameObject mainCanvas;
    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 60;
        audioSource = GetComponent<AudioSource>();
        //Timeline = tutorial.GetComponent<PlayableDirector>();
        mainCanvas = GameObject.FindGameObjectWithTag("canvas");
        SetPlayers();
        SetGoalUI();
        spawnPoints = GameObject.FindGameObjectsWithTag("spawnLocation");
        SetBanks();
        SetStartMenu();
        Debug.Log("HLALOO");
    }
	
	// Update is called once per frame
	void Update () {
        if(inStartMenu)
        {
            StartMenu();
        }
        else if(inCharacterSelection)
        {
            CharacterSelection();
        }
        else if(inTutorial)
        {
            Tutorial();
        }
        else
        {
            
            timer = Time.timeSinceLevelLoad - startTime;
            timeSlider.GetComponent<Slider>().value = timer / timeLimit;
            timeLeftText.GetComponent<Text>().text = ((int)(60 - timer) + 1).ToString();
            if (timer > timeLimit)
            {
                
                StartCoroutine(EndGame(true));
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //timer = 0;
                //SetPlayers();
                //Run every objects start function again
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
            //THIEFS WIN
           // winText.SetActive(true);
            StartCoroutine(EndGame(false));
        }
        coinsText.GetComponent<Text>().text = coinsLeft.ToString()+"/"+goalLimit.ToString();
    }

    public void CoinDropped()
    {
        coinsLeft--;
        coinsText.GetComponent<Text>().text = coinsLeft.ToString() + "/" + goalLimit.ToString();
        goalUI.transform.GetChild(coinsLeft).gameObject.SetActive(true);
    }
    public Vector3 getSpawnPoint(int less = 0)
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length-less)].transform.position;
    }

    private void SetGoalUI()
    {
        //goalUI = Instantiate(goalUI, GameObject.FindGameObjectWithTag("canvas").transform);
        //Vector3 pos = new Vector3(transform.position.x, transform.position.y + GetComponent<BoxCollider>().size.y, transform.position.z);
        //goalUI.transform.position = Camera.main.WorldToScreenPoint(pos);
        //bulletBar = weaponUI.transform.GetChild(0).gameObject;
        coinsLeft = 0;
        coinsText.GetComponent<Text>().text = coinsLeft.ToString() + "/" + goalLimit.ToString();
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
    private void SetStartMenu()
    {
        if(playTest)
        {
            startMenu.SetActive(false);
            characterSelection.SetActive(false);
            characterSelCamera.SetActive(false);
            StartGame();
        }
        else
        {
            startMenuButtons = new List<Transform>();
            foreach (Transform child in startMenu.transform)
            {
                startMenuButtons.Add(child);
            }
            inStartMenu = true;
            characterSelection.SetActive(false);
            
        }
        

    }
    private void StartMenu()
    {
        if (Input.GetButtonDown("p1_button_b"))
        {
            startMenuButtons[menuSelection].GetComponent<Animator>().Play("Pressed");
            inStartMenu = false;
            if (menuSelection == 0)
            {
                characterSelection.SetActive(true);
                inCharacterSelection = true;
                fadeAnimator.Play("FadeOut");
                audioSource.clip = buttonClip;
                audioSource.Play();
            }
            else if (menuSelection == 1)
            {
                //exit game
                Application.Quit();
                audioSource.clip = buttonClip;
                audioSource.Play();
            }
        }
        float dir = Input.GetAxis("p1_joystick_vertical");
        //Debug.Log(dir);
        //move to play button
        if (dir > 0)
        {
            //startMenuButtons[menuSelection].GetChild(0).gameObject.SetActive(false);
            startMenuButtons[1].GetComponent<Animator>().Play("Normal");
            menuSelection = 0;
            startMenuButtons[0].GetComponent<Animator>().Play("Highlighted");
            //startMenuButtons[menuSelection].GetChild(0).gameObject.SetActive(true);
            //selectionArrow.transform.position = new Vector2(selectionArrow.transform.position.x, startMenuButtons[menuSelection].transform.position.y);
        }
        //move to exit button
        else if (dir < 0)
        {
            //startMenuButtons[menuSelection].GetChild(0).gameObject.SetActive(false);
            startMenuButtons[0].GetComponent<Animator>().Play("Normal");
            menuSelection = 1;
            startMenuButtons[1].GetComponent<Animator>().Play("Highlighted");
            //startMenuButtons[menuSelection].GetChild(0).gameObject.SetActive(true);
            //selectionArrow.transform.position = new Vector2(selectionArrow.transform.position.x, startMenuButtons[menuSelection].transform.position.y);
        }
    }
    private void CharacterSelection()
    {
        if (fadeAnimator.GetCurrentAnimatorStateInfo(0).IsName("FadeIn"))
        {
            characterSelCamera.SetActive(true);
            startMenu.SetActive(false);
            mainCanvas.SetActive(false);
            fadeAnimatorCharacterSelect.Play("FadeIn");
        }
    }
    private void Tutorial()
    {
        if (fadeAnimatorCharacterSelect.GetCurrentAnimatorStateInfo(0).IsName("FadeIn"))
        {
            mainCanvas.SetActive(true);
            fadeAnimator.Play("FadeIn");
            characterSelection.SetActive(false);
            characterSelCamera.SetActive(false);
            //Timeline.Play();
            //StartGame();
            GameObject.Find("Skip").GetComponent<Text>().enabled = true;
            
        }
        if(Input.GetButtonDown("p1_button_b"))
        {
            if(!tutorialStarted)
            {
                tutorial.SetActive(true);
                Destroy(GameObject.Find("Skip"));
                //.Find("Skip").GetComponent<Text>().enabled = false;
                audioSource.clip = buttonClip;
                audioSource.Play();
                tutorialStarted = true;
            }
            
        }
        else if(Input.GetButtonDown("p1_button_a"))
        {
            if(!tutorialStarted)
            {
                GameObject.Find("Skip").GetComponent<Text>().enabled = false;
                StartGame();
                audioSource.clip = buttonClip;
                audioSource.Play();
                tutorialStarted = true;
            }
            
        }
    }
    private void SetPlayers()
    {
        players = new List<GameObject>();
        lockIcons = new List<GameObject>();
        int i = 0;
        GameObject[] police = GameObject.FindGameObjectsWithTag("police");
        foreach (GameObject obj in police)
        {
            //obj.GetComponent<Movement>().SetSpeed(policeSpeed);
           // obj.transform.position = startLocations.transform.GetChild(i).position;
            players.Add(obj);
            i++;
            if (playTest)
            {
                obj.GetComponent<Movement>().SetPlayerNum(i);
            }
        }
        GameObject[] burglars = GameObject.FindGameObjectsWithTag("burglar");
        foreach (GameObject obj in burglars)
        {
            //obj.GetComponent<Movement>().SetSpeed(burglarSpeed);
            //obj.transform.position = startLocations.transform.GetChild(i).position;
            players.Add(obj);
            i++;
            if (playTest)
            {
                obj.GetComponent<Movement>().SetPlayerNum(i);
                //obj.GetComponent<BurglarLogic>().setUp();
            }
        }
        foreach(GameObject player in players)
        {
            GameObject lockObj = Instantiate(lockIcon, characterSelection.transform);
            lockObj.transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
            lockObj.transform.localPosition = new Vector3(lockObj.transform.localPosition.x+106, lockObj.transform.localPosition.y-170, 0);
            lockObj.SetActive(false);
            lockIcons.Add(lockObj);
            
        }
        
    }

    private void SetBanks()
    {
        GameObject[] banks = GameObject.FindGameObjectsWithTag("robbable");
        int perBank = bankCoinsTotal / banks.Length;
        int leftovers = bankCoinsTotal % banks.Length;
        for (int i = 0; i < banks.Length; i++)
        {

            banks[i].GetComponent<Robbable>().robAmount = perBank;
            if (leftovers > 0)
            {
                banks[i].GetComponent<Robbable>().ProduceMoney();
                leftovers--;
            }
            banks[i].GetComponent<Robbable>().UpdateValue();
        }
    }
    private void ExitCharacterSelect()
    {
        
        inCharacterSelection = false;
        inTutorial = true;
        
        fadeAnimatorCharacterSelect.Play("FadeOut");
    }
    public GameObject LockCharacter(int selection)
    {
        if(!mainCanvas.activeSelf)
        {
            if (players[selection].GetComponent<Movement>().locked == false)
            {
                players[selection].GetComponent<Movement>().locked = true;
                lockIcons[selection].SetActive(true);
                audioSource.clip = buttonClip;
                audioSource.Play();
                bool allLocked = true;
                foreach (GameObject player in players)
                {
                    if (player.GetComponent<Movement>().locked == false)
                    {
                        allLocked = false;
                    }
                }
                if (allLocked)
                {
                    ExitCharacterSelect();
                }
                return players[selection];
            }
            else
            {
                return null;
            }
        }
        return null;
       
    }
    public bool UnlockCharacter(int selection)
    {
        if (players[selection].GetComponent<Movement>().locked == true)
        {
            players[selection].GetComponent<Movement>().locked = false;
            lockIcons[selection].SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void StartGame()
    {
        Vector3 same = Vector3.zero;
        for (int i = 0; i < players.Count; i++)
        {
            if(randomizedStartPositions)
            {
                if (i > 1)
                {
                    Vector3 pos = getSpawnPoint(1);
                    while (same == pos)
                    {
                        pos = getSpawnPoint(1);
                    }
                    players[i].transform.position = pos;
                    same = pos;
                }
                else
                {
                    players[i].transform.position = startLocations.transform.GetChild(i).transform.position;
                }
            }
            else
            {
                players[i].transform.position = startLocations.transform.GetChild(i).transform.position;
            }

        }
        gameObject.GetComponent<PlayableDirector>().Play();
        StartCoroutine(Waitabit(4.0f));
        skipObj.SetActive(false);
    }
    public IEnumerator EndGame(bool policeWin)
    {
        yield return new WaitForSeconds(1);
        if (policeWin)
            SceneManager.LoadScene("PoliceWin", LoadSceneMode.Single);
        else
            SceneManager.LoadScene("ThiefWin", LoadSceneMode.Single);

    }
    private IEnumerator Waitabit(float time)
    {
        yield return new WaitForSeconds(time);
        //audioSource.Play(mainTheme);
        gameStarted = true;
        startTime = Time.timeSinceLevelLoad;
        inTutorial = false;
        
        GameObject[] trucks = GameObject.FindGameObjectsWithTag("truck");
        foreach (GameObject truck in trucks)
        {
            truck.GetComponent<truckMove>().StartMoving();
        }

    }
    public int GetCoinsGathered()
    {
        return coinsLeft;
    }
}
