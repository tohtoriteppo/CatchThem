using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class burglarLogic : MonoBehaviour {

    public int maxBag;
    public GameObject coin;

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
    private gameController controller;
    private GameObject teleportObject;


    // Use this for initialization
    void Start () {
        controller = Camera.main.GetComponent<gameController>();
        playerNum = int.Parse(name.Substring(6, 1));
        originalSpeed = GetComponent<movement>().getSpeed();
        originalScale = transform.GetChild(0).transform.localScale;
        slowAmount = controller.slowPerLoot;
        startPosition = transform.position;
        deadPosition = new Vector3(1000, 1000, 1000);
        robbableObjects = new List<GameObject>();
        teleportObject = null;
    }
	
	// Update is called once per frame
	void Update () {

        if (dead)
        {
            deathCounter++;
            if(deathCounter > respawnLimit)
            {
                respawn();
            }
        }
        else
        {
            Vector3 newDir = (transform.position - lastPos).normalized;
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
                    rob();
                }
            }
            //drop coin
            if (Input.GetButtonDown("p" + playerNum.ToString() + "_button_a"))
            {
                dropCoin();
            }
            //if teleport
            if(teleportObject != null)
            {
                if (Input.GetButtonDown("p" + playerNum.ToString() + "_button_b"))
                {
                    teleport();
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
        if (other.tag == "coin")
        {
            robbableObjects.Add(other.gameObject);
        }
        if (other.tag == "teleport")
        {
            teleportObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "robbable")
        {
            robbableObjects.Remove(other.gameObject);
        }
        if (other.tag == "coin")
        {
            robbableObjects.Remove(other.gameObject);
        }
        if (other.tag == "teleport")
        {
            teleportObject = null;
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
        deathCounter = 0;
        dead = true;
        while(bagSize > 0)
        {
            Debug.Log("WHUUT");
            //Instantiate(coin, transform.position, transform.rotation);
            bagSize--;
            controller.coinDropped();
        }
        transform.position = deadPosition;
    }
    
    private void respawn()
    {
        transform.position = controller.getSpawnPoint();
        GetComponent<movement>().setSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
        dead = false;
        reSizeBag();
    }

    private void rob()
    {
        int index = 0;
        if (robbableObjects[0].name.Substring(0, 3) == "rob")
        {
            if (robbableObjects[0].gameObject.GetComponent<robbable>().robAmount > 0)
            {
                bagSize++;
                robbableObjects[0].gameObject.GetComponent<robbable>().robAmount--;
                robbableObjects[0].gameObject.GetComponent<robbable>().updateValue();
                Vector3 scale = transform.GetChild(0).transform.localScale;
                reSizeBag();
                GetComponent<movement>().setSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
                controller.coinGathered();
            }
            else
            {
                index++;
            }
        }
        if (robbableObjects[index].name.Substring(0, 4) == "coin")
        {
            bagSize++;
            Vector3 scale = transform.GetChild(0).transform.localScale;
            reSizeBag();
            GetComponent<movement>().setSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
            GameObject toDestroy = robbableObjects[index];
            robbableObjects.Remove(toDestroy);
            Destroy(toDestroy);
            controller.coinGathered();
        }


    } 
    private void dropCoin()
    {
        if (bagSize > 0)
        {
            bagSize--;
            reSizeBag();
            GetComponent<movement>().setSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
            Instantiate(coin, transform.position - direction*0.5f, transform.rotation);
            controller.coinDropped();
        }
    }

    private bool pay(int howMuch)
    {
        if (bagSize >= howMuch)
        {
            for(int i = 0; i < howMuch; i++)
            {
                bagSize--;
                reSizeBag();
                GetComponent<movement>().setSpeed(Mathf.Max(minSpeed, originalSpeed - bagSize * slowAmount));
                controller.coinDropped();
            }
            return true;
        }
        return false;
    }

    private void reSizeBag()
    {
        transform.GetChild(0).transform.localScale = new Vector3(
            originalScale.x + bagScaleFactor * bagSize, 
            originalScale.y + bagScaleFactor * bagSize, 
            originalScale.z + bagScaleFactor * bagSize);
    }

    private void teleport()
    {
        
        if (pay(2))
        {
            transform.position = teleportObject.GetComponent<phoneBoothLogic>().targetPhoneBooth.transform.position;
        }
          
    }
}
