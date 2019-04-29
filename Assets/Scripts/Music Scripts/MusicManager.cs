using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip forestClip;
    [SerializeField] private AudioClip mountainClip;
    [SerializeField] private AudioClip miniBossClip;
	[SerializeField] private AudioClip finalBossClip;
    // Start is called before the first frame update
    void Start()
    {
        audio.Play();
    }
	
	public void SetMiniBossMusic()
	{
        audio.Stop();
		audio.clip = miniBossClip;
		audio.Play();
	}
	
	public void SetFinalBossMusic()
	{
        audio.Stop();
		audio.clip = finalBossClip;
		audio.Play();
	}

    public void SetMountainMusic()
    {
        audio.Stop();
        audio.clip = mountainClip;
        audio.Play();
    }

    public void SetForestMusic()
    {
        audio.Stop();
        audio.clip = forestClip;
        audio.Play();
    }

    public AudioSource GetAudioSource()
    {
        return audio;
    }

    // Update is called once per frame
}
