using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] musicClip;
    private AudioSource audioSource;
    private int musicIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        audioSource=GetComponent<AudioSource>();
        audioSource.clip = musicClip[0];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying==false)
        {
            if (musicIndex + 1 < musicClip.Length)
            {
                musicIndex += 1;
                audioSource.clip = musicClip[musicIndex];
                audioSource.Play();

            }
            else
            {
                musicIndex = 0;
                audioSource.clip = musicClip[musicIndex];
                audioSource.Play();


            }
        }
       
    }
}
