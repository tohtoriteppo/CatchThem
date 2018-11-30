using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Robbable : MonoBehaviour {

    public int robAmount = 5;
    public GameObject coinsLeft;
    public GameObject bankUI;
    public GameObject bankPartition;

    private int maxCoins;
    private int fillCounter = 0;
    private int bankRefillTime = 600;
    private Vector3 screenPos;
    // Use this for initialization
    void Start () {
        screenPos = Camera.main.WorldToScreenPoint(transform.position);
        coinsLeft = Instantiate(coinsLeft, GameObject.FindGameObjectWithTag("UIContainer").transform) as GameObject;
        //coinsLeft.GetComponent<Text>().text = robAmount.ToString();
        coinsLeft.GetComponent<Text>().text = "";
        coinsLeft.transform.position = screenPos;
        bankRefillTime = (int)Camera.main.GetComponent<GameController>().bankRefillTime*60;
        maxCoins = Camera.main.GetComponent<GameController>().maxCoinsInBank;
        SetBankUI();
    }
	
	// Update is called once per frame
	void Update () {
        fillCounter++;
        if(fillCounter> bankRefillTime)
        {
            ProduceMoney();
            fillCounter = 0;
            UpdateValue();
        }
	}
    public void UpdateValue()
    {
        bankUI.GetComponentInChildren<Text>().text = robAmount.ToString();
        
    }
    public void ProduceMoney()
    {
        if(robAmount < maxCoins)
        {
            bankUI.transform.GetChild(robAmount+1).gameObject.SetActive(true);
            robAmount++;
            
        }
        
        
    }
    public void Rob()
    {
        robAmount--;
        bankUI.transform.GetChild(robAmount+1).gameObject.SetActive(false);
        UpdateValue();
    }
    private void SetBankUI()
    {
        bankUI = Instantiate(bankUI, GameObject.FindGameObjectWithTag("UIContainer").transform);
        bankUI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        bankUI.transform.position = new Vector2(bankUI.transform.position.x, bankUI.transform.position.y + 40);
        //bulletBar = weaponUI.transform.GetChild(0).gameObject;
        /*
        float height = bankUI.GetComponent<RectTransform>().sizeDelta.y;
        float heightPerOne = height / maxCoins;

        for (int i = 0; i < maxCoins; i++)
        {
            GameObject obj = Instantiate(bankPartition, bankUI.transform) as GameObject;
            //PanelSpacerRectTransform.offsetMin = new Vector2(0, 0); new Vector2(left, bottom);
            //PanelSpacerRectTransform.offsetMax = new Vector2(-360, -0); new Vector2(-right, -top);
            obj.GetComponent<RectTransform>().offsetMin = new Vector2(0, i * heightPerOne); // left,bottom
            obj.GetComponent<RectTransform>().offsetMax = new Vector2(0, -(maxCoins - i - 1) * heightPerOne); // right,top
            if(i>=robAmount)
            {
                obj.SetActive(false);
            }
        }
        */
        float height = bankUI.GetComponent<RectTransform>().sizeDelta.y;
        float heightPerOne = 52;
        for (int i = 0; i < maxCoins; i++)
        {
            GameObject obj = Instantiate(bankPartition, bankUI.transform) as GameObject;
            //PanelSpacerRectTransform.offsetMin = new Vector2(0, 0); new Vector2(left, bottom);
            //PanelSpacerRectTransform.offsetMax = new Vector2(-360, -0); new Vector2(-right, -top);
            obj.GetComponent<RectTransform>().offsetMin = new Vector2(0, i * 10); // left,bottom
            obj.GetComponent<RectTransform>().offsetMax = new Vector2(0, -height+i*10+heightPerOne); // right,top
            if (i >= robAmount)
            {
                obj.SetActive(false);
            }
        }


    }
}
