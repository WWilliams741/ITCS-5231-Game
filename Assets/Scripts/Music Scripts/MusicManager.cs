using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip clip;
	[SerializeField] private AudioClip miniBossClip;
	[SerializeField] private AudioClip finalBossClip;
    // Start is called before the first frame update
    void Start()
    {
        audio.Play();
    }
	
	public void SetMiniBossMusic()
	{
		audio.clip = miniBossClip;
		audio.Play();
	}
	
	public void SetFinalBossMusic()
	{
		audio.clip = finalBossClip;
		audio.Play();
	}

    // Update is called once per frame
}
