using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurglarLogic : MonoBehaviour {

    public int maxBag;
    public GameObject coin;
    public GameObject bagUI;
    public GameObject respawnEffect;
    public GameObject teleportZapPrefab;
    public GameObject respawnTextPrefab;
    public AudioClip dumpCoinClip;
    public AudioClip gatherCoinClip;
    public AudioClip[] gatherCoinClips;
    public AudioClip dropCoinClip;
    public AudioClip teleportClip;
    private GameObject respawnText;
    private GameObject dumpster;
    private List<GameObject> robbableObjects;
    private float slowAmount;
    private float originalSpeed = 0.1f;
    private float deathCounter = 0;
    private float bagScaleFactor = 0.05f;
    private float minSpeed = 0.01f;
    private float teleportSpeed = 0.6f;
    private int playerNum;
    private int respawnLimit = 180;
    private int bagSize;
    private int immortalTime = 120;
    private int immortalCounter = 0;
    private bool teleporting = false;
    private bool dead = false;
    private bool immortal = false;
    private bool respawning = false;
    private Vector3 targetTeleportPos;
    private Vector3 startPosition;
    private Vector3 deadPosition;
    private Vector3 originalScale;
    private Vector3 lastPos;
    private Vector3 direction;
    private Vector3 spawnLocation;
    private Movement movement;
    private GameController controller;
    private GameObject teleportObject;
    private GameObject teleportZap;
    private AudioSource audioSource;



    // Use this for initialization
    void Start () {
        controller = Camera.main.GetComponent<GameController>();
        movement = GetComponent<Movement>();
        playerNum = int.Parse(name.Substring(6, 1));
        originalSpeed = movement.GetSpeed();
        originalScale = transform.GetChild(0).transform.localScale;
        slowAmount = controller.slowPerCoin;
        startPosition = transform.position;
        deadPosition = new Vector3(1000, 0.75f, 1000);
        robbableObjects = new List<GameObject>();
        teleportObject = null;
        dumpster = null;
        setBagUI();
        audioSource = GetComponent<AudioSource>();
    }
	public void setSpeed(float speed)
    {
        originalSpeed = speed;
    }
	// Update is called once per frame
	void Update () {

        if(controller.gameStarted)
        {
            if (dead)
            {
                respawnText.GetComponent<Text>().text = "Respawn in "+((int)(respawnLimit - deathCounter)/60+1).ToString();
                deathCounter++;
                if (deathCounter > respawnLimit)
                {
                    Respawn();
                }

            }
            else if (teleporting)
            {
                teleportZap.transform.position = Vector3.MoveTowards(teleportZap.transform.position, targetTeleportPos, teleportSpeed);
                if (teleportZap.transform.position == targetTeleportPos)
                {
                    teleporting = false;
                    bagUI.SetActive(true);
                    Destroy(teleportZap);
                    transform.position = targetTeleportPos;
                }
            }
            else if(respawning)
            {
                teleportZap.transform.position = Vector3.MoveTowards(teleportZap.transform.position, spawnLocation, teleportSpeed);
                if (teleportZap.transform.position == spawnLocation)
                {
                    respawning = false;
                    bagUI.SetActive(true);
                    Destroy(teleportZap);
                    
                    immortal = true;
                    immortalCounter = 0;
                    transform.position = spawnLocation;
                    transform.rotation = Quaternion.identity;
                    bagUI.SetActive(true);
                    Debug.Log("ÖHH");
                }
            }
            else
            {
                if (immortal)
                {
                    immortalCounter++;
                    if (immortalCounter > immortalTime)
                    {
                        immortal = false;
                    }
                }

                Vector3 newDir = (transform.position - lastPos).normalized;
                

                if (newDir != Vector3.zero)
                {
                    direction = newDir;
                    //okk
                }
                lastPos = transform.position;
                //if there is an object to rob or pick up
                if (robbableObjects.Count > 0)
                {
                    if (Input.GetButtonDown("p" + playerNum.ToString() + "_button_b"))
                    {
                        Rob();
                    }
                }
                //drop coin
                if (Input.GetButtonDown("p" + playerNum.ToString() + "_button_a"))
                {
                    DropCoin();
                }
                //if teleport
                if (teleportObject != null)
                {
                    if (Input.GetButtonDown("p" + playerNum.ToString() + "_button_b"))
                    {
                        Teleport();
                    }
                }
                if (dumpster != null)
                {
                    if (Input.GetButtonDown("p" + playerNum.ToString() + "_button_b"))
                    {
                        DumpCoin();
                    }
                }

            }
        }
        bagUI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 pos = new Vector2(bagUI.transform.position.x + bagUI.GetComponent<RectTransform>().sizeDelta.x / 4, bagUI.transform.position.y + bagUI.GetComponent<RectTransform>().sizeDelta.y * 0.7f);
        bagUI.transform.position = pos;



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "robbable")
        {
            robbableObjects.Add(other.gameObject);
        }
        else if (other.tag == "coin")
        {
            robbableObjects.Add(other.gameObject);
        }
        else if (other.tag == "teleport")
        {
            teleportObject = other.gameObject;
        }
        else if (other.tag == "dumpster")
        {
            dumpster = other.gameObject;
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "hide")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            bagUI.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "robbable")
        {
            robbableObjects.Remove(other.gameObject);
        }
        else if (other.tag == "coin")
        {
            robbableObjects.Remove(other.gameObject);
        }
        else if (other.tag == "teleport")
        {
            teleportObject = null;
        }
        else if (other.tag == "dumpster")
        {
            dumpster = null;
        }
        else if(other.tag == "hide")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            bagUI.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!immortal)
        {
            if (other.collider.tag == "police" || other.collider.tag == "bullet")
            {
                if (other.gameObject.name.Substring(0, 6) == "bullet")
                {
                    Destroy(other.gameObject);
                }
                Die();
            }
        }
        
        
    }

    public void Die()
    {
        bagUI.SetActive(false);
        deathCounter = 0;
        dead = true;
        while(bagSize > 0)
        {
            //Instantiate(coin, transform.position, transform.rotation);
           decreaseBag();
            //controller.coinDropped();
        }
        teleportZap = Instantiate(teleportZapPrefab);
        teleportZap.transform.position = transform.position;
        teleportZap.SetActive(false);
        
        respawnText = Instantiate(respawnTextPrefab, GameObject.FindGameObjectWithTag("UIContainer").transform);
        respawnText.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        transform.position = deadPosition;
        transform.rotation = Quaternion.identity;
        SetSpawnLocation();
        
    }
    private void SetSpawnLocation()
    {
        spawnLocation = controller.getSpawnPoint();
        GameObject effect = Instantiate(respawnEffect);
        effect.transform.position = spawnLocation;
    }
    private void Respawn()
    {

        Destroy(respawnText);
        teleportZap.SetActive(true);
        respawning = true;
        movement.SetSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
        dead = false;
        controller.GetComponent<AudioSource>().clip = controller.respawnClip;
        controller.GetComponent<AudioSource>().Play();
        ReSizeBag();
    }
    private void incrementBag()
    {
        //bagUI.transform.GetChild(bagSize).gameObject.SetActive(true);
        bagSize++;
        ReSizeBag();
    }
    private void decreaseBag()
    {
        bagSize--;
        //bagUI.transform.GetChild(bagSize).gameObject.SetActive(false);
        ReSizeBag();
    }
    private void Rob()
    {
        if(bagSize<maxBag)
        {
            int index = 0;
            if (robbableObjects[0].name.Substring(0, 3) == "rob")
            {
                if (robbableObjects[0].gameObject.GetComponent<Robbable>().robAmount > 0)
                {
                    incrementBag();
                    robbableObjects[0].gameObject.GetComponent<Robbable>().Rob();
                    robbableObjects[0].gameObject.GetComponent<Robbable>().UpdateValue();
                    Vector3 scale = transform.GetChild(0).transform.localScale;
                    movement.SetSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
                    audioSource.clip = gatherCoinClips[Mathf.Max(Mathf.Min(5, bagSize - 1), 0)];
                    audioSource.Play();
                    //controller.coinGathered(1);
                }
                else
                {
                    index++;
                }
            }
            if (robbableObjects.Count > index && robbableObjects[index].name.Substring(0, 4) == "coin")
            {
                incrementBag();
                Vector3 scale = transform.GetChild(0).transform.localScale;
                movement.SetSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
                GameObject toDestroy = robbableObjects[index];
                robbableObjects.Remove(toDestroy);
                Destroy(toDestroy);
                audioSource.clip = gatherCoinClips[Mathf.Max(Mathf.Min(5, bagSize - 1), 0)];
                audioSource.Play();
                //controller.coinGathered(1);
            }
        }
        


    } 
    private void DropCoin()
    {
        if (bagSize > 0)
        {
            decreaseBag();
            movement.SetSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
            GameObject coinObj = Instantiate(coin, transform.position - direction*0.5f, transform.rotation);
            coinObj.transform.position = new Vector3(coinObj.transform.position.x, transform.position.y - 0.8f, coinObj.transform.position.z);
            //controller.coinDropped();
            audioSource.clip = dropCoinClip;
            audioSource.Play();
        }
    }

    private bool Pay(int howMuch)
    {
        if (bagSize >= howMuch)
        {
            for(int i = 0; i < howMuch; i++)
            {
                decreaseBag();
                movement.SetSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
               //controller.coinDropped();
            }
            return true;
        }
        return false;
    }

    private void ReSizeBag()
    {
        /*transform.GetChild(0).transform.localScale = new Vector3(
            originalScale.x + bagScaleFactor * bagSize, 
            originalScale.y + bagScaleFactor * bagSize, 
            originalScale.z + bagScaleFactor * bagSize);*/
        bagUI.GetComponent<Text>().text = bagSize.ToString();
    }

    private void Teleport()
    {
        
        if (Pay(1))
        {
            teleporting = true;
            targetTeleportPos = teleportObject.GetComponent<phoneBoothLogic>().targetPhoneBooth.transform.position;
            targetTeleportPos = new Vector3(targetTeleportPos.x, transform.position.y, targetTeleportPos.z);
            bagUI.SetActive(false);
            teleportZap = Instantiate(teleportZapPrefab);
            teleportZap.transform.position = transform.position;
            transform.position = deadPosition;
            audioSource.clip = teleportClip;
            audioSource.Play();
            //transform.position = new Vector3(phonePos.x, transform.position.y, phonePos.z);
        }
          
    }
    private void DumpCoin()
    {
        if(bagSize>0)
        {
            dumpster.GetComponent<DumpsterLogic>().coinsInStash++;
            dumpster.GetComponent<DumpsterLogic>().UpdateValue();
            decreaseBag();
            movement.SetSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
            audioSource.clip = dumpCoinClip;
            audioSource.Play();
        }

    }

    private void setBagUI()
    {
        bagUI = Instantiate(bagUI, GameObject.FindGameObjectWithTag("UIContainer").transform);
        bagUI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 pos = new Vector2(bagUI.transform.position.x + bagUI.GetComponent<RectTransform>().sizeDelta.x, bagUI.transform.position.y + bagUI.GetComponent<RectTransform>().sizeDelta.y*2);
        bagUI.transform.position = pos;
        bagUI.GetComponent<Text>().text = bagSize.ToString();
        //bulletBar = weaponUI.transform.GetChild(0).gameObject;
        /*
        float width = bagUI.GetComponent<RectTransform>().sizeDelta.x;
        float widthPerOne = width / maxBag;

        for (int i = 0; i < maxBag; i++)
        {
            GameObject obj = Instantiate(bagPartition, bagUI.transform) as GameObject;
            //PanelSpacerRectTransform.offsetMin = new Vector2(0, 0); new Vector2(left, bottom);
            //PanelSpacerRectTransform.offsetMax = new Vector2(-360, -0); new Vector2(-right, -top);
            obj.GetComponent<RectTransform>().offsetMin = new Vector2(i * widthPerOne, 0); // left,bottom
            obj.GetComponent<RectTransform>().offsetMax = new Vector2(-(maxBag - i - 1) * widthPerOne, 0); // right,top
            if (i >= bagSize)
            {
                obj.SetActive(false);
            }
        }
        */
    }
    public void SetPlayerNum(int number)
    {
        playerNum = number;
    }
}
