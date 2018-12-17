using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Arrow_UI : MonoBehaviour {

    public GameObject target;
    public int onright,ontop;
    private int distance;
   
    public float waitTime;
    public bool active,flash;
    private int count;
    public int Flash_Time;
    public GameObject text;
    private GameObject[] UIs;
    private PlayableDirector countdown;
    

    // Use this for initialization
    void Start () {
        flash = false;
        active = false;
        count = 0;
        waitTime = 0.5f;
        Flash_Time = 3;
        distance = 50;
        //Vector3 pos = new Vector3(transform.position.x, transform.position.y + GetComponent<BoxCollider>().size.y, transform.position.z);
        transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        transform.position = new Vector3(transform.position.x+ontop*distance, transform.position.y + distance* onright);
        countdown = GameObject.Find("Main Camera").GetComponent<PlayableDirector>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        transform.position = new Vector3(transform.position.x + ontop * distance, transform.position.y + distance * onright);
        if(active&&!flash)
        {
            StartCoroutine(WaitAndFlash(waitTime));
            flash = true;
        }
        if(count == Flash_Time)
        {
            text.SetActive(false);
            //active = false;
            //StartCoroutine(UISendMessage(waitTime));

        }
    }

    private IEnumerator WaitAndFlash(float waitTime)
    {
        gameObject.GetComponent<Image>().enabled = true;
       count++;
        yield return new WaitForSeconds(waitTime);

        StartCoroutine(WaitAndNotFlash(waitTime));

    }
    private IEnumerator WaitAndNotFlash(float waitTime)
    {
        gameObject.GetComponent<Image>().enabled = false;
       
        yield return new WaitForSeconds(waitTime);
        if(count < Flash_Time)
         StartCoroutine(WaitAndFlash(waitTime));
        else
        {
            //text.SetActive(false);
            active = false;
            StartCoroutine(UISendMessage(waitTime));
        }

    }
    private IEnumerator UISendMessage(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);

        if (gameObject.tag == "BankUI")
        {
            //GameObject.FindWithTag("DumpsterUI").GetComponent<Arrow_UI>().SendMessage("StartFlash");
            UIs = GameObject.FindGameObjectsWithTag("DumpsterUI");
            foreach (GameObject UI in UIs)
            {
                UI.GetComponent<Arrow_UI>().SendMessage("StartFlash");
            }

            GetComponent<Arrow_UI>().SendMessage("StartFlash");
        }
        else if (gameObject.tag == "DumpsterUI")
            GameObject.FindWithTag("TrunkUI").GetComponent<Arrow_UI>().SendMessage("StartFlash");
        else if (gameObject.tag == "TrunkUI")
        {
            countdown.Play();
        }

    }

    public void StartFlash()
    {
        //Debug.Log("sendmessage");
        active = true;
        text.SetActive(true);
    }
    //public void StopFlash()
    //{
    //    active = false;
   // }



}
