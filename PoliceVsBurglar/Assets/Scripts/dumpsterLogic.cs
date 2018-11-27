using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DumpsterLogic : MonoBehaviour {

    public int coinsInStash = 10;
    public GameObject coinText;
	public GameObject coin;
	
    private Vector3 screenPos;
    // Use this for initialization
    void Start () {
        screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenPos = new Vector3(screenPos.x+4, screenPos.y+4, screenPos.z);
        coinText = Instantiate(coinText, GameObject.FindGameObjectWithTag("canvas").transform) as GameObject;
        coinText.GetComponent<Text>().text = coinsInStash.ToString();
        coinText.transform.position = screenPos;
		coin = Instantiate(coin, transform.position, transform.rotation);
		coin.transform.position = new Vector3(coin.transform.position.x+0.6f, coin.transform.position.y+1.5f, coin.transform.position.z);
        coin.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if(coinsInStash>0)
        {
            
            coin.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * 50);
        }
		
	}
    public void UpdateValue()
    {
        if(coinsInStash>0)
        {
            coin.SetActive(true);
        }
        else
        {
            coin.SetActive(false);
        }
        coinText.GetComponent<Text>().text = coinsInStash.ToString();
    }
    public void EmptyCoin()
    {
        if (coinsInStash > 0)
        {
            coinsInStash--;
            UpdateValue();
        }
    }
}
