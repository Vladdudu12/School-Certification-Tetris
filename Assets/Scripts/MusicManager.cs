using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _lineClips;

    [SerializeField]
    private AudioSource _soundFxSource;
    
    void Start()
    {
        _soundFxSource = GameObject.Find("SoundFx").GetComponent<AudioSource>();        
    }

    public void ChangeSoundClip(int line)
    {
        _soundFxSource.clip = _lineClips[line - 1];
        _soundFxSource.Play();
    }
}
