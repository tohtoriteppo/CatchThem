using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurglarLogic : MonoBehaviour {

    public int maxBag;
    public GameObject coin;
    public GameObject bagUI;
    public GameObject bagPartition;

    private GameObject dumpster;
    private List<GameObject> robbableObjects;
    private float slowAmount;
    private float originalSpeed;
    private float deathCounter = 0;
    private float bagScaleFactor = 0.05f;
    private float minSpeed = 0.01f;
    private int playerNum;
    private int respawnLimit = 180;
    private int bagSize;
    private bool dead = false;
    private Vector3 startPosition;
    private Vector3 deadPosition;
    private Vector3 originalScale;
    private Vector3 lastPos;
    private Vector3 direction;
    private Movement movement;
    private GameController controller;
    private GameObject teleportObject;


    // Use this for initialization
    void Start () {
        controller = Camera.main.GetComponent<GameController>();
        movement = GetComponent<Movement>();
        playerNum = int.Parse(name.Substring(6, 1));
        originalSpeed = movement.GetSpeed();
        originalScale = transform.GetChild(0).transform.localScale;
        slowAmount = controller.slowPerCoin;
        startPosition = transform.position;
        deadPosition = new Vector3(1000, 1000, 1000);
        robbableObjects = new List<GameObject>();
        teleportObject = null;
        dumpster = null;
        setBagUI();
    }
	
	// Update is called once per frame
	void Update () {

        if (dead)
        {
            deathCounter++;
            if(deathCounter > respawnLimit)
            {
                Respawn();
            }
        }
        else
        {
            Vector3 newDir = (transform.position - lastPos).normalized;
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + GetComponent<BoxCollider>().size.y, transform.position.z);
            bagUI.transform.position = Camera.main.WorldToScreenPoint(pos);
            if (newDir != Vector3.zero)
            {
                direction = newDir;
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
            if(teleportObject != null)
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
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "police" || other.collider.tag == "bullet")
        {
            if(other.gameObject.name.Substring(0, 6) == "bullet")
            {
                Destroy(other.gameObject);
            }
            Die();
        }
        
    }

    public void Die()
    {
        bagUI.SetActive(false);
        deathCounter = 0;
        dead = true;
        while(bagSize > 0)
        {
            Debug.Log("WHUUT");
            //Instantiate(coin, transform.position, transform.rotation);
           decreaseBag();
            //controller.coinDropped();
        }
        transform.position = deadPosition;
    }
    
    private void Respawn()
    {
        bagUI.SetActive(true);
        transform.position = controller.getSpawnPoint();
        movement.SetSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
        dead = false;
        ReSizeBag();
    }
    private void incrementBag()
    {
        bagUI.transform.GetChild(bagSize).gameObject.SetActive(true);
        bagSize++;
        ReSizeBag();
    }
    private void decreaseBag()
    {
        bagSize--;
        bagUI.transform.GetChild(bagSize).gameObject.SetActive(false);
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
                    //controller.coinGathered(1);
                }
                else
                {
                    index++;
                }
            }
            if (robbableObjects[index].name.Substring(0, 4) == "coin")
            {
                incrementBag();
                Vector3 scale = transform.GetChild(0).transform.localScale;
                movement.SetSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
                GameObject toDestroy = robbableObjects[index];
                robbableObjects.Remove(toDestroy);
                Destroy(toDestroy);
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
            Instantiate(coin, transform.position - direction*0.5f, transform.rotation);
            //controller.coinDropped();
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
        transform.GetChild(0).transform.localScale = new Vector3(
            originalScale.x + bagScaleFactor * bagSize, 
            originalScale.y + bagScaleFactor * bagSize, 
            originalScale.z + bagScaleFactor * bagSize);
    }

    private void Teleport()
    {
        
        if (Pay(1))
        {
            Vector3 phonePos = teleportObject.GetComponent<phoneBoothLogic>().targetPhoneBooth.transform.position;
            transform.position = new Vector3(phonePos.x, transform.position.y, phonePos.z);
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

        }

    }

    private void setBagUI()
    {
        bagUI = Instantiate(bagUI, GameObject.FindGameObjectWithTag("canvas").transform);
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + GetComponent<BoxCollider>().size.y, transform.position.z);
        bagUI.transform.position = Camera.main.WorldToScreenPoint(pos);
        //bulletBar = weaponUI.transform.GetChild(0).gameObject;
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
    }
}
