using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameController : MonoBehaviour {

    public float slowPerLoot;
    public float burglarSpeed;
    public float policeSpeed;
    public GameObject timeSlider;
    private float timer = 0;
    private float timeLimit = 60.0f;
	// Use this for initialization
	void Start () {
        foreach (string t in Input.GetJoystickNames())
        {
            Debug.Log(t);
        }
    }
	
	// Update is called once per frame
	void Update () {
        timer = Time.timeSinceLevelLoad;
        timeSlider.GetComponent<Slider>().value = timer / timeLimit;
		if(timer > timeLimit)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            timer = 0;
        }
	}

}
