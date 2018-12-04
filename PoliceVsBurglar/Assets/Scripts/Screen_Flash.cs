using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Screen_Flash : MonoBehaviour {
    public PostProcessingProfile Profile;
    public int r,g,b;
    // Use this for initialization
    void Start () {
        ChangeBloomAtRuntime();
    }
	
	// Update is called once per frame
	void Update () {
        ChangeBloomAtRuntime();
    }
    void ChangeBloomAtRuntime()
    {
        //copy current bloom settings from the profile into a temporary variable
        //BloomModel.Settings bloomSettings = Profile.bloom.settings;

        //change the intensity in the temporary settings variable
        //bloomSettings.bloom.intensity = 2;

        //set the bloom settings in the actual profile to the temp settings with the changed value
        //Profile.bloom.settings = bloomSettings;

        ColorGradingModel.Settings ColorSettings = Profile.colorGrading.settings;
        ColorSettings.channelMixer.red = new Vector3(r, g, b);
        Profile.colorGrading.settings = ColorSettings;

    }
    private void ChangeGradually()
    {
        
    }
}
