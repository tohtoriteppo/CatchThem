using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

    public float max_open_angle;
    public float wait_time;
    public bool isopening, isactive;
    public float speed;
    public float sum;
   // private IEnumerator Doorclose;
    public int Rotate_Axis;
    // Use this for initialization
    void Start()
    {
        isactive = false;
        sum = 0;
        isopening = true;
    }

    // Update is called once per frame
    void Update()
    {

        // if the door is activated
        if (isactive)
        {
            if (isopening == true)
            {
                if (Rotate_Axis == 0)
                    gameObject.transform.Rotate(Vector3.forward, speed * Time.deltaTime);
                else if (Rotate_Axis == 1)
                    gameObject.transform.Rotate(Vector3.right, speed * Time.deltaTime);
                else if (Rotate_Axis == 2)
                    gameObject.transform.Rotate(Vector3.up, speed * Time.deltaTime);
                //else { }
                sum += speed * Time.deltaTime;
                Debug.Log("add");
            }
            else if(isopening==false)
            {
                if (Rotate_Axis == 0)
                    gameObject.transform.Rotate(Vector3.forward, -speed * Time.deltaTime);
                else if (Rotate_Axis == 1)
                    gameObject.transform.Rotate(Vector3.right, -speed * Time.deltaTime);
                else if (Rotate_Axis == 2)
                    gameObject.transform.Rotate(Vector3.up, -speed * Time.deltaTime);
                //else { }
                sum -= speed * Time.deltaTime;
            }

            //check whether the door has reach the limitation
            if (Mathf.Abs(sum) > max_open_angle&& isopening == true)
            {
                isactive = false;
                StartCoroutine(Doorclose(wait_time));
            }
            if (Mathf.Abs(sum)< 1 && isopening==false)
            {
                isactive = false;
                sum = 0;
                isopening = true;
            }
        }
    }
    private IEnumerator Doorclose(float wait_time)
    {
        yield return new WaitForSeconds(wait_time);
        isopening = false;
        isactive = true;
    }
}
