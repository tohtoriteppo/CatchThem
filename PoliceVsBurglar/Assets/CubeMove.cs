using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CubeMove : MonoBehaviour
{
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime, 0f, Input.GetAxis("Vertical") * Time.deltaTime);
    }
}