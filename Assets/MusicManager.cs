using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance {get; private set;}

    public AudioSource levelMusic;
    public AudioSource levelMusicLayer;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        levelMusicLayer.mute = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeInPowerupMusic()
    {
        levelMusicLayer.mute = false;
    }

    public void FadeOutPowerupMusic()
    {
        levelMusicLayer.mute = true;
    }
}
