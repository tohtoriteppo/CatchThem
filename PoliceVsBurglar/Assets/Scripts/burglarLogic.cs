using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class burglarLogic : MonoBehaviour {

    public int maxBag;
    public GameObject coin;

    private float slowAmount;
    private int playerNum;
    private GameObject robbableObject;
    private int bagSize;
    private float originalSpeed;
    private Vector3 startPosition;
    private float deathCounter = 0;
    private int respawnLimit = 180;
    private Vector3 deadPosition;
    private bool dead = false;

	// Use this for initialization
	void Start () {
		playerNum = int.Parse(name.Substring(6, 1));
        originalSpeed = GetComponent<movement>().speed;
        slowAmount = Camera.main.GetComponent<gameController>().slowPerLoot;
        GetComponent<movement>().speed = Camera.main.GetComponent<gameController>().burglarSpeed;
        startPosition = transform.position;
        deadPosition = new Vector3(1000, 1000, 1000);
       
    }
	
	// Update is called once per frame
	void Update () {
        
        if (dead)
        {
            deathCounter++;
            if(deathCounter > respawnLimit)
            {
                transform.position = startPosition;
                dead = false;
                
            }
        }
        if(robbableObject != null)
        {
            if (Input.GetButtonDown("p" + playerNum.ToString() + "_button_b"))
            {
                if(robbableObject.tag == "coin")
                {
                    bagSize++;
                    Vector3 scale = transform.GetChild(0).transform.localScale;
                    float factor = 0.05f;
                    transform.GetChild(0).transform.localScale = new Vector3(scale.x + factor, scale.y + factor, scale.z + factor);
                    GetComponent<movement>().speed = Mathf.Max(0, originalSpeed - bagSize * slowAmount);
                    Destroy(robbableObject);

                }
                if (robbableObject.gameObject.GetComponent<robbable>().robAmount > 0)
                {
                    robbableObject.gameObject.GetComponent<robbable>().robAmount--;
                    bagSize++;
                    Vector3 scale = transform.GetChild(0).transform.localScale;
                    float factor = 0.05f;
                    transform.GetChild(0).transform.localScale = new Vector3(scale.x + factor, scale.y + factor, scale.z + factor);
                    GetComponent<movement>().speed = Mathf.Max(0, originalSpeed - bagSize * slowAmount);

                }
            }
        }
        if (Input.GetButtonDown("p" + playerNum.ToString() + "_button_a"))
        {
            if(bagSize > 0)
            {
                bagSize--;
                Vector3 scale = transform.GetChild(0).transform.localScale;
                float factor = 0.05f;
                transform.GetChild(0).transform.localScale = new Vector3(scale.x - factor, scale.y - factor, scale.z - factor);
                GetComponent<movement>().speed = Mathf.Max(0, originalSpeed - bagSize * slowAmount);
                Instantiate(coin, transform.position, transform.rotation);
            }

        }
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "robbable")
        {
            robbableObject = other.gameObject;
        }
        if (other.tag == "coin")
        {
            robbableObject = other.gameObject;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "robbable")
        {
            robbableObject = other.gameObject;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.collider.gameObject == robbableObject)
        {
            robbableObject = null;
        }
    }
    public void Die()
    {
        transform.position = deadPosition;
        deathCounter = 0;
        dead = true;
        bagSize = 0;
    }
}
