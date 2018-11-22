using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pileBarLogic : MonoBehaviour {


    public GameObject partition;
    public List<GameObject> partitions;
    public int maxAmount;

    private float width;
	// Use this for initialization
	void Start () {
        maxAmount = 5;
        width = GetComponent<RectTransform>().sizeDelta.x;
        float widthPerOne = width / maxAmount;
        for(int i = 0; i < maxAmount; i++)
        {
            GameObject obj = Instantiate(partition,transform) as GameObject;
            //PanelSpacerRectTransform.offsetMin = new Vector2(0, 0); new Vector2(left, bottom);
            //PanelSpacerRectTransform.offsetMax = new Vector2(-360, -0); new Vector2(-right, -top);
            GetComponent<RectTransform>().offsetMin = new Vector2(i* widthPerOne, 15); // left,bottom
            GetComponent<RectTransform>().offsetMax = new Vector2((maxAmount-i) * widthPerOne, 0); // right,top
            partitions.Add(obj);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addOne()
    {

    }
}
