using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class TimeLine_Control : MonoBehaviour {

    // Use this for initialization
    public GameObject next;
    private PlayableDirector Timeline;
    public GameObject Current;

    void Start () {
        Timeline = next.GetComponent<PlayableDirector>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("p1" + "_button_b") || Input.anyKeyDown)
        {
            Debug.Log("ÖÖG");
            Camera.main.GetComponent<AudioSource>().clip = Camera.main.GetComponent<GameController>().buttonClip;
            Camera.main.GetComponent<AudioSource>().Play();
            Timeline.Play();
            Current.SetActive(false);
            
        }
    }
}
